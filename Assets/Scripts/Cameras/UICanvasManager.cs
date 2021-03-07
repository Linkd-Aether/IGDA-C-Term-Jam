﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UICanvasManager : MonoBehaviour
{
    // Constants
    static private Vector3 HEALTH_UI_ORIGIN;
    static private Vector3 STREAK_UI_ORIGIN;
    static private Vector3 SCORE_UI_ORIGIN;
    static private float[] SCALAR = new float[3] { 1, .75f, .5f };
    static private float HEALTH_FADE_TIME = .5f;
    static private float HEALTH_FADE_ALPHA = .4f;
    
    // Variables
    private int playerValue;
    private int healthDisplayed = 0;
    private int healthTarget; // 0-3 for number of health

    // Components
    private RectTransform healthUI;
    private RectTransform streakUI;
    private RectTransform scoreUI;
    

    private void Awake() {
        healthUI = transform.Find("HealthUI").GetComponent<RectTransform>();
        HEALTH_UI_ORIGIN = healthUI.anchoredPosition;
        streakUI = transform.Find("StreakUI").GetComponent<RectTransform>();
        STREAK_UI_ORIGIN = streakUI.anchoredPosition;
        scoreUI = transform.Find("ScoreUI").GetComponent<RectTransform>();
        SCORE_UI_ORIGIN = scoreUI.anchoredPosition;
    }

    public void SetPlayer(int value) {
        playerValue = value;
        GameManager.playerManagers[playerValue].ConnectUIManager(this);
        InitComponents(gameObject, LayerMask.NameToLayer($"UILayer{playerValue}"), GameManager.GetColor(playerValue));
    }

    private void InitComponents(GameObject obj, int layer, Color color) {
        obj.layer = layer;

        SpriteRenderer spr = obj.GetComponent<SpriteRenderer>();
        if (spr != null) spr.color = color;

        foreach (Transform child in obj.transform) {
            InitComponents(child.gameObject, layer, color);
        }
    }

    #region Update UI Elements
        public void UpdateScore(int newTotal, int addedScore) {

        }

        public void UpdateStreak(int streak) {
            Sprite digitImage = FontUtils.DigitToFont(streak);
            
            Transform streakIcon = streakUI.Find("StreakIcon");
            streakIcon.GetComponent<SpriteRenderer>().sprite = digitImage;
        }

        public void UpdateHealth(int health) {
            healthTarget = health;
            if (healthTarget < healthDisplayed) {
                // lose health
                SpriteRenderer target = healthUI.GetComponentsInChildren<SpriteRenderer>()[healthDisplayed - 1];
                StartCoroutine(HealthFade(HEALTH_FADE_ALPHA, target));
                healthDisplayed--;
            } else if (healthTarget > healthDisplayed) {
                // gain health
                SpriteRenderer target = healthUI.GetComponentsInChildren<SpriteRenderer>()[healthDisplayed];
                StartCoroutine(HealthFade(1, target));
                healthDisplayed++;
            }
        }

        private IEnumerator HealthFade(float targetAlpha, SpriteRenderer spriteRenderer) {
            yield return StartCoroutine(Utils.FadeLerp(HEALTH_FADE_TIME, targetAlpha, spriteRenderer));
            UpdateHealth(healthTarget);
        }
    #endregion

    #region Canvas Resizing
        public void ResizeCanvas(int playerCount) {
            if ((playerCount == 3 && playerValue == 1) || (playerCount == 4 && (playerValue == 0 || playerValue == 2))) {
                // Do not change the size of these canvases
            } else {
                float scalar = SCALAR[Mathf.Min(playerCount-1, 2)];

                ResizeHealthUI(scalar);
                ResizeStreakUI(scalar);
                ResizeScoreUI(scalar);
            }
        }

        private void ResizeHealthUI(float scalar) {
            ResizeLocalScale(healthUI, scalar);
            ResizeAnchorPos(healthUI, HEALTH_UI_ORIGIN, scalar);
        }
        
        private void ResizeStreakUI(float scalar) {
            ResizeLocalScale(streakUI, scalar);
            ResizeAnchorPos(streakUI, STREAK_UI_ORIGIN, scalar);
        }

        private void ResizeScoreUI(float scalar) {
            ResizeLocalScale(scoreUI, scalar);
            ResizeAnchorPos(scoreUI, SCORE_UI_ORIGIN, scalar);
        }

        private void ResizeLocalScale(RectTransform transform, float scalar) {
            transform.localScale = Vector3.one * scalar;
        }

        private void ResizeAnchorPos(RectTransform transform, Vector3 origin, float scalar) {
            transform.anchoredPosition = origin * scalar;
        }
    #endregion
}
