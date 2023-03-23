using System.Collections.Generic;
using UnityEngine;

namespace Contaquest.Mobile.Input
{
    public class ButtonGroup : MonoBehaviour
    {
        [SerializeField] private List<ButtonInteractable> buttons = new List<ButtonInteractable>();

        private void Start()
        {
            RegisterButtons();
        }
        public void RegisterButtons()
        {
            foreach (ButtonInteractable button in buttons)
            {
                button.onPressed.AddListener((isActive) => OnButtonClicked(isActive, button));
            }
        }

        public void OnButtonClicked(bool isActive, ButtonInteractable button)
        {
            if (!isActive)
                return;

            foreach(ButtonInteractable button1 in buttons)
            {
                if(button1 != button)
                    button1.DeactivateButton();
            }
        }
    }
}
