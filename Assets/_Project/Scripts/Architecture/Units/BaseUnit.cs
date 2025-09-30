using Architecture.Units.Components;
using Architecture.Units.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Architecture.Units
{
    public class BaseUnit : MonoBehaviour
    {
        [Header("Health Settings")] 
        [SerializeField] private int _maxHealth = 15;

        [Header("Movement Settings")] 
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private MovementSettings _movementSettings;

        private IMovementComponent _movementComponent;
        private IHealthComponent _healthComponent;
        
        private MovementData _movementData;


        private void Awake()
        {
            _movementData = new MovementData();
            
            _movementComponent = new MovementComponent(_navMeshAgent, _movementSettings);
            _healthComponent = new HealthComponent(_maxHealth);
            
            
            var context = new UnitContext(this, _movementData, _healthComponent, _movementComponent);
            var unitStateMachine = GetComponent<UnitStateMachine>();
            unitStateMachine.Initialize(context);
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