using UnityEngine;

public class SharedController : MonoBehaviour
{
    public enum LifeState
    {
        IsAlive,
        IsDead,
    }

    protected LifeState _currentLifeState;
    protected int _health;

    protected virtual void Start()
    {
        _currentLifeState = LifeState.IsAlive;
    }

    public virtual void TakeDamage(int amount)
    {
        if (_currentLifeState == LifeState.IsDead) { return; }

        _health -= amount;

        if (_health <= 0)
        {
            _health = 0;
            _currentLifeState = LifeState.IsDead;
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        // Override
    }

    public int GetHealth()
    {
        return _health;
    }

    public LifeState GetCurrentState()
    {
        return _currentLifeState;
    }
}