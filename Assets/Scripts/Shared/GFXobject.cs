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

    protected IEnumerator FadeLerp(float fadeTime, float targetAlpha) {
        yield return StartCoroutine(Utils.FadeLerp(fadeTime, targetAlpha, spriteRenderer));
    }
}
