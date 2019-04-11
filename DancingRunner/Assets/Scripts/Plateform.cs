using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateform : MonoBehaviour
{
    public Color PlateformColor;

    private new Renderer renderer;
    private new Collider collider;

    private bool isOutFading;
    private bool isEnable;
    private const float OUT_FADING_TIME = 1;
    private float outFadingCurrentTime;
    
    /// <summary>
    /// Init variables
    /// </summary>
    private void Start()
    {
        isOutFading = false;

        PlateformColor.a = 0;

        collider = GetComponent<BoxCollider>();

        renderer = GetComponent<Renderer>();
        renderer.material.color = PlateformColor;

        PlateformColor.a = 1;
    }

    /// <summary>
    /// Check if the bloc highlither is triggered
    /// Highlight the triggered plateform
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlateformHighlighter")
        {
            isEnable = LevelManager.CurrentPossibleColor.Contains(PlateformColor);

            if (isEnable)
            {
                isOutFading = false;
                Color color = renderer.material.color;
                renderer.material.color = new Color(color.r, color.g, color.b, 1);
            }
        }
    }

    /// <summary>
    /// Check if the bloc highlither is triggered
    /// Enable out fading
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlateformHighlighter")
        {
            if(isEnable)
            {
                isOutFading = true;
                outFadingCurrentTime = 0;
            }
        }
    }
    
    private void Update()
    {
        UpdateAlphaColor();
        UpdateCollider();
    }

    /// <summary>
    /// Ignore collision between this plateform and all the players if
    /// the alpha channel of this plateform is equal or under 0
    /// </summary>
    private void UpdateCollider()
    {
        Color color = renderer.material.color;
        
        foreach (GameObject player in LevelManager.Players)
        {
            Physics.IgnoreCollision(player.GetComponent<Collider>(), collider, color.a <= 0);
        }
    }

    /// <summary>
    /// Manage the alpha channel of the plateform mat.
    /// </summary>
    private void UpdateAlphaColor()
    {
        if (isOutFading)
        {
            Color color = renderer.material.color;

            outFadingCurrentTime += Time.deltaTime;

            float outFadingCurrentPercent = outFadingCurrentTime / OUT_FADING_TIME;

            float alpha = Mathf.Lerp(1, 0, outFadingCurrentPercent);
            renderer.material.color = new Color(color.r, color.g, color.b, alpha);

            if (outFadingCurrentPercent > 1)
            {
                isOutFading = false;
            }
        }
    }
}
