using UnityEngine;

namespace Architecture.Units.State
{
    public class MoveUnitState : BaseUnitState
    {
        public MoveUnitState(UnitContext context) : base(context) { }

        public override void OnEnter()
        {
            Debug.Log("[Move] Entering Move State");
            var targetPos = Context.MovementData.GetCurrentTargetPosition();
            if (targetPos.HasValue)
            {
                Context.MovementComponent.StartMoving(targetPos.Value);
                Debug.Log($"[Move] Started moving to {targetPos.Value}");
            }
        }

        public override void Update()
        {
            if (!Context.MovementComponent.IsMoving && Context.MovementComponent.HasPath)
            {
                Debug.Log("[Move] Destination reached!");
                Context.MovementData.Clear();
            }
        }
        
        public override void OnExit()
        {
            Debug.Log("[Move] Exiting Move State");
        }
    }
}