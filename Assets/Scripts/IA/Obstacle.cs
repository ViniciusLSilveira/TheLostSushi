using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public Vector2 m_Size;
    public Vector3 m_CenterPosition;

    private Renderer m_Renderer;

    private void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_Size = m_Renderer.bounds.size;
        m_CenterPosition = transform.position;
    }
}
