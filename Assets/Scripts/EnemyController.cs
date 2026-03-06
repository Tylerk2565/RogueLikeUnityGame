using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        isAttacking,
        isFollowing
    }

    private EnemyState _currentState;

    private void Start()
    {

    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.isFollowing:
                IsFollowing();
                break;
            case EnemyState.isAttacking:
                isAttacking();
                break;
        }
    }

    public void IsFollowing()
    {


    }

    public void isAttacking()
    {


    }

}

