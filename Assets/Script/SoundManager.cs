using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource audioSource2;

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // AudioSource コンポーネントの確認と設定
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip is null. Cannot play sound.");
        }
    }
    public void StopBGM()
    {
        audioSource2.Stop();
    }
    public void StartBGM()
    {
        audioSource2.loop = true;
        audioSource2.Play();
    }
}

