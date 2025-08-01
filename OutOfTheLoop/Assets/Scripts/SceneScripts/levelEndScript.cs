using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class levelEndScript : MonoBehaviour
{

    public ParticleSystem levelWinParticles;
    public string SceneName;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Do whatever is needed when the level is over, like particles or annimation and end with loading the next level
        if (collision.gameObject.CompareTag("Player"))
        {
            levelWinParticles.Play();
            loadNextLevel();
            Debug.Log("level win");

        }
    }

    private void loadNextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }

    void Update()
    {
        //DON'T FORGET TO GET RID OF THIS LINE OF CODE WHEN RELEASING THE GAME
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
