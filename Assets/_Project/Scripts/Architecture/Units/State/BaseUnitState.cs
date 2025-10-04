using Architecture.FSM;
using Architecture.Units.Core;

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
}