using Architecture.Units.Core;
using UnityEngine;

namespace Architecture.Units.State
{
    public class MoveUnitState : BaseUnitState
    {
        private float _updateInterval = 0.2f;
        private float _timer;
        
        public MoveUnitState(UnitContext context) : base(context) { }

        public override void OnEnter()
        {
            _timer = 0f;

            var targetPos = Context.MovementData.GetCurrentTargetPosition();
            if (targetPos.HasValue)
            {
                Context.MovementComponent.StartMoving(targetPos.Value);
            }
        }

        public override void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _updateInterval)
            {
                _timer = 0f;
                
                var targetPos = Context.MovementData.GetCurrentTargetPosition();
                if (targetPos.HasValue)
                {
                    Context.MovementComponent.StartMoving(targetPos.Value);
                }
            }

            if (!Context.MovementComponent.IsMoving && Context.MovementComponent.HasPath)
            {
                Context.MovementData.Clear();
            }
        }
        
        public override void OnExit()
        {
        }
    }
}