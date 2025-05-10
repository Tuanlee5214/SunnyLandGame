using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dog : MonoBehaviour
{
    public GameObject fx;
    private Playercontroller Playercontroller;



    private float rightBound;
    private float leftBound;

    private bool isFacingLeft = true;
    private Rigidbody2D rd;
    private Animator anim;

    private float _timeMove = 1f;
    private float _timeFirst = 0f;

    private float _timeMove1 = 1f;
    private float _timeFirst1 = 0f;

    private float tocdo = 6.4f;
    private Vector2 direction;
    private bool isPlayer = false;
    private float previousX;



    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rightBound = this.transform.position.x + 4f;
        leftBound = this.transform.position.x - 4f;
        Playercontroller = FindObjectOfType<Playercontroller>();
        previousX = this.transform.position.x;
    }

    private void FixedUpdate()
    {
        if (Playercontroller != null)
        {
            direction = new Vector2(Playercontroller.transform.position.x - this.transform.position.x, 0).normalized;
        }
        CheckIsPlayer();
        CheckMoveLeftOrRight();
        _timeFirst += Time.deltaTime;
        _timeFirst1 += Time.deltaTime;
        if (_timeFirst >= _timeMove && !isPlayer)
        {
            Move();
            _timeFirst = 0f;
        }
        if (_timeFirst1 >= _timeMove1 && isPlayer)
        {
            Move1();
            _timeFirst1 = 0f;
        }
        anim.SetFloat("move", Mathf.Abs(rd.velocity.x));
    }
    void Update()
    {

    }

    private void Move()
    {
        if (isFacingLeft)
        {
            if (this.transform.position.x <= leftBound)
            {
                isFacingLeft = false;
                Flip();
            }
            else
            {
                rd.velocity = new Vector2(-tocdo, rd.velocity.y);
            }

        }
        else
        {
            if (this.transform.position.x >= rightBound)
            {
                isFacingLeft = true;
                Flip();
            }
            else
            {
                rd.velocity = new Vector2(tocdo, rd.velocity.y);
            }
        }
    }

    private void Move1()
    {
        Vector3 scaler = transform.localScale;
        if (Playercontroller.transform.position.x < this.transform.position.x)
        {
            rd.velocity = new Vector2(-tocdo, rd.velocity.y);
            if (this.transform.localScale.x < 0)
            {
                scaler.x *= -1;
                transform.localScale = scaler;
            }
        }
        else if (Playercontroller.transform.position.x > this.transform.position.x)
        {
            rd.velocity = new Vector2(tocdo, rd.velocity.y);
            if (this.transform.localScale.x > 0)
            {
                scaler.x *= -1;
                transform.localScale = scaler;
            }
        }
    }
    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void CheckIsPlayer()
    {
        if (Playercontroller.transform.position.x > (leftBound - 10) && Playercontroller.transform.position.x < (rightBound + 10))
        {
            isPlayer = true;
        }
        else isPlayer = false;
    }

    void CheckMoveLeftOrRight()
    {
        Vector3 scaler = transform.localScale;
        if (transform.position.x > previousX)
        {
            if (scaler.x > 0)
            {
                scaler.x *= -1;
                transform.localScale = scaler;
            }
        }
        else if (transform.position.x < previousX)
        {
            if (scaler.x < 0)
            {
                scaler.x *= -1;
                transform.localScale = scaler;
            }
        }
        previousX = transform.position.x;
    }
}
