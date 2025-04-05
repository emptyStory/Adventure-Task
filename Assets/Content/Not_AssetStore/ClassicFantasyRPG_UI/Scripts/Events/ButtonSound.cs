using UnityEngine;

namespace Events
{
    public class ButtonSound : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip clickSound;

        public void PlayButtonClick()
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}