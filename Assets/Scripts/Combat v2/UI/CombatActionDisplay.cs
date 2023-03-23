using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using TMPro;

namespace Contaquest.Metaverse.Combat2
{
    public class CombatActionDisplay : MonoBehaviour
    {
        [TabGroup("References")] [SerializeField] private TextMeshProUGUI actionName;
        [TabGroup("References")] [SerializeField] private TextMeshProUGUI energyCost;
        [TabGroup("References")] [SerializeField] private Image actionImage;

        [TabGroup("References")] [SerializeField] private GameObject damageUI;
        [TabGroup("References")] [SerializeField] private GameObject knockbackUI;
        [TabGroup("References")] [SerializeField] private TextMeshProUGUI damageAmount;
        [TabGroup("References")] [SerializeField] private TextMeshProUGUI knockbackAmount;
        // [TabGroup("References")] [SerializeField] private Transform blockGraphic;
        // [TabGroup("References")] [SerializeField] private Transform dodgeGraphic;

        [TabGroup("Events")] [SerializeField] private UnityEvent onInitialized;

        [TabGroup("State")] [ShowInInspector, ReadOnly] private CombatActionContext combatActionContext;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private CombatUI combatUI;
        

        public void InitializeUI(CombatActionContext combatActionContext, CombatUI combatUI)
        {
            this.combatActionContext = combatActionContext;
            this.combatUI = combatUI;

            CombatAction combatAction = combatActionContext.CombatAction;

            actionName.text = combatAction.actionName;
            energyCost.text = combatAction.energyCost.ToString();
            actionImage.sprite = combatAction.actionImage;

            if(combatAction.showDamageAmountUI)
                damageAmount.text = combatAction.GetTotalDamage(combatActionContext).ToString();
            else
                damageUI.SetActive(false);

            if(combatAction.showKnockBackAmountUI)
                knockbackAmount.text = combatAction.GetTotalKnockback(combatActionContext).ToString();
            else
                knockbackUI.SetActive(false);

            onInitialized?.Invoke();
        }
        public void OnDropped(UIDropSlot slot)
        {
            // slot.GetComponent<>
        }

        public void UseCombatAction()
        {
            combatUI?.UseCombatAction(combatActionContext);
        }
    }
}