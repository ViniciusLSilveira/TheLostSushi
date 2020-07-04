using UnityEngine;

public class BeeHive : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Partícula das abelhas saindo da colméia")]
    private GameObject m_BeeEmission;
    [SerializeField]
    [Tooltip("Particula do enxame de abelhas")]
    private GameObject m_BeeSwarm;

    enum Directions {
        Left,
        Right
    }

    [SerializeField]
    [Tooltip("Direção que as abelhas sairam da colméia")]
    private Directions m_Direction = Directions.Left;

    [SerializeField]
    [Tooltip("Posição que a particula do emissão de abelhas irá nascer")]
    private Transform m_BeeEmissionSpawn;
    
    [SerializeField]
    [Tooltip("Posição que a particula do enxame de abelhas irá nascer")]
    private Transform m_BeeSwarmSpawn;

    [SerializeField]
    [Tooltip("Tempo que a colméia demorará para spawnar o enxame")]
    private float m_DelayToSpawnSwarm = 0.3f;
    private GameObject m_Target;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Vector2 direction = Vector2.zero;
            switch (m_Direction) {
                case Directions.Left:
                    direction = new Vector2(0, -90);
                    m_BeeSwarmSpawn.position = new Vector3(transform.position.x - 1, transform.position.y - 0.097f);
                    break;
                case Directions.Right:
                    direction = new Vector2(0, 90);
                    m_BeeSwarmSpawn.position = new Vector3(transform.position.x + 1, transform.position.y - 0.097f);
                    break;
            }
            Instantiate(m_BeeEmission, m_BeeEmissionSpawn.position, Quaternion.Euler(direction));
            m_Target = collision.gameObject;
            Invoke("SpawnPursuerBees", m_DelayToSpawnSwarm);
        }
    }

    private void SpawnPursuerBees() {
        GameObject bees = Instantiate(m_BeeSwarm, m_BeeSwarmSpawn.position, Quaternion.identity);
        if(m_Target) bees.GetComponent<Pursuer>().SetObjToPursue(m_Target);
    }
}
