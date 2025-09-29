using Architecture.FSM;
using Architecture.Units.Components;
using UnityEngine;

namespace Architecture.Units.State
{
    public abstract class BaseUnitState : IState
    {
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
        private readonly IMovementComponent _movementComponent;

        public IdleUnitState(IMovementComponent movementComponent)
        {
            _movementComponent = movementComponent;
        }

        public override void OnEnter()
        { 
            Debug.Log("Entering Idle State");
            _movementComponent.Stop();
        }
    }
    
    public class MoveUnitState : BaseUnitState
    {
        private readonly IMovementComponent _movementComponent;

        public MoveUnitState(IMovementComponent movementComponent)
        {
            _movementComponent = movementComponent;
        }

        public override void OnEnter()
        {
            Debug.Log("Entering Move State");
            
        }
        
    }
    
    public class FleeUnitState : BaseUnitState
    {
        
        public override void OnEnter()
        {
            Debug.Log("Entering Flee State");
            // Implement flee logic, e.g., move to a safe location
        }
    }
    
    public class DeadUnitState : BaseUnitState
    {
        private readonly IMovementComponent _movementComponent;

        public DeadUnitState(IMovementComponent movementComponent)
        {
            _movementComponent = movementComponent;
        }
        
        public override void OnEnter()
        {
            Debug.Log("Entering Dead State");
            _movementComponent.Stop();
        }
    }
}