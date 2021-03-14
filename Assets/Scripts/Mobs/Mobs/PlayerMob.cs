using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMob : Mob
{
    // Constants
    const float MAX_SHOT_CHARGE = 2.5f;
    const float MAX_SHOT_FLASH_TIME = 0.25f;
    const float MIN_SHOT_FLASH_TIME = 0.15f;
    const float MAX_SHOT_FLASH_ALPHA = 0.4f;
    const float MIN_SHOT_FLASH_ALPHA = 0.25f;
    const float FULL_CHARGE_ALPHA = 0.75f;
    const float DASH_MOD = 6f;
    const float DASH_BULLET_MOD = 1.5f;
    const float DASH_MAX_TIME = .35f;
    const float DASH_COOLDOWN = .6f;
    const float DASH_REFLECT_DIST = .65f;
    const float DASH_CHANGE_TIME = .5f;
    const float DASH_DARKEN_FX = .75f;
    const float DRAW_AIM_DISTANCE = .15f;

    // Variables
    private Vector2 moveInput = Vector2.zero;
    private float chargeTime = 0f;
    private bool dashing = false;
    private float dashTime = 0f;
    private float dashCooldown = 0f;
    private Vector2 lastDashCoordinate;

    // Components & References
    private Transform playerAim;

    ParticleSystem.EmissionModule particleEmissions;


    private void Awake() {
       foreach (Transform child in transform) {
            if (child.name == "PlayerAim") {
                playerAim = child;
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        transform.parent.GetComponent<PlayerManager>().SetHealthUI(3);
        speed = 1000f; 
    }

    private void FixedUpdate() {
        particleEmissions = GetComponent<ParticleSystem>().emission;
        particleEmissions.enabled = isBounty;
        if (isAlive) {
            // Movement
            Vector2 force = moveInput * speed * Time.fixedDeltaTime;
            if (dashing) {
                force *= DASH_MOD;
                dashTime += Time.fixedDeltaTime;
                CheckDashReflect();
                if (dashTime >= DASH_MAX_TIME) {
                    StartCoroutine(EndDash());
                }
            } else if (dashCooldown > 0) {
                dashCooldown -= Time.fixedDeltaTime;
            }
            rb.AddForce(force);

            // Shooting Charge & Cooldown
            if (shoot) {
                chargeTime += Time.fixedDeltaTime;
            }
            if (shotCooldown > 0) {
                shotCooldown -= Time.fixedDeltaTime;
            }
        }
    }

    // Spawn a bullet checking for if a Dash Shot was performed
    protected override Bullet SpawnProjectile() {
        Bullet bullet = base.SpawnProjectile();

        float modifier = Mathf.Min(chargeTime, MAX_SHOT_CHARGE);
        bullet.speed += bullet.speed * modifier;
        bullet.transform.localScale += bullet.transform.localScale * modifier / 2;

        rb.AddForce(-aimDir * RECOIL * modifier);

        return bullet;
    }

    private IEnumerator ChargingEffect() {
        yield return new WaitForSeconds(.1f);
        if (shoot) {
            float percentThroughCharge = Mathf.Min(chargeTime, MAX_SHOT_CHARGE) / MAX_SHOT_CHARGE;
            bool fullCharge = (percentThroughCharge == 1);
            float flashTime = Mathf.Lerp(MIN_SHOT_FLASH_TIME, MAX_SHOT_FLASH_TIME, percentThroughCharge);
            float targetAlpha = Mathf.Lerp(MIN_SHOT_FLASH_ALPHA, MAX_SHOT_FLASH_ALPHA, percentThroughCharge);
            if (fullCharge) targetAlpha = FULL_CHARGE_ALPHA;
            yield return StartCoroutine(ColorFlash(WHITE, flashTime, targetAlpha));
            yield return StartCoroutine(ColorFlash(WHITE, flashTime, 0));
            StartCoroutine(ChargingEffect());
        }
    }

    private IEnumerator EndDash() {
        dashing = false;
        yield return StartCoroutine(AlphaLerpFX(DASH_CHANGE_TIME, 0));
        isImmune = false;
    }

    private void CheckDashReflect() {
        if (Vector2.Distance(lastDashCoordinate, transform.position) >= DASH_REFLECT_DIST) {
            SpawnDashReflect(lastDashCoordinate, transform.position);
            lastDashCoordinate = transform.position;
        }
    }

    public override void LoseHealth(Mob shooter) {
        base.LoseHealth(shooter);
        transform.parent.GetComponent<PlayerManager>().SetHealthUI(health);
    }

    #region Handle Control Changes
        public void SetMoveInput(Vector2 input) {
            moveInput = input;
        }

        public void SetShooting(bool isPressed) {
            if (isPressed) { // this was a press, start a shot charge
                shoot = true;
                chargeTime = 0f;
                StartCoroutine(ChargingEffect());
            } else { // this was a release, release the shot
                if (shoot) {
                    if (shotCooldown <= 0){
                        SpawnProjectile();
                        shotCooldown = SHOT_COOLDOWN;
                    }
                    shoot = false;
                } 
            }
        }

        public void SetDash() {
            if (dashCooldown <= 0) {
                dashing = true;
                dashTime = 0f;
                dashCooldown = DASH_COOLDOWN;
                lastDashCoordinate = transform.position;
                
                isImmune = true;
                spriteOverlay.color = BLACK;
                StartCoroutine(AlphaLerpFX(DASH_CHANGE_TIME, DASH_DARKEN_FX));
            }
        }

        public void SetAimInput(Vector2 input) {
            aimDir = (input == Vector2.zero) ? aimDir : input;            
            playerAim.localPosition = aimDir * DRAW_AIM_DISTANCE;
            playerAim.localRotation = Utils.DirectionToAngle(aimDir);
        }
    #endregion
}
