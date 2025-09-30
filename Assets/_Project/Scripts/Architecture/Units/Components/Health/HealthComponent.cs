using System;
using UnityEngine;

namespace Architecture.Units.Components
{
    public class HealthComponent : IHealthComponent
    { 
        public int CurrentHealth { get; private set;}
        public int MaxHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0;
        
        private bool _isEnabled;
        
        public HealthComponent(int maxHealth)
        {
            if (maxHealth <= 0)
                throw new ArgumentException("MaxHealth must be positive", nameof(maxHealth));
            
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }
        
        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }
        
        public void TakeDamage(int amount)
        {
            if (IsDead || amount <= 0 || !_isEnabled) return;
            
            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        }

        public void Heal(int amount)
        {
            if (IsDead || amount <= 0 || !_isEnabled) return;
            
            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
        }

        public void Kill()
        {
            CurrentHealth = 0;
        }

        public void Reset()
        {
            CurrentHealth = MaxHealth;
        }
    }
}