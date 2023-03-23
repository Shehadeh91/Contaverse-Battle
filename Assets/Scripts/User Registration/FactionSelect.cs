using Contaquest.Server;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Data
{
    public class FactionSelect : MonoBehaviour
    {
        [SerializeField] private UnityEvent onFinished;

        public void Initialize()
        {
            if(UserManager.Instance.HasFaction())
            {
                onFinished?.Invoke();
            }
        }
        public void SelectFaction(int factionIndex)
        {
            UserManager.Instance.TrySetUserFaction(factionIndex, onFinished.Invoke);
        }
    }
}
