using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour 
{
    #region State Variables
    private enum EnemyState { IsChasing, IsAttacking }
    private EnemyState _currentEnemyState = EnemyState.IsChasing;
    #endregion

    #region Components & References
    private NavMeshAgent _navMeshAgent;
    private Health _health;
    private Transform _playerTransform;
    #endregion

    #region Settings
    [Header("AI Behavior")]
    [SerializeField] private float _stoppingDistance = 1.5f;
    [SerializeField] private float _attackRange = 1.5f;

    [Header("Attack")]
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private float _attackCooldown = 2.0f;
    private float _lastAttackTime = 0f;

    [Header("Combat")]
    [SerializeField] private LayerMask _playerMask;
    #endregion

    #region State Variables (Private)
    private bool _hasHandledDealth = false;
    #endregion

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();

        // Cache player transform
        GameObject playerObject = GameObject.Find("PlayerObject");
        if (playerObject != null)
        {
            _playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("PlayerObject not found!");
        }

        if (_playerMask == 0)
        {
            _playerMask = LayerMask.GetMask("Player");
        }

        _health.OnDeath += HandleDeath;
    }

    private void Update()
    {
        if (_health.IsDead())
        {
            if (!_hasHandledDealth)
            {
                _hasHandledDealth = true;
                _navMeshAgent.enabled = false;
            }
            return;
        }

        if (_playerTransform == null) { return; }

        _currentEnemyState = GetEnemyState();

        switch (GetEnemyState())
        {
            case EnemyState.IsChasing:
                Chase();
                break;
            case EnemyState.IsAttacking:
                Attack();
                break;
        }

        Debug.Log($"Enemy State: {_currentEnemyState}, Distance to player: {Vector3.Distance(transform.position, _playerTransform.position):F2}");
    }

    #region State Management
    private EnemyState GetEnemyState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        
        if (distanceToPlayer <= _attackRange)
        {
            return EnemyState.IsAttacking;
        }
        else
        {
            return EnemyState.IsChasing;
        }  
    }
    #endregion

    #region AI Behaviors
  
    private void Chase()
    {
        _navMeshAgent.destination = _playerTransform.position;
        _navMeshAgent.stoppingDistance = _stoppingDistance;
    }

    private void Attack()
    {
        _navMeshAgent.destination = transform.position;

        if (Time.time - _lastAttackTime >= _attackCooldown)
        {
            AttackPlayer();
            _lastAttackTime = Time.time;
        }
    }
    #endregion

    #region Combat
    public void AttackPlayer()
    {
        if (CombatSystem.TryRaycastAttack(transform.position, transform.forward,
            _attackRange, _playerMask, out RaycastHit hit))
        {
            Health playerHealth = hit.collider.GetComponent<Health>();
            if (playerHealth != null && !playerHealth.IsDead()) 
            {
                playerHealth.TakeDamage(_attackDamage);
                Debug.Log("Player Hit! Dealt " + _attackDamage + " damage.");
            }
        }
    }

    public void TakeDamage(float amount)
    {
        _health.TakeDamage(amount);
    }
    #endregion

    #region Events
    private void HandleDeath()
    {
        Debug.Log("Enemy died!");
        _navMeshAgent.enabled = false;
        // Additional death logic here
    }
    #endregion

    private void OnDestroy()
    {
        if (_health != null)
            _health.OnDeath -= HandleDeath;
    }
}