using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    enum CollectableType {
        Points,
        Life
    }

    [SerializeField]
    [Tooltip("O tipo de coletável")]
    CollectableType m_Collectable = CollectableType.Points;

    private bool m_CanDestroy = true;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Player p = collision.gameObject.GetComponent<Player>();
            switch (m_Collectable) {
                case CollectableType.Points:
                    p.AddPoints(1);
                    break;
                case CollectableType.Life:
                    m_CanDestroy = p.RegenerateHealth(1);
                    break;
            }
            
            if(m_CanDestroy) Destroy(this.gameObject);
        }
    }

}
