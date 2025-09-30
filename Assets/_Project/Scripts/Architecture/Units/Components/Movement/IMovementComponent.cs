using System;
using UnityEngine;

namespace Architecture.Units.Components
{
    public interface IMovementComponent : IComponent, IDisposable
    {
       bool IsMoving { get; }
       float RemainingDistance { get; }
       bool HasPath { get; }
       Vector3 Velocity { get; }
       
       void StartMoving(Vector3 destination);
       void StopMoving();
       void SetSpeed(float speed);
    }
}