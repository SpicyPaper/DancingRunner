using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateform : MonoBehaviour
{
    private new Renderer renderer;
    private bool isOutFading;
    private const float OUT_FADING_TIME = 1;
    private float outFadingCurrentTime;
    
    /// <summary>
    /// Init variables
    /// </summary>
    private void Start()
    {
        isOutFading = false;
        renderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// Check if the bloc highlither is triggered
    /// Highlight the triggered plateform
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BlocHighlighter")
        {
            isOutFading = false;
            Color color = renderer.material.color;
            renderer.material.color = new Color(color.r, color.g, color.b, 1);
        }
    }

    /// <summary>
    /// Check if the bloc highlither is triggered
    /// Enable out fading
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BlocHighlighter")
        {
            isOutFading = true;
            outFadingCurrentTime = 0;
        }
    }
    
    /// <summary>
    /// Manage the alpha channel of the plateform mat.
    /// </summary>
    private void Update()
    {
        Color color = renderer.material.color;

        if(isOutFading)
        {
            outFadingCurrentTime += Time.deltaTime;

            float outFadingCurrentPercent = outFadingCurrentTime / OUT_FADING_TIME;

            float alpha = Mathf.Lerp(1, 0, outFadingCurrentPercent);
            renderer.material.color = new Color(color.r, color.g, color.b, alpha);

            if(outFadingCurrentPercent > 1)
            {
                isOutFading = false;
            }
        }
    }
}
