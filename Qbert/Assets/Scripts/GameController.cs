using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject interstitialScreen;
    public GameObject gameOverScreen;
    public AudioSource audioSource;
    public AudioClip levelComplete;
    public AudioClip gameOverSound;

    public Text score;
    public Text lives;
    public Text level;

    public Text finalScore;

    public Text interstitialLevel;
    public Text interstitialCount;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject platformPrefab;
    private GameObject player;

    private bool gameRunning = false;
    private int scoreValue = 0;
    private int livesValue = 3;
    private int levelValue = 1;

    private int tileGoal = 0;
    private int tileCurrent = 0;

    private int spawnRateEnemies = 0;
    private float elapsedSpawnTimeEnemy = 0.0f;
    private int spawnRatePlatform = 0;
    private float elapsedSpawnTimePlatform = 0.0f;


    // Use this for initialization
    void Start () {
        LoadGameValues();
        ShowInterstitialScreen();
        SpawnPlayer();
        InitializeSpawns();
    }
	
	// Update is called once per frame
	void Update () {
        SpawnEnemy();
        spawnPlatform();
	}

    public void StartGame()
    {
        gameRunning = true;
    }

    public bool IsGameRunning()
    {
        return gameRunning;
    }

    //Adds the value to the current score
    public void AddScore(int value)
    {
        scoreValue += value;
        score.text = "" + scoreValue;
    }


    public void IncrementTileChange()
    {
        tileCurrent++; //Increments the number of tiles that have changes
        if(tileCurrent == tileGoal) //if all tiles have been changed (Level won)
        {
            gameRunning = false;
            Destroy(player.GetComponent<PlayerMovement>()); //Remove the PlayerMovement so player is unmoveable
            audioSource.PlayOneShot(levelComplete, 3.0f);
            LoadNextLevel();
        }
    }

    public void DecrementLives()
    {
        livesValue--;
        lives.text = "" + livesValue;

        if(livesValue < 1) // if the player has run out of lives
        {
            gameRunning = false;
            finalScore.text = "" + scoreValue; //Set final score text
            gameOverScreen.SetActive(true); //Show Gameover screen
            audioSource.PlayOneShot(gameOverSound, 3.0f);
        }
    }

    void IncrementLevel()
    {
        levelValue++;
        if (levelValue > 3) //Spawning gets ridicules after level 3, player will just play level 3 over and over and can still increment score
            levelValue = 3;
        level.text = "" + levelValue;
    }

    public int getLevel()
    {
        return levelValue;
    }

    void LoadNextLevel()
    {
        IncrementLevel();
        PlayerPrefs.SetInt("currentScore", scoreValue); //Updates PlayerPrefs with current game values
        PlayerPrefs.SetInt("currentLives", livesValue);
        PlayerPrefs.SetInt("currentLevel", levelValue);

        PlayerPrefs.Save(); 

        StartCoroutine(ReloadScene(5.0f)); //waits 5 seconds and reloads the scene
    }

    //Function gets the game information from the player prefs so that
    // appropriate level and difficuly is loaded and score and lives from
    // previous level are carried over
    void LoadGameValues()
    {
        if (PlayerPrefs.HasKey("currentScore"))
        {
            scoreValue = PlayerPrefs.GetInt("currentScore");
        } else {
            scoreValue = 0;
            PlayerPrefs.SetInt("currentScore", scoreValue);
        }

        if (PlayerPrefs.HasKey("currentLives"))
        {
            livesValue = PlayerPrefs.GetInt("currentLives");
        }
        else {
            livesValue = 3;
            PlayerPrefs.SetInt("currentLives", livesValue);
        }

        if (PlayerPrefs.HasKey("currentLevel"))
        {
            levelValue = PlayerPrefs.GetInt("currentLevel");
        }
        else {
            levelValue = 1;
            PlayerPrefs.SetInt("currentLevel", levelValue);
        }

        PlayerPrefs.Save();

        score.text = "" + scoreValue;
        lives.text = "" + livesValue;
        level.text = "" + levelValue;

        //Sets the total number of tiles that much be changed for the level to be complete
        tileGoal = 15 * levelValue; 
    }

    //Updates the interstitial screen and displays to the player
    void ShowInterstitialScreen()
    {
        interstitialLevel.text = "" + levelValue;
        interstitialCount.text = "" + levelValue;
        interstitialScreen.SetActive(true);
    }

    void SpawnPlayer()
    {
        //spawns the player at the starting spot
        player = (GameObject)Instantiate(playerPrefab, new Vector3(0.0f, 4.875f, 2.84f), Quaternion.identity);
    }

    //Sets the enemy respawn time and platform respawn time
    void InitializeSpawns()
    {
        spawnRateEnemies = 6 - ((levelValue - 1) * 2); //higher level = faster enemy respawn
        elapsedSpawnTimeEnemy = Time.time;

        spawnRatePlatform = levelValue * 4; //higher level = slower platform respawn
        elapsedSpawnTimePlatform = Time.time;
    }

    void SpawnEnemy()
    {
        //if time for an enemy to respawn and game is running
        if (Time.time - spawnRateEnemies > elapsedSpawnTimeEnemy && gameRunning) 
        {
            int rand = Random.Range(-1, 2);
            //spawn an enemy on any cube in the 3rd row
            Instantiate(enemyPrefab, new Vector3((rand * 1.42f), 2.75f, 1.42f), Quaternion.identity);
            elapsedSpawnTimeEnemy = Time.time;
        }
    }

    void spawnPlatform()
    {
        //if a platform isn't up and its time to spawn a platform
        if (!GameObject.FindGameObjectWithTag("Platform") && Time.time - spawnRatePlatform > elapsedSpawnTimePlatform)
        {

            int multiplyer = Random.Range(2, 6); //value used to randomly pick a row for the platform to spawn
            Vector3 platformInit = new Vector3(0.0f, 5.4f, 4.26f);
            Vector3 downLeft = new Vector3(-0.71f, -1.0f, -0.71f);
            Vector3 downRight = new Vector3(0.71f, -1.0f, -0.71f);

            if (Random.Range(0, 2) == 0) //spawn platform down left side
            {
                platformInit = platformInit + (downLeft * multiplyer);
            }
            else //spawn platform down right side
            {
                platformInit = platformInit + (downRight * multiplyer);
            }

            Instantiate(platformPrefab, platformInit, Quaternion.Euler(new Vector3(0, 45, 0)));
        }
    }

    //public function so the platform respawn can be reset by PlayerMovement
    public void ResetPlatformSpawn()
    {
        elapsedSpawnTimePlatform = Time.time;
    }

    //waits a specified amount of time and reloads scene
    IEnumerator ReloadScene(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene("Game");
    }

}
