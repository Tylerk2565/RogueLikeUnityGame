using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    private InputAction _pause;
    private enum GameState { IsRunning, IsPaused };
    private GameState _currentGameState = GameState.IsRunning;

    private void Start()
    {
        _pause = InputSystem.actions.FindAction("Pause");
    }

    private void Update()
    {
        if (_pause.WasPressedThisFrame())
        {
            if (_currentGameState == GameState.IsRunning)
            {
                _currentGameState = GameState.IsPaused;
                Pause();
            }
            else
            {
                _currentGameState = GameState.IsRunning;
                Resume();
            }
        }      
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _currentGameState = GameState.IsPaused;
        _pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _currentGameState = GameState.IsRunning;
        _pauseMenu.SetActive(false);
    }

    public void QuitToDesktop()
    { 
        Application.Quit();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}