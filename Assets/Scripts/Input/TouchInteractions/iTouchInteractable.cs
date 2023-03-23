using System;
namespace Contaquest.Mobile.Input
{
    public interface iTouchInteractable
    {
        void EnableInteractability();
        void DisableInteractability();

        void StartTouchInteraction(TouchInputAction touchInputAction);

        void EndTouchInteraction(TouchInputAction touchInputAction);
    }
}
