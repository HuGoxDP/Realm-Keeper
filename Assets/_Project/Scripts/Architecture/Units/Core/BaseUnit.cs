using Architecture.Selection;
using Architecture.Units.Components;
using Architecture.Units.Configs;
using Architecture.Units.Data;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.AI;

namespace Architecture.Units.Core
{
    public class BaseUnit : MonoBehaviour
    {
        [SerializeField] private HealthConfig _healthConfig;
        [SerializeField] private MovementConfig _movementSettings;

        [Header("Components")]
        [SerializeField] private UnitStateMachine _stateMachine;
        
        private NavMeshAgent _navMeshAgent;
        private IMovementComponent _movementComponent;
        private IHealthComponent _healthComponent;
        
        private MovementData _movementData;
        
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            _movementData = new MovementData();
            _movementComponent = new MovementComponent(_navMeshAgent, _movementSettings);
            _healthComponent = new HealthComponent(_healthConfig);
            
            var context = new UnitContext(this, _movementData, _healthComponent, _movementComponent);
            _stateMachine.Initialize(context);
        }

        private void OnEnable()
        {
            _movementComponent.Enable();
            _healthComponent.Enable();
        }

        private void OnDisable()
        {
            _movementComponent.Disable();
            _healthComponent.Disable();
        }

        private void OnDestroy()
        {
            _movementComponent?.Dispose();
            _healthComponent?.Dispose();
        }

        public void MoveTo(Vector3 position) => _movementData.SetTarget(position);
        public void Chase(Transform target) => _movementData.SetTarget(target.position);
        public void Stop() => _movementData.Clear();
        public void ApplyDamage(int damage) => _healthComponent?.TakeDamage(damage);
        public void Heal(int amount) => _healthComponent?.Heal(amount);
        public void Kill() => _healthComponent?.Kill();
        public void Reset() => _healthComponent?.Reset();

    }
}