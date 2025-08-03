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

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
