using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class BehaviourData
    {
        public string behaviourName;
        public string behaviourAddress;
        [ListDrawerSettings(Expanded = true)]
        public float[] behaviourValues;
    }
}