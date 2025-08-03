using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;



public class NewSceneManager : MonoBehaviour
{
    public string SceneName;
    public Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnButtonPressed);

    }

    private void OnButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }

    public void LoadLevelSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }
}
