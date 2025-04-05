using System.Collections.Generic;
using UnityEngine;

namespace Interacting
{
    public class MovableKinematic : MonoBehaviour
    {
        private const float MinMoveDistance = 0.001f;
        private const float ShellRadius = 0.01f;
        private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
        private readonly List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D> (16);
        private static readonly int RunningAnimationString = Animator.StringToHash("Running");
        private static readonly int OnGroundAnimationString = Animator.StringToHash("OnGround");
        private static readonly int JumpAnimationString = Animator.StringToHash("Jump");
    
        public float minGroundNormalY = .65f;
        public float gravityModifier = 4f;
        public float jumpTakeOffSpeed = 16;
        public float runSpeed = 9;
        public Animator animator;
        public Transform transformToFlip;

        public float HorizontalMovement { private get; set; }
        public bool JumpOccured { private get; set; }
        public bool JumpStopped { private get; set; }
        public bool MoveAllowed { private get; set; }

        private bool _grounded;
        private Vector2 _groundNormal;
        private Rigidbody2D _rb2D;
        private Vector2 _actualVelocity;
        private ContactFilter2D _contactFilter;

        public void ChangeFacing(Vector2 direction)
        {
            var localScale = transformToFlip.localScale;
            localScale.x = direction.x > 0 ? Mathf.Abs(localScale.x) : - Mathf.Abs(localScale.x);
            transformToFlip.localScale = localScale;
        }

        private void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D> ();
        }

        private void Start () 
        {
            _contactFilter.useTriggers = false;
            _contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
            _contactFilter.useLayerMask = true;
            MoveAllowed = true;
        }

        private void FixedUpdate()
        {
            var movement = MoveAllowed ? HorizontalMovement : 0f;

            animator.SetBool(RunningAnimationString, Mathf.Abs(movement) > 0f);
            if (JumpOccured && _grounded && MoveAllowed)
            {
                animator.SetTrigger(JumpAnimationString);
                _actualVelocity.y = jumpTakeOffSpeed;
                JumpOccured = false;
            }

            if (JumpStopped)
            {
                JumpStopped = false;
                if (_actualVelocity.y > 0)
                {
                    _actualVelocity.y = _actualVelocity.y * 0.5f;
                }
            }
        
            var localScale = transformToFlip.localScale;
            if (movement > 0f)
            {
                localScale.x = Mathf.Abs(localScale.x);
                transformToFlip.localScale = localScale;
            }
            else if (movement < 0f)
            {
                localScale.x = - Mathf.Abs(localScale.x);
                transformToFlip.localScale = localScale;
            }

            _actualVelocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            _actualVelocity.x = movement * runSpeed;

            _grounded = false;

            var deltaPosition = _actualVelocity * Time.deltaTime;

            var moveAlongGround = new Vector2 (_groundNormal.y, -_groundNormal.x);

            var move = moveAlongGround * deltaPosition.x;

            Movement (move, false);

            move = Vector2.up * deltaPosition.y;

            Movement (move, true);
            animator.SetBool(OnGroundAnimationString, _grounded);
        }

        private void Movement(Vector2 move, bool yMovement)
        {
            var distance = move.magnitude;

            if (distance > MinMoveDistance) 
            {
                var count = _rb2D.Cast (move, _contactFilter, _hitBuffer, distance + ShellRadius);
                _hitBufferList.Clear ();
                for (var i = 0; i < count; i++) {
                    _hitBufferList.Add (_hitBuffer [i]);
                }

                foreach (var hit in _hitBufferList)
                {
                    var currentNormal = hit.normal;
                    if (currentNormal.y > minGroundNormalY) 
                    {
                        _grounded = true;
                        if (yMovement) 
                        {
                            _groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }

                    var projection = Vector2.Dot (_actualVelocity, currentNormal);
                    if (projection < 0) 
                    {
                        _actualVelocity = _actualVelocity - projection * currentNormal;
                    }

                    var modifiedDistance = hit.distance - ShellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }


            }

            _rb2D.position = _rb2D.position + move.normalized * distance;
        }
    }
}