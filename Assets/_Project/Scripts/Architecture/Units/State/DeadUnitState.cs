using Architecture.Units.Core;
using UnityEngine;

namespace Architecture.Units.State
{
    public class DeadUnitState : BaseUnitState
    {
        public DeadUnitState(UnitContext context) : base(context) { }
        
        public override void OnEnter()
        {
            Debug.Log($"[{Context.Unit.name}] Entering Death State");
            
            Context.MovementComponent.StopMoving();
            Context.MovementData.Clear();
            
            Context.MovementComponent.Disable();
            Context.HealthComponent.Disable();
  
            Object.Destroy(Context.Unit.gameObject, 0.5f);
        }
    }
}