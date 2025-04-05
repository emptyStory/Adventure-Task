using UnityEngine;

namespace Events
{
    public class AudioEventManager : MonoBehaviour
    {
        public AudioSource audioSource;
        [Range(0, 1)]
        public float footstepsVolume = 1f;
        public AudioClip footstepsClip;
        [Range(0, 1)]
        public float attackVolume = 1f;
        public AudioClip attackClip;
        [Range(0, 1)]
        public float skillVolume = 1f;
        public AudioClip skillClip;
        [Range(0, 1)]
        public float dieVolume = 1f;
        public AudioClip dieClip;
        [Range(0, 1)]
        public float hurtVolume = 1f;
        public AudioClip hurtClip;
        [Range(0, 1)]
        public float blockVolume = 1f;
        public AudioClip blockClip;
        [Range(0, 1)]
        public float potionVolume = 1f;
        public AudioClip potionClip;
        

        public void PlayFootsteps()
        {
            audioSource.PlayOneShot(footstepsClip, footstepsVolume);
        }
        
        public void PlayAttack()
        {
            audioSource.PlayOneShot(attackClip, attackVolume);
        }
        
        public void PlaySkill()
        {
            audioSource.PlayOneShot(skillClip, skillVolume);
        }
        
        public void PlayDie()
        {
            audioSource.PlayOneShot(dieClip, dieVolume);
        }
        
        public void PlayHurt()
        {
            audioSource.PlayOneShot(hurtClip, hurtVolume);
        }
        
        public void PlayBlock()
        {
            audioSource.PlayOneShot(blockClip, blockVolume);
        }
        
        public void PlayPotion()
        {
            audioSource.PlayOneShot(potionClip, potionVolume);
        }
    }
}