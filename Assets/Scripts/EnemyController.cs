using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour 
{
    private enum EnemyState { isChasing, isAttacking }
    private EnemyState _currentEnemyState;
    private enum LifeState { IsAlive, IsDead }
    private LifeState _currentLifeState = LifeState.IsAlive;
    private bool _hasHandledDeath = false;

    private NavMeshAgent _enemy;
    private Transform _player;
 
    [Header("Attack Settings")]
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _cooldownDuration = 2.0f;
    private float _lastAttackTime = 0f;

    [Header("Enemy Stats")] 
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private float _initialHealth = 100f; 
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _initialHealth;
        _currentEnemyState = EnemyState.isChasing;
        _enemy = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("PlayerObject").transform;
    }

    private void Update()
    {
        _currentLifeState = GetLifeState();
        if (_currentLifeState == LifeState.IsDead)
        {
            if (!_hasHandledDeath)
            {
                 OnDeath();
                _hasHandledDeath = true;
            }
            return;
        }

        _hasHandledDeath = false;

        _currentEnemyState = GetEnemyState();
        switch (GetEnemyState())
        {
            case EnemyState.isChasing:
                IsChasing();
                break;
            case EnemyState.isAttacking:
                IsAttacking();
                break;
        }

        Debug.Log($"Enemy State: { _currentEnemyState }");
    }

    private LifeState GetLifeState()
    {
        if (_currentHealth <= 0)
        {
            return LifeState.IsDead;
        }
        else
        {
            return LifeState.IsAlive;
        }
    }

    private EnemyState GetEnemyState()
    {
        if (_currentEnemyState == EnemyState.isChasing)
        {
            return EnemyState.isChasing;
        }
        else if (_currentEnemyState == EnemyState.isAttacking)
        {
            return EnemyState.isAttacking;
        }
        return _currentEnemyState;
    }
    // TODO - Implement enemy stopping at a certain distance from the player so it's not pushing against player
    private void IsChasing()
    {
        if (Vector3.Distance(_enemy.transform.position, _player.position) >= _attackRange)
        {
            _enemy.destination = _player.position;
        }
        else
        {
            _currentEnemyState = EnemyState.isAttacking;
        }
    }

    private void IsAttacking()
    {
        if (Time.time - _lastAttackTime >= _cooldownDuration)
        {
            _lastAttackTime = Time.time;
            //AttackPlayer();
        }

        if (Vector3.Distance(_enemy.transform.position, _player.position) >= _attackRange)
        {
            _currentEnemyState = EnemyState.isChasing;
        }
    }

    public void AttackPlayer()
    {
        PlayerController player = _player.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(_attackDamage);
        }
    }


    private void OnDeath()
    {
        Debug.Log("Enemy died!");
        DropGold();
        Destroy(gameObject);
    }

    private void DropGold()
    {
       
    }

    internal void TakeDamage(float amount)
    {
        throw new NotImplementedException();
    }
}