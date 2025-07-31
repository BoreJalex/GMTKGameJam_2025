using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attractorScript : MonoBehaviour
{
	// GameObjects and Transforms
	Transform missile;
	Transform attractor;

	// Variables
	private float distance; // The distance of this attractor from the player
	private float strengthPercentage; // The percent of strength the satelite uses based on distance

	[SerializeField] private float generalStrength; // The general strength of this attractor, which is multiplied by the strengthPercentage 
	[SerializeField] private float missileRotationSpeed; // The general speed at which the missile rotates

	// Bools
	private bool inRange = false; // Whether or not the player is in range

	private void Start()
	{
		// Getting/Setting
		attractor = transform.parent;
	}

	private void Update()
	{
		if (inRange)
		{
			// Calculating the distance between the atractor and the player, and the strengthPercentage based on that
			distance = Vector2.Distance(attractor.position, missile.position); // Obtaining distance from Missile to Satellite 
			strengthPercentage = 1 - (distance / transform.localScale.x); // Obtaining the strength of the pull

			// Moving the player towards the attractor
			missile.transform.position = Vector2.MoveTowards(missile.position, attractor.position, generalStrength * strengthPercentage * Time.deltaTime);

			// Rotating the player's missile, and like the speed, is based on the strengthPercentage
			if(attractor.position.y > missile.position.y)
			{
				float missileRotation = missileRotationSpeed * strengthPercentage * Time.deltaTime;
				missile.Rotate(0f, 0f, missileRotation);
			}
			else // The attractor is below the missile
			{
				float missileRotation = missileRotationSpeed * strengthPercentage * Time.deltaTime;
				missile.Rotate(0f, 0f, -missileRotation);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// If player triggers, they're in range
		if (collision.gameObject.CompareTag("Player"))
		{
			if (missile != collision.transform)
				missile = collision.transform;

			inRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		// If player exits, they're not in range
		if (collision.gameObject.CompareTag("Player"))
		{
			inRange = false;
		}
	}
}
