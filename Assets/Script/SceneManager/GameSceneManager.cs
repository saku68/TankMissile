using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip SelectButtonSound1;
    public void OnTitleButton()
    {
        SoundManager.Instance.PlaySound(SelectButtonSound1);
        SceneManager.LoadScene("TitleScene");
    }
}
