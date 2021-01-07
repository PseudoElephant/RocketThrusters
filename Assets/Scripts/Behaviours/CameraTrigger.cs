using UnityEngine;
using UnityEngine.Events;


namespace Behaviours
{
    public class CameraTrigger : TriggerBehaviour
    {
        
        private CameraAnimatorBehaviour cameraAnimator;
        public bool useCustomBlend = true;
        public AnimationCurve zoomCurve;
        public float zoomTime;
        public float targetZoom;
        public override void Start()
        {
            base.Start();
            
            triggerEvent.AddListener(SetCamera);
            cameraAnimator = SectionManager.instance.defaultCamera.GetComponent<CameraAnimatorBehaviour>();

        }
        private void SetCamera()
        {
            if (useCustomBlend)
            {
                cameraAnimator.ZoomTo(targetZoom,zoomTime,zoomCurve);
            }
            else
            {
                cameraAnimator.ZoomTo(targetZoom);
            }
            
            triggerEvent.AddListener(SetCamera);
        }
    }
}