using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SoundMark
{
    pageTurn = 1,

}

[System.Serializable]
public class Sound
{
    public SoundMark soundMark;
    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;

    public void Init(AudioSource audioSource, float volume)
    {
        source = audioSource;
        source.clip = clip;
        source.volume = volume;
        source.playOnAwake = false;
    }

    public void SetVolume(float volume)
    {
        if (source != null)
            source.volume = volume;
    }

    public void SetOnLoop()
    {
        source.loop = true;
    }
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource walkingAudioSource;
    [SerializeField] private AudioSource singleActionAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource backgroundNatureAmbient;

    [SerializeField] private float delayBetweenSongs;

    [SerializeField] [Range(0f, 1f)] private float gameVolume;

    [SerializeField] private AudioClip ambientOnNextDayEvent;
    [SerializeField] private AudioClip[] ambients;

    [SerializeField] private Slider volumeSettingsSlider;

    [SerializeField] private Sound[] soundList;



    private Sound _currentSound;

    public virtual void Init()
    {
        for (int i = 0; i < soundList.Length; i++)
        {
            if (soundList[i] != null)
            {
                soundList[i].Init(gameObject.AddComponent<AudioSource>(), gameVolume);
            }
        }

        ToggleSettingsSlider(false);

        volumeSettingsSlider.onValueChanged.AddListener(delegate{ OnValueChange(); });
        OnValueChange();

        GetRandomSongAsFirst();
        PlayAmbient();
    }

    public void ToggleSettingsSlider(bool show)
    {
        volumeSettingsSlider.gameObject.SetActive(show);
    }

    public void OnValueChange()
    {
        float value = volumeSettingsSlider.value;
        gameVolume = value / 10;

        if (walkingAudioSource != null)
            walkingAudioSource.volume = gameVolume;
        if (singleActionAudioSource != null)
            singleActionAudioSource.volume = gameVolume;
        if (musicAudioSource != null)
            musicAudioSource.volume = gameVolume;
        if (backgroundNatureAmbient != null)
            backgroundNatureAmbient.volume = gameVolume;

        Debug.Log("Game volune changed to - " + gameVolume);

        for(int i = 0; i < soundList.Length; i++)
        {
            soundList[i].SetVolume(gameVolume);
        }
    }


    int currentSongIndex;

    public void GetRandomSongAsFirst()
    {
        currentSongIndex = Random.Range(0, ambients.Length);
    }
    public void PlayAmbient()
    {
        AudioClip ambientClip = ambients[currentSongIndex];
        currentSongIndex++;
        if (currentSongIndex >= ambients.Length)
            currentSongIndex = 0;
        musicAudioSource.clip = ambientClip;
        musicAudioSource.Play();
    }

    public virtual void PlaySound(SoundMark soundMark, bool onLoop = false)
    {
        Sound sound = GetSoundBySoundMark(soundMark);
        _currentSound = sound;
        if (onLoop)
            sound.SetOnLoop();
        sound.source.Play();

    }

    public void ResumeBackgroundAmbient()
    {
        backgroundNatureAmbient.UnPause();
    }

    public void StopBackgroundAmbient()
    {
        StartCoroutine(StopAudioSourseSmoothly(backgroundNatureAmbient));
    }

    public void StopBackgroundAmbientImmediately()
    {
        backgroundNatureAmbient.Stop();
    }
    float switchSoundSpeed = 1f;
    IEnumerator StopAudioSourseSmoothly(AudioSource source)
    {
        while (source.volume > 0)
        {
            source.volume -= switchSoundSpeed * Time.deltaTime;
            yield return null;
        }
        source.Stop();
        source.volume = gameVolume;

    }

    Sound GetSoundBySoundMark(SoundMark soundMark)
    {
        foreach (Sound sound in soundList)
        {
            if (sound.soundMark == soundMark)
            {
                return sound;
            }
        }

        return null;
    }

}
