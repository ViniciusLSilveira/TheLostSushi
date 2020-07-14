using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Velocidade em que a plataforma irá girar")]
    private float m_RotationSpeed;

    private enum RotationType {
        ClockWise = 1,
        AntiClockWise = -1
    }

    [SerializeField]
    [Tooltip("Direção em que a plataforma irá girar")]
    private RotationType m_Orientation = RotationType.ClockWise;

    private void FixedUpdate() {
        RotatePlatformGroup();
    }

    private void RotatePlatformGroup() {
        int direction = (int)m_Orientation;
        transform.Rotate(Vector3.back * m_RotationSpeed * direction * Time.deltaTime, Space.World);
    }

}
