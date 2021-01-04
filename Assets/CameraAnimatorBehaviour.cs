using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraAnimatorBehaviour : MonoBehaviour
{
    // Parameters
    public float zoomTime = 3f; 
    public AnimationCurve curve;
    private CinemachineVirtualCamera _zoomCamera;
    
    
    void Start()
    {
        _zoomCamera = GetComponent<CinemachineVirtualCamera>();
       
    }
    
    public void ZoomTo(float target)
    {
        StopAllCoroutines();
        if (zoomTime == 0)
        {
            _zoomCamera.m_Lens.OrthographicSize = target;
        }
        else
        {
            StartCoroutine(Zooming(target));
        }
     
    }
    public void ZoomTo(float target,float zt, AnimationCurve c)
    {
        StopAllCoroutines();
        if (zoomTime == 0)
        {
            _zoomCamera.m_Lens.OrthographicSize = target;
        }
        else
        {
            StartCoroutine(Zooming(target,zt,c));
        }
     
    }

    IEnumerator Zooming(float target,float zt, AnimationCurve c)
    {
        // initial time in blend
        float startTime = Time.time * Time.timeScale;
        float endTime = startTime;
        float durationInBlend = (endTime - startTime);
        // Before Lerping
        float initial = _zoomCamera.m_Lens.OrthographicSize;
        while (_zoomCamera.m_Lens.OrthographicSize != target && durationInBlend < zt)
        {
            yield return new WaitForFixedUpdate();
            // Animation Evaluated At Time T (Speed Multiplier)
            float normalizedAnimationTime = c.Evaluate(Mathf.Clamp01(durationInBlend / zt ));
            
            _zoomCamera.m_Lens.OrthographicSize = Mathf.Lerp(initial, target, normalizedAnimationTime);
            
            endTime = Time.time * Time.timeScale;
            durationInBlend = (endTime - startTime);
        }
    }
    IEnumerator Zooming(float target)
    {
        // initial time in blend
        float startTime = Time.time * Time.timeScale;
        float endTime = startTime;
        float durationInBlend = (endTime - startTime);
        // Before Lerping
        float initial = _zoomCamera.m_Lens.OrthographicSize;
        while (_zoomCamera.m_Lens.OrthographicSize != target && durationInBlend < zoomTime)
        {
            yield return new WaitForFixedUpdate();
            // Animation Evaluated At Time T (Speed Multiplier)
            float normalizedAnimationTime = curve.Evaluate(Mathf.Clamp01(durationInBlend / zoomTime ));
            
            _zoomCamera.m_Lens.OrthographicSize = Mathf.Lerp(initial, target, normalizedAnimationTime);
            
            endTime = Time.time * Time.timeScale;
            durationInBlend = (endTime - startTime);
        }
    }

    
    }
