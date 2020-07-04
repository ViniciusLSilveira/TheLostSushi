using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttachPlatform))]
public class FixOrientation : MonoBehaviour
{
    /*
    private enum OrientationDirection {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField]
    private OrientationDirection m_Orientation = OrientationDirection.Up;

    private Vector3 m_OrientationDirection; */

    private void Start() {
        /*switch (m_Orientation) {
            case OrientationDirection.Up:
                m_OrientationDirection = Vector3.up;
                break;
            case OrientationDirection.Down:
                m_OrientationDirection = Vector3.down;
                break;
            case OrientationDirection.Left:
                m_OrientationDirection = Vector3.left;
                break;
            case OrientationDirection.Right:
                m_OrientationDirection = Vector3.right;
                break;
        }*/
    }

    private void FixedUpdate() {
        transform.localRotation = Quaternion.Inverse(transform.parent.rotation);
    }


}
