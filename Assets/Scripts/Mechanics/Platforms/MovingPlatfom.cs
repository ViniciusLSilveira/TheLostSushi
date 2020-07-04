using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttachPlatform))]
public class MovingPlatfom : MonoBehaviour
{

    [Header("Spawn Point")]
    public Vector2 m_SpawnPoint;
    [Header("Moving Points")]
    public Transform[] m_Points;
    public float m_Speed = 1.0f;
    public float m_Distance = 0.01f;
    public float m_TimeToNextPoint = 1.0f;

    // Controladores
    private bool m_Waiting;
    private int m_CurrentPoint;
    private int m_Direction = 1;

    private void Start() {
        foreach (Transform point in m_Points) {
            point.transform.parent = null;
        }
    }

    private void Update() {
        if (!m_Waiting) {
            Vector2 targetPoint = m_Points[m_CurrentPoint].position;
            Debug.DrawLine(transform.position, targetPoint, Color.red);
            if (Vector2.Distance(transform.position, targetPoint) <= m_Distance) {
                m_Waiting = true;
                transform.position = targetPoint;
                Invoke("NextPoint", m_TimeToNextPoint);
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPoint, Time.deltaTime * m_Speed);
        }
    }

    public void NextPoint() {
        m_CurrentPoint += m_Direction;

        if (m_CurrentPoint == m_Points.Length || m_CurrentPoint < 0) {
            m_Direction *= -1;
            m_CurrentPoint += m_Direction * 2;
        }

        m_Waiting = false;
    }

}
