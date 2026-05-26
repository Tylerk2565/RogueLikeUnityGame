using UnityEngine;

public class CharacterCreationBase : MonoBehaviour
{
    protected void ApplyOption(GameObject[] options, int selectedIndex)
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].SetActive(i == selectedIndex);
        }
    }

    protected void ApplyColor(Color[] colors, int selectedIndex, Renderer[] renderers)
    {
        var currentColor = colors[selectedIndex];
        foreach (var renderer in renderers)
        {
            renderer.material.color = currentColor;
        }
    }
    protected Renderer[] GetRenderer(GameObject[] options, int index)
    {
        return options[index].GetComponentsInChildren<Renderer>();
    }
}
