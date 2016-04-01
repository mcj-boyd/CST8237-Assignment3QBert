using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public GameObject controlScreen;
    public void ToggleControlsScreen()
    {
        if (controlScreen.activeSelf)
        {
            controlScreen.SetActive(false);
        }
        else
        {
            controlScreen.SetActive(true);
        }
    }

    public void ChangeSceneWithName(string sceneName)
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(LoadAfterDelay(sceneName));
    }

    public void ExitGame()
    {
        StartCoroutine(ExitAfterDelay());
    }

    IEnumerator LoadAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.8f);
        // Load the Scene specified by the button click.
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator ExitAfterDelay()
    {
        yield return new WaitForSeconds(0.8f);
        // Load the Scene specified by the button click.
        Application.Quit();
    }

}
