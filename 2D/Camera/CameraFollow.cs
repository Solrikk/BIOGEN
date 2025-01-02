using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SupanthaPaul
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float smoothSpeed = 0.125f;
        public Vector3 offset;
        [Header("Camera bounds")]
        public Vector3 minCameraBounds;
        public Vector3 maxCameraBounds;

        private void LateUpdate()
        {
            if (target == null)
            {
                Debug.LogWarning("Target not set for CameraFollow script.");
                return;
            }

            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Клэмпинг позиции камеры между минимальными и максимальными границами
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minCameraBounds.x, maxCameraBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minCameraBounds.y, maxCameraBounds.y);
            smoothedPosition.z = Mathf.Clamp(smoothedPosition.z, minCameraBounds.z, maxCameraBounds.z);

            transform.position = smoothedPosition;
        }

        public void SetTarget(Transform targetToSet)
        {
            target = targetToSet;
        }
    }
}
