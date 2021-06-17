using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRepeater : MonoBehaviour
{
    public AnimationCurve pitch = AnimationCurve.Constant(0, 1, 1);
    public AnimationCurve delay = AnimationCurve.Constant(0, 1, 1);
    public AnimationCurve volume = AnimationCurve.Constant(0, 1, 1);
    
    bool isQueued = false;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //Find audio source component
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if no audio is already playing or waiting to play
        if (!audioSource.isPlaying && !isQueued)
        {
            //Play audio
            isQueued = true;
            audioSource.pitch = Sample(pitch);
            audioSource.PlayDelayed(Sample(delay));
        }
        else if (audioSource.isPlaying)
        {
            isQueued = false;
        }
    }

    private float Sample(AnimationCurve curve)
        => curve.Evaluate(Random.Range(0, curve.length));
}
