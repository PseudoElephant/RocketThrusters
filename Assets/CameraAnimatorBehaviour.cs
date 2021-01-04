using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraAnimatorBehaviour : MonoBehaviour
{
    // Parameters
    private Animator _animator;
    public CinemachineStateDrivenCamera _stateCamera;
    public CinemachineVirtualCamera _zoomCamera;
    public CinemachineVirtualCamera _defaultCamera;

    // Constants
    private const string Zoom = "isZoom";
    private static readonly int IsZoom = Animator.StringToHash(Zoom);
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ZoomIn(int zoom)
    {
        if (!_animator.GetBool(IsZoom))
        {
            //_zoomCamera.m_Lens.OrthographicSize = zoom;   
            _animator.SetBool(IsZoom,true);
           // StartCoroutine(SwapToDefault());
          
        }
        else
        {
         //   StopAllCoroutines();
         //   _defaultCamera.m_Lens.OrthographicSize = zoom;
            _animator.SetBool(IsZoom,false);
        }
            
    }

    IEnumerator Check()
    {
        yield return new WaitForSeconds(1);
        _zoomCamera.m_Lens.OrthographicSize = 1;
    }
    IEnumerator SwapToDefault()
    {
     
        yield return new WaitForSeconds(_stateCamera.m_DefaultBlend.BlendTime+1f);
        _defaultCamera.m_Lens.OrthographicSize = _zoomCamera.m_Lens.OrthographicSize;
        _animator.SetBool(IsZoom,false);
    }

}
