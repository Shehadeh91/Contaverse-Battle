using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.UI
{
    public class UIBillboard : MonoBehaviour
    {
        [SerializeField] private bool useMainCamera;

        [HideIf("useMainCamera")]
        [SerializeField] private Transform targetTransform;

        private void Start()
        {
            targetTransform = ReferencesManager.Instance.MainCamera.transform;
        }
        private void LateUpdate()
        {
            transform.LookAt(targetTransform);
        }
    }
}
