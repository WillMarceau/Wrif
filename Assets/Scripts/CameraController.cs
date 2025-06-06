// NO LONGER IN USE

/*
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;

    [Range(0.1f, 10f)]
    public float mouseSensitivity = 2f;

    void Start()
    {
        if (freeLookCam == null)
        {
            freeLookCam = GetComponent<CinemachineFreeLook>();
        }

        ApplySensitivity();
    }

    void OnValidate()
    {
        // Automatically updates sensitivity when changed in inspector
        ApplySensitivity();
    }

    public void ApplySensitivity()
    {
        if (freeLookCam != null)
        {
            freeLookCam.m_XAxis.m_MaxSpeed = 300f * mouseSensitivity;
            freeLookCam.m_YAxis.m_MaxSpeed = 2f * mouseSensitivity;
        }
    }

    // Optional: Expose for a UI slider
    public void SetSensitivity(float value)
    {
        mouseSensitivity = value;
        ApplySensitivity();
    }
}
*/