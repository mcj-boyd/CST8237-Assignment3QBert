using UnityEngine;
using System.Collections;

public class HideInterstitialScreen : MonoBehaviour {

    public GameObject screen;
    public GameController controller;
    
    // Hides the interstitial screen once the player has clicked the buttopn
    public void HideScreen()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controller.StartGame(); //Sets the gameRunning value to true in the GameController
        screen.SetActive(false); //Removes the interstitial screen
    }
}
