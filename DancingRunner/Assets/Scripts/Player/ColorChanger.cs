using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to a player used to change its color
/// </summary>
public class ColorChanger : MonoBehaviour
{
    public int CurrentIndex = 0;

    private Color[] colors = { Color.red, Color.green, Color.blue };

    SkinnedMeshRenderer skinRenderer;

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        skinRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        ChangeColor();
    }

    /// <summary>
    /// Change the player color with the next color in the list
    /// </summary>
    public void ChangeColor()
    {
        Color color = colors[CurrentIndex++ % colors.Length];
        skinRenderer.material.SetColor("_Color", color);
        skinRenderer.material.SetColor("_EmissionColor", color * 0.75f);
    }

    /// <summary>
    /// Get the current color of the player
    /// </summary>
    /// <returns></returns>
    public Color GetColor()
    {
        return skinRenderer.material.GetColor("_Color");
    }
}
