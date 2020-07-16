using UnityEngine;

public class Brain : MonoBehaviour
{
    [Header("Genetic Algorithm")]
    public int m_InputNumber = 12;
    public int m_WeightNumber = 24;
    public float[] m_Inputs;
    public float[] m_Weights;
    public float m_ActionTime = 0.05f;
    public float m_Fitness;
    public float m_SensorDistance = 50.0f;
    public LayerMask m_MaskLayer;

    [Header("Normalizer")]
    public float m_WidthNormalizer = 24.0f;
    public float m_HeightNormalizer = 10.0f;

    public bool m_Dead = false;


    private Player m_Player;
    private PlayerMovement m_PlayerMovement;
    private GameManager m_GameManager;

    private Transform m_Transform;

    private void Awake()
    {
        m_Inputs = new float[m_InputNumber];
        m_Weights = new float[m_WeightNumber];
        m_Player = GetComponent<Player>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_Transform = GetComponent<Transform>();
        m_GameManager = GameManager.Instance;
    }

    private void Start()
    {
        InvokeRepeating("Action", m_ActionTime, m_ActionTime);
    }

    private void FixedUpdate()
    {
        m_Fitness = m_PlayerMovement.m_CurrentDistance + m_Player.GetPoints();
        if (m_Fitness > m_GameManager.m_MaxFitness) {
            m_GameManager.m_MaxFitness = m_Fitness;
        }

        for (int i = 0; i < m_InputNumber; i++) {
            m_Inputs[i] = CalculateDistanceSensor(Quaternion.AngleAxis(i * 360 / m_InputNumber, Vector3.forward) * Vector2.down);
        }

        /*
            m_Inputs[0] = CalculateDistanceSensor(Vector2.up);
            m_Inputs[1] = CalculateDistanceSensor(Vector2.down);
            m_Inputs[2] = CalculateDistanceSensor(Vector2.right);
            m_Inputs[3] = CalculateDistanceSensor(Vector2.left);
            m_Inputs[4] = CalculateDistanceSensor(Quaternion.AngleAxis(-30, Vector3.forward) * Vector2.right);
            m_Inputs[5] = CalculateDistanceSensor(Quaternion.AngleAxis(30, Vector3.forward) * Vector2.right);
            m_Inputs[6] = CalculateDistanceSensor(Quaternion.AngleAxis(-60, Vector3.forward) * Vector2.right);
            m_Inputs[7] = CalculateDistanceSensor(Quaternion.AngleAxis(60, Vector3.forward) * Vector2.right);
            m_Inputs[7] = CalculateDistanceSensor(Quaternion.AngleAxis(-30, Vector3.forward) * Vector2.left);
            m_Inputs[9] = CalculateDistanceSensor(Quaternion.AngleAxis(30, Vector3.forward) * Vector2.left);
            m_Inputs[10] = CalculateDistanceSensor(Quaternion.AngleAxis(-60, Vector3.forward) * Vector2.left);
            m_Inputs[11] = CalculateDistanceSensor(Quaternion.AngleAxis(60, Vector3.forward) * Vector2.left);
        */
    }

    private float CalculateDistanceSensor(Vector2 direction)
    {
        float distance = 0.0f;

        RaycastHit2D hit = Physics2D.Raycast(m_Transform.position, direction, m_SensorDistance, m_MaskLayer);
        if (hit.collider != null)
            distance = hit.distance / m_SensorDistance;
        else
            distance = 1;

        //Debug.DrawRay(m_Transform.position, direction * (distance * m_SensorDistance), Color.red);

        return 1 - distance;
    }

    public void Jump()
    {
        if (!m_PlayerMovement.m_IsGrounded) return;

        float sum = 0.0f;
        for (int i = 0; i < m_InputNumber; i++) {
            sum += m_Inputs[i] * Mathf.Abs(m_Weights[i]);
        }

        m_PlayerMovement.Jump(sum / m_InputNumber);
    }

    public void Move()
    {
        float sum = 0.0f;
        for (int i = m_InputNumber; i < m_WeightNumber; i++) {
            sum += m_Inputs[i - m_InputNumber] * m_Weights[i];
        }

        m_PlayerMovement.Move(sum / m_InputNumber);
    }

    private void Action()
    {
        if (m_Dead) {
            m_Player.Die();
            return;
        }
        Jump();
        Move();
    }
}