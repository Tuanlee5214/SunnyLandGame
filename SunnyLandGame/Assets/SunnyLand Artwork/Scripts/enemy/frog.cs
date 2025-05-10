using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog : MonoBehaviour
{
    public GameObject fx;

    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;

    private float rightBound;
    private float leftBound;

    private bool isFacingLeft = true;
    private Rigidbody2D rd;
    private Animator anim;

    private float _timeJump = 1.5f;
    private float _timeFirst = 0f;

    private float previousY;




    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rightBound = this.transform.position.x + 4.25f;
        leftBound = this.transform.position.x - 4.25f;
        previousY = this.transform.position.y;
    }

    void Update()
    {
        _timeFirst += Time.deltaTime;
        if (_timeFirst >= _timeJump)
        {
            Jump();
            _timeFirst = 0f;
        }

        CheckJumpOrFall();
    }

    public void Jump()
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
                rd.velocity = new Vector2(-jumpLength, jumpHeight);
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
                rd.velocity = new Vector2(jumpLength, jumpHeight);
            }    
        }
    }

    public void Move()
    {
        Debug.Log("Move function called from Animation Event");
    }

    public void CheckJumpOrFall()
    {
        if (transform.position.y < previousY)
        {
            //Cập nhật giá trị biến IsFalling trong animation, giúp chuyển đổi animation
            anim.SetBool("Fall", true);
            //cập nhật biến đặt cờ để sử dụng song song cho phù hợp
            anim.SetBool("Jump", false);
        }
        else if (transform.position.y > previousY)
        {
            anim.SetBool("Jump", true);
            anim.SetBool("Fall", false);
        }
        //cập nhật vị trí của Y sau mỗi frame để so sánh đưa ra kết quả
        previousY = transform.position.y;
    }
    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("Idle", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("Idle", false);
        }
    }
}