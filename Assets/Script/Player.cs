using System;
using System.Collections;
using Event;
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
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private float attackCd = 1.0f;
    [SerializeField] private int health = 100;
    [SerializeField] private int strength = 10;
    [SerializeField] private float freezeTime = 1.0f;
    private static readonly Vector2 AttackDirection = Vector2.up;
    public float leftAccelerate = 1.0f, rightAccelerate = 1.0f;
    private static readonly JSONObject HurtMsgFormat = new JSONObject("{\"playerName\":\"yee\" , \"health\":87 , \"dmg\":87}");
    private IEnumerator _hurt(int dmg)
    {
        print($"{gameObject.name} hurt for {dmg}");
        health -= dmg;
        if (health <= 0)
        {
            GameRound.Instance.EndGame();
        }

        var jSonObject = HurtMsgFormat.Copy();
        jSonObject["playerName"].str =  this.gameObject.name;
        jSonObject["health"].n = health;
        jSonObject["dmg"].n = dmg;
        _eventManager.InvokeEvent("playerHurt" , jSonObject);
        yield return null;
    }
    private IEnumerator _attack()
    {
        var buffer = new RaycastHit2D[10];
        var position = transform.position;
        var items = Physics2D.RaycastNonAlloc(position,AttackDirection,buffer,attackRange);
        
        for (var i = 0; i < items; i++)
        {
            if (!buffer[i].collider.gameObject.CompareTag("Player")) continue;
            var entity = buffer[i].collider.gameObject.GetComponent<Player>();
            if(entity.gameObject == gameObject) continue;
            entity.StartCoroutine(nameof(_hurt) , strength);
        }
        yield return new WaitForSeconds(attackCd);
        _triggered = false;
    }

    private IEnumerator _freeze()
    {
        var oldScales = new[]
        {
            moveScale,
            jumpScale
        };
        moveScale = 0;
        jumpScale = 0;
        _triggered = true;
        yield return new WaitForSeconds(freezeTime);
        moveScale = oldScales[0];
        jumpScale = oldScales[1];
        _triggered = false;
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
    public IEnumerator ResetSpeed()
    {
        print("reset speed");
        leftAccelerate = 1;
        rightAccelerate = 1;
        yield break;
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
    private EventManager _eventManager;
    private void Awake()
    { 
        _rb = GetComponent<Rigidbody2D>();
        _eventManager = EventManager.GetInstance();
        _eventManager.RegisterEvent("swap"  , (s, o) =>
        {
            _rb.gravityScale *= -1;
            _click = (_click == nameof(_jump)) ? nameof(_attack) : nameof(_jump);
        });
    }

    private void Update()
    {
        if (controller.OnClicked() && !_triggered)
        {
            _triggered = true;
            StartCoroutine(_click);
        }
        var move = controller.OnMove();
        if (move != Vector2.zero)
        {
            if (move == Vector2.left)
                move *= leftAccelerate;
            else if(move == Vector2.right)
                move *= rightAccelerate;
            
            transform.Translate(move * moveScale);
        }
    }

    private bool _touchedGround;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Platform"))
            _touchedGround = true;

    }
}