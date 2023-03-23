using System;
using UnityEngine;

namespace Contaquest.UI
{
    public class WorldToScreenSpaceUI : MonoBehaviour
    {
        public Transform targetTransform;
        public Vector3 offset;
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = ReferencesManager.Instance.MainCamera;
        }

        public void Update()
        {
            Vector3 targetTransformPos = mainCamera.WorldToScreenPoint(targetTransform.position);

            transform.position = targetTransformPos + offset;
        }
    }
}
