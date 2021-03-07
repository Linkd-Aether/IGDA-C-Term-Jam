using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class AudioLoader
{
    // Variables
    static bool loaded = false;

    // Sound Effect References
    public static List<AudioClip> BountyPost = new List<AudioClip>();
    public static List<AudioClip> BulletHit = new List<AudioClip>();
    public static List<AudioClip> BulletShot = new List<AudioClip>();
    public static List<AudioClip> Death = new List<AudioClip>();
    public static List<AudioClip> Sizzle = new List<AudioClip>();
    public static List<AudioClip> Payday = new List<AudioClip>();
    public static List<AudioClip> Teleport = new List<AudioClip>();


    static public void LoadAudio() {
        if (!loaded) {
            AudioClip[] audioClips = (AudioClip[]) Resources.LoadAll<AudioClip>("Audio/SFX");
            foreach (AudioClip audioClip in audioClips) {
                string name = audioClip.name;
                name = name.Substring(0,name.Length - 1);

                switch (name) {
                    case "BountyPost":
                        BountyPost.Add(audioClip);
                        break;
                    case "BulletHit":
                        BulletHit.Add(audioClip);
                        break;
                    case "BulletShot":
                        BulletShot.Add(audioClip);
                        break;
                    case "Death":
                        Death.Add(audioClip);
                        break;
                    case "Sizzle":
                        Sizzle.Add(audioClip);
                        break;
                    case "Payday":
                        Payday.Add(audioClip);
                        break;
                    case "Teleport":
                        Teleport.Add(audioClip);
                        break;
                }
            }
        }
        loaded = true;
    }

    static public AudioClip GetAudio(string name) {
        if (!loaded) LoadAudio();

        switch (name) {
            case "BountyPost":
                return SelectRandomFrom(BountyPost);
            case "BulletHit":
                return SelectRandomFrom(BulletHit);
            case "BulletShot":
                return SelectRandomFrom(BulletShot);
            case "Death":
                return SelectRandomFrom(Death);
            case "Sizzle":
                return SelectRandomFrom(Sizzle);
            case "Payday":
                return SelectRandomFrom(Payday);
            case "Teleport":
                return SelectRandomFrom(Teleport);
        }
        return null;
    }

    static private AudioClip SelectRandomFrom(List<AudioClip> clips) {
        int select = Random.Range(0, clips.Count);
        Debug.Log(select);
        return clips[select];
    }

    static public void SetContext(AudioSource source, string name) {
        switch (name) {
            case "BountyPost":
                source.volume = .3f;
                source.pitch = 1.2f;
                break;
            case "BulletHit":
                source.volume = 0.1f;
                source.pitch = 1.5f + Random.Range(-.25f, .25f);
                break;
            case "BulletShot":
                source.volume = .2f;
                source.pitch = 1.2f  + Random.Range(-.25f, .25f);
                break;
            case "Death":
                source.volume = .3f;
                source.pitch = .9f;
                break;
            case "Sizzle":
                source.volume = .3f;
                source.pitch = 1.6f + Random.Range(-.25f, .25f);
                break;
            case "Payday":
                source.volume = .5f;
                source.pitch = .8f;
                break;
            case "Teleport":
                source.volume = .4f;
                source.pitch = 1.8f;
                break;
        }
    }
}
