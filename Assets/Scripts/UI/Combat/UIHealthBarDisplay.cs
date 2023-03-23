//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//namespace Contaquest.Metabots.Combat.UI
//{
//    [DefaultExecutionOrder(10)]
//    public class UIHealthBarDisplay : MonoBehaviour
//    {
//        [SerializeField] private CombatController sourceCombatController;
        
//        [Header ("References")]
//        [SerializeField] private Slider healthSlider;
//        [SerializeField] private TextMeshProUGUI text;

//        protected void Start()
//        {
//            if (!sourceCombatController)
//            {
//                Destroy(this);
//                return;
//            }
//            healthSlider.maxValue = sourceCombatController.MaxHealth;
//            healthSlider.value = sourceCombatController.CurrentHealth;
//            text.text = sourceCombatController.CurrentHealth.ToString();
//        }

//        public void UpdateHealth(float newHealth)
//        {
//            healthSlider.value = newHealth;
//            text.text = newHealth.ToString();
//        }
//    }
//}