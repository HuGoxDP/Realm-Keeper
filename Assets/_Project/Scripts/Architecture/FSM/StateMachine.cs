using System;
using System.Collections.Generic;

namespace Architecture.FSM
{
    public class StateMachine
    {
        private StateNode _current;
        private readonly Dictionary<Type, StateNode> _nodes = new();
        private readonly HashSet<ITransition> _anyTransitions = new();

        public void Update()
        {
            if (_current == null) return;
            
            var transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);
            
            _current.State?.Update();
        }

        public void FixedUpdate()
        {
            if (_current == null) return;
            
            _current.State?.FixedUpdate();
        }

        public void SetState(IState state)
        {
            var node = GetOrAddNode(state);
            _current = node;
            _current.State?.OnEnter();
        }

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }
        
        
        private StateNode GetOrAddNode(IState state)
        {
            var stateType = state.GetType();
            if (!_nodes.TryGetValue(stateType, out var node))
            {
                node = new StateNode(state);
                _nodes.Add(stateType, node);
            }
            
            return node;
        }
        
        private void ChangeState(IState state)
        {
            if (state == _current?.State)
                return;
            
            var previousState = _current?.State;
            var nextStateNode = GetOrAddNode(state);
            
            previousState?.OnExit();
            nextStateNode.State?.OnEnter();
            _current = nextStateNode;
        }
        
        private ITransition GetTransition()
        {
            if (_current == null) return null;
            
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }
            
            foreach (var transition in _current.Transitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            return null;
        }
        
        
        private class StateNode
        {
            public IState State { get;}
            public HashSet<ITransition> Transitions { get;}
            
            public StateNode(IState state)
            {
                State = state ?? throw new ArgumentNullException(nameof(state));
                Transitions = new HashSet<ITransition>();
            }
            
            public void AddTransition(IState to, IPredicate condition)
            {
                if (to == null) throw new ArgumentNullException(nameof(to));
                if (condition == null) throw new ArgumentNullException(nameof(condition));
                
                Transitions.Add(new Transition(to, condition));
            }
        }
    }

  
}