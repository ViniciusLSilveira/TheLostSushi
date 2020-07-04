using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class DisappearingPlatform : MonoBehaviour
{
    [Tooltip("Tempo para o objeto sumir")]
    public float m_TimeToDestroy = 1.0f;

    [SerializeField]
    [Tooltip("Tempo que leva para o objeto aparecer quando sumir")]
    private float m_TimeToAppearAgain = 5.0f;

    private float m_Time = 0f;
    private bool m_Disappeared;
    
    private SpriteRenderer m_PlatformToDisappear;
    private Collider2D[] colliders;

    private void Start()
    {
        m_PlatformToDisappear = GetComponent<SpriteRenderer>();
        colliders = m_PlatformToDisappear.GetComponents<Collider2D>();
    }

    IEnumerator DestroyPlatform()
    {
        SetAlpha(0.5f);

        yield return new WaitForSeconds(m_TimeToDestroy);
        
        m_Disappeared = true;
    }

    private void Update()
    {
        if (!m_Disappeared) return;

        foreach (Collider2D collider in colliders) {
            collider.enabled = false;
        }

        m_PlatformToDisappear.enabled = false;
        m_Time += Time.deltaTime;
        if(m_Time >= m_TimeToAppearAgain)
        {
            m_Disappeared = false;
        }

        if (m_Disappeared == false)
        {
            foreach (Collider2D collider in colliders) {
                collider.enabled = true;
            }
            m_PlatformToDisappear.enabled = true;
            SetAlpha(1f);
            m_Time = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            StartCoroutine(DestroyPlatform());
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = m_PlatformToDisappear.color;

        color.a = alpha;

        m_PlatformToDisappear.color = color;
    }
}
