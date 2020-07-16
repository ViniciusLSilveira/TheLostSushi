using UnityEngine;

public class Player : MonoBehaviour
{
    public int Life { get; private set; }

    private int m_Points;

    private PlayerMovement m_PlayerMovement;

    private Transform m_Transform;

    private void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_Transform = GetComponent<Transform>();

        m_Transform.position = GameManager.Instance.m_Spawn.position;
        LifeUIManager.Instance.UpdateHeartUI(Life);

        PlayMenuMusic pmm = FindObjectOfType<PlayMenuMusic>();
        if (pmm) Destroy(pmm.gameObject);

        PointsUIManager.Instance.UpdatePointsUI(m_Points);
    }

    public void AddPoints(int points)
    {
        m_Points += points;
        PointsUIManager.Instance.UpdatePointsUI(m_Points);
    }

    public void SetPlayerMaxPoints()
    {
        PlayerPrefs.SetInt("Points", PlayerPrefs.GetInt("Points", 0) + m_Points);
    }

    public int GetPoints()
    {
        return m_Points;
    }

    public void Die()
    {
        m_PlayerMovement.LockPlayerMovement(true);
    }

}
