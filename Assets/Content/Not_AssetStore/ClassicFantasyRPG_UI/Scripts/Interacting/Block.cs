using UnityEngine;

namespace Interacting
{
    public class Block : MonoBehaviour
    {
        private static readonly int AnimatorStringBlock = Animator.StringToHash("Block");
        public Animator animator;
        public MovableKinematic movableKinematic;
        public bool Blocking { get; private set; }

        public void BeginBlock()
        {
            Blocking = true;
            if (movableKinematic == null) return;
            movableKinematic.MoveAllowed = false;
        }

        public void ReleaseBlock()
        {
            Blocking = false;
            if (movableKinematic == null) return;
            movableKinematic.MoveAllowed = true;
        }

        private void FixedUpdate()
        {
            animator.SetBool(AnimatorStringBlock, Blocking);
        }
    }
}