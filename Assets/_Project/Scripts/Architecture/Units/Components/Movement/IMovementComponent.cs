using System;
using UnityEngine;

namespace Architecture.Units.Components
{
    public interface IMovementComponent : IComponent, IDisposable
    {
        void MoveTo(Vector3 position);
        void Stop();
    }
}