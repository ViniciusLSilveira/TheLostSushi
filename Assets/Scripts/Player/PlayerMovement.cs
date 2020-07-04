﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour {
    [Header("Move")]
    public float m_Speed = 8.0f;

    [Header("Mobile")]
    [SerializeField]
    private GameObject m_MobileCanvas;
    [SerializeField]
    private Joystick m_Joystick;

    private Vector2 m_Movement;

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
    private float m_JumpForce = 900.0f;
    [SerializeField]
    private float m_JumpTime = 0.33f;

    private bool m_IsGrounded;
    private bool m_IsJumping;
    private bool m_FacingRight;

    // Variavel para determinar se o jogador pode se mexer
    private bool m_LockMove = false;

    // Componente que contém as informações de renderização do objeto
    private SpriteRenderer m_Renderer;

    // Componente que controla as animações do objeto
    private Animator m_Animator;

    // Objeto que controla toda a física do corpo
    private Rigidbody2D m_Body;

    private void Start() {
        // Pega o componente SpriteRenderer atrelado ao objeto
        m_Renderer = GetComponent<SpriteRenderer>();

        // Pega o componente Rigidbody atrelado ao objeto
        m_Body = GetComponent<Rigidbody2D>();
        // Congela a rotação por fisica do objeto
        m_Body.freezeRotation = true;

        // Pega o componente Animator atrelado ao objeto
        m_Animator = GetComponent<Animator>();

        // Habilita o canvas que permite a movimentação do jogador
        if (m_MobileCanvas) m_MobileCanvas.SetActive(true);

        // Se não tiver joystick associado, pegar qualquer joystick da cena
        if (!m_Joystick) m_Joystick = GameObject.FindObjectOfType<Joystick>();

        // Garantir que quando a fase iniciar o player pode se mover
        m_LockMove = false;
    }

    private void Update() {
        if (m_LockMove || PauseGame.m_Paused) return;

        m_IsGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(m_Feet.position, m_GroundCheckSize, 0.0f, m_GroundLayer);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject)
                m_IsGrounded = true;
        }

        m_Movement.x = m_Joystick.Horizontal;

        if (m_Movement.x > 0 && m_FacingRight) {
            Rotate();
        }

        if (m_Movement.x < 0 && !m_FacingRight) {
            Rotate();
        }
    }

    private void FixedUpdate() {
        if (m_LockMove) {
            m_Animator.SetFloat("Speed", 0.0f);
            m_Body.velocity = new Vector2(0.0f, m_Body.velocity.y);
            return;
        }
        Move();
        Animate();
    }

    private void Move() {
        m_Body.velocity = new Vector2(m_Speed * m_Movement.x, m_Body.velocity.y);
    }

    private void Rotate() {
        // Troca a variavel booleana que determina a direção que o player está olhando.
        m_FacingRight = !m_FacingRight;

        // Troca a orientação em X do sprite
        m_Renderer.flipX = !m_Renderer.flipX;
    }

    public void Jump(bool jump) {
        m_IsJumping = jump;
        
        if (!m_IsGrounded) return;
        
        m_Body.AddForce(Vector2.up * m_JumpForce);

        m_IsJumping = false;
    }

    private void Animate() {
        m_Animator.SetFloat("Speed", Mathf.Abs(m_Body.velocity.x));
        m_Animator.SetBool("Jump", m_IsJumping);
    }

    public void LockPlayerMovement(bool locked) {
        m_LockMove = locked;
    }
}