using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeMapper : MonoBehaviour
{
    private Camera _mainCamera;

    private Camera _renderTextureCamera;

    private Transform _blurChild;

    private float _initialOrtho;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = SectionManager.instance.defaultCamera.gameObject.GetComponentInParent<Camera>();
        _renderTextureCamera = GetComponent<Camera>();
        _blurChild = GetComponentInChildren<Transform>();
        _initialOrtho = _mainCamera.orthographicSize;
    }


    private void LateUpdate()
    {
        var orthographicSize = _mainCamera.orthographicSize;
        _renderTextureCamera.orthographicSize = orthographicSize;
        _blurChild.localScale = Vector2.one * (_renderTextureCamera.orthographicSize * 2)/_initialOrtho;
   
    }
}
