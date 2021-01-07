using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePartLoader : MonoBehaviour
{

    public LayerMask mask;
    
    // State 
    private bool _isLoaded;
    private bool _shouldLoad = false;
    private Collider2D _collider2D;
    
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.sceneCount > 0)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                // Should have the same name as the scene
                if (scene.name == gameObject.name)
                {
                    _isLoaded = true;
                }
            }
        }

        _collider2D = GetComponent<Collider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_collider2D.IsTouchingLayers(mask) && !_shouldLoad)
        {
            _shouldLoad = true;
            StartCoroutine(TriggerCheck());
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_collider2D.IsTouchingLayers(mask)  && _shouldLoad)
        {
            _shouldLoad = false;
            StartCoroutine(TriggerCheck());
        }
    }
    
    IEnumerator TriggerCheck()
    {
        yield return new WaitForFixedUpdate();
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
            AsyncOperation operation =  SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
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
            AsyncOperation operation =  SceneManager.UnloadSceneAsync(gameObject.name);
            while (!operation.isDone)
            {
                yield return null;
            }
            _isLoaded = false;
        }
    }
}
