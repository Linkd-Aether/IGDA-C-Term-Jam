using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Utils
{
    static public Quaternion DirectionToAngle(Vector2 direction) {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angle);
    }

    static public IEnumerator FadeLerp(float fadeTime, float targetAlpha, SpriteRenderer spriteRenderer) {
        float startingAlpha = spriteRenderer.color.a;
        float lerpT = 0;

        while (lerpT < 1) {
            lerpT += Time.deltaTime / fadeTime;
            lerpT = Mathf.Clamp(lerpT, 0, 1);
            float alpha = Mathf.Lerp(startingAlpha, targetAlpha, lerpT);
            SetAlpha(alpha, spriteRenderer);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    static public void SetAlpha(float alpha, SpriteRenderer spriteRenderer) {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}

