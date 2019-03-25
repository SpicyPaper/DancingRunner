using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateform : MonoBehaviour
{
    private Renderer renderer;
    private bool isOutFading;
    private bool isAnimated;
    private const float OUT_FADING_TIME = 2;
    private float outFadingCurrentTime;

    // Start is called before the first frame update
    void Start()
    {
        isOutFading = false;
        isAnimated = false;
        renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BlocHighlighter")
        {
            isOutFading = false;
            Color color = renderer.material.color;
            renderer.material.color = new Color(color.r, color.g, color.b, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BlocHighlighter")
        {
            isOutFading = true;
            isAnimated = true;
            outFadingCurrentTime = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Color color = renderer.material.color;

        if(isAnimated)
        {
            outFadingCurrentTime += Time.deltaTime;

            float outFadingCurrentPercent = outFadingCurrentTime / OUT_FADING_TIME;

            if (isOutFading)
            {
                float alpha = Mathf.Lerp(1, 0, outFadingCurrentPercent);
                Debug.Log(alpha);
                renderer.material.color = new Color(color.r, color.g, color.b, alpha);
            }

            if(outFadingCurrentPercent > 1)
            {
                isAnimated = false;
            }
        }
        else
        {
            renderer.material.color = new Color(color.r, color.g, color.b, 0);
        }
    }
}
