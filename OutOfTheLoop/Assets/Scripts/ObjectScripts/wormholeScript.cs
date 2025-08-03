using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wormholeScript : MonoBehaviour
{

    public GameObject otherPoint;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player
        if (collision.CompareTag("Player"))
        {
            // Move the player to the other wormhole point
            collision.transform.position = otherPoint.transform.position;
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        // Disable the wormhole for a short duration to prevent immediate re-entry
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        otherPoint.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(.3f); // Adjust the cooldown duration as needed
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        otherPoint.GetComponent<CircleCollider2D>().enabled = true;
        Debug.Log("Wormhole activated, cooldown complete.");

    }

}
