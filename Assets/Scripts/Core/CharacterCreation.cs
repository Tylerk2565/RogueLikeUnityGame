using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour
{
    #region Renderer References
    private Renderer[] _skinRenderers;
    private Renderer[] _eyeRenderers;
    private Renderer[] _mouthRenderers;
    private Renderer[] _noseRenderers;
    private Renderer[] _shirtRenderers;
    private Renderer[] _pantRenderers;
    private Renderer[] _beltRenderers;
    private Renderer[] _shoeRenderers;
    private Renderer[] _hatRenderers;
    #endregion

    #region Customization Options
    [Header("Customization Options")]
    [SerializeField] private Color[] _skinColorOptions;
    [SerializeField] private Color[] _eyeColorOptions;
    [SerializeField] private Color[] _shirtColorOptions;
    [SerializeField] private Color[] _pantColorOptions;
    [SerializeField] private Color[] _beltColorOptions;
    [SerializeField] private Color[] _shoeColorOptions;

    [SerializeField] private GameObject[] _skinTypeOptions;
    [SerializeField] private GameObject[] _hatTypeOptions;
    [SerializeField] private GameObject[] _eyeTypeOptions;
    [SerializeField] private GameObject[] _noseTypeOptions;
    [SerializeField] private GameObject[] _mouthTypeOptions;
    [SerializeField] private GameObject[] _shirtTypeOptions;
    [SerializeField] private GameObject[] _pantTypeOptions;
    [SerializeField] private GameObject[] _beltTypeOptions;
    [SerializeField] private GameObject[] _shoeTypeOptions;
    #endregion

    #region Current Selection Indexes
    private int _skinColorIndex = 0;
    private int _skinTypeIndex = 0;
    private int _hatTypeIndex = 0;
    private int _eyeTypeIndex = 0;
    private int _eyeColorIndex = 0;
    private int _noseTypeIndex = 0;
    private int _mouthTypeIndex = 0;
    private int _shirtTypeIndex = 0;
    private int _shirtColorIndex = 0;
    private int _pantTypeIndex = 0;
    private int _pantColorIndex = 0;
    private int _beltTypeIndex = 0;
    private int _beltColorIndex = 0;
    private int _shoeTypeIndex = 0;
    private int _shoeColorIndex = 0;
    #endregion

    private void Start()
    {
        ResetCharacter();
    }


    public void CreateCharacter()
    {
        PlayerPrefs.SetInt("SkinColor", _skinColorIndex);
        PlayerPrefs.SetInt("SkinType", _skinTypeIndex);
        PlayerPrefs.SetInt("HatType", _hatTypeIndex);
        PlayerPrefs.SetInt("EyeType", _eyeTypeIndex);
        PlayerPrefs.SetInt("EyeColor", _eyeColorIndex);
        PlayerPrefs.SetInt("NoseType", _noseTypeIndex);
        PlayerPrefs.SetInt("MouthType", _mouthTypeIndex);
        PlayerPrefs.SetInt("ShirtType", _shirtTypeIndex);
        PlayerPrefs.SetInt("ShirtColor", _shirtColorIndex);
        PlayerPrefs.SetInt("PantType", _pantTypeIndex);
        PlayerPrefs.SetInt("PantColor", _pantColorIndex);
        PlayerPrefs.SetInt("BeltType", _beltTypeIndex);
        PlayerPrefs.SetInt("BeltColor", _beltColorIndex);
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
        _eyeTypeIndex = 0;
        _eyeColorIndex = 0;
        _noseTypeIndex = 0;
        _mouthTypeIndex = 0;
        _shirtTypeIndex = 0;
        _shirtColorIndex = 0;
        _pantTypeIndex = 0;
        _pantColorIndex = 0;
        _beltTypeIndex = 0;
        _beltColorIndex = 0;
        _shoeTypeIndex = 0;
        _shoeColorIndex = 0;

        ApplyCustomization();
    }

    public void RotateCharacter()
    {

    }

    private void ApplyCustomization()
    {
        ApplyOption(_skinTypeOptions, _skinTypeIndex);
        ApplyOption(_hatTypeOptions, _hatTypeIndex);
        ApplyOption(_eyeTypeOptions, _eyeTypeIndex);
        ApplyOption(_noseTypeOptions, _noseTypeIndex);
        ApplyOption(_mouthTypeOptions, _mouthTypeIndex);
        ApplyOption(_shirtTypeOptions, _shirtTypeIndex);
        ApplyOption(_pantTypeOptions, _pantTypeIndex);
        ApplyOption(_beltTypeOptions, _beltTypeIndex);
        ApplyOption(_shoeTypeOptions, _shoeTypeIndex);

        _skinRenderers = GetRenderer(_skinTypeOptions, _skinTypeIndex);
        _eyeRenderers = GetRenderer(_eyeTypeOptions, _eyeTypeIndex);
        _mouthRenderers = GetRenderer(_mouthTypeOptions, _mouthTypeIndex);
        _noseRenderers = GetRenderer(_noseTypeOptions, _noseTypeIndex);
        _shirtRenderers = GetRenderer(_shirtTypeOptions, _shirtTypeIndex);
        _pantRenderers = GetRenderer(_pantTypeOptions, _pantTypeIndex);
        _beltRenderers = GetRenderer(_beltTypeOptions, _beltTypeIndex);
        _shoeRenderers = GetRenderer(_shoeTypeOptions, _shoeTypeIndex);
        _hatRenderers = GetRenderer(_hatTypeOptions, _hatTypeIndex);

        ApplyColor(_skinColorOptions, _skinColorIndex, _skinRenderers);
        ApplyColor(_eyeColorOptions, _eyeColorIndex, _eyeRenderers);
        ApplyColor(_shirtColorOptions, _shirtColorIndex, _shirtRenderers);
        ApplyColor(_pantColorOptions, _pantColorIndex, _pantRenderers);
        ApplyColor(_beltColorOptions, _beltColorIndex, _beltRenderers);
        ApplyColor(_shoeColorOptions, _shoeColorIndex, _shoeRenderers);
    }

    #region Helper Methods
    private Renderer[] GetRenderer(GameObject[] options, int index)
    {
        return options[index].GetComponentsInChildren<Renderer>();
    }

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

    private void ApplyOption(GameObject[] options, int selectedIndex)
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].SetActive(i == selectedIndex);
        }
    }

    private void ApplyColor(Color[] colors, int selectedIndex, Renderer[] renderers)
    {
        var currentColor = colors[selectedIndex];
        foreach (var renderer in renderers)
        {
            renderer.material.color = currentColor;
        }
    }
    #endregion

    #region Next/Previous Methods 
    public void NextSkinColor()
    {
        _skinColorIndex = NextIndex(_skinColorIndex, _skinColorOptions.Length);
        ApplyColor(_skinColorOptions, _skinColorIndex, _skinRenderers);
    }

    public void PreviousSkinColor()
    {
        _skinColorIndex = PreviousIndex(_skinColorIndex, _skinColorOptions.Length);
        ApplyColor(_skinColorOptions, _skinColorIndex, _skinRenderers);
    }

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
        _eyeTypeIndex = NextIndex(_eyeTypeIndex, _eyeTypeOptions.Length);
        ApplyOption(_eyeTypeOptions, _eyeTypeIndex);
        _eyeRenderers = GetRenderer(_eyeTypeOptions, _eyeTypeIndex);
    }

    public void PreviousEyeType()
    {
        _eyeTypeIndex = PreviousIndex(_eyeTypeIndex, _eyeTypeOptions.Length);
        ApplyOption(_eyeTypeOptions, _eyeTypeIndex);
        _eyeRenderers = GetRenderer(_eyeTypeOptions, _eyeTypeIndex);
    }

    public void NextEyeColor()
    {
        _eyeColorIndex = NextIndex(_eyeColorIndex, _eyeColorOptions.Length);
        ApplyColor(_eyeColorOptions, _eyeColorIndex, _eyeRenderers);
    }

    public void PreviousEyeColor()
    {
        _eyeColorIndex = PreviousIndex(_eyeColorIndex, _eyeColorOptions.Length);
        ApplyColor(_eyeColorOptions, _eyeColorIndex, _eyeRenderers);
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

    public void NextShirtColor()
    {
        _shirtColorIndex = NextIndex(_shirtColorIndex, _shirtColorOptions.Length);
        ApplyColor(_shirtColorOptions, _shirtColorIndex, _shirtRenderers);
    }

    public void PreviousShirtColor()
    {
        _shirtColorIndex = PreviousIndex(_shirtColorIndex, _shirtColorOptions.Length);
        ApplyColor(_shirtColorOptions, _shirtColorIndex, _shirtRenderers);
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

    public void NextPantColor()
    {
        _pantColorIndex = NextIndex(_pantColorIndex, _pantColorOptions.Length);
        ApplyColor(_pantColorOptions, _pantColorIndex, _pantRenderers);
    }

    public void PreviousPantColor()
    {
        _pantColorIndex = PreviousIndex(_pantColorIndex, _pantColorOptions.Length);
        ApplyColor(_pantColorOptions, _pantColorIndex, _pantRenderers);
    }

    public void NextBeltType()
    {
        _beltTypeIndex = NextIndex(_beltTypeIndex, _beltTypeOptions.Length);
        ApplyOption(_beltTypeOptions, _beltTypeIndex);
        _beltRenderers = GetRenderer(_beltTypeOptions, _beltTypeIndex);
    }

    public void PreviousBeltType()
    {
        _beltTypeIndex = PreviousIndex(_beltTypeIndex, _beltTypeOptions.Length);
        ApplyOption(_beltTypeOptions, _beltTypeIndex);
        _beltRenderers = GetRenderer(_beltTypeOptions, _beltTypeIndex);
    }

    public void NextBeltColor()
    {
        _beltColorIndex = NextIndex(_beltColorIndex, _beltColorOptions.Length);
        ApplyColor(_beltColorOptions, _beltColorIndex, _beltRenderers);
    }

    public void PreviousBeltColor()
    {
        _beltColorIndex = PreviousIndex(_beltColorIndex, _beltColorOptions.Length);
        ApplyColor(_beltColorOptions, _beltColorIndex, _beltRenderers);
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

    public void NextShoeColor()
    {
        _shoeColorIndex = NextIndex(_shoeColorIndex, _shoeColorOptions.Length);
        ApplyColor(_shoeColorOptions, _shoeColorIndex, _shoeRenderers);
    }

    public void PreviousShoeColor()
    {
        _shoeColorIndex = PreviousIndex(_shoeColorIndex, _shoeColorOptions.Length);
        ApplyColor(_shoeColorOptions, _shoeColorIndex, _shoeRenderers);
    }
    #endregion
}
