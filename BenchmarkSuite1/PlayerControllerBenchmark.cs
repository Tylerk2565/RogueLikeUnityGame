using BenchmarkDotNet.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using Microsoft.VSDiagnostics;

[CPUUsageDiagnoser]
public class PlayerControllerBenchmark
{
    private PlayerController _playerController;
    private GameObject _playerObject;
    private CharacterController _characterController;
    private Camera _camera;
    [GlobalSetup]
    public void Setup()
    {
        // Create a game object for the player
        _playerObject = new GameObject("Player");
        // Add CharacterController
        _characterController = _playerObject.AddComponent<CharacterController>();
        // Create a camera
        var cameraObject = new GameObject("MainCamera");
        _camera = cameraObject.AddComponent<Camera>();
        cameraObject.transform.position = Vector3.zero;
        // Add PlayerController
        _playerController = _playerObject.AddComponent<PlayerController>();
        // Reflect to set private fields
        var camField = typeof(PlayerController).GetField("_camera", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        camField.SetValue(_playerController, _camera);
        // Initialize InputSystem (if available)
        InputSystem.actions.FindAction("Move");
        InputSystem.actions.FindAction("Jump");
    }

    [Benchmark]
    public void BenchmarkUpdate()
    {
        _playerController.Update();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        Object.Destroy(_playerObject);
        Object.Destroy(_camera.gameObject);
    }
}