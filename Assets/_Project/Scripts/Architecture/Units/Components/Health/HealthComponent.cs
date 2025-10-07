using System;
using Architecture.Units.Configs;
using R3;
using UnityEngine;

namespace Architecture.Units.Components
{
    public class HealthComponent : IHealthComponent
    { 
        private HealthConfig _config;
        private bool _isEnabled;
        
        // === Subjects ===
        private readonly Subject<int> _onDamagedSubject = new();
        private readonly Subject<int> _onHealedSubject = new();
        private readonly Subject<Unit> _onDeathSubject = new();
        private readonly Subject<int> _onHealthChangedSubject = new();
        
        // === Observables ===
        public Observable<int> OnDamaged => _onDamagedSubject;
        public Observable<int> OnHealed => _onHealedSubject;
        public Observable<Unit> OnDeath => _onDeathSubject;
        public Observable<int> OnHealthChanged => _onHealthChangedSubject;
        
        // === Data ===
        public int CurrentHealth { get; private set;}
        public int MaxHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0;
        
        public HealthComponent(HealthConfig config)
        {
            _config = config;
            SetupHealthComponent();
        }

        private void SetupHealthComponent()
        {
            if (_config.MaxHealth <= 0)
                throw new ArgumentException("MaxHealth must be positive");
            
            MaxHealth = _config.MaxHealth;
            CurrentHealth = _config.MaxHealth;
        }

        public void Enable() => _isEnabled = true;
        public void Disable() => _isEnabled = false;

        public void TakeDamage(int amount)
        {
            if (IsDead || amount <= 0 || !_isEnabled) return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            
            _onDamagedSubject?.OnNext(amount);
            _onHealthChangedSubject?.OnNext(CurrentHealth);
            
            if (IsDead)
            {
                _onDeathSubject?.OnNext(Unit.Default);
            }
        }

        public void Heal(int amount)
        {
            if (IsDead || amount <= 0 || !_isEnabled) return;
            
            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
            
            _onHealedSubject?.OnNext(amount);
            _onHealthChangedSubject?.OnNext(CurrentHealth);
        }

        public void Kill()
        {
            if (IsDead) return;
            
            CurrentHealth = 0;
            _onDeathSubject?.OnNext(Unit.Default);
        }

        public void Reset()
        {
            CurrentHealth = MaxHealth;
            _onHealthChangedSubject?.OnNext(CurrentHealth);
        }

        public void Dispose()
        {
            _onDamagedSubject?.Dispose();
            _onHealedSubject?.Dispose();
            _onDeathSubject?.Dispose();
            _onHealthChangedSubject?.Dispose();
        }
    }
}