using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    public static AudioLoudnessDetection Instance
    {
        get;
        private set;
    }

    [SerializeField] private int sampleWindow = 64;
    private AudioClip microphoneClip;

    private void Start()
    {
        MicrophoneToAudioClip();
    }


    public void MicrophoneToAudioClip()
    {
        //Get the first microphone in the device list
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }


    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if(startPosition<0)
        {
            return 0;
        }

        float[] waveData = new float[sampleWindow];
        
        clip.GetData(waveData, startPosition);
        
        float totalLoudness = 0f;

        for(int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);

        }

        return (totalLoudness / sampleWindow);

    }
}


