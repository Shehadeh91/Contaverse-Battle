using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace Contaquest.Metaverse.Combat2
{
    public class Lane : MonoBehaviour
    {
        [TabGroup("Properties")] [SerializeField] private float laneOffset = 0f;

        [TabGroup("References")] [SerializeField] private Transform[] laneTransforms;
        [TabGroup("References")] [SerializeField, ReadOnly] private float laneLength;

        void OnValidate()
        {
            UpdateLaneLength();
        }

        private void UpdateLaneLength()
        {
            laneLength = 0f;
            for (int i = 0; i < laneTransforms.Length - 1; i++)
            {
                float distance = Vector3.Distance(laneTransforms[i].position, laneTransforms[i + 1].position);
                laneLength += distance;
            }
        }

        public bool IsProgressInsideLane(float lanePosition)
        {
            lanePosition = lanePosition - laneOffset;
            if(lanePosition < 0 || lanePosition > laneLength)
                return false;
            return true;
        }

        public Vector3 GetPositionOnLane(float lanePosition)
        {
            float lastDistance = 0;
            for (int i = 0; i < laneTransforms.Length - 1; i++)
            {
                float distance = Vector3.Distance(laneTransforms[i].position, laneTransforms[i+1].position);
                float currentDistance = lastDistance + distance;

                if(lastDistance < lanePosition && lanePosition < currentDistance)
                {
                    float t = (lanePosition - lastDistance) / distance;
                    return Vector3.Lerp(laneTransforms[i].position, laneTransforms[i + 1].position, t);
                }
            }

            Debug.LogError("Something is wrong...", gameObject);
            return Vector3.zero;
        }

        public Vector3 GetForwardOnLane(float lanePosition)
        {
            float lastDistance = 0;
            for (int i = 0; i < laneTransforms.Length - 1; i++)
            {
                float distance = Vector3.Distance(laneTransforms[i].position, laneTransforms[i+1].position);
                float currentDistance = lastDistance + distance;

                if(lastDistance < lanePosition && lanePosition < currentDistance)
                {
                    float t = (lanePosition - lastDistance) / distance;
                    return Vector3.Lerp(laneTransforms[i].position, laneTransforms[i + 1].position, t);
                }
            }

            Debug.LogError("Something is wrong...", gameObject);
            return Vector3.zero;
        }

        #if UNITY_EDITOR
        [TabGroup("References")] [Button] [HideInPlayMode]
        public void GetLaneTransformsFromChildren()
        {
            laneTransforms= new Transform[transform.childCount];
            int index=0;
            foreach(Transform c in transform)
            {
                laneTransforms[index++]= c;
            }
            UpdateLaneLength();
        }
        #endif
    }
}