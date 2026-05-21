using UnityEngine;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private Renderer _playerRenderer;

    public void SetRed()
    {
        _playerRenderer.material.color = Color.red;
    }

    public void SetOrange()
    {
        _playerRenderer.material.color = Color.orange;
    }

    public void SetYellow()
    {
        _playerRenderer.material.color = Color.yellow;
    }

    public void SetGreen()
    {
        _playerRenderer.material.color = Color.green;
    }

    public void SetBlue()
    {
        _playerRenderer.material.color = Color.blue;
    }

    public void SetIndigo()
    {
        _playerRenderer.material.color = Color.indigo;
    }

    public void SetViolet()
    {
        _playerRenderer.material.color = Color.violet;
    }
}
