using UnityEngine;

public abstract class Detection : MonoBehaviour
{
    // this script create the interface for drone detection
    public abstract void PlayerDetected(Transform Player);
    public abstract void PlayerLost();
}
