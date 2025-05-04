using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    void Awake()
    {
        // 单例初始化
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    [Header("音效 Clips")]
    public AudioClip sfx_jump;
    public AudioClip sfx_footstep;
    public AudioClip sfx_bird;
    public AudioClip sfx_pickup;
    public AudioClip sfx_inventory;
    public AudioClip sfx_hurt;

    [Header("背景音乐 Clips")]
    public AudioClip bgm_theme;

    [Header("声音对象")]
    public GameObject soundObject; // 用于播放音效的预制体
    public GameObject currentMusicObject; // 当前正在播放的音乐对象

    /// <summary>
    /// 播放音效（支持中文名称）
    /// </summary>
    public void PlaySFX(string sfxName)
    {
        switch (sfxName)
        {
            case "跳跃":
                SoundObjectCreation(sfx_jump);
                break;
            case "脚步":
                SoundObjectCreation(sfx_footstep);
                break;
            case "鸟叫":
                SoundObjectCreation(sfx_bird);
                break;
            case "拾取":
                SoundObjectCreation(sfx_pickup);
                break;
            case "背包":
                SoundObjectCreation(sfx_inventory);
                break;
            case "受伤":
                SoundObjectCreation(sfx_hurt);
                break;
            default:
                Debug.LogWarning("未识别的音效：" + sfxName);
                break;
        }
    }

    /// <summary>
    /// 播放背景音乐（支持中文名称）
    /// </summary>
    public void PlayMusic(string musicName)
    {
        switch (musicName)
        {
            case "主题":
                MusicObjectCreation(bgm_theme);
                break;
            default:
                Debug.LogWarning("未识别的音乐名称：" + musicName);
                break;
        }
    }

    void SoundObjectCreation(AudioClip clip)
    {
        if (clip == null) return;

        GameObject newObj = Instantiate(soundObject, transform);
        AudioSource source = newObj.GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(newObj, clip.length + 0.1f); // 播放完自动销毁
    }

    void MusicObjectCreation(AudioClip clip)
    {
        if (clip == null) return;

        if (currentMusicObject)
            Destroy(currentMusicObject);

        currentMusicObject = Instantiate(soundObject, transform);
        AudioSource source = currentMusicObject.GetComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.Play();
    }
}