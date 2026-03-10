using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        isChasing,
        isAttacking
        
    }

    private EnemyState _currentState;
    private NavMeshAgent _agent;
    private Transform _player;

    [SerializeField] private float _attackRange = 1.5f;
    private float _attackDamage;
    private float _health;

    private void Start()
    {
        _currentState = EnemyState.isChasing;
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("PlayerObject").transform;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.isChasing:
                IsChasing();
                break;
            case EnemyState.isAttacking:
                IsAttacking();
                break;
        }
    }

    private void IsChasing()
    {
        if (Vector3.Distance(_agent.transform.position, _player.position) >= _attackRange)
        {
            _agent.destination = _player.position;
        }
        else
        {
            _currentState = EnemyState.isAttacking;
        }
    }

    private void IsAttacking()
    {
        if (Vector3.Distance(_agent.transform.position, _player.position) <= _attackRange)
        {
            _agent.destination = _player.position;
        } 
        else
        {
            _currentState = EnemyState.isChasing;
        }
    }
}

