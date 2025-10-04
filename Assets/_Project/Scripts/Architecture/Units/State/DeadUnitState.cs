using Architecture.Units.Core;
using UnityEngine;

namespace Architecture.Units.State
{
    public class DeadUnitState : BaseUnitState
    {
        public DeadUnitState(UnitContext context) : base(context) { }
        
        public override void OnEnter()
        {
            Context.MovementComponent.StopMoving();
            Context.MovementData.Clear();
  
            Object.Destroy(Context.Unit.gameObject, 2);
        }
    }
}