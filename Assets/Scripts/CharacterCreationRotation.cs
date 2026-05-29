using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationRotation : MonoBehaviour
{
    [SerializeField] private Slider _rotationSlider;
    [SerializeField] private Transform _characterModel;

    private float _targetRotation;
    private float _currentRotation;
    private float _lerpSpeed = 5.0f;
    private float[] _snapAngles = { 0.0f, 90.0f, 180.0f, 270.0f, 360.0f };
    

    // snap points 0 90 180 270 360

    private void Start()
    {
        _targetRotation = _characterModel.eulerAngles.y;
        _currentRotation = _characterModel.eulerAngles.y;
        _rotationSlider.value = _currentRotation;
    }

    private void Update()
    {
        _currentRotation = Mathf.Lerp(_currentRotation, _targetRotation, Time.deltaTime * _lerpSpeed);
        _characterModel.rotation = Quaternion.Euler(0, _currentRotation, 0);
    }
       
    public void WhileDragging()
    {
        _targetRotation = _rotationSlider.value;
    }

    public void OnRelease()
    {
        var nearestSnapPoint = _snapAngles[0];
        for (int i = 0; i < _snapAngles.Length; i++)
        {
            var distance = Mathf.Abs(_currentRotation - _snapAngles[i]);
            if (distance < Mathf.Abs(_currentRotation - nearestSnapPoint))
            {
                nearestSnapPoint = _snapAngles[i];
            }
        }
        _targetRotation = nearestSnapPoint;
        _rotationSlider.value = _targetRotation;
    }
}
