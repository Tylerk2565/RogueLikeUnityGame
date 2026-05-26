using UnityEngine;

public class AppearanceLoader : CharacterCreationBase
{
    #region Renderer References
    [SerializeField] private Renderer[] _eyeballRenderers;
    [SerializeField] private Renderer[] _pupilRenderers; 
    private Renderer[] _skinRenderers;
    private Renderer[] _eyelidRenderers; 
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
    [SerializeField] private Color[] _hatColorOptions;
    [SerializeField] private Color[] _pupilColorOptions;
    [SerializeField] private Color[] _noseColorOptions;
    [SerializeField] private Color[] _mouthColorOptions;
    [SerializeField] private Color[] _shirtColorOptions;
    [SerializeField] private Color[] _pantColorOptions;
    [SerializeField] private Color[] _shoeColorOptions;

    [SerializeField] private GameObject[] _skinTypeOptions;
    [SerializeField] private GameObject[] _hatTypeOptions;
    [SerializeField] private GameObject[] _eyelidTypeOptions;
    [SerializeField] private GameObject[] _noseTypeOptions;
    [SerializeField] private GameObject[] _mouthTypeOptions;
    [SerializeField] private GameObject[] _shirtTypeOptions;
    [SerializeField] private GameObject[] _pantTypeOptions;
    [SerializeField] private GameObject[] _shoeTypeOptions;
    #endregion 

    private void Start()
    {
        LoadCharacter();
    }

    private void LoadCharacter()
    {
        int skinType = PlayerPrefs.GetInt("SkinType");
        int skinColor = PlayerPrefs.GetInt("SkinColor");
        int hatType = PlayerPrefs.GetInt("HatType");
        int hatColor = PlayerPrefs.GetInt("HatColor");
        int eyelidType = PlayerPrefs.GetInt("EyelidType");
        int pupilColor = PlayerPrefs.GetInt("PupilColor");
        int noseType = PlayerPrefs.GetInt("NoseType");
        int noseColor = PlayerPrefs.GetInt("NoseColor");
        int mouthType = PlayerPrefs.GetInt("MouthType");
        int mouthColor = PlayerPrefs.GetInt("MouthColor");
        int shirtType = PlayerPrefs.GetInt("ShirtType");
        int shirtColor = PlayerPrefs.GetInt("ShirtColor");
        int pantType = PlayerPrefs.GetInt("PantType");
        int pantColor = PlayerPrefs.GetInt("PantColor");
        int shoeType = PlayerPrefs.GetInt("ShoeType");
        int shoeColor = PlayerPrefs.GetInt("ShoeColor");

        ApplyOption(_skinTypeOptions, skinType);
        ApplyOption(_hatTypeOptions, hatType);
        ApplyOption(_eyelidTypeOptions, eyelidType);
        ApplyOption(_noseTypeOptions, noseType);
        ApplyOption(_mouthTypeOptions, mouthType);
        ApplyOption(_shirtTypeOptions, shirtType);
        ApplyOption(_pantTypeOptions, pantType);
        ApplyOption(_shoeTypeOptions, shoeType);

        _skinRenderers = GetRenderer(_skinTypeOptions, skinType);
        _eyelidRenderers = GetRenderer(_eyelidTypeOptions, eyelidType);
        _mouthRenderers = GetRenderer(_mouthTypeOptions, mouthType);
        _noseRenderers = GetRenderer(_noseTypeOptions, noseType);
        _shirtRenderers = GetRenderer(_shirtTypeOptions, shirtType);
        _pantRenderers = GetRenderer(_pantTypeOptions, pantType);
        _shoeRenderers = GetRenderer(_shoeTypeOptions, shoeType);
        _hatRenderers = GetRenderer(_hatTypeOptions, hatType);

        ApplyColor(_skinColorOptions, skinColor, _skinRenderers);
        ApplyColor(_skinColorOptions, skinColor, _eyelidRenderers);
        ApplyColor(_pupilColorOptions, pupilColor, _pupilRenderers);
        ApplyColor(_hatColorOptions, hatColor, _hatRenderers);
        ApplyColor(_noseColorOptions, noseColor, _noseRenderers);
        ApplyColor(_mouthColorOptions, mouthColor, _mouthRenderers);
        ApplyColor(_shirtColorOptions, shirtColor, _shirtRenderers);
        ApplyColor(_pantColorOptions, pantColor, _pantRenderers);
        ApplyColor(_shoeColorOptions, shoeColor, _shoeRenderers);
    }
}
