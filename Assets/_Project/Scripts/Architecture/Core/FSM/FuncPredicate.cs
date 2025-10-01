using System;

namespace Architecture.FSM
{
    public class FuncPredicate : IPredicate
    {
        private readonly Func<bool> _predicate;

        public FuncPredicate(Func<bool> predicate)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        public bool Evaluate()
        {
            return _predicate.Invoke();
        }
    }
}