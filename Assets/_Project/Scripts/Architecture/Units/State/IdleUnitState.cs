using UnityEngine;

namespace Architecture.Units.State
{
    public class IdleUnitState : BaseUnitState
    {
        public IdleUnitState(UnitContext context) : base(context) { }

        public override void OnEnter()
        { 
            Debug.Log("[Idle] Entering Idle State");
            
            Context.MovementComponent.StopMoving();
            Context.MovementData.Clear();
        }
    }
}