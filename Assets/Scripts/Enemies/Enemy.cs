using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Dano que a unidade dará no player")]
    [SerializeField]
    protected int m_Damage;

    [Tooltip("Velocidade da unidade")]
    [SerializeField]
    protected float m_Speed;
}
