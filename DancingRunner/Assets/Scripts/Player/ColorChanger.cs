using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Color[] colors = { Color.red, Color.green, Color.blue };
    private int currentIndex = 0;

    SkinnedMeshRenderer skinRenderer;
    // Start is called before the first frame update
    void Start()
    {
        skinRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        ChangeColor();
    }

    public void ChangeColor()
    {
        Color color = colors[currentIndex++ % colors.Length];
        skinRenderer.material.SetColor("_Color", color);
        skinRenderer.material.SetColor("_EmissionColor", color);
    }

    public Color GetColor()
    {
        return skinRenderer.material.GetColor("_Color");
    }
}
