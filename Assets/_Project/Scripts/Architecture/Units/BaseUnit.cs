using System;
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
            _movementComponent.Dispose();
            _healthComponent.Dispose();
        }

        public void Heal(int amount) => _healthComponent?.Heal(amount);
        
        public void TakeDamage(int damage) => _healthComponent?.TakeDamage(damage);
        public void MoveTo(Vector3 position) => _movementComponent?.MoveTo(position);

        public void Stop() => _movementComponent?.Stop();
    }
}