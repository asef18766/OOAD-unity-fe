﻿using System;
using System.Collections;
using System.Linq;
using InputControllers;
using UnityEngine;
using UUID;

public enum PlayerState
{
    Attack,
    Jump
}
[RequireComponent(typeof(Rigidbody2D))]
public class Player : UuidObject
{
    private string _click = "";
    [SerializeField] private IPlayerController controller;
    [SerializeField] private float moveScale = 0.3f;
    [SerializeField] private float jumpScale = 3.0f;
    private IEnumerator _attack()
    {
        yield break;
    }
    
    private bool _triggered;
    private IEnumerator _jump()
    {
        if(!_touchedGround)
            yield break;
        _touchedGround = false;
        _rb.AddForce(Vector2.up * jumpScale);
        while (!_touchedGround)
        {
            yield return new WaitForEndOfFrame();
        }
        _triggered = false;
    }

    public void InitPlayer(IPlayerController playerController, PlayerState state)
    {
        controller = playerController;
        switch (state)
        {
            case PlayerState.Attack:
                _click = nameof(_attack);
                _rb.gravityScale = -1;
                break;
            case PlayerState.Jump:
                _click = nameof(_jump);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private Rigidbody2D _rb;

    private void Awake()
    { 
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (controller.OnClicked() && !_triggered)
        {
            _triggered = true;
            StartCoroutine(_click);
        }
        var move = controller.OnMove();
        if(move != Vector2.zero)
            transform.Translate(move * moveScale);
    }

    private bool _touchedGround = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Platform"))
            _touchedGround = true;

    }
}