using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour {
    [Header("Move")]
    public float m_Speed = 8.0f;

    public Vector2 m_CurrentSpeed;
    private Vector2 m_Movement;

    public float m_CurrentDistance;
    public float m_MaxDistance = float.MinValue;

    [Header("Ground")]
    [SerializeField]
    [Tooltip("Layer que o objeto interpretará como chão")]
    private LayerMask m_GroundLayer;
    [SerializeField]
    [Tooltip("Tamanho da caixa para verificar se está no chão")]
    private Vector2 m_GroundCheckSize = new Vector2(1f, 0.05f);
    [SerializeField]
    [Tooltip("Transform para indicar onde é o pé do objeto")]
    private Transform m_Feet;

    [Header("Jump")]
    [SerializeField]
    private float m_JumpForce = 100.0f;

    [Header("Controllers")]
    public bool m_IsGrounded;
    public bool m_IsJumping;
    private bool m_FacingRight;

    // Variavel para determinar se o jogador pode se mexer
    private bool m_LockMove = false;

    // Variável para otimizar as operações de transform do objeto
    private Transform m_Transform;

    // Componente que contém as informações de renderização do objeto
    private SpriteRenderer m_Renderer;

    // Componente que controla as animações do objeto
    private Animator m_Animator;

    // Objeto que controla toda a física do corpo
    private Rigidbody2D m_Body;

    private void Start() {
        // Pega o componente Transform atrelado ao objeto
        m_Transform = GetComponent<Transform>();

        // Pega o componente SpriteRenderer atrelado ao objeto
        m_Renderer = GetComponent<SpriteRenderer>();

        // Pega o componente Rigidbody atrelado ao objeto
        m_Body = GetComponent<Rigidbody2D>();
        // Garantir que o player possa se mover
        m_Body.constraints = RigidbodyConstraints2D.None;
        // Congela a rotação por fisica do objeto
        m_Body.freezeRotation = true;

        // Pega o componente Animator atrelado ao objeto
        m_Animator = GetComponent<Animator>();

        // Garantir que quando a fase iniciar o player pode se mover
        m_LockMove = false;

    }

    private void Update() {
        if (m_LockMove) return;

        m_IsGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(m_Feet.position, m_GroundCheckSize, 0.0f, m_GroundLayer);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject)
                m_IsGrounded = true;
        }

        m_Movement.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && m_IsGrounded) {
            m_IsJumping = true;
            m_IsGrounded = false;
        }

        if (m_Body.velocity.x > 0 && m_FacingRight) {
            Rotate();
        }

        if (m_Body.velocity.x < 0 && !m_FacingRight) {
            Rotate();
        }
    }

    private void FixedUpdate() {
        if (m_LockMove) {
            m_Animator.SetFloat("Speed", 0.0f);
            return;
        }
        Animate();
        CalculateMaxDistance();
    }

    public void Move(float movement) {
        m_Body.velocity = new Vector2(m_Speed * movement, m_Body.velocity.y);
        m_CurrentDistance = m_Transform.position.x - GameManager.Instance.m_Spawn.position.x;
        m_CurrentSpeed = m_Body.velocity;
    }

    public void Rotate() {
        // Troca a variavel booleana que determina a direção que o player está olhando.
        m_FacingRight = !m_FacingRight;

        // Troca a orientação em X do sprite
        m_Renderer.flipX = !m_Renderer.flipX;
    }

    public void Jump(float multiplier) {
        m_Body.velocity = new Vector2(m_Body.velocity.x, 0.0f);
        m_Body.AddForce(Vector2.up * m_JumpForce * multiplier);
        m_IsJumping = false;
    }

    private void CalculateMaxDistance()
    {
        if(m_CurrentDistance > m_MaxDistance) {
            m_MaxDistance = m_CurrentDistance;
        }
    }

    private void Animate() {
        m_Animator.SetFloat("Speed", Mathf.Abs(m_Body.velocity.x));
        m_Animator.SetBool("Jump", m_IsJumping);
    }

    public void LockPlayerMovement(bool locked) {
        m_LockMove = locked;
        m_Body.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}