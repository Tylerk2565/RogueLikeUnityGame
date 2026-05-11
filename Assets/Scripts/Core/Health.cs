using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;
    private bool _isDead = false;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    public void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;
        OnHealthChanged?.Invoke(_currentHealth);

        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            _isDead = true;
            OnDeath?.Invoke();
        }
    }

    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => _maxHealth;
    public bool IsDead() => _isDead;
}
