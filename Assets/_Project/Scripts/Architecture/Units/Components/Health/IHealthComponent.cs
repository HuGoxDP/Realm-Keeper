using System;
using R3;
using UnityEngine;

namespace Architecture.Units.Components
{
    public interface IHealthComponent : IComponent, IDisposable
    {
        ReactiveProperty<int> Health { get; }
        bool IsDead { get; }
        
        void ApplyDamage(int amount);
    }
}