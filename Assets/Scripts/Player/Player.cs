using UnityEngine;

public class Player : MonoBehaviour {
    public int Life { get; private set; }

    private int m_Points;

    [SerializeField]
    [Tooltip("Quanto de vida o jogador tem")]
    private int m_MaxLife = 3;

    [SerializeField]
    [Tooltip("Posição em que o jogador retorna quando morrer")]
    private Transform m_Spawn;

    private void Awake() {
        Life = m_MaxLife;
        if (m_Spawn == null) m_Spawn = GameObject.Find("Spawn").transform;
    }

    private void Start() {
        if (m_Spawn) transform.position = m_Spawn.position;
        LifeUIManager.Instance.UpdateHeartUI(Life);

        if (FindObjectOfType<PlayMenuMusic>()) Destroy(FindObjectOfType<PlayMenuMusic>().gameObject);

        PointsUIManager.Instance.UpdatePointsUI(m_Points);
    }

    public void TakeDamage(int damage) {
        Life -= damage;
        if (Life <= 0) {
            Life = 0;
            Die();
        }
        LifeUIManager.Instance.UpdateHeartUI(Life);
    }

    public bool RegenerateHealth(int amount) {
        Life += amount;

        if (Life > m_MaxLife) {
            Life = m_MaxLife;
            return false;
        }

        LifeUIManager.Instance.UpdateHeartUI(Life);

        return true;
    }

    public void AddPoints(int points) {
        m_Points += points;
        PointsUIManager.Instance.UpdatePointsUI(m_Points);
    }

    public void SetPlayerMaxPoints() {
        PlayerPrefs.SetInt("Points", PlayerPrefs.GetInt("Points", 0) + m_Points);
    }

    private void Die() {
        Life = 3;
        ResetPosition();
    }

    private void ResetPosition() {
        transform.position = m_Spawn.transform.position;
    }

}
