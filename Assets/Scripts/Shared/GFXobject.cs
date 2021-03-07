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
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    public void SetSprite(Sprite sprite) {
        spriteRenderer.sprite = sprite;
    }

    public void LoadComponents() {
        foreach (Transform child in transform) {
            if (child.tag == "GFX") {
                spriteRenderer = child.GetComponent<SpriteRenderer>();
            }
        }
    }

    protected IEnumerator FadeOut(float fadeTime) {
        float alpha = spriteRenderer.color.a;
        while (alpha > 0) {
            alpha -= Time.deltaTime / fadeTime;
            SetAlpha(alpha);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
