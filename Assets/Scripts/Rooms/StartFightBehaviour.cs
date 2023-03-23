using System;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Rooms
{
    public class StartFightBehaviour : MonoBehaviour
    {
        public FloatRangeVariable chargeAmount;

        public UnityEvent onStartSuccess;
        public UnityEvent onNoCharge;
        public UnityEvent onNoConnection;

        public void TryStartFight()
        {
            if(CanFight())
            {
                StartFight();
            }
        }

        private void StartFight()
        {
            SubtractCharge();
            onStartSuccess?.Invoke();
        }

        public bool CanFight()
        {
            if (!CheckConnection())
            {
                onNoConnection?.Invoke();
                return false;
            }
            if(chargeAmount.Value.Value < 1)
            {
                onNoCharge?.Invoke();
                return false;
            }
            return true;
        }

        public bool CheckConnection()
        {
            return true;
        }
        public void SubtractCharge()
        {
            chargeAmount.Value.Value--;
            chargeAmount.Save();
        }
    }
}
