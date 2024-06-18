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
        // audioSource = gameObject.AddComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
    }
    public void OnStartButton()
    {
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        audioSource.PlayOneShot(SelectButtonSound);

        // 効果音の長さだけ待つ
        yield return new WaitForSeconds(SelectButtonSound.length);

        // シーンをロード
        SceneManager.LoadScene("GameScene");
    }
}
