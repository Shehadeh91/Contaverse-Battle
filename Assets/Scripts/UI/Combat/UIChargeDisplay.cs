//using System;
//using Contaquest.Metabots.Combat;
//using UnityEngine;
//using UnityEngine.UI;

//namespace AssemblyCSharp.Assets.Scripts.Combat.UI
//{
//    public class UIChargeDisplay : MonoBehaviour
//    {
//        [SerializeField] private CombatController sourceCombatController;

//        [Header("References")]
//        [SerializeField] private Slider healthSlider;

//        protected void Start()
//        {
//            if (!sourceCombatController)
//            {
//                Destroy(this);
//                return;
//            }
//            healthSlider.maxValue = sourceCombatController.MaxStamina;
//        }

//        protected void LateUpdate()
//        {
//            healthSlider.value = sourceCombatController.CurrentStamina;
//        }
//    }
//}
