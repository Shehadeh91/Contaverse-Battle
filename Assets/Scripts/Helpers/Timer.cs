using System;
using UnityEngine;

namespace Contaquest.Metaverse.Combat
{
    [System.Serializable]
    public class Timer
    {
        [SerializeField] private FloatRangeVariable timerValue;
        [SerializeField] private FloatReference timerSpeed;

        public void ResetTimer()
        {
            timerValue.Value.Value = timerValue.defaultValue.Value;
        }
        public void UpdateTimer()
        {
            float newTime = timerValue.Value.Value + (Time.deltaTime * timerSpeed.Value);
            timerValue.Value.SetValueClamped(newTime);
        }
    }
}
