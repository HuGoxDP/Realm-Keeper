using System;
using R3;

namespace Architecture.Units
{
    public interface IHealthComponent : IComponent, IDisposable
    {
        ReactiveProperty<int> Health { get; }
        ReactiveProperty<bool> IsDead { get; }
        void TakeDamage(int damage);
        void Heal(int amount);
        void ResetHealth();
    }

    public class HealthComponent : IHealthComponent
    {
        public ReactiveProperty<int> Health { get; }
        public ReactiveProperty<bool> IsDead { get; }

        private readonly int _maxHealth;

        private bool _isEnabled;

        public HealthComponent(int maxHealth)
        {
            if (maxHealth <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHealth), "HealthComponent : maxHealth must be greater than zero");

            _maxHealth = maxHealth;

            Health = new ReactiveProperty<int>(maxHealth);
            IsDead = new ReactiveProperty<bool>(false);
        }

        ~HealthComponent() => Dispose();

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
            IsDead?.Dispose();
        }

        public void TakeDamage(int damage)
        {
            if (IsDead.Value || !_isEnabled)
                return;

            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "HealthComponent : damage must be greater than zero");

            if (Health.Value - damage <= 0)
            {
                IsDead.Value = true;
            }

            Health.Value = Math.Max(Health.Value - damage, 0);
        }

        public void Heal(int amount)
        {
            if (IsDead.Value || !_isEnabled)
                return;

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "HealthComponent : heal amount must be greater than zero");

            Health.Value = Math.Min(Health.Value + amount, _maxHealth);
        }

        public void ResetHealth()
        {
            if (IsDead.Value || !_isEnabled)
                return;

            Health.Value = _maxHealth;
        }
    }
}