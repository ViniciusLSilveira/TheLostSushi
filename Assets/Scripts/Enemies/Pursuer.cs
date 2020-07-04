using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuer : Enemy {

    [SerializeField]
    [Tooltip("GameObject que perseguirá o jogador (deixe vazio caso o objeto for o mesmo que possui o script)")]
    private GameObject m_Pursuer;

    private bool m_IsPursuer = false;

    private GameObject m_ObjToPursue;
    private Rigidbody2D m_Body;

    private void Start() {
        if (m_Pursuer == null) {
            m_Pursuer = this.gameObject;
            m_IsPursuer = true;
        }

        if (m_Pursuer.GetComponent<Rigidbody2D>()) m_Body = m_Pursuer.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (m_ObjToPursue) {
            PursuePlayer(m_ObjToPursue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (m_IsPursuer) {
                collision.GetComponent<Player>().TakeDamage(m_Damage);
            }
            if (!m_ObjToPursue) {
                PursuePlayer(collision.gameObject);
            }
        }
    }

    private void PursuePlayer(GameObject target) {
        Vector2 direction = target.transform.position - m_Pursuer.transform.position;
        m_Body.MovePosition(m_Body.position + direction.normalized * m_Speed * Time.fixedDeltaTime);
    }

    public void SetObjToPursue(GameObject target) {
        m_ObjToPursue = target;
    }

}
