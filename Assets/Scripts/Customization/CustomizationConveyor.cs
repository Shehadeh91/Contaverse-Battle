using System;
using System.Collections;
using System.Collections.Generic;
using Contaquest.Metaverse.Behaviours;
using Contaquest.Metaverse.Data;
using Contaquest.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Robot.Customization
{
    public class CustomizationConveyor : MonoBehaviour
    {
        [TabGroup("Properties")] [SerializeField] private float conveyorCenterOffset = 0;

        [TabGroup("References")] [SerializeField] private List<Transform> conveyorPoints = new List<Transform>();
        [TabGroup("References")] [SerializeField] private FloatVariable progress;

        [TabGroup("State")] [SerializeField] private List<Transform> conveyorItems = new List<Transform>();

        private void OnEnable()
        {
            progress.OnChanged += OnProgressChanged;
        }
        private void OnDisable()
        {
            progress.OnChanged -= OnProgressChanged;
        }

        public void UpdateConveyorItems(List<Transform> transforms)
        {
            conveyorItems = transforms;
        }

        public void OnProgressChanged()
        {
            for (int i = 0; i < conveyorItems.Count; i++)
            {
                Transform currentTransform = conveyorItems[i];

                float itemProgress = progress.Value + conveyorCenterOffset - i;

                if (itemProgress < 0 || itemProgress >= conveyorPoints.Count)
                {
                    currentTransform.gameObject.SetActive(false);
                    continue;
                }

                currentTransform.gameObject.SetActive(true);

                int floor = (int)Math.Floor(itemProgress);
                int ceiling = floor + 1;
                float lerpProgress = itemProgress - floor;

                if(floor >= conveyorPoints.Count || floor < 0 || ceiling >= conveyorPoints.Count || ceiling < 0)
                {
                    currentTransform.localScale = Vector3.zero;
                    continue;
                }

                Transform lastTransform = conveyorPoints[floor];
                Transform nextTransform = conveyorPoints[ceiling];

                currentTransform.position = Vector3.Lerp(lastTransform.position, nextTransform.position, lerpProgress);
                currentTransform.localScale = Vector3.Lerp(lastTransform.localScale, nextTransform.localScale, lerpProgress);
                currentTransform.rotation = Quaternion.Slerp(lastTransform.rotation, nextTransform.rotation, lerpProgress);
            }
        }
    }
}
