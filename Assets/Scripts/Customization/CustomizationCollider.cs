using UnityEngine;
namespace Contaquest.Metaverse.Robot.Customization
{
    public class CustomizationCollider : MonoBehaviour
    {
        [SerializeField]
        private Transform SnapPosition;
        private bool activated = true;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") && activated)
            {
                CustomizationMovement.instance.HoverState(SnapPosition);
                activated = false;      
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !activated)
            {                
                activated = true;
            }
        }
    }
}