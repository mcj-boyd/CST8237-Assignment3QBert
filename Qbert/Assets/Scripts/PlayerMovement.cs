using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    
    public GameObject player;
    GameController controller;

    public AudioClip playerDying;
    public AudioClip platformSound;
    public AudioClip moveSound;
    public AudioClip fallingSound;
    private AudioSource audioSource;

    private SphereCollider sc;
    private Rigidbody rb;

    private bool falling = false;

	void Start () {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        audioSource = GetComponent<AudioSource>();
        sc = GetComponent<SphereCollider>();
        sc.isTrigger = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (controller.IsGameRunning()) //If the game is running
        {
            if (checkBounds() == 1) //if the player has moved off the game area
            {
                rb.isKinematic = false;
                if (falling == false) //checks if this is the first update that the player is falling
                {
                    audioSource.Stop(); //Stops other sounds
                    audioSource.PlayOneShot(fallingSound, 1.0f); //play falling sound
                    falling = true; //Sets falling to true, so sounds dont play again
                }
            }

            if (transform.position.y < -5) //If the player has fallen off the screen
            {
                falling = false; //reset the falling flag
                rb.isKinematic = true; 
                controller.DecrementLives();
                transform.position = new Vector3(0.0f, 4.875f, 2.84f); //Move the player back to the starting position
            }

            if (Input.GetKeyDown(KeyCode.Keypad1) && falling == false)
            {
                sc.isTrigger = true; //enables the trigger if it is still disabled from when player is spawned
                transform.Translate(-0.71f, -1.0f, -0.71f, Space.Self); //move the player
                audioSource.PlayOneShot(moveSound, 0.5f); //play movement sound
            }

            if (Input.GetKeyDown(KeyCode.Keypad3) && falling == false)
            {
                sc.isTrigger = true;
                transform.Translate(0.71f, -1.0f, -0.71f, Space.Self);
                audioSource.PlayOneShot(moveSound, 0.5f);
            }

            if (Input.GetKeyDown(KeyCode.Keypad7) && falling == false)
            {
                sc.isTrigger = true;
                transform.Translate(-0.71f, 1.0f, 0.71f, Space.Self);
                audioSource.PlayOneShot(moveSound, 0.5f);
            }

            if (Input.GetKeyDown(KeyCode.Keypad9) && falling == false)
            {
                sc.isTrigger = true;
                transform.Translate(0.71f, 1.0f, 0.71f, Space.Self);
                audioSource.PlayOneShot(moveSound, 0.5f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Platform") //if the player hits the platform
        {
            audioSource.Stop();
            falling = false;
            audioSource.PlayOneShot(platformSound, 0.5f);
            rb.isKinematic = true;
            transform.position = new Vector3(0.0f, 4.875f, 2.84f); //move player to starting position
            Destroy(other.gameObject); //Remove the platform
            controller.ResetPlatformSpawn(); //Reset the timer for platform respawns
        }

        if(other.tag == "Enemy1") //If the player hits an enemy
        {
            audioSource.Stop();
            audioSource.PlayOneShot(playerDying, 0.5f);
            Destroy(other.gameObject); //Destroy the enemy
            controller.DecrementLives(); 
            transform.position = new Vector3(0.0f, 4.875f, 2.84f); //move player to starting position
        }
    }

    //Function used to check if player has left the game area
    int checkBounds()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        if (y > 5 || y < 0)
            return 1;

        if(y == 4.875)
        {
            if (x > 1 || x < -1)
                return 1;
        }

        if (y == 3.875)
        {
            if (x > 2 || x < -2)
                return 1;
        }

        if (y == 2.875)
        {
            if (x > 2.8 || x < -2.8)
                return 1;
        }

        if (y == 1.875)
        {
            if (x > 3.5 || x < -3.5)
                return 1;
        }

        if (y == 0.875)
        {
            if (x > 4 || x < -4)
                return 1;
        }

        return 0;
    }
}
