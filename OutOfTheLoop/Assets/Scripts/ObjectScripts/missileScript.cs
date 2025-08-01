using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class missileScript : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private ParticleSystem explosion;

    // Variables
    [SerializeField] private float speed;
    private Vector2 startPos;
    private quaternion startRot;

    // Bools
    public bool alive = false;
    // Start is called before the first frame update
    void Start()
    {
        // Getting/Setting
        rb = GetComponent<Rigidbody2D>();
		explosion = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();

        // Game Setup
        startPos = transform.position;
        startRot = transform.rotation;
    }
    private void FixedUpdate()
	{
        if(Input.GetKey(KeyCode.Space))
        {
            GameStart();
		}
		if (Input.GetKey(KeyCode.R))
        {
            Restart();
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

    public void Restart()
    {
		//reset the spaceship position
		transform.position = startPos;
		transform.rotation = startRot;
		alive = false;
		rb.velocity = new Vector2(0, 0);
		rb.angularVelocity = 0f;
	}

    public void GameStart()
    {
		alive = true;
	}

}
