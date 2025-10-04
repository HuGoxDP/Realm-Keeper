using System;
using Architecture.Units.Components;
using Architecture.Units.Data;

namespace Architecture.Units.Core
{
    public class UnitContext
    {
        public BaseUnit Unit { get; }

        public MovementData MovementData { get; }
        
        public IHealthComponent HealthComponent { get; }
        public IMovementComponent MovementComponent { get; }
        
        public UnitContext(BaseUnit unit, MovementData movementData, IHealthComponent healthComponent, IMovementComponent movementComponent)
        {
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            MovementData = movementData ?? throw new ArgumentNullException(nameof(movementData));
            HealthComponent = healthComponent ?? throw new ArgumentNullException(nameof(healthComponent));
            MovementComponent = movementComponent ?? throw new ArgumentNullException(nameof(movementComponent));

        }
        
    }
}