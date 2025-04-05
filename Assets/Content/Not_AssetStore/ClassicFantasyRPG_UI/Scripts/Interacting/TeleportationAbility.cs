using Events;
using UnityEngine;

namespace Interacting
{
    public class TeleportationAbility : MonoBehaviour
    {
        private static readonly int Blink = Animator.StringToHash("Blink");
        public float cooldown = 15f;
        public Transform teleportationPoint;
        public AudioEventManager audioEventManager;

        private float _readyTime;

        public void Teleport(Transform target)
        {
            if (!IsReady()) return;

            _readyTime = Time.time + cooldown;
            target.position = teleportationPoint.position;
            var animator = target.GetComponent<Durable>().animator;
            if (animator == null) return;
            animator.SetTrigger(Blink);
            if (audioEventManager)
            {
                audioEventManager.PlaySkill();
            }
        }

        public bool IsReady()
        {
            return Time.time > _readyTime;
        }
    }
}