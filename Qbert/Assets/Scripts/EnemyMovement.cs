using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    private AudioSource audioSource;
    public AudioClip enemySpawn;
    public AudioClip enemyMove;
    public AudioClip enemyFall;

    GameController controller;
    Rigidbody rb;

    Vector3 downLeft;
    Vector3 downRight;

    private float nextActionTime;
    public float movementPeriod = 2.0f;
    bool falling = false;

    // Use this for initialization
    void Start () {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioSource = GetComponent<AudioSource>();
        //Enemies move faster on higher levels
        movementPeriod = 2.0f/controller.getLevel();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        downLeft = new Vector3(-0.71f, -1.0f, -0.71f);
        downRight = new Vector3(0.71f, -1.0f, -0.71f);
        nextActionTime = Time.time + movementPeriod;
        audioSource.PlayOneShot(enemySpawn, 1.0f);
    }
	
	// Update is called once per frame
	void Update () {

        if (controller.IsGameRunning()) // if the game is running
        {
            if (transform.position.y < 0 && transform.position.y > -1) // If the enemy is off the game area
            {

                rb.isKinematic = false;
                if (falling == false)
                    audioSource.PlayOneShot(enemyFall, 3.0f);
                falling = true;
            }

            if (transform.position.y < -5) //If the enemy has fallen of the screen
            {

                Destroy(gameObject);
            }

            //If its time to enemy to move and enemy is on the game area
            if (Time.time > nextActionTime && transform.position.y > 0) 
            {
                //reset movement timer
                nextActionTime += movementPeriod;

                if (Random.Range(0, 2) == 0) //move down left
                {
                    transform.Translate(downLeft, Space.Self);
                    audioSource.PlayOneShot(enemyMove, 1.0f);
                }
                else //move down right
                {
                    transform.Translate(downRight, Space.Self);
                    audioSource.PlayOneShot(enemyMove, 1.0f);
                }
            }
        }
	}
}
