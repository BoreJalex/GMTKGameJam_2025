using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileScript : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private ParticleSystem explosion;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    //private ParticleSystem thruster;

    // Variables
    [SerializeField] private float speed;
    private Vector2 startPos;
    private Quaternion startRot;

    // Bools
    public bool alive = false;
    public bool atStart = true;

    // Sounds
    [SerializeField] private AudioSource explosionSound;
    public AudioSource thrusterSound;

    // Start is called before the first frame update
    void Start()
    {
        // Getting/Setting
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
        explosion = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        col = GetComponent<Collider2D>();

        // Game Setup
        startPos = transform.position;
        startRot = transform.rotation;
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
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
            // Particle
            explosion.gameObject.SetActive(true);
            explosion.Play();

            // Sound
            float randPitch = UnityEngine.Random.Range(.4f, .8f);
            explosionSound.pitch = randPitch;
            explosionSound.Play();
            thrusterSound.Stop();

            // Sprite
            spriteRenderer.enabled = false;

            // Movement
            rb.velocity = Vector2.zero;

            // Bool
            alive = false;
			col.enabled = false;
			StartCoroutine(shipDeathRespawnTime(1f));

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LevelEnd"))
        {
            if (alive)
            {
                spriteRenderer.enabled = false;
                thrusterSound.Stop();
                rb.velocity = Vector2.zero;
            }
        }
        if (collision.gameObject.CompareTag("ScreenBounds"))
            Restart();
    }

    public void Restart()
    {
        //reset the spaceship position
        alive = false;
		col.enabled = false;
		rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0f;
        transform.position = startPos;
        transform.rotation = startRot;

        // Sound/Visual
        explosion.Stop();
        explosion.gameObject.SetActive(false);
        spriteRenderer.enabled = true;
        thrusterSound.Stop();

        // Bool
        atStart = true;
    }

    public void GameStart()
    {
        if (atStart)
        {
            thrusterSound.Play();

            alive = true;
			col.enabled = true;
			atStart = false;
        }
    }

    IEnumerator shipDeathRespawnTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Restart();
        yield return new WaitForSeconds(.1f);

        Restart();
    }
}
