using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Input Actions
    private InputAction _move;
    private InputAction _jump;
    private InputAction _crouch;
    private InputAction _sprint;
    private InputAction _attack;
    #endregion

    #region State Variables
    private enum LifeState { IsAlive, IsDead };
    private LifeState _currentLifeState = LifeState.IsAlive;
    private enum CombatState { IsUnarmed, IsArmed, IsAttacking }
    private CombatState _currentCombatState = CombatState.IsUnarmed;
    private enum MovementState { IsIdle, IsWalking, IsSprinting, IsCrouching, IsJumping }
    private MovementState _currentMovementState = MovementState.IsIdle;
    private bool _hasHandledDeath = false;
    #endregion

    #region Player Stats & Settings
    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Player Control Settings")]
    [SerializeField] private float _jumpHeight = 2.5f;
    [SerializeField] private float _gravity = -50.0f;
    [SerializeField] private float _walkSpeed = 10.0f;
    [SerializeField] private float _sprintSpeed = 20.0f;
    [SerializeField] private float _rotationSpeed = 15.0f;
    [SerializeField] private Vector3 _crouchScale = new Vector3(1f, 0.5f, 1f);
    [SerializeField] private Vector3 _standingScale = Vector3.one;

    [Header("Player Stats")]
    private float _currentHealth;
    [SerializeField] private float _initialHealth = 100f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private float _attackRange = 20f;
    [SerializeField] private float _attackCooldown = 2f;
    private float _lastAttackTime = 0f;

    private float _verticalVelocity;
    private CharacterController _character;
    private EnemyController _enemy;

    private int _gold = 0;
    private int _experience = 0;
    
    #endregion

    private void Start()
    {
        _currentHealth = _initialHealth;
        _character = GetComponent<CharacterController>();
        _move = InputSystem.actions.FindAction("Move");
        _jump = InputSystem.actions.FindAction("Jump");
        _crouch = InputSystem.actions.FindAction("Crouch");
        _sprint = InputSystem.actions.FindAction("Sprint");
        _attack = InputSystem.actions.FindAction("Attack");
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

        HandleGravity();
        HandleCrouch();

        _currentMovementState = GetMovementState();
        switch (_currentMovementState)
        {
            case MovementState.IsIdle:
                HandleMovement(0f);
                break;
            case MovementState.IsWalking:
                HandleMovement(_walkSpeed);
                break; 
            case MovementState.IsSprinting:
                HandleMovement(_sprintSpeed);
                break;
            case MovementState.IsCrouching:
                HandleMovement(_walkSpeed * 0.5f);
                break;
            case MovementState.IsJumping:
                HandleJump();
                float jumpSpeed = _sprint.IsPressed() ? _sprintSpeed : _walkSpeed;
                HandleMovement(jumpSpeed);
                break;
        }

        _currentCombatState = CombatState.IsAttacking;
        //switch (_currentCombatState)
        //{
        //    case CombatState.IsUnarmed:
        //        // Handle unarmed state behavior
        //        break;
        //    case CombatState.IsArmed:
        //        // Handle armed state behavior
        //        break;
        //    case CombatState.IsAttacking:
        //        AttackEnemy(_attackDamage);
        //        break;
        //}

        Debug.Log($"Player State - Movement: {_currentMovementState}, Combat: {_currentCombatState}, Life: {_currentLifeState}");
    }

    #region State Management
    private MovementState GetMovementState()
    {
        if (_jump.IsPressed())
        {
            return MovementState.IsJumping;
        }
        else if (_crouch.IsPressed())
        {
            return MovementState.IsCrouching;
        }
        else if (_sprint.IsPressed())
        {
            return MovementState.IsSprinting;
        }
        else if (_move.ReadValue<Vector2>().sqrMagnitude > 0.01f)
        {
            return MovementState.IsWalking;
        }
        else
        {
            return MovementState.IsIdle;
        }
    }

    //private CombatState GetCombateState()
    //{
    //    if (_attack.WasPressedThisFrame())
    //    {
    //        return CombatState.IsAttacking;
    //    }
    //    else if (_attack.ReadValue<float>() > 0.01f)
    //    {
    //        return CombatState.IsArmed;
    //    }
    //    else
    //    {
    //        return CombatState.IsUnarmed;
    //    }
    //}

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
    #endregion

    #region State Handlers

    private void HandleMovement(float speed)
    {
        Vector2 moveValue = _move.ReadValue<Vector2>();
        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraRight = _camera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraRight * moveValue.x + cameraForward * moveValue.y).normalized;
        Vector3 move = moveDirection * speed * Time.deltaTime;

        move.y = _verticalVelocity * Time.deltaTime;

        _character.Move(move);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleCrouch()
    { 
        if (_crouch.IsPressed())
        {
            transform.localScale = _crouchScale;
        } 
        else
        {
            transform.localScale = _standingScale;
        }
    } 

    private void HandleGravity()
    {
        _verticalVelocity += _gravity * Time.deltaTime;
    }

    private void HandleJump()
    {
        if (_character.isGrounded)
        {
            _verticalVelocity = 0f;

            if (_jump.WasPressedThisFrame())
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
    #endregion

    #region Combat & Health
    public void AttackEnemy(float amount)
    {
        if (Time.time - _lastAttackTime < _attackCooldown) { return; }

        LayerMask enemyMask = LayerMask.GetMask("Enemy", "Obstacle", "Ground");

        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _attackRange, enemyMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.red);

            _enemy = hit.collider.GetComponent<EnemyController>();
            if (_enemy != null)
            {
                _enemy.TakeDamage(amount);
                _lastAttackTime = Time.time;
                Debug.Log("Enemy hit! Dealt " + amount + " damage.");
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * _attackRange, Color.yellow);
        }

    }

    public void TakeDamage(float amount)
    {
        if (_currentLifeState == LifeState.IsDead) return;

        _currentHealth -= amount;
        Debug.Log($"Player took {amount} damage, current health: {_currentHealth}");
    }

    private void OnDeath()
    {
        // Handle player death & player specific death behavior
        if (_currentLifeState == LifeState.IsDead )
        {
            Debug.Log("Player is Dead");
        }
    }
    #endregion

}