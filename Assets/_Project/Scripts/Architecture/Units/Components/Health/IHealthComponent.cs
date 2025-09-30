namespace Architecture.Units.Components
{
    public interface IHealthComponent : IComponent
    {
        int CurrentHealth { get; }
        int MaxHealth { get; }
        bool IsDead { get; }
        
        void TakeDamage(int amount);
        void Heal(int amount);
        void Kill();
        void Reset();
    }
}