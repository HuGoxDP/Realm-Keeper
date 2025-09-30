using UnityEngine;
using UnityEngine.AI;

namespace Architecture.Units.Components
{
    public class MovementComponent : IMovementComponent
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly MovementSettings _settings;
        private bool _isEnabled;

        public bool IsMoving => _navMeshAgent.hasPath
                                && _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance;

        public float RemainingDistance => _navMeshAgent.remainingDistance;
        public bool HasPath => _navMeshAgent.hasPath;
        public Vector3 Velocity => _navMeshAgent.velocity;

        public MovementComponent(NavMeshAgent navMeshAgent, MovementSettings settings)
        {
            _navMeshAgent = navMeshAgent;
            _settings = settings;

            SetupNavMeshAgent();
        }

        public void Enable()
        {
            _isEnabled = true;
            _navMeshAgent.enabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
            StopMoving();
            _navMeshAgent.enabled = false;
        }

        public void Dispose()
        {
            if (_navMeshAgent != null)
            {
                StopMoving();
                _navMeshAgent.enabled = false;
            }
        }

        /// <summary>
        /// Start moving to the position.
        /// </summary>
        public void StartMoving(Vector3 destination)
        {
            if (!_navMeshAgent.isActiveAndEnabled || !_isEnabled)
                return;
            
            _navMeshAgent.SetDestination(destination);
        }

        /// <summary>
        /// Stop moving.
        /// </summary>
        public void StopMoving()
        {
            if (_navMeshAgent.isActiveAndEnabled && (_isEnabled || _navMeshAgent.hasPath))
            {
                _navMeshAgent.ResetPath();
            }
        }
        
        /// <summary>
        /// Set the speed of the unit.
        /// </summary>
        /// <param name="speed">The speed of the unit.</param>
        public void SetSpeed(float speed)
        {
            if (_navMeshAgent != null)
                _navMeshAgent.speed = speed;
        }
        
        private void SetupNavMeshAgent()
        {
            _navMeshAgent.speed = _settings.moveSpeed;
            _navMeshAgent.angularSpeed = _settings.rotationSpeed;
            _navMeshAgent.stoppingDistance = _settings.stoppingDistance;
            _navMeshAgent.autoBraking = _settings.autoBraking;
        }
    }
}