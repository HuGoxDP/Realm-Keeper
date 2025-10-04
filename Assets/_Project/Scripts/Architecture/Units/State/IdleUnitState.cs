using Architecture.Units.Core;
using UnityEngine;

namespace Architecture.Units.State
{
    public class IdleUnitState : BaseUnitState
    {
        public IdleUnitState(UnitContext context) : base(context) { }

        public override void OnEnter()
        { 
            Context.MovementComponent.StopMoving();
            Context.MovementData.Clear();
        }

        public override void Update()
        {
             Context.MovementComponent.StopMoving();
        }
    }
}