using Newtonsoft.Json.Linq;
using UnityEngine;

public class Player : MonoBehaviour, ISerializable {
    public int Life { get; private set; }

    private int m_Points;

    [SerializeField]
    [Tooltip("Quanto de vida o jogador tem")]
    private int m_MaxLife = 3;

    [SerializeField]
    [Tooltip("Posição em que o jogador retorna quando morrer")]
    private Transform m_Spawn;

    private Transform m_LastCheckpoint;

    private void Awake() {
        Life = m_MaxLife;
        if (m_Spawn == null) m_Spawn = GameObject.Find("Spawn").transform;
    }

    private void Start() {
        StateManager.Instance.Register(this);

        if (m_Spawn) transform.position = m_Spawn.position;
        if (!m_LastCheckpoint) m_LastCheckpoint = m_Spawn;

        if (FindObjectOfType<PlayMenuMusic>()) Destroy(FindObjectOfType<PlayMenuMusic>().gameObject);

        LifeUIManager.Instance.UpdateHeartUI(Life);
        PointsUIManager.Instance.UpdatePointsUI(m_Points);
    }

    public string GetKey() {
        return name;
    }

    public JObject Serialize() {
        SaveData data = new SaveData(Life, m_Points, m_LastCheckpoint.position);
        string json = JsonUtility.ToJson(data);
        return JObject.Parse(json);
    }

    public void Deserialize(string json) {
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Life = data.life;
        m_Points = data.points;
        transform.position = data.checkpoint;
        if (Life <= 0) {
            m_LastCheckpoint = m_Spawn;
        }
    }

    private class SaveData {
        public int life;
        public int points;
        public Vector3 checkpoint;

        public SaveData(int life, int points, Vector3 checkpoint) {
            this.life = life;
            this.points = points;
            this.checkpoint = checkpoint;
        }
    }

    public void TakeDamage(int damage) {
        Life -= damage;
        if (Life <= 0) {
            Life = 0;
            Die();
        }
        LifeUIManager.Instance.UpdateHeartUI(Life);
        ResetPosition();
    }

    public bool RegenerateHealth(int amount) {
        Life += amount;

        if (Life > m_MaxLife) {
            Life = m_MaxLife;
            LifeUIManager.Instance.UpdateHeartUI(Life);
            return amount != 1;
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

    public void SetLastCheckPoint(Transform checkpoint) {
        m_LastCheckpoint = checkpoint;
    }

    private void Die() {
        Life = 3;
        m_LastCheckpoint = m_Spawn;
        ResetPosition();
    }

    private void ResetPosition() {
        transform.position = m_LastCheckpoint.transform.position;
    }

}
