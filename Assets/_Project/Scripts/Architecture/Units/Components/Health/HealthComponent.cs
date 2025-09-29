using System;
using R3;

namespace Architecture.Units.Components
{
    public class HealthComponent : IHealthComponent
    {
        public ReactiveProperty<int> Health { get; }
        public bool IsDead { get; private set;}

        private readonly int _maxHealth;
        private bool _isEnabled;

        public HealthComponent(int maxHealth)
        {
            if (maxHealth <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHealth), "HealthComponent : maxHealth must be greater than zero");

            _maxHealth = maxHealth;

            Health = new ReactiveProperty<int>(maxHealth);
        }
        
        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        public void Dispose()
        {
            Health?.Dispose();
        }

        public void ApplyDamage(int amount)
        {
            if (IsDead || !_isEnabled)
                return;

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "HealthComponent : damage must be greater than zero");

            if (Health.Value - amount <= 0)
            {
                IsDead = true;
            }

            Health.Value = Math.Max(Health.Value - amount, 0);
        }
    }
}