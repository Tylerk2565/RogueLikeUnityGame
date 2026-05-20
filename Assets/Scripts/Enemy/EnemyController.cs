using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class EnemyController : MonoBehaviour 
{
    #region State Variables
    private enum EnemyState { IsChasing, IsAttacking }
    private EnemyState _currentEnemyState = EnemyState.IsChasing;
    private enum LifeState { IsAlive, IsDead }
    private LifeState _currentLifeState = LifeState.IsAlive;
    #endregion

    #region Components & References
    private NavMeshAgent _enemy;
    private PlayerController _player;
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
    private float _initialHealth = 100f;
    private float _currentHealth;
    #endregion

    #region State Variables (Private)
    private bool _hasHandledDeath = false;
    #endregion

    private void Start()
    {
        _currentHealth = _initialHealth;
        _enemy = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("PlayerObject").GetComponent<PlayerController>();

        if (_playerMask == 0)
        {
            _playerMask = LayerMask.GetMask("Player");
        }
    }

    private void Update()
    {
        _currentLifeState = GetLifeState();
        if (_currentLifeState == LifeState.IsDead)
        {
            if (!_hasHandledDeath)
            {
                HandleDeath();
                _hasHandledDeath = true;
            }
            return;
        }

        _hasHandledDeath = false; 

        _currentEnemyState = GetEnemyState();
        switch (_currentEnemyState)
        {
            case EnemyState.IsChasing:
                Chase();
                break;
            case EnemyState.IsAttacking:
                AttackPlayer();
                break;
        }

        //Debug.Log($"Enemy State: {_currentEnemyState}, Distance to player: {Vector3.Distance(transform.position, _player.transform.position):F2}");
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

    #region State Management
    private EnemyState GetEnemyState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        
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
        _enemy.destination = _player.transform.position;
        _enemy.stoppingDistance = _stoppingDistance;
    }
    #endregion

    #region Combat
    public void AttackPlayer()
    {
        if (Time.time - _lastAttackTime < _attackCooldown) { return; }

        LayerMask playerMask = LayerMask.GetMask("Player");

        Ray ray = new(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _attackRange, playerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.red);

            PlayerController player = hit.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(_attackDamage);
                _lastAttackTime = Time.time;
                Debug.Log("Player hit! Dealt " + _attackDamage + " damage.");
            }
        }
        
    }

    public void TakeDamage(float amount)
    {
        if (_currentLifeState == LifeState.IsDead) { return; }
        _currentHealth -= amount;
    }
    #endregion

    #region Events
    private void HandleDeath()
    {
        if (_currentLifeState == LifeState.IsDead) { return; }
        Debug.Log("Enemy died!");
        _enemy.enabled = false;
        // Additional death logic here
    }

    private void DropLoot()
    {
        // drop gold and xp and add them to the players gold and xp
        // drop small gold coins that the player picks up
        // drop small xp orbs that the player picks up


    }

    private void PlayDeathParticals()
    {
        // todo - look into partical system on death
    }

    private void PlayDeathSounds()
    {
        // todo - look into play sounds on death
    }
    #endregion

    private void OnDestroy()
    {
        // TODO - fix this 
        DropLoot();
        PlayDeathParticals();
        PlayDeathSounds();
    }
}