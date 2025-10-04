using Architecture.Units.Core;
using UnityEngine;

namespace Architecture.Units.State
{
    public class ChaseUnitState : BaseUnitState
    {
        private float _updateInterval = 0.2f;
        private float _timer;

        public ChaseUnitState(UnitContext context) : base(context) { }
        
        public override void OnEnter()
        {
            _timer = 0f;
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

            if (Context.MovementData.TargetTransform != null)
            {
                float distance = Vector3.Distance( Context.Unit.transform.position, Context.MovementData.TargetTransform.position);
                
                if (distance <= 2f)
                {
                    Context.MovementData.Clear();
                }
            }
        }

        public override void OnExit()
        {
        }
    }
}