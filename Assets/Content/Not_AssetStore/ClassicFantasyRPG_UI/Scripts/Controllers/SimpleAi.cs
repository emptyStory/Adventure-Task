using System;
using Interacting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class SimpleAi : MonoBehaviour
    {
        private enum State
        {
            Roam,
            Idle,
            Chase,
            Attack
        }
    
        public Transform scanPoint;
        public Transform leavePoint;
        public Transform backPoint;
        public LayerMask whoToChase;
        public MovableKinematic movableKinematicComponent;
        public IAttack attackComponent;
        public Vector2 roamRange = new Vector2(-6f, 8f);
        public Vector2 idleTimeRange = new Vector2(1.5f, 4f);
        public float attackRange = 2f;

        private float _startPosition;
        private State _currentState;
        private float _roamPosition;
        private float _idleTime;
        private Transform _spottedTarget;
        private Durable _targetDurable;

        private void Start()
        {
            _startPosition = transform.position.x;
            _roamPosition = _startPosition + Random.Range(roamRange.x, roamRange.y);
            _currentState = State.Roam;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case State.Roam:
                    DoRoam();
                    break;
                case State.Idle:
                    DoIdle();
                    break;
                case State.Chase:
                    DoChase();
                    break;
                case State.Attack:
                    DoAttack();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FixedUpdate()
        {
            if (_currentState == State.Chase)
            {
                var overlappedCollider = Physics2D.OverlapArea(backPoint.position, leavePoint.position, whoToChase);
                if (!ReferenceEquals(overlappedCollider, null)) return;
                ResetState();
            }
            else if (_currentState != State.Attack)
            {
                var overlappedCollider = Physics2D.OverlapArea(backPoint.position, scanPoint.position, whoToChase);
                if (ReferenceEquals(overlappedCollider, null)) return;
                _spottedTarget = overlappedCollider.transform;
                _targetDurable = _spottedTarget.GetComponent<Durable>();
                _currentState = State.Chase;
            
            }
        }

        private void ResetState()
        {
            _currentState = State.Roam;
            _roamPosition = _startPosition;
            _spottedTarget = null;
            _targetDurable = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var scanPointPosition = scanPoint.position;
            Gizmos.DrawLine(backPoint.position, scanPointPosition);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(scanPointPosition, leavePoint.position);
        }

        private void DoRoam()
        {
            var distance = _roamPosition - transform.position.x;
            if (Math.Abs(distance) < 1f)
            {
                _idleTime = Time.time + Random.Range(idleTimeRange.x, idleTimeRange.y);
                _currentState = State.Idle;
                movableKinematicComponent.HorizontalMovement = 0;
                return;
            }

            movableKinematicComponent.HorizontalMovement = distance / Math.Abs(distance);
        }

        private void DoIdle()
        {
            if (Time.time < _idleTime) return;
            _roamPosition = _startPosition + Random.Range(roamRange.x, roamRange.y);
            _currentState = State.Roam;
        }

        private void DoChase()
        {
            if (_targetDurable.Dead)
            {
                ResetState();
                return;
            }
        
            var distance = _spottedTarget.position.x - transform.position.x;
            var distanceAbs = Math.Abs(distance);
            if (distanceAbs < attackRange)
            {
                movableKinematicComponent.HorizontalMovement = 0;
                _currentState = State.Attack;
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
                ResetState();
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
            }
        }
    }
}
