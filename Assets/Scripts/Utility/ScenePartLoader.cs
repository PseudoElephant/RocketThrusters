using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

public class ScenePartLoader : MonoBehaviour
{

    public LayerMask mask;

    public SceneField sceneAsset;
    // State 
    private bool _isLoaded;
    private bool _shouldLoad = false;
    private Collider2D _collider2D;
    private int _framesDelay;
    
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.sceneCount > 0 && sceneAsset.BuildIndex < SceneManager.sceneCount)
        {
            // Should have the same name as the scene
            _isLoaded = true;
            
        }

        _collider2D = GetComponent<Collider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_collider2D.IsTouchingLayers(mask) && !_shouldLoad && _isLoaded)
        {
            print(_isLoaded);
            _shouldLoad = true;
            StopAllCoroutines();
            StartCoroutine(TriggerCheck());
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_collider2D.IsTouchingLayers(mask)  && _shouldLoad && !_isLoaded)
        {
            _shouldLoad = false;
            StartCoroutine(TriggerCheck());
        }
    }
    
    IEnumerator TriggerCheck()
    {
        // Delay Trigger For Frames
        for (int i = 0; i < _framesDelay; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        if (_shouldLoad)
        {
            
            StartCoroutine(LoadScene());
        }
        else
        {
        
            StartCoroutine(UnLoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        if (!_isLoaded)
        {
            AsyncOperation operation =  SceneManager.LoadSceneAsync(sceneAsset.BuildIndex, LoadSceneMode.Additive);
            while (!operation.isDone)
            {
                yield return null;
            }
           
            _isLoaded = true;
        }
    }   
    IEnumerator UnLoadScene()
    {
        if (_isLoaded)
        {
            AsyncOperation operation =  SceneManager.UnloadSceneAsync(sceneAsset.BuildIndex);
            while (!operation.isDone)
            {
                yield return null;
            }
            _isLoaded = false;
        }
    }
}
