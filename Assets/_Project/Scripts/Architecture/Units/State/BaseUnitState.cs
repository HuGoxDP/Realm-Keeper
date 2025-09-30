using Architecture.FSM;
using UnityEngine;

namespace Architecture.Units.State
{
    public abstract class BaseUnitState : IState
    {
        protected readonly UnitContext Context;

        protected BaseUnitState(UnitContext context)
        {
            Context = context;
        }

        public virtual void OnEnter()
        {
            // Initialization if needed
        }

        public virtual void Update()
        {
            // Regular updates
        }

        public virtual void FixedUpdate()
        {
            // Physics related updates
        }

        public virtual void OnExit()
        {
            // Cleanup if needed
        }
    }
    
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

    public class ChaseUnitState : BaseUnitState
    {
        private float _updateInterval = 0.2f;
        private float _timer;

        public ChaseUnitState(UnitContext context) : base(context) { }
        
        public override void OnEnter()
        {
            Debug.Log("[Chase] Entering Chase State");
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
            Debug.Log("[Chase] Stopped chasing");
        }
    }
    
    public class DeadUnitState : BaseUnitState
    {
        public DeadUnitState(UnitContext context) : base(context) { }
        
        public override void OnEnter()
        {
            Debug.Log($"[Dead] Unit died! Health: {Context.HealthComponent.CurrentHealth}");
            
            Context.MovementComponent.StopMoving();
            Context.MovementData.Clear();
  
            Object.Destroy(Context.Unit.gameObject, 2);
        }
    }
}