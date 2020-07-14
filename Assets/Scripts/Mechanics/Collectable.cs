using Newtonsoft.Json.Linq;
using UnityEngine;

public class Collectable : MonoBehaviour, ISerializable {
    enum CollectableType {
        Points,
        Life
    }

    [SerializeField]
    [Tooltip("O tipo de coletável")]
    CollectableType m_Collectable = CollectableType.Points;

    private bool m_CanCollect = true;

    public bool m_Collected;

    private void Start() {
        StateManager.Instance.Register(this);
    }

    public string GetKey() {
        return name;
    }

    public JObject Serialize() {
        SaveData data = new SaveData(m_Collected, transform.position);
        string json = JsonUtility.ToJson(data);
        return JObject.Parse(json);
    }

    public void Deserialize(string json) {
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        m_Collected = data.collected;
        transform.position = data.position;
        if (m_Collected) gameObject.SetActive(false);
    }

    private class SaveData {
        public bool collected;
        public Vector3 position;

        public SaveData(bool collected, Vector3 position) {
            this.collected = collected;
            this.position = position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Player p = collision.gameObject.GetComponent<Player>();
            switch (m_Collectable) {
                case CollectableType.Points:
                    p.AddPoints(1);
                    break;
                case CollectableType.Life:
                    m_CanCollect = p.RegenerateHealth(1);
                    break;
            }

            m_Collected = m_CanCollect;
            gameObject.SetActive(!m_CanCollect);
        }
    }

}
