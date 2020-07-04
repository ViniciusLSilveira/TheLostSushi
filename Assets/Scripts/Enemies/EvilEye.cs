using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilEye : Enemy {

    [SerializeField]
    [Tooltip("Transform do raio de perseguição do inimigo para determinar quando parar de perseguir se chegar em um buraco")]
    private Transform m_PursuingRadiusTransform;

    [SerializeField]
    [Tooltip("Os pontos que o inimigo vai patrulhar")]
    private Transform[] m_Points = new Transform[2];

    [SerializeField]
    [Tooltip("A distância para o inimigo ir para o próximo ponto marcado")]
    private float m_Distance = 0.01f;

    [SerializeField]
    [Tooltip("O Tempo para o inimigo esperar no ponto")]
    private float m_TimeToNextPoint = 1.0f;

    // Controladores
    private int m_CurrentPoint;
    private int m_Direction = 1;
    private bool m_Waiting;
    private bool m_Pursuing = false;

    private Vector2 m_LeftGroundCheck;
    private Vector2 m_RightGroundCheck;

    private void Start() {
        foreach (Transform point in m_Points) {
            point.transform.parent = null;
        }
    }

    private void Update() {
        CalculateGround();

        if (m_Pursuing || m_Waiting) return;

        Vector2 targetPoint = m_Points[m_CurrentPoint].position;
        Debug.DrawLine(transform.position, targetPoint, Color.red);
        if (Vector2.Distance(transform.position, targetPoint) <= m_Distance) {
            m_Waiting = true;
            transform.position = targetPoint;
            Invoke("NextPoint", m_TimeToNextPoint);
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPoint, Time.deltaTime * m_Speed);
    }

    public void NextPoint() {
        m_CurrentPoint += m_Direction;

        if (m_CurrentPoint == m_Points.Length || m_CurrentPoint < 0) {
            m_Direction *= -1;
            m_CurrentPoint += m_Direction * 2;
        }

        m_Waiting = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.gameObject.GetComponent<Player>().TakeDamage(m_Damage);
        }
    }

    private void CalculateGround() {
        m_LeftGroundCheck = m_PursuingRadiusTransform.position - GetComponent<Collider2D>().bounds.extents;
        m_LeftGroundCheck.y = m_PursuingRadiusTransform.position.y;
        m_RightGroundCheck = m_PursuingRadiusTransform.position + GetComponent<Collider2D>().bounds.extents;
        m_RightGroundCheck.y = m_PursuingRadiusTransform.position.y;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(m_LeftGroundCheck, Vector2.one * 0.1f);
        Gizmos.DrawWireCube(m_RightGroundCheck, Vector2.one * 0.1f);
    }

}
