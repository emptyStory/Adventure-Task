using System;
using Interacting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class BossAi : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Chase,
            Attack,
            Block
        }
    
        public Transform scanPoint;
        public Transform leavePoint;
        public Transform backPoint;
        public Transform blockPoint;
        public LayerMask whoToChase;
        public MovableKinematic movableKinematicComponent;
        public IAttack attackComponent;
        public Block block;
        public float attackRange = 50f;
        public Vector2 attackDurationRange = new Vector2(1.9f, 3f);
        public Vector2 blockDurationRange = new Vector2(4.5f, 6.5f);
        public TeleportationAbility teleportationAbility;
        public Durable myDurable;
        public float whenToTeleportHp = 0.5f;

        private float _startPosition;
        private State _currentState;
        private Transform _spottedTarget;
        private Durable _targetDurable;
        private float _attackDueTo;
        private float _blockDueTo;
        private bool _targetInBlockRange;

        private void Start()
        {
            _startPosition = transform.position.x;
            _currentState = State.Idle;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case State.Idle:
                    DoIdle();
                    break;
                case State.Chase:
                    DoChase();
                    break;
                case State.Attack:
                    DoAttack();
                    break;
                case State.Block:
                    DoBlock();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FixedUpdate()
        {
            _targetInBlockRange = (_currentState == State.Attack || _currentState == State.Block) &&
                                  !ReferenceEquals(
                                      Physics2D.OverlapArea(backPoint.position, blockPoint.position, whoToChase), null);
            if (_currentState == State.Chase)
            {
                var overlappedCollider = Physics2D.OverlapArea(backPoint.position, leavePoint.position, whoToChase);
                if (!ReferenceEquals(overlappedCollider, null)) return;
                _currentState = State.Idle;
                _spottedTarget = null;
            }
            else if (_currentState != State.Attack && _currentState != State.Block)
            {
                var overlappedCollider = Physics2D.OverlapArea(backPoint.position, scanPoint.position, whoToChase);
                if (ReferenceEquals(overlappedCollider, null)) return;
                _spottedTarget = overlappedCollider.transform;
                _targetDurable = _spottedTarget.GetComponent<Durable>();
                _currentState = State.Chase;
            } else if (_targetInBlockRange && myDurable.CurrentHealth / myDurable.maxHealth < whenToTeleportHp && teleportationAbility.IsReady())
            {
                teleportationAbility.Teleport(_spottedTarget);
            }
        }

        private void OnDrawGizmos()
        {
            var scanPointPosition = scanPoint.position;
            var blockPointPosition = blockPoint.position;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(backPoint.position, blockPointPosition);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(blockPointPosition, scanPointPosition);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(scanPointPosition, leavePoint.position);
        }

        private void DoIdle()
        {
            var distance = _startPosition - transform.position.x;
            if (Math.Abs(distance) > 1f)
            {
                movableKinematicComponent.HorizontalMovement = distance / Math.Abs(distance);
            }
            else
            {
                movableKinematicComponent.HorizontalMovement = 0;
                movableKinematicComponent.ChangeFacing(Vector2.left);
            }
        }

        private void DoChase()
        {
            if (_targetDurable.Dead)
            {
                _currentState = State.Idle;
                _spottedTarget = null;
                _targetDurable = null;
                return;
            }
            var distance = _spottedTarget.position.x - transform.position.x;
            var distanceAbs = Math.Abs(distance);
            if (distanceAbs < attackRange)
            {
                movableKinematicComponent.HorizontalMovement = 0;
                StartAttack();
            }
            else
            {
                movableKinematicComponent.HorizontalMovement = distance / distanceAbs;
            }
        }

        private void DoAttack()
        {
            if (_targetDurable.Dead)
            {
                _currentState = State.Idle;
                _spottedTarget = null;
                _targetDurable = null;
                attackComponent.CancelAttack();
                return;
            }
            var distance = Math.Abs(_spottedTarget.position.x - transform.position.x);
            if (distance > attackRange)
            {
                _currentState = State.Chase;
                attackComponent.CancelAttack();
            }
            else
            {
                attackComponent.Attack();
                if (Time.time < _attackDueTo) return;
                if (_targetInBlockRange)
                {
                    _currentState = State.Block;
                    _blockDueTo = Time.time + Random.Range(blockDurationRange.x, blockDurationRange.y);
                }
                else
                {
                    StartAttack();
                }
            }
        }
        
        private void StartAttack()
        {
            _currentState = State.Attack;
            _attackDueTo = Time.time + Random.Range(attackDurationRange.x, attackDurationRange.y);
        }

        private void DoBlock()
        {
            if (Time.time > _blockDueTo)
            {
                block.ReleaseBlock();
                StartAttack();
            }
            else
            {
                block.BeginBlock();
            }
        }
    }
}
