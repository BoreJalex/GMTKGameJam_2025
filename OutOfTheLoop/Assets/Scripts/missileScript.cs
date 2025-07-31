using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileScript : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private ParticleSystem explosion;

    // Variables
    [SerializeField] private float speed;

    // Bools
    private bool alive = true; 

    // Start is called before the first frame update
    void Start()
    {
        // Getting/Setting
        rb = GetComponent<Rigidbody2D>();
        explosion = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
    }
	private void FixedUpdate()
	{
		// Movement
        if(gameObject != null && alive)
		    rb.velocity = speed * transform.up;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (alive)
        {
            explosion.Play();
            alive = false;
        }
	}
}
