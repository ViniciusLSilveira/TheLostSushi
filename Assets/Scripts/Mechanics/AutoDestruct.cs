using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Tempo que o objeto se auto-destroirá")]
    private float m_TimeToAutoDestruct;

    private void OnEnable() {
        Destroy(this.gameObject, m_TimeToAutoDestruct);
    }

}
