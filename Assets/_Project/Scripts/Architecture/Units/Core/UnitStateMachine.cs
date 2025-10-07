using System;
using Architecture.FSM;
using Architecture.Units.State;
using R3;
using UnityEngine;

namespace Architecture.Units.Core
{
    public class UnitStateMachine : MonoBehaviour
    {
        private UnitContext _context;
        private StateMachine _fsm;
        
        private IDisposable _deathSubscription;

        private void Awake()
        {
            _fsm = new StateMachine();
        }

        public void Initialize(UnitContext context)
        {
            if (context == null)
            {
                Debug.LogError("UnitStateMachine: Context is null!");
                return;
            }

            _context = context;
            InitializeStates();
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _deathSubscription = _context.HealthComponent.OnDeath
                .Subscribe(_ =>
                {
                    Debug.Log($"[{_context.Unit.name}] Death event received!");
                    TransitionToDeadState();
                });
        }

        private void TransitionToDeadState()
        {
            var deadState = new DeadUnitState(_context);
            _fsm.SetState(deadState);
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
            var idleState = new IdleUnitState(_context);
            var moveState = new MoveUnitState(_context);
            var chaseState = new ChaseUnitState(_context);

            _fsm.AddTransition(idleState, moveState, new FuncPredicate(() => _context.MovementData.TargetPosition.HasValue));
            _fsm.AddTransition(idleState, chaseState, new FuncPredicate(() => _context.MovementData.TargetTransform != null));
            _fsm.AddTransition(moveState, idleState, new FuncPredicate(() => !_context.MovementData.HasMoveCommand));
            _fsm.AddTransition(chaseState, idleState, new FuncPredicate(() => !_context.MovementData.HasMoveCommand));
            
            _fsm.SetState(idleState);
        }
        
        private void OnDestroy()
        {
            _deathSubscription?.Dispose();
        }
    }
}