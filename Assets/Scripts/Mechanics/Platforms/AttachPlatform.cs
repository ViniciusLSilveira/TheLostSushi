using UnityEngine;

public class AttachPlatform : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Objeto que terá o jogador como filho (se não tiver nenhum pode deixar vazio)")]
    private Transform m_PlayerParent;

    private bool m_IsParent = false;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player") || !m_IsParent) {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if(Mathf.Abs(rb.velocity.y) > 1) return;
            collision.transform.parent = transform;
            m_IsParent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.transform.parent = m_PlayerParent;
            m_IsParent = false;
        }
    }

}
