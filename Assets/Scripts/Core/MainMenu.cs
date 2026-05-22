using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _newGamePanel;
    [SerializeField] private GameObject _loadGamePanel;
    [SerializeField] private Button _continueButton;
    private bool _hasSave;

    private void Start()
    {
        _hasSave = PlayerPrefs.HasKey("HasSave");
    }

    public void PlayGame()
    {
        if (_hasSave)
        {
            SceneManager.LoadScene("PlayerBase");
        }
        else
        {
            SceneManager.LoadScene("CharacterCreation");
        }
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
 