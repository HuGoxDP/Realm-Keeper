using System;
using R3;
using UnityEngine;
using UnityEngine.AI;

namespace Architecture.Units
{
    public interface IMovementComponent : IComponent, IDisposable
    {
        void MoveTo(Vector3 position);
        void MoveTo(Transform target);
        void Stop();
    }
    
    public class MovementComponent : IMovementComponent
    {
        private readonly NavMeshAgent _navMeshAgent;
        private IDisposable _movementCheckDisposable;
        private bool _isEnabled;
        public MovementComponent(NavMeshAgent navMeshAgent, MovementSettings movementSettings)
        {
            if (navMeshAgent == null)
                throw new ArgumentNullException(nameof(navMeshAgent), "MovementComponent : NavMeshAgent cannot be null");
            
            _navMeshAgent = navMeshAgent;
            
            SetupNavMeshAgent(movementSettings);
        }
        
        ~MovementComponent() => Dispose();
        
        public void Enable()
        {
            _isEnabled = true;
            _navMeshAgent.enabled = true;

            _movementCheckDisposable = Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Where(_ => _navMeshAgent.hasPath)
                .Subscribe(_ => CheckDestination());
        }
        
        public void Disable()
        {
            _isEnabled = false;
            Dispose();
        }
        
        public void Dispose()
        {
            _movementCheckDisposable?.Dispose();
            
            if (_navMeshAgent != null)
            {
                Stop();
                _navMeshAgent.enabled = false;
            }
        }
        
        public void MoveTo(Vector3 position)
        {
            if (!_navMeshAgent.isActiveAndEnabled || !_isEnabled)
                return;
            
            _navMeshAgent.SetDestination(position);
        }

        public void MoveTo(Transform target)
        {
            if(target != null)
                MoveTo(target.position);
        }

        public void Stop()
        {
            if (_navMeshAgent.isActiveAndEnabled && _isEnabled)
            {
                _navMeshAgent.ResetPath();
            }
        }
        private void CheckDestination()
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                Stop();
            }
        }
        
        private void SetupNavMeshAgent(MovementSettings movementSettings)
        {
            if (_navMeshAgent == null)
                throw new ArgumentNullException(nameof(_navMeshAgent), "MovementComponent : NavMeshAgent cannot be null");
            
            if (movementSettings.moveSpeed <= 0)
                throw new ArgumentOutOfRangeException(nameof(movementSettings.moveSpeed), "MovementComponent : moveSpeed must be greater than zero");
            
            if (movementSettings.rotationSpeed <= 0)
                throw new ArgumentOutOfRangeException(nameof(movementSettings.rotationSpeed), "MovementComponent : rotationSpeed must be greater than zero");
            
            if (movementSettings.stoppingDistance < 0)
                throw new ArgumentOutOfRangeException(nameof(movementSettings.stoppingDistance), "MovementComponent : stoppingDistance cannot be negative");
            
            _navMeshAgent.speed = movementSettings.moveSpeed;
            _navMeshAgent.angularSpeed = movementSettings.rotationSpeed;
            _navMeshAgent.stoppingDistance = movementSettings.stoppingDistance;
            _navMeshAgent.autoBraking = movementSettings.autoBraking;
        }
    }
}