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
    private Vector2 startPos;

    // Bools
    public bool alive = false;
    // Start is called before the first frame update
    void Start()
    {
        // Getting/Setting
        rb = GetComponent<Rigidbody2D>();
        explosion = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        startPos = transform.position;

    }
    private void FixedUpdate()
	{
        if(Input.GetKey(KeyCode.Space))
        {
            alive = true;

        }
        if (Input.GetKey(KeyCode.R))
        {
            //reset the spaceship position
            transform.position = startPos;
            alive = false;
            rb.velocity = new Vector2(0,0);
        }
        // Movement
        if (gameObject != null && alive)
		    rb.velocity = speed * transform.up;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (alive && !collision.gameObject.CompareTag("LevelEnd"))
        {
            explosion.Play();
            alive = false;
        }
	}

}
