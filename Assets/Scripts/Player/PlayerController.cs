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
    private enum MovementState { IsIdle, IsWalking, IsSprinting, IsCrouching, IsJumping }
    private MovementState _currentMovementState = MovementState.IsIdle;
    private enum CombatState { IsUnarmed, IsArmed, IsAttacking }
    private CombatState _currentCombatState = CombatState.IsUnarmed;
    private enum LifeState { IsAlive, IsDead }
    private LifeState _currentLifeState = LifeState.IsAlive;
    #endregion

    #region Component References
    [SerializeField] private Camera _camera;
    private CharacterController _player;
    private EnemyController _enemy;
    #endregion

    #region Settings
    [Header("Movement")]
    [SerializeField] private float _jumpHeight = 2.5f;
    [SerializeField] private float _gravity = -50.0f;
    [SerializeField] private float _walkSpeed = 10.0f;
    [SerializeField] private float _sprintSpeed = 20.0f;
    [SerializeField] private float _rotationSpeed = 15.0f;
    [SerializeField] private Vector3 _crouchScale = new(1f, 0.5f, 1f);
    [SerializeField] private Vector3 _standingScale = Vector3.one;

    [Header("Combat")]
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private float _attackRange = 20f;
    [SerializeField] private float _attackCooldown = 2f;
    private float _lastAttackTime = 0f;
    #endregion

    [Header("Health")]
    private float _currentHealth;
    [SerializeField] private float _initialHealth = 100f;

    #region State Variables (Private)
    private float _verticalVelocity;
    private bool _hasHandledDeath = false;
    #endregion

    private void Start()
    {
        _currentHealth = _initialHealth;
        _player = GetComponent<CharacterController>();
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
                HandleDeath();
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

        _currentCombatState = GetCombateState();
        switch (_currentCombatState)
        {
            case CombatState.IsUnarmed:
                // Handle unarmed state behavior
                break;
            case CombatState.IsArmed:
                // Handle armed state behavior
                break;
            case CombatState.IsAttacking:
                AttackEnemy();
                break;
        }

        Debug.Log($"Player State - Movement: {_currentMovementState}, Combat: {_currentCombatState}");
    }

    #region State Management
    private MovementState GetMovementState()
    {
        if (_jump.IsPressed())
            return MovementState.IsJumping;
        else if (_crouch.IsPressed())
            return MovementState.IsCrouching;
        else if (_sprint.IsPressed())
            return MovementState.IsSprinting;
        else if (_move.ReadValue<Vector2>().sqrMagnitude > 0.01f)
            return MovementState.IsWalking;
        else
            return MovementState.IsIdle;
    }

    private CombatState GetCombateState()
    {
        if (_attack.IsPressed())
            return CombatState.IsAttacking;
        else if (_attack.ReadValue<float>() > 0.01f)
            return CombatState.IsArmed;
        else
            return CombatState.IsUnarmed;
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
    #endregion

    #region Movement Handlers
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
        Vector3 move = speed * Time.deltaTime * moveDirection;

        move.y = _verticalVelocity * Time.deltaTime;

        _player.Move(move);

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
        if (_player.isGrounded)
        {
            _verticalVelocity = 0f;

            if (_jump.IsPressed())
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
    #endregion

    #region Combat
    public void AttackEnemy()
    {
        if (Time.time - _lastAttackTime < _attackCooldown) { return; }

        LayerMask enemyMask = LayerMask.GetMask("Enemy");
        Ray crosshairRay = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(crosshairRay, out RaycastHit hit, _attackRange, enemyMask)) 
        {
            Debug.DrawRay(crosshairRay.origin, crosshairRay.direction * hit.distance, Color.green);
            Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.red);

            _enemy = hit.collider.GetComponent<EnemyController>();
            if (_enemy != null)
            {
                _enemy.TakeDamage(_attackDamage);
                _lastAttackTime = Time.time;
                Debug.Log("Enemy Hit! Dealt " + _attackDamage + " damage.");
            }
        }
        else
        {
            Debug.DrawRay(crosshairRay.origin, crosshairRay.direction * _attackRange, Color.yellow);
        }
    }

    public void TakeDamage(float amount)
    {
        if (_currentLifeState == LifeState.IsDead) { return; }

        _currentHealth -= amount;
        Debug.Log($"Player took {amount} damage, current health: {_currentHealth}");
    }
    #endregion

    #region Events
    private void HandleDeath()
    {
        if (_currentLifeState == LifeState.IsDead) { return; }

        Debug.Log("Player died!");
    }

    private void PlayDeathParticals()
    {
        // Implement particle effects for death
    }

    private void PlayDeathSounds()
    {
        // Implement sound effects for death
    }
    #endregion

    private void OnDestroy()
    {
        PlayDeathParticals();
        PlayDeathSounds();
    }
}