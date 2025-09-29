using Architecture.Units.Components;

namespace Architecture.Units
{
    public class UnitContext
    {
        public BaseUnit Unit { get; }
        public IHealthComponent HealthComponent { get; }
        public IMovementComponent MovementComponent { get; }
        
        public UnitContext(BaseUnit unit, IHealthComponent healthComponent, IMovementComponent movementComponent)
        {
            Unit = unit;
            HealthComponent = healthComponent;
            MovementComponent = movementComponent;
        }
        
    }
}