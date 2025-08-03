using UnityEngine;

public class repulserScript : MonoBehaviour
{
	// GameObjects and Transforms
	Transform missile;
	Transform repulser;

	// Variables
	private float distance; // The distance of this repulser from the player
	private float strengthPercentage; // The percent of strength the satelite uses based on distance

	[SerializeField] private float generalStrength; // The general strength of this repulser, which is multiplied by the strengthPercentage 
	[SerializeField] private float missileRotationSpeed; // The general speed at which the missile rotates

	// Bools
	private bool inRange = false; // Whether or not the player is in range

	private void Start()
	{
		// Getting/Setting
		repulser = transform.parent;
	}

	private void Update()
	{
		if (inRange)
		{
			// Calculating the distance between the repulser and the player, and the strengthPercentage based on that
			distance = Vector2.Distance(repulser.position, missile.position); // Obtaining distance from Missile to Satellite 
			strengthPercentage = 1 - (distance / transform.localScale.x); // Obtaining the strength of the push

			// Moving the player away from the repulser
			Vector3 awayDirection = (missile.position - repulser.position).normalized;
			missile.transform.position += awayDirection * generalStrength * strengthPercentage * Time.deltaTime;

			// Rotating the player's missile, and like the speed, is based on the strengthPercentage
			Vector2 toRepulsor = (repulser.position - missile.position).normalized; // Vector pointing at the repulsor
			float angleToTarget = Vector2.SignedAngle(missile.up, toRepulsor); // The angle difference of the missile and the repulsor
			float rotationRate = missileRotationSpeed * strengthPercentage * Time.deltaTime; // Rate at which missile rotates
			float clampedRotation = Mathf.Clamp(angleToTarget, -rotationRate, rotationRate); // Clamping said rotation

			missile.Rotate(0f, 0f, -clampedRotation);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
        missileScript missileScript = collision.GetComponent<missileScript>();

        // If player triggers, they're in range
        if (collision.gameObject.CompareTag("Player") && missileScript.alive)
        {
			if (missile != collision.transform)
				missile = collision.transform;

			missileScript misScript = collision.gameObject.GetComponent<missileScript>();
			AudioSource playerAudio = misScript.thrusterSound;
			playerAudio.pitch = .55f;

			inRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		// If player exits, they're not in range
		if (collision.gameObject.CompareTag("Player"))
		{
			missileScript misScript = collision.gameObject.GetComponent<missileScript>();
			AudioSource playerAudio = misScript.thrusterSound;
			playerAudio.pitch = .5f;

			inRange = false;
		}
	}
}
