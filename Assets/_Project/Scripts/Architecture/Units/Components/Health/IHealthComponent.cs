using System;
using Architecture.Units.Core;
using R3;

namespace Architecture.Units.Components
{
    public interface IHealthComponent : IComponent, IDisposable
    {
        // === Data ===
        int CurrentHealth { get; }
        int MaxHealth { get; }
        bool IsDead { get; }
        
        // === Events ===
        Observable<int> OnDamaged { get; }
        Observable<int> OnHealed { get; }
        Observable<Unit> OnDeath { get; }
        Observable<int> OnHealthChanged { get; }
        
        // === Methods ===
        void TakeDamage(int amount);
        void Heal(int amount);
        void Kill();
        void Reset();
    }
}