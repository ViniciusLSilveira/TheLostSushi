using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raio : MonoBehaviour
{

    public LayerMask m_Mask;

    private void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 10.0f);
        if (hit.collider != null) {
            Debug.Log(hit.distance);
            Debug.DrawRay(transform.position, Vector2.right * 10.0f, Color.green);
        }
        else {
            Debug.DrawRay(transform.position, Vector2.right * 10.0f, Color.red);
        }
    }

}
