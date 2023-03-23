using UnityEngine;

namespace Contaquest.UI.Settings
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicSettings : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private Sprite MusicOn, MusicOff;
        private bool isMuted = false;

        private void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnDisable()
        {
            audioSource = null;
        }

        public void MuteToggle()
        {
            if (audioSource != null)
                audioSource.mute = isMuted = !isMuted;
        }

        public void UpdateIcon(UnityEngine.UI.Image img)
        {
            if (isMuted)
                img.sprite = MusicOff;
            else
                img.sprite = MusicOn;
        }
    }
}
