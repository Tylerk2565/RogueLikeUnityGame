using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CharacterCreation : CharacterCreationBase
{
    #region Renderer References
    [SerializeField] private GameObject[] _eyeballRenderers; // fixed color
    private Renderer[] _pupilRenderers; // color change
    private Renderer[] _skinRenderers;
    private Renderer[] _eyelidRenderers; // mesh change (type options)
    private Renderer[] _mouthRenderers;
    private Renderer[] _noseRenderers;
    private Renderer[] _shirtRenderers;
    private Renderer[] _pantRenderers;
    private Renderer[] _shoeRenderers;
    private Renderer[] _hatRenderers;
    #endregion

    #region Customization Options
    [Header("Customization Options")]
    [SerializeField] private Color[] _skinColorOptions;
    [SerializeField] private Color[] _pupilColorOptions;
    [SerializeField] private Color[] _hatColorOptions;
    [SerializeField] private Color[] _noseColorOptions;
    [SerializeField] private Color[] _mouthColorOptions;
    [SerializeField] private Color[] _shirtColorOptions;
    [SerializeField] private Color[] _pantColorOptions;
    [SerializeField] private Color[] _shoeColorOptions;

    [SerializeField] private GameObject[] _skinTypeOptions;
    [SerializeField] private GameObject[] _hatTypeOptions;
    [SerializeField] private GameObject[] _eyelidTypeOptions;
    [SerializeField] private GameObject[] _pupilTypeOptions;
    [SerializeField] private GameObject[] _noseTypeOptions;
    [SerializeField] private GameObject[] _mouthTypeOptions;
    [SerializeField] private GameObject[] _shirtTypeOptions;
    [SerializeField] private GameObject[] _pantTypeOptions;
    [SerializeField] private GameObject[] _shoeTypeOptions;
    #endregion

    #region Current Selection Indexes
    private int _skinColorIndex = 0;
    private int _skinTypeIndex = 0;
    private int _hatTypeIndex = 0;
    private int _hatColorIndex = 0;
    private int _eyelidTypeIndex = 0;
    private int _pupilColorIndex = 0;
    private int _noseTypeIndex = 0;
    private int _noseColorIndex = 0;
    private int _mouthTypeIndex = 0;
    private int _mouthColorIndex = 0;
    private int _shirtTypeIndex = 0;
    private int _shirtColorIndex = 0;
    private int _pantTypeIndex = 0;
    private int _pantColorIndex = 0;
    private int _shoeTypeIndex = 0;
    private int _shoeColorIndex = 0;
    #endregion

    #region References
    private InputAction _click;
    private InputAction _move;
    [SerializeField] private Transform _characterModel;
    #endregion

    private void Start()
    {
        _click = InputSystem.actions.FindAction("Click");
        _move = InputSystem.actions.FindAction("Move");
        ResetCharacter();
    }

    public void CreateCharacter()
    {
        PlayerPrefs.SetInt("SkinColor", _skinColorIndex);
        PlayerPrefs.SetInt("SkinType", _skinTypeIndex);
        PlayerPrefs.SetInt("HatType", _hatTypeIndex);
        PlayerPrefs.SetInt("HatColor", _hatColorIndex);
        PlayerPrefs.SetInt("EyelidType", _eyelidTypeIndex);
        PlayerPrefs.SetInt("PupilColor", _pupilColorIndex);
        PlayerPrefs.SetInt("NoseType", _noseTypeIndex);
        PlayerPrefs.SetInt("NoseColor", _noseColorIndex);
        PlayerPrefs.SetInt("MouthType", _mouthTypeIndex);
        PlayerPrefs.SetInt("MouthColor", _mouthColorIndex);
        PlayerPrefs.SetInt("ShirtType", _shirtTypeIndex);
        PlayerPrefs.SetInt("ShirtColor", _shirtColorIndex);
        PlayerPrefs.SetInt("PantType", _pantTypeIndex);
        PlayerPrefs.SetInt("PantColor", _pantColorIndex);
        PlayerPrefs.SetInt("ShoeType", _shoeTypeIndex);
        PlayerPrefs.SetInt("ShoeColor", _shoeColorIndex);

        PlayerPrefs.SetInt("HasSave", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("PlayerBase");
    }

    public void ResetCharacter()
    {
        Debug.Log("Reset called");
        _skinColorIndex = 0;
        _skinTypeIndex = 0;
        _hatTypeIndex = 0;
        _hatColorIndex = 0;
        _eyelidTypeIndex = 0;
        _pupilColorIndex = 0;
        _noseTypeIndex = 0;
        _noseColorIndex = 0;
        _mouthTypeIndex = 0;
        _mouthColorIndex = 0;
        _shirtTypeIndex = 0;
        _shirtColorIndex = 0;
        _pantTypeIndex = 0;
        _pantColorIndex = 0;
        _shoeTypeIndex = 0;
        _shoeColorIndex = 0;

        ApplyCustomization();
    }

    private void ApplyCustomization()
    {
        ApplyOption(_skinTypeOptions, _skinTypeIndex);
        ApplyOption(_hatTypeOptions, _hatTypeIndex);
        ApplyOption(_eyelidTypeOptions, _eyelidTypeIndex);
        ApplyOption(_noseTypeOptions, _noseTypeIndex);
        ApplyOption(_mouthTypeOptions, _mouthTypeIndex);
        ApplyOption(_shirtTypeOptions, _shirtTypeIndex);
        ApplyOption(_pantTypeOptions, _pantTypeIndex);
        ApplyOption(_shoeTypeOptions, _shoeTypeIndex);

        _skinRenderers = GetRenderer(_skinTypeOptions, _skinTypeIndex);
        _eyelidRenderers = GetRenderer(_eyelidTypeOptions, _eyelidTypeIndex);
        _mouthRenderers = GetRenderer(_mouthTypeOptions, _mouthTypeIndex);
        _noseRenderers = GetRenderer(_noseTypeOptions, _noseTypeIndex);
        _shirtRenderers = GetRenderer(_shirtTypeOptions, _shirtTypeIndex);
        _pantRenderers = GetRenderer(_pantTypeOptions, _pantTypeIndex);
        _shoeRenderers = GetRenderer(_shoeTypeOptions, _shoeTypeIndex);
        _hatRenderers = GetRenderer(_hatTypeOptions, _hatTypeIndex);
        _pupilRenderers = GetRenderer(_pupilTypeOptions, _eyelidTypeIndex);

        ApplyColor(_skinColorOptions, _skinColorIndex, _skinRenderers);
        ApplyColor(_hatColorOptions, _hatColorIndex, _hatRenderers);
        ApplyColor(_skinColorOptions, _skinColorIndex, _eyelidRenderers);
        ApplyColor(_pupilColorOptions, _pupilColorIndex, _pupilRenderers);
        ApplyColor(_noseColorOptions, _noseColorIndex, _noseRenderers);
        ApplyColor(_mouthColorOptions, _mouthColorIndex, _mouthRenderers);
        ApplyColor(_shirtColorOptions, _shirtColorIndex, _shirtRenderers);
        ApplyColor(_pantColorOptions, _pantColorIndex, _pantRenderers);
        ApplyColor(_shoeColorOptions, _shoeColorIndex, _shoeRenderers);
    }

    #region Helper Methods
    private int NextIndex(int currentIndex, int optionCount)
    {
        currentIndex++;

        if (currentIndex >= optionCount)
        {
            currentIndex = 0;
        }

        return currentIndex;
    }

    private int PreviousIndex(int currentIndex, int optionCount)
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = optionCount - 1;
        }

        return currentIndex;
    }
    #endregion


    #region Setter Methods
    public void SetSkinColor(int index)
    {
        _skinColorIndex = index;
        ApplyColor(_skinColorOptions, _skinColorIndex, _skinRenderers);
        ApplyColor(_skinColorOptions, _skinColorIndex, _eyelidRenderers);
    }

    public void SetHatColor(int index)
    {
        _hatColorIndex = index;
        ApplyColor(_hatColorOptions, _hatColorIndex, _hatRenderers);
    }

    public void SetEyeColor(int index)
    {
        _pupilColorIndex = index;
        ApplyColor(_pupilColorOptions, _pupilColorIndex, _pupilRenderers);
    }

    public void SetNoseColor(int index)
    {
        _noseColorIndex = index;
        ApplyColor(_noseColorOptions, _noseColorIndex, _noseRenderers);
    }

    public void SetMouthColor(int index)
    {
        _mouthColorIndex = index;
        ApplyColor(_mouthColorOptions, _mouthColorIndex, _mouthRenderers);
    }

    public void SetShirtColor(int index)
    {
        _shirtColorIndex = index;
        ApplyColor(_shirtColorOptions, _shirtColorIndex, _shirtRenderers);
    }

    public void SetPantColor(int index)
    {
        _pantColorIndex = index;
        ApplyColor(_pantColorOptions, _pantColorIndex, _pantRenderers);
    }

    public void SetShoeColor(int index)
    {
        _shoeColorIndex = index;
        ApplyColor(_shoeColorOptions, _shoeColorIndex, _shoeRenderers);
    }
    #endregion

    #region Next/Previous Methods 
    public void NextSkinType()
    {
        _skinTypeIndex = NextIndex(_skinTypeIndex, _skinTypeOptions.Length);
        ApplyOption(_skinTypeOptions, _skinTypeIndex);
        _skinRenderers = GetRenderer(_skinTypeOptions, _skinTypeIndex);
    }

    public void PreviousSkinType()
    {
        _skinTypeIndex = PreviousIndex(_skinTypeIndex, _skinTypeOptions.Length);
        ApplyOption(_skinTypeOptions, _skinTypeIndex);
        _skinRenderers = GetRenderer(_skinTypeOptions, _skinTypeIndex);
    }

    public void NextHatType()
    {
        _hatTypeIndex = NextIndex(_hatTypeIndex, _hatTypeOptions.Length);
        ApplyOption(_hatTypeOptions, _hatTypeIndex);
        _hatRenderers = GetRenderer(_hatTypeOptions, _hatTypeIndex);
    }

    public void PreviousHatType()
    {
        _hatTypeIndex = PreviousIndex(_hatTypeIndex, _hatTypeOptions.Length);
        ApplyOption(_hatTypeOptions, _hatTypeIndex);
        _hatRenderers = GetRenderer(_hatTypeOptions, _hatTypeIndex);
    }

    public void NextEyeType()
    {
        _eyelidTypeIndex = NextIndex(_eyelidTypeIndex, _eyelidTypeOptions.Length);
        ApplyOption(_eyelidTypeOptions, _eyelidTypeIndex);
        _eyelidRenderers = GetRenderer(_eyelidTypeOptions, _eyelidTypeIndex);
        _pupilRenderers = GetRenderer(_pupilTypeOptions, _eyelidTypeIndex);
    }

    public void PreviousEyeType()
    {
        _eyelidTypeIndex = PreviousIndex(_eyelidTypeIndex, _eyelidTypeOptions.Length);
        ApplyOption(_eyelidTypeOptions, _eyelidTypeIndex);
        _eyelidRenderers = GetRenderer(_eyelidTypeOptions, _eyelidTypeIndex);
        _pupilRenderers = GetRenderer(_pupilTypeOptions, _eyelidTypeIndex);
    }

    public void NextNoseType()
    {
        _noseTypeIndex = NextIndex(_noseTypeIndex, _noseTypeOptions.Length);
        ApplyOption(_noseTypeOptions, _noseTypeIndex);
        _noseRenderers = GetRenderer(_noseTypeOptions, _noseTypeIndex);
    }

    public void PreviousNoseType()
    {
        _noseTypeIndex = PreviousIndex(_noseTypeIndex, _noseTypeOptions.Length);
        ApplyOption(_noseTypeOptions, _noseTypeIndex);
        _noseRenderers = GetRenderer(_noseTypeOptions, _noseTypeIndex);
    }

    public void NextMouthType()
    {
        _mouthTypeIndex = NextIndex(_mouthTypeIndex, _mouthTypeOptions.Length);
        ApplyOption(_mouthTypeOptions, _mouthTypeIndex);
        _mouthRenderers = GetRenderer(_mouthTypeOptions, _mouthTypeIndex);
    }

    public void PreviousMouthType()
    {
        _mouthTypeIndex = PreviousIndex(_mouthTypeIndex, _mouthTypeOptions.Length);
        ApplyOption(_mouthTypeOptions, _mouthTypeIndex);
        _mouthRenderers = GetRenderer(_mouthTypeOptions, _mouthTypeIndex);
    }

    public void NextShirtType()
    {
        _shirtTypeIndex = NextIndex(_shirtTypeIndex, _shirtTypeOptions.Length);
        ApplyOption(_shirtTypeOptions, _shirtTypeIndex);
        _shirtRenderers = GetRenderer(_shirtTypeOptions, _shirtTypeIndex);
    }

    public void PreviousShirtType()
    {
        _shirtTypeIndex = PreviousIndex(_shirtTypeIndex, _shirtTypeOptions.Length);
        ApplyOption(_shirtTypeOptions, _shirtTypeIndex);
        _shirtRenderers = GetRenderer(_shirtTypeOptions, _shirtTypeIndex);
    }

    public void NextPantType()
    {
        _pantTypeIndex = NextIndex(_pantTypeIndex, _pantTypeOptions.Length);
        ApplyOption(_pantTypeOptions, _pantTypeIndex);
        _pantRenderers = GetRenderer(_pantTypeOptions, _pantTypeIndex);
    }

    public void PreviousPantType()
    {
        _pantTypeIndex = PreviousIndex(_pantTypeIndex, _pantTypeOptions.Length);
        ApplyOption(_pantTypeOptions, _pantTypeIndex);
        _pantRenderers = GetRenderer(_pantTypeOptions, _pantTypeIndex);
    }

    public void NextShoeType()
    {
        _shoeTypeIndex = NextIndex(_shoeTypeIndex, _shoeTypeOptions.Length);
        ApplyOption(_shoeTypeOptions, _shoeTypeIndex);
        _shoeRenderers = GetRenderer(_shoeTypeOptions, _shoeTypeIndex);
    }

    public void PreviousShoeType()
    {
        _shoeTypeIndex = PreviousIndex(_shoeTypeIndex, _shoeTypeOptions.Length);
        ApplyOption(_shoeTypeOptions, _shoeTypeIndex);
        _shoeRenderers = GetRenderer(_shoeTypeOptions, _shoeTypeIndex);
    }
    #endregion
}
