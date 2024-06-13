using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip SelectButtonSound;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void OnStartButton()
    {
        audioSource.PlayOneShot(SelectButtonSound);
        SceneManager.LoadScene("GameScene");
    }
}
