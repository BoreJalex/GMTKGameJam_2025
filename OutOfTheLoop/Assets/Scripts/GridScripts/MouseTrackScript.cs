using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrackScript : MonoBehaviour
{
	public bool isColliding;

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Hazard") || collision.gameObject.CompareTag("LevelEnd"))
			isColliding = true;
	}
}
