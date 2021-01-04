using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Behaviours
{
    public class CameraTrigger : TriggerBehaviour
    {
        
        public CameraAnimatorBehaviour cameraAnimator;
        public bool useCustomBlend = true;
        public AnimationCurve zoomCurve;
        public float zoomTime;
        public float targetZoom;
        public override void Start()
        {
            base.Start();
            
            UnityEventTools.AddVoidPersistentListener(triggerEvent,SetCamera);
        }

        private void SetCamera()
        {
            print("Activated");
            if (useCustomBlend)
            {
                cameraAnimator.ZoomTo(targetZoom,zoomTime,zoomCurve);
            }
            else
            {
                cameraAnimator.ZoomTo(targetZoom);
            }
        }
    }
}