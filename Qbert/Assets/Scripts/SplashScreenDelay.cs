using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenDelay : MonoBehaviour {

    public int showTime;
	// Use this for initialization
	IEnumerator Start () {
        //Delays on the splash screen for 4 seconds
    
        yield return new WaitForSeconds(showTime);
        SceneManager.LoadScene("MainMenu");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
