using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class levelEndScript : MonoBehaviour
{

    public ParticleSystem levelWinParticles;
    public string SceneName;
    [SerializeField] private AudioSource cheering;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Do whatever is needed when the level is over, like particles or annimation and end with loading the next level
        if (collision.gameObject.CompareTag("Player"))
        {
            levelWinParticles.Play();
            loadNextLevel();
            Debug.Log("level win");

            cheering.Play();
		}
    }

    private void loadNextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }

   
}
