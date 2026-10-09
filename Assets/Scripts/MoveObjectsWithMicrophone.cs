using UnityEngine;

public class MoveObjectsWithMicrophone : MonoBehaviour
{
    public VoiceDetection detector;

    public float loudnessSensitivity = 60;
    public float threshold = 0.3f;

    [Tooltip("Direction the object moves when voice is active in Move Platforms mode.")]
    public Vector3 moveDirection = Vector3.up;

    public float moveSpeed = 0.5f;

    [Tooltip("Maximum distance from the start position the object can travel.")]
    public float maxDistance = 5f;

    [Tooltip("Seconds of silence before the object returns to its start position.")]
    public float silenceBeforeReturn = 2f;

    [Tooltip("How quickly the object returns to its start position.")]
    public float returnSpeed = 3f;

    Vector3 m_startPosition;
    float m_silenceTimer;

    void Start()
    {
        m_startPosition = transform.position;
    }

    void Update()
    {
        if (detector == null)
            return;

        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensitivity;
        bool hasNoise = loudness >= threshold;

        if (hasNoise)
        {
            m_silenceTimer = 0f;

            if (VoiceControl.CurrentMode != VoiceControlMode.MovePlatforms)
                return;

            Vector3 direction = moveDirection.normalized;
            Vector3 nextPosition = transform.position + direction * moveSpeed * loudness * Time.deltaTime;

            float distanceFromStart = Vector3.Distance(m_startPosition, nextPosition);
            if (distanceFromStart > maxDistance)
                nextPosition = m_startPosition + direction * maxDistance;

            transform.position = nextPosition;
            return;
        }

        m_silenceTimer += Time.deltaTime;

        if (m_silenceTimer >= silenceBeforeReturn && transform.position != m_startPosition)
            transform.position = Vector3.MoveTowards(transform.position, m_startPosition, returnSpeed * Time.deltaTime);
    }
}
