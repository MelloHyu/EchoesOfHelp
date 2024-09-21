using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echolocation : MonoBehaviour
{
   [SerializeField] private AudioLoudnessDetection detector;
    [SerializeField] private float minScale = 0.4f;
    [SerializeField] private Vector2 maxScale;
    [SerializeField] private float loudnessSensibility = 100f;
    [SerializeField] private float audioThreshold = 0.1f;
    [SerializeField] private float maxLoudness = 30f;
    public AnimationCurve curve;
    private void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness < audioThreshold)
        {
            loudness = 0;
        }

        UpscaleRadiusOfEcho(loudness);

        //lerp value from minScale to maxScale dpending on loudness
    }


    private float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        float xy = (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        if (float.IsNaN(xy))
        {
            return 0;
        }
        return xy;
    }

    private void UpscaleRadiusOfEcho(float loudness)
    {
        float range = map(loudness, 0, maxLoudness, minScale, 1);
        float sc = curve.Evaluate(range);
        
        transform.localScale = (Vector2.one*sc*maxScale);
    }

}
