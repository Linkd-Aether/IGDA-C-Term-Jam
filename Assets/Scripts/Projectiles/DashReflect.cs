using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashReflect : Projectile
{
    // Constants
    private static Sprite[] VARIANTS = new Sprite[2];
    const float LIFETIME = .4f;

    // Variables


    protected override void Start() {
        base.Start();

        lifetime = LIFETIME;
        
        VARIANTS = (Sprite[]) Resources.LoadAll<Sprite>($"Sprites/Projectiles/LaserTrail");
        SetSprite(VARIANTS[Random.Range(0,VARIANTS.Length)]);
        SetAlpha(150/255f);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.collider.gameObject.tag == "Projectile") {
            
        }
    }
}
