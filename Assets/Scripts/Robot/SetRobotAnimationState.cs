using UnityEngine;

namespace Contaquest.Metaverse.Rooms
{
    public class SetRobotAnimationState : MonoBehaviour
    {
        [SerializeField] private string stateName;
        [SerializeField] private float transitionDuration = 0.25f;
        private int stateHash;

        private void Start()
        {
            stateHash = Animator.StringToHash(stateName);
        }

        public void SetState()
        {
            RoomManager.Instance.robotController?.animator.CrossFadeInFixedTime(stateHash, transitionDuration);
        }
        public void SetPosition(Vector3 position)
        {
            RoomManager.Instance.robotController.transform.position = position;
        }
        public void SetScale(float scale)
        {
            RoomManager.Instance.robotController.transform.localScale = new Vector3(scale, scale, scale);
        }
        public void SetRotation(float rotation)
        {
            RoomManager.Instance.robotController.transform.localEulerAngles = new Vector3(rotation, 0f, 0f);
        }
    }
}
