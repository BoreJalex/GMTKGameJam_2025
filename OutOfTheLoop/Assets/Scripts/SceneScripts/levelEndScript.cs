using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class levelEndScript : MonoBehaviour
{

    public ParticleSystem levelWinParticles;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Do whatever is needed when the level is over, like particles or annimation and end with loading the next level
        if (collision.gameObject.CompareTag("Player"))
        {
            levelWinParticles.Play();
            loadNextLevel();

        }
    }

    private void loadNextLevel()
    {
        // Load the next level here
    }
}
