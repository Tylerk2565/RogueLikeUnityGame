using UnityEngine;
using UnityEngine.AI;

public class EnemyController : SharedController
{
    private enum EnemyState { isChasing, isAttacking }
    private EnemyState _currentEnemyState;
    private NavMeshAgent _enemy;
    private Transform _player;

    [Header("Attack Settings")]
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _cooldownDuration = 2.0f;
    private float _lastAttackTime;

    [Header("Enemy Stats")] 
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private int _initialHealth = 100;

    protected override void Start()
    {
        base.Start();
        _health = _initialHealth;
        _currentEnemyState = EnemyState.isChasing;
        _enemy = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("PlayerObject").transform;
    }

    private void Update()
    {
        if (_currentLifeState == LifeState.IsDead) return;

        switch (_currentEnemyState)
        {
            case EnemyState.isChasing:
                IsChasing();
                break;
            case EnemyState.isAttacking:
                IsAttacking();
                break;
        }
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
            AttackPlayer();
        }
            
        if (Vector3.Distance(_enemy.transform.position, _player.position) >= _attackRange)
        {
            _currentEnemyState = EnemyState.isChasing;
        }     
    }

    private void AttackPlayer()
    {
        PlayerController player = _player.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(_attackDamage);
        }
    }
   

    protected override void OnDeath()
    {
        Debug.Log("Enemy died!");
        DropGold();
        Destroy(gameObject);
    }

    private void DropGold()
    {
       
    }
}