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
        foreach (PlayerManager player in GameManager.playerManagers) {
            streakMax = Mathf.Max(streakMax, player.streak);
        }
        
        if (intensity != streakMax) {
            float intensityFactor = (float) (streakMax - 1) / 8;

            float targPitch = (PITCH_MAX - PITCH_MIN) * intensityFactor + PITCH_MIN;
            ChangePitch(targPitch);

            float targVolume = (VOLUME_MAX - VOLUME_MIN) * intensityFactor + VOLUME_MIN;
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
