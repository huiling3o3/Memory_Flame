using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundsSO SO;

    private AudioSource audioSource;

    private void Awake()
    {
        Game.SetSoundManager(this);
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1)
    {
        SoundList soundList = Game.GetSoundManager().SO.sounds[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        if (source)
        {
            source.outputAudioMixerGroup = soundList.mixer;
            source.clip = randomClip;
            source.volume = volume * soundList.volume;
            source.Play();
            if (sound == SoundType.MAIN_MENU)
            {
                source.loop = true;
            }
        }
        else
        {
            Game.GetSoundManager().audioSource.outputAudioMixerGroup = soundList.mixer;
            if (sound == SoundType.MAIN_MENU)
            {
                Game.GetSoundManager().audioSource.Play();
                Game.GetSoundManager().audioSource.loop = true;
            }
            else
            {
                Game.GetSoundManager().audioSource.PlayOneShot(randomClip, volume * soundList.volume);
            }
            
        }
    }
}

[Serializable]
public struct SoundList
{
    [HideInInspector] public string name;
    [Range(0, 1)] public float volume;
    public AudioMixerGroup mixer;
    public AudioClip[] sounds;
}

public enum SoundType
{
    HURT,
    FOOTSTEP,
    DASH,
    SHOOT,
    CUTTREE,
    FIRECRACKLING,
    LEVELAMBIENCE,
    CLAW_ATTACK,
    ENEMY_SHOOT,
    MAIN_MENU,
    LEVEL1,
    HOVER,
    SUBMIT,
    CANCEL
}