using UnityEngine;
using System.Collections.Generic;

namespace Contaquest.Metaverse.Robot.Customization
{
    public class CustomizationMovement : MonoBehaviour
    {
        public static CustomizationMovement instance;
        public Cinemachine.CinemachineVirtualCamera MovementCam;
        
        [SerializeField]
        private RuntimeAnimatorController MovementController, HoverController;
        [SerializeField]
        private List<GameObject> CollidersForHover, CollidersForMovement, UIForHover;

        private Animator anim;
        private bool movement = false;
        private float VelocityX, VelocityZ, speed = 2f, rotationSpeed = 100f;
        private int xHash, zHash;
        private int movementPriority = 110, hoverPriority = -2;

        private void Awake()
        {
            anim = GetComponent<RobotController>().animator;
            if(anim == null)
            {
                Debug.LogError("Please assign appropiate RobotController component in order for this script to functoin properly.");
            }
            if(MovementController == null || HoverController == null)
            {
                Debug.LogError("Please assign appropiate AnimationController object to the Movement Controller field of this component for it to perform properly.");
            }
            xHash = Animator.StringToHash("VelocityX");
            zHash = Animator.StringToHash("VelocityZ");
            //HoverState(transform);
            MovementState();
        }

        private void OnEnable()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        private void OnDisable()
        {            
            instance = null;
        }

        public void HoverState(Transform snapPostion)
        {
            if (anim != null)
                anim.runtimeAnimatorController = HoverController;
            else return;
            movement = false;
            MovementCam.Priority = hoverPriority;
            transform.position = snapPostion.position;
            transform.rotation = Quaternion.identity;
            CollidersForHover.ForEach(x => x.SetActive(true));
            CollidersForMovement.ForEach(x => x.SetActive(false));
            UIForHover.ForEach(x => x.SetActive(true));
        }

        public void MovementState()
        {
            if(anim != null)
                anim.runtimeAnimatorController = MovementController;
            else return;
            MovementCam.Priority = movementPriority;
            movement = true;
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            CollidersForHover.ForEach(x => x.SetActive(false));
            UIForHover.ForEach(x => x.SetActive(false));
            CollidersForMovement.ForEach(x => x.SetActive(true));
        }

        public void Move()
        {
            float HorizontalInput = Input.GetAxis("Horizontal");
            float VerticalInput = Input.GetAxis("Vertical");
            float angle = Mathf.Atan2(VerticalInput, HorizontalInput);
            //Vector3 dir = new Vector3(HorizontalInput, 0f, VerticalInput);
            Vector3 dir = new Vector3(0f, 0f, VerticalInput);
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime * HorizontalInput);

            anim.SetFloat(xHash, HorizontalInput);
            anim.SetFloat(zHash, VerticalInput);
            transform.position += transform.forward * VerticalInput * speed * Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (movement)
                Move();
        }
    }
}
