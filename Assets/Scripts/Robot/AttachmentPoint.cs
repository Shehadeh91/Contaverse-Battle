using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse.Robot
{
    public class AttachmentPoint : MonoBehaviour
    {
        [HorizontalGroup("Name")]
        public string attachmentPointName;

        [Button(Style = ButtonStyle.CompactBox)]
        [HorizontalGroup("Name")]
        public void ObjectName()
        {
            attachmentPointName = gameObject.name;
        }
        public Vector3 startPosition;
        public Quaternion startRotation;

        public void Awake()
        {
            startPosition = transform.localPosition;
            startRotation = transform.rotation;
        }

        public void Initialize(string sideSuffix = "")
        {
            attachmentPointName += sideSuffix;
        }

        public Vector3 GetDeltaPosition()
        {
            Vector3 deltaPosition = transform.localPosition - startPosition;
            deltaPosition = transform.parent.TransformVector(deltaPosition);
            return deltaPosition;
        }
        public Quaternion GetDeltaRotation()
        {
            return transform.rotation * Quaternion.Inverse(startRotation);
        }
    }
}
