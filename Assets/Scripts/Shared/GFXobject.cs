using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GFXobject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color baseColor;


    protected virtual void Start() {
        LoadComponents();
    }

    public void SetBaseColor(Color color) {
        baseColor = color;
        SetColor(color);
    }

    public void SetColor(Color color) {
        spriteRenderer.color = color;
    }

    public void SetAlpha(float alpha) {
        Utils.SetAlpha(alpha, spriteRenderer);
    }

    public void SetSprite(Sprite sprite) {
        spriteRenderer.sprite = sprite;
    }

    public virtual void LoadComponents() {
        foreach (Transform child in transform) {
            if (child.tag == "GFX") {
                spriteRenderer = child.GetComponent<SpriteRenderer>();
            }
        }
    }

    // protected IEnumerator FadeOut(float fadeTime, float fadeTarget = 0) {
    //     float alpha = spriteRenderer.color.a;
    //     while (alpha > fadeTarget) {
    //         alpha -= Time.deltaTime / fadeTime;
    //         SetAlpha(alpha);
    //         yield return new WaitForEndOfFrame();
    //     }
    //     yield return null;
    // }

    // protected IEnumerator FadeIn(float fadeTime, float fadeTarget = 1) {
    //     float alpha = spriteRenderer.color.a;
    //     while (alpha < fadeTarget) {
    //         alpha += Time.deltaTime / fadeTime;
    //         SetAlpha(alpha);
    //         yield return new WaitForEndOfFrame();
    //     }
    //     yield return null;
    // }

    // protected IEnumerator FadeLerp(float fadeTime, float targetAlpha) {
    //     float startingAlpha = spriteRenderer.color.a;
    //     float lerpT = 0;

    //     while (lerpT < 1) {
    //         lerpT += Time.deltaTime / fadeTime;
    //         lerpT = Mathf.Clamp(lerpT, 0, 1);
    //         float alpha = Mathf.Lerp(startingAlpha, targetAlpha, lerpT);
    //         SetAlpha(alpha);
    //         yield return new WaitForEndOfFrame();
    //     }
    //     yield return null;
    // }

    protected IEnumerator FadeLerp(float fadeTime, float targetAlpha) {
        yield return StartCoroutine(Utils.FadeLerp(fadeTime, targetAlpha, spriteRenderer));
    }
}
