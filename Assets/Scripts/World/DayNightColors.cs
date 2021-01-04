using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightColors : MonoBehaviour
{
    private Color sunColor;
    private Color sunsetColor;

    [SerializeField]
    private Material skyMat;

    private Light lt;
    
    // Start is called before the first frame update
    void Start()
    {
        lt = this.GetComponent<Light>();

        sunColor = skyMat.GetColor("_sunColor");
        sunsetColor = skyMat.GetColor("_sunsetColor");

            
            //Shader.GetGlobalColor("_sunColor");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (SunOverHead() > 0)
        {
            lt.color = Color.Lerp(sunColor, sunsetColor, SunAtHorizon());
        }
        else
        {
            lt.color = Color.Lerp(sunsetColor, Color.black, SunAtHorizon());
        }
    }

    private float SunAtHorizon()
    {
        return Mathf.Pow(1.0f - Mathf.Abs(SunOverHead()), 4);
    }

    private float SunOverHead()
    {
        return -Vector3.Dot(transform.forward, Vector3.up);
    }
    


}
