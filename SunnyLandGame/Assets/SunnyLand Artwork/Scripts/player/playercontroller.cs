using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Playercontroller : MonoBehaviour
{
    public float tocdo;
    public float luc_nhay;
    public float crouchSpeed;

    public float climbingSpeed;
    private bool isFacingRight = true;
    private float trai_phai;
    private float trai_phai1;
    private float trai_phai2;

    private float vertical;
    private float vertical1;
    private float vertical2;
    private Rigidbody2D rd;
    private Animator anim;

    private GameManager gameManager;
    private bool isGrounded;
    private bool isClimbing = false;
    //biến này isCrouch kiểm tra có đang ngồi hay không
    private bool isCrouch = false;
    private float previousY;
    private bool isJumping = false;
    private bool isFalling = false;
    private float hurtForce = 10f;
    private bool IsHurt;
    private float jumpCount;

    public Joystick joystick;
    private float moveHorizontal;
    private float moveVertical;

    public GameObject fx;
    private AudioManager audioManager;
    


    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        previousY = transform.position.y;
        audioManager = FindAnyObjectByType<AudioManager>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        //Lấy giá trị theo chiều ngang trục x, giao động từ -1 đến 1
        if (!IsHurt)
        {
            trai_phai = Input.GetAxis("Horizontal");
            rd.velocity = new Vector2(trai_phai * tocdo, rd.velocity.y);
            MoveByJoystick();
        }

        //cập nhật tốc độ theo chiều ngang giữ nguyên chiều dọc, 
        //sau khi dừng lại joystick nhân vật có xu hướng chậm dần đều, thêm 
        //2 dòng với biến trai_phai thì sau khi không sử dụng joystick thì 
        //nhân vật sẽ dừng lại ngay vì biến trai_phai phụ thuộc bàn phím
        //CHÚ Ý ĐẶT 2 DÒNG CODE ĐẦU TRƯỚC HÀM MOVEBYJOYSTICK KHÔNG THÌ NHÂN VẬT KHÔNG DI CHUYỂN ĐƯỢC
        //Flip();//kiểm tra quay đầu
        Flip1();
        anim.SetFloat("move", Mathf.Abs(moveHorizontal));

        //Cập nhật giá trị cho move, giúp chuyển animation từ đứng sang chạy và ngược lại
        //anim.SetFloat("move", Mathf.Abs(trai_phai));

        //Cập nhật cho biến cờ isGrounded
        if (isGrounded)
        {
            anim.SetBool("IsGrounded", true);
            rd.gravityScale = 2;
            jumpCount = 0;
        }
        else
        {
            anim.SetBool("IsGrounded", false);
        }

        // Giusp nhân vật leo thang được
        Climbing();

        //Kiểm tra nhảy hay rơi để chuyển trạng thái cho phù hợp 
        CheckJumpOrFall();

        //phần này là để chuyển từ đứng sang ngồi, từ ngồi sang đứng, nhảy và chạy.
        //TransformStatus();

        //phần này giúp cho nhân vật có thể nhảy bằng việc thay đổi vận tốc theo chiều dọc khi nhấn nút hướng lên 
        Jumping1();

        //phần này chuyển trạng thái từ chạy sang rơi khi nhân vật đang đi trên đá thì rớt xuống vách
        RunningtoFalling();

    }

    public bool GetIsFalling()
    {
        return isFalling;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }
    void MoveByJoystick()
    {
        moveHorizontal = joystick.Horizontal;
        moveVertical = joystick.Vertical;
       
        Vector2 joystickInput = new Vector2(moveHorizontal, 0);
        
        if (Mathf.Abs(joystickInput.x) > Mathf.Abs(joystickInput.y))
        {
            rd.velocity = new Vector2(moveHorizontal * tocdo, rd.velocity.y);
        }
        if ((Mathf.Abs(joystickInput.x) > Mathf.Abs(joystickInput.y) && isCrouch && isGrounded && !isClimbing) ||
                (Mathf.Abs(joystickInput.x) > Mathf.Abs(joystickInput.y) && isCrouch && isGrounded && !isClimbing))
        {
            trai_phai1 = Input.GetAxis("Horizontal");
            rd.velocity = new Vector2(tocdo * trai_phai1, rd.velocity.y);
            anim.SetBool("IsRun]", true);
            anim.SetBool("IsCrouch", false);
            isCrouch = false;
        }
    }


    void Climbing()
    {
        if (isClimbing)
        {
            rd.gravityScale = 0;
            Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);
            if (Mathf.Abs(joystickInput.y) > Mathf.Abs(joystickInput.x))
            {
                anim.SetBool("IsOnLadder", true);
                if (joystickInput.y > 0)
                {
                    vertical2 = 1; // Lên
                }
                else if (joystickInput.y < 0)
                {
                    vertical2 = -1; // Xuống
                }
                rd.velocity = new Vector2(rd.velocity.x, vertical2 * climbingSpeed);
                anim.SetFloat("len_xuong", Mathf.Abs(climbingSpeed * vertical2));
            }
            else
            {
                rd.velocity = new Vector2(rd.velocity.x, 0);
                anim.SetFloat("len_xuong", Mathf.Abs(0));

            }
        }
        else
        {
            anim.SetBool("IsOnLadder", false);
            rd.gravityScale = 2;
        }
    }
    public void TransformStatus()
    {
        if (!isCrouch && isGrounded && !isClimbing)
        {
            anim.SetBool("IsCrouch", true);
            anim.SetBool("IsJump]", false);
            anim.SetBool("IsRun]", false);
            isCrouch = true;
        }
    }
    void Flip()
    {   //Kiểm tra xem có đối nghịch để quay đầu, 
        if (isFacingRight && trai_phai < 0 || !isFacingRight && trai_phai > 0)
        {
            isFacingRight = !isFacingRight;
            //tạo ra biến kich_thuoc rồi gán giá trị x là số đối, nó sẽ tự động quay đầu
            Vector3 kich_thuoc = transform.localScale;
            kich_thuoc.x = kich_thuoc.x * -1;
            //cập nhật lại giá trị localScale
            transform.localScale = kich_thuoc;
        }
    }
    void Flip1()
    {   //Kiểm tra xem có đối nghịch để quay đầu, 
        if (isFacingRight && moveHorizontal < 0 || !isFacingRight && moveHorizontal > 0)
        {
            isFacingRight = !isFacingRight;
            //tạo ra biến kich_thuoc rồi gán giá trị x là số đối, nó sẽ tự động quay đầu
            Vector3 kich_thuoc = transform.localScale;
            kich_thuoc.x = kich_thuoc.x * -1;
            //cập nhật lại giá trị localScale
            transform.localScale = kich_thuoc;
        }
    }

    //Hàm kiểm tra mỗi khi xảy ra va chạm, có phải là chạm vào phần có tag là "Ground" hay không
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("isGrounded true");
            //audioManager.PlayOntheGroundSound();
        }
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("bullet"))
        {
            if(anim.GetBool("IsFalling") && !anim.GetBool("IsGrounded"))
            {
                GameObject fx1 = Instantiate(fx, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
                audioManager.PlayKillEnemySound();
                Destroy(fx1, 0.5f);
                anim.SetBool("isOnEnemy", true);
                anim.SetBool("isJumping", true);
                rd.velocity = new Vector2(rd.velocity.x, 10f);
                if(SceneManager.GetActiveScene().name == "Level1")
                {
                   gameManager.MinusEnemy(1);
                }
                else if(SceneManager.GetActiveScene().name == "Level2")
                {
                    gameManager.MinusEnemy1(1);
                }
            }
            else
            {
                if (!GetIsCrouch())
                {
                    anim.SetBool("isHurt", true);
                    IsHurt = true;
                    audioManager.PlayHurtSound();
                    if (collision.gameObject.transform.position.x > this.transform.position.x)
                    {
                        Debug.Log("hurt");
                        rd.velocity = new Vector2(-10, rd.velocity.y);
                    }
                    else
                    {
                        rd.velocity = new Vector2(10, rd.velocity.y);
                        Debug.Log("hurt");
                    }
                    gameManager.MinusHealth(1);
                }
            }
        }
        if(collision.gameObject.CompareTag("boss") && !isFalling)
        {
            anim.SetBool("isHurt", true);
            IsHurt = true;
            audioManager.PlayHurtSound();
                if (collision.gameObject.transform.position.x > this.transform.position.x)
                {
                    Debug.Log("hurt");
                    rd.velocity = new Vector2(-10, rd.velocity.y);
                }
                else
                {
                    rd.velocity = new Vector2(10, rd.velocity.y);
                    Debug.Log("hurt");
                }
                gameManager.MinusHealth(1);
        }
    }

    private void ChangeIsHurtValue()
    {
        IsHurt = false;
    }    

    //Hàm kiểm tra mỗi lần rời đi một vật, có phải rời đi khỏi vật có tag là "Ground" hay không
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("ground");
        }
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("boss") || collision.gameObject.CompareTag("bullet"))
        {
            Invoke("ChangeAnim", 0.25f);
            Invoke("ChangeIsHurtValue", 0.5f);
        }
        
    }
    
    private void ChangeAnim()
    {
        anim.SetBool("isHurt", false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered: " + other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            isClimbing = true;
            anim.SetBool("IsClimbing", true);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsJump]", false);
            Debug.Log("isClimbing true");
            //isGrounded = false;
        }
        if (other.gameObject.CompareTag("flag1") && !gameManager.GetIsLevelComplete())
        {
            gameManager.LevelComplete();
        }
        if(other.gameObject.CompareTag("flag2"))
        {
            gameManager.LevelComplete();
        }
        
    
    
            /*else if (other.gameObject.CompareTag("cherry"))
            {
                GameObject fx1 = Instantiate(fx, gameObject.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                Destroy(fx1, 0.5f);
                gameManager.AddForce1(1);

            }
            else if (other.gameObject.CompareTag("diamond"))
            {
                GameObject fx1 = Instantiate(fx, gameObject.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                Destroy(fx1, 0.5f);
                gameManager.AddForce2(1);
            }*/
    }

    private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
            {
                isClimbing = false;
                anim.SetBool("IsClimbing", false);
                Debug.Log("isClimbing false");
                //isGrounded = true;
            }
        }

        //Hàm kiểm tra trạng thái nhảy hay rơi để chuyển animation của nhân vật cho đúng 
        //phụ thuộc vào sự thay đổi tung độ sau mỗi frame, liên tục so sánh nên được đặt ở trong hàm 
        //update()
        private void CheckJumpOrFall()
        {
            if (transform.position.y < previousY && !isClimbing)
            {
                //Cập nhật giá trị biến IsFalling trong animation, giúp chuyển đổi animation
                anim.SetBool("IsFalling", true);
                //cập nhật biến đặt cờ để sử dụng song song cho phù hợp
                isFalling = true;
                anim.SetBool("IsJumping", false);
                isJumping = false;
            }
            else if (transform.position.y > previousY && !isClimbing)
            {
                anim.SetBool("IsJumping", true);
                isJumping = true;
                anim.SetBool("IsFalling", false);
                isFalling = false;
            }
            //cập nhật vị trí của Y sau mỗi frame để so sánh đưa ra kết quả
            previousY = transform.position.y;
        }
    
        public void Jumping()
        {
            if (isGrounded && !isClimbing && jumpCount < 1)
            {
                rd.velocity = new Vector2(rd.velocity.x, luc_nhay);
                audioManager.PlayJumpSound();
                jumpCount++;
            }
            if(!isGrounded && !isClimbing && jumpCount < 1)
            {
                rd.velocity = new Vector2(rd.velocity.x, luc_nhay);
                audioManager.PlayJumpSound();
                jumpCount++;
            }
            if (isCrouch && isGrounded && !isClimbing)
            {
                rd.velocity = new Vector2(rd.velocity.x, luc_nhay);
                anim.SetBool("IsJump]", true);
                anim.SetBool("IsCrouch", false);
                isCrouch = false;
            }
        }

    public void Jumping1()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isClimbing && jumpCount < 1)
        {
            rd.velocity = new Vector2(rd.velocity.x, luc_nhay);
            audioManager.PlayJumpSound();
            jumpCount++;
            
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isGrounded && !isClimbing && jumpCount < 1)
        {
            rd.velocity = new Vector2(rd.velocity.x, luc_nhay);
            audioManager.PlayJumpSound();
            jumpCount++;
        }
    }

    void RunningtoFalling()
    {
            if ((Input.GetKeyDown(KeyCode.RightArrow) && !isGrounded && isFalling && !isClimbing) ||
                (Input.GetKeyDown(KeyCode.LeftArrow) && !isGrounded && isFalling && !isClimbing))
            {
                anim.SetBool("IsGrounded", false);
                anim.SetBool("IsFalling", true);
            }
    }

    public bool GetIsCrouch()
    {
        return isCrouch;
    }
}