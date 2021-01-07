using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// TODO: Should Always Load Gameplay and Checkpoint Scenes
public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    public float transitionTime;
    // Start is called before the first frame update
    

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ResetScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        
        // Get Scenes To Load
        SceneManager.LoadSceneAsync(levelIndex,LoadSceneMode.Single);
       
        
    }
}
