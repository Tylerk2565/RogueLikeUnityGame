using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SharedController
{
    private InputAction _move;
    private InputAction _jump;
    private InputAction _crouch;
    private InputAction _sprint;
    private InputAction _attack;

    private bool _isCrouching = false;
    private bool _isSprinting = false;

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Player Control Settings")]
    [SerializeField] private float _jumpHeight = 5.0f;
    [SerializeField] private float _gravity = -50.0f;
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _sprintSpeed = 20.0f;
    [SerializeField] private float _rotationSpeed = 15.0f;
    [SerializeField] private Vector3 _crouchScale = new Vector3(1f, 0.5f, 1f);
    [SerializeField] private Vector3 _standingScale = Vector3.one;

    [Header("Player Stats")]
    [SerializeField] private int _initialHealth = 100;

    private float _verticalVelocity;
    private CharacterController _character;

    private int _gold = 0;
    private int _experience = 0;
    private int _attackDamage = 10;
    
    protected override void Start()
    {
        base.Start();
        _health = _initialHealth;
        _character = GetComponent<CharacterController>();
        _move = InputSystem.actions.FindAction("Move");
        _jump = InputSystem.actions.FindAction("Jump");
        _crouch = InputSystem.actions.FindAction("Crouch");
        _sprint = InputSystem.actions.FindAction("Sprint");
        _attack = InputSystem.actions.FindAction("Attack");
    }

    private void Update()
    {
        if (_currentLifeState == LifeState.IsDead) { return; }

        HandleMovement();
        HandleGravityAndJump();
        HandleCrouch();
        HandleSprint();

        Debug.Log($"Player Health: {_health}");
    }

    private void HandleMovement()
    {
        Vector2 moveValue = _move.ReadValue<Vector2>();

        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraRight = _camera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraRight * moveValue.x + cameraForward * moveValue.y).normalized;
        Vector3 move = moveDirection * (_isSprinting ? _sprintSpeed : _speed) * Time.deltaTime;

        move.y = _verticalVelocity * Time.deltaTime;

        _character.Move(move);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        } 
    }

    private void HandleGravityAndJump()
    {
        if (_character.isGrounded)
        {
            if (_verticalVelocity < 0f)
                _verticalVelocity = -2f;

            if (_jump.WasPressedThisFrame())
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _verticalVelocity += _gravity * Time.deltaTime;
    }

    private void HandleCrouch()
    {
        if (_crouch.IsPressed() && !_isCrouching)
        {
            _character.transform.localScale = _crouchScale;
            _isCrouching = true;
            _isSprinting = false;
        }
        else if (!_crouch.IsPressed() && _isCrouching)
        {
            _character.transform.localScale = _standingScale;
            _isCrouching = false;
        }
    }

    private void HandleSprint()
    {
        if (_sprint.IsPressed() && !_isCrouching)
        {
            _isSprinting = true;
            _isCrouching = false;
        }
        else if (!_sprint.IsPressed() && _isSprinting)
        {
            _isSprinting = false;
        } 
    }

    public void AttackEnemy(int damage)
    {
        // todo
    }

    protected override void OnDeath()
    {
        Debug.Log("Player died");
        // Handle player death & player specific death behavior
    }

}