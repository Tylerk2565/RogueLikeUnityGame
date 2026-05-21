using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _newGamePanel;
    [SerializeField] private GameObject _loadGamePanel;
    [SerializeField] private Button _continueButton;

    private bool _saveExists = true;

    private void Start()
    {

    }

    private void Update()
    {
        if (_saveExists)
        {
            _continueButton.interactable = true;
        }
        else
        {
            _continueButton.interactable = false;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainLevel");
    }
    
    public void NewGame()
    {
        _newGamePanel.SetActive(true);
        // todo function for closing new game and logic for creating a new game
    }

    public void LoadGame()
    {
        _loadGamePanel.SetActive(true);
        // todo function for closing load game and logic for selecting a save
    }

    public void OpenSettings()
    {
      _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
 