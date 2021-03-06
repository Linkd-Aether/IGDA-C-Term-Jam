using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // Constants
    private const float VOLUME_CHANGE_RATE = .1f;
    private const float VOLUME_MIN = .25f;
    private const float VOLUME_MAX = .3f;
    private const float PITCH_CHANGE_RATE = .05f;
    private const float PITCH_MIN = .85f;
    private const float PITCH_MAX = 1.15f;
    private const float SCORE_TO_INTENSITY = 10000f;
    private const int SCORE_ONLY_INTENSITY_MAX = 6;
    private const int INTENSITY_MAX = 8;

    // Variables
    static private int intensity = 1;

    // Components & References
    static private AudioSource musicSource;
    static private AudioSource soundSource;
    

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.pitch = PITCH_MIN;
        ChangeVolume(VOLUME_MIN);

        soundSource = transform.Find("SFXPlayer").GetComponent<AudioSource>();
    }

    public void UpdateIntensity() {
        int streakMax = 1;
        int scoreMax = 0;
        foreach (PlayerManager player in GameManager.playerManagers) {
            streakMax = Mathf.Max(streakMax, player.streak);
            scoreMax = Mathf.Max(scoreMax, player.score);
        }
        
        if (intensity != streakMax) {
            int lowerBound = (int) Mathf.Floor(scoreMax / SCORE_TO_INTENSITY);
            lowerBound = Mathf.Min(lowerBound, SCORE_ONLY_INTENSITY_MAX);
            int intensityFactor = streakMax - 1 + lowerBound;
            intensityFactor = Mathf.Min(intensityFactor, INTENSITY_MAX);
            float intensityModifier = (float) intensityFactor / 8;

            float targPitch = (PITCH_MAX - PITCH_MIN) * intensityModifier + PITCH_MIN;
            ChangePitch(targPitch);

            float targVolume = (VOLUME_MAX - VOLUME_MIN) * intensityModifier + VOLUME_MIN;
            ChangeVolume(targVolume);

            intensity = streakMax;
        }
    }

    static public void PlaySound(string name) {
        AudioClip clip = AudioLoader.GetAudio(name);

        if (clip != null) {
            AudioLoader.SetContext(soundSource, name);
            soundSource.PlayOneShot(clip);
        } else {
            Debug.LogWarning($"{name} produced no relevant audio.");
        }
    }

    private void ChangeVolume(float volume) {
        StartCoroutine(LerpValue(musicSource.volume, volume, VOLUME_CHANGE_RATE, VolumeLerp));
    }

    private void ChangePitch(float pitch) {
        StartCoroutine(LerpValue(musicSource.pitch, pitch, PITCH_CHANGE_RATE, PitchLerp));
    }

    private IEnumerator LerpValue(float start, float target, float rate, LerpChange method) {
        float lerpT = 0;
        float fadeTime = Mathf.Abs(target - start) / (float) rate;

        while (lerpT < 1) {
            lerpT += Time.deltaTime / fadeTime;
            lerpT = Mathf.Clamp(lerpT, 0, 1);
            float value = Mathf.Lerp(start, target, lerpT);
            method(value);
            yield return new WaitForEndOfFrame();
        }
    }

    private delegate void LerpChange(float num);

    private void VolumeLerp(float volume) {
        musicSource.volume = volume;
    }

    private void PitchLerp(float pitch) {
        musicSource.pitch = pitch;
    }
}
