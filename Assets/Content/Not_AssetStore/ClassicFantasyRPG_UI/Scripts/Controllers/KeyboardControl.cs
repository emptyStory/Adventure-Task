using Interacting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class KeyboardControl : MonoBehaviour
    {
        public KeyCode jumpButton = KeyCode.UpArrow;
        public KeyCode attackButton = KeyCode.Mouse0;
        public KeyCode blockButton = KeyCode.Mouse1;
        public KeyCode abilityButton = KeyCode.Space;
        public KeyCode potionButton = KeyCode.X;
        public IAttack attackComponent;
        public Block block;
        public HeavyAttack heavyAttack;
        public Potion potion;
        private MovableKinematic _movement;
        private bool _couldBlock;
        private bool _hasAnAbility;

        private void Start()
        {
            _movement = GetComponent<MovableKinematic>();
            _couldBlock = block != null;
            _hasAnAbility = heavyAttack != null;
        }

        private void Update()
        {
            _movement.JumpOccured = Input.GetKeyDown(jumpButton);
            _movement.JumpStopped = Input.GetKeyUp(jumpButton);
            _movement.HorizontalMovement = Input.GetAxisRaw("Horizontal");
            if (IsKeyDown(attackButton))
            {
                attackComponent.Attack();
            }
            else if (IsKeyUp(attackButton))
            {
                attackComponent.CancelAttack();
            }

            if (_couldBlock && IsKeyDown(blockButton))
            {
                block.BeginBlock();
            }
            else if (_couldBlock && IsKeyUp(blockButton))
            {
                block.ReleaseBlock();
            }
            
            if (_hasAnAbility && IsKeyDown(abilityButton))
            {
                heavyAttack.Perform();
            }
//            else if (_hasAnAbility && IsKeyUp(abilityButton))
//            {
//                heavyAttack.Cancel();
//            }
            
            if (IsKeyDown(potionButton))
            {
                potion.Use();
            }
        }

        private bool IsKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key) && !EventSystem.current.IsPointerOverGameObject();
        }
        
        private bool IsKeyUp(KeyCode key)
        {
            return Input.GetKeyUp(key) && !EventSystem.current.IsPointerOverGameObject();
        }

        private void OnDisable()
        {
            _movement.JumpStopped = true;
            _movement.HorizontalMovement = 0;
        }
    }
}