using System;
using Architecture.FSM;
using Architecture.Units.Components;
using Architecture.Units.State;
using UnityEngine;

namespace Architecture.Units
{
    public class UnitStateMachine : MonoBehaviour
    {
        private UnitContext _context;
        private StateMachine _fsm;

        private void Awake()
        {
            _fsm = new StateMachine();
        }

        public void Initialize(UnitContext context)
        {
           // _context = context;
            //InitializeStates();
        }

        private void Update()
        {
            _fsm?.Update();
        }

        private void FixedUpdate()
        {
            _fsm?.FixedUpdate();
        }

        private void InitializeStates()
        {
            //var idleState = new IdleUnitState(_context.MovementComponent);
            //var moveState = new MoveUnitState(_context.MovementComponent);
            //var deadState = new DeadUnitState(_context.MovementComponent);

            //_fsm.AddTransition(idleState, moveState, new FuncPredicate(() => _context.MovementComponent.TargetPosition.CurrentValue == null));
           // _fsm.AddTransition(moveState, idleState, new FuncPredicate(() => _context.MovementComponent.TargetPosition.CurrentValue != null));
            //_fsm.AddAnyTransition(deadState, new FuncPredicate(() => _context.HealthComponent.isDead));
            
            //_fsm.SetState(idleState);
        }

    }
}