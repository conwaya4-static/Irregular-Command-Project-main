using UnityEngine;

public class ScaleFromMicrophone : MonoBehaviour
{
    public AudioSource audioSource;
    public Vector3 minScale;
    public Vector3 maxScale;
    public VoiceDetection detector;

    public float loudnessSensitivity = 100;
    public float threshold = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensitivity;

        if(loudness < threshold)
            loudness = 0;

        //lerp value from minScale to maxScale based on loudness
        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }

}
