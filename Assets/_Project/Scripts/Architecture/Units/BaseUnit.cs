using System;
using Architecture.Units.Components;
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

        private void Awake()
        {
            _movementComponent = new MovementComponent(_navMeshAgent, _movementSettings);
            _healthComponent = new HealthComponent(_maxHealth);
            
            
            if (_movementComponent == null)
                throw new ArgumentNullException(nameof(_movementComponent), "BaseUnit : MovementComponent cannot be null");
            
            if (_healthComponent == null)
                throw new ArgumentNullException(nameof(_healthComponent), "BaseUnit : HealthComponent cannot be null");
            
            var context = new UnitContext(this, _healthComponent, _movementComponent);
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
            _healthComponent?.Dispose();
        }

        
        public void ApplyDamage(int damage) => _healthComponent?.ApplyDamage(damage);
        public void MoveTo(Vector3 position) => _movementComponent?.MoveTo(position);

        public void Stop() => _movementComponent?.Stop();
    }
}