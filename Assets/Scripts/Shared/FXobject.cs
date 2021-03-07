using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FXobject : GFXobject
{
    static protected Color BLACK = new Color(0,0,0,0);
    static protected Color RED = new Color(165/255f,0,0,0);
    static protected Color WHITE = new Color(1,1,1,0);

    public SpriteRenderer spriteOverlay;

    private void SetAlphaFX(float alpha) {
        Color color = spriteOverlay.color;
        color.a = alpha;
        spriteOverlay.color = color;
    }

    public override void LoadComponents() {
        base.LoadComponents();

        foreach (Transform child in transform) {
            if (child.tag == "FX") {
                spriteOverlay = child.GetComponent<SpriteRenderer>();
            }
        }
        SetAlphaFX(0);
    }

    #region FX Tools
        protected IEnumerator ColorFlash(Color color, float fadeTime, float targetAlpha) {
            spriteOverlay.color = color;

            yield return StartCoroutine(AlphaLerpFX(fadeTime, targetAlpha));
            yield return new WaitForEndOfFrame();
            yield return StartCoroutine(AlphaLerpFX(fadeTime, 0));
            yield return null;
        }

        protected IEnumerator AlphaLerpFX(float fadeTime, float targetAlpha) {
            float startingAlpha = spriteOverlay.color.a;
            float lerpT = 0;

            while (lerpT < 1) {
                lerpT += Time.deltaTime / fadeTime;
                lerpT = Mathf.Clamp(lerpT, 0, 1);
                float alpha = Mathf.Lerp(startingAlpha, targetAlpha, lerpT);
                SetAlphaFX(alpha);
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
    #endregion
}
