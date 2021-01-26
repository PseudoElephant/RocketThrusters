using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MenuBehaviour
{
    public GameObject menu;
    public GameObject loadingScreen;
    public Slider progressBar;
    public SceneAsset gamePlayScene;
    
    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();
    public void PlayGame()
    {
        // Loads Game
        HideMenu();
        ShowLoadingScreen();
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(gamePlayScene.name));
        //_scenesToLoad.Add(SceneManager.LoadSceneAsync("Level2 Test",LoadSceneMode.Additive));
        //_scenesToLoad.Add(SceneManager.LoadSceneAsync("Level3 Test",LoadSceneMode.Additive));
        StartCoroutine(LoadingScreen());
    }


    public void QuitGame()
    {
        // Should Handle Quit Game
        Application.Quit();
    }

    private IEnumerator LoadingScreen()
    {
        float progress = 0.0f;
        foreach (var operation in _scenesToLoad)
        {
            progress += Mathf.Clamp01((operation.progress / 0.9f));
            print( progress);
            progressBar.value = (progress / _scenesToLoad.Count);
            yield return new WaitForEndOfFrame();
        }
    }

    private void HideMenu()
    {
        menu.SetActive(false);
    }
    
    private void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }
    
}
