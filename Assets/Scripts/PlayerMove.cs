using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    bool inputRight = false;
    bool inputLeft = false;
    bool stop = false;
    bool inputJump = false;


    int jumpCount = 0;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();   
    }

    void Update() // 단발적인 키 입력은 Update에서 하는게 좋다.
    {
        // 좌우 키 입력 받기. 
        if (Input.GetKey(KeyCode.RightArrow)) {
            inputRight = true;
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            inputLeft = true;
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("isWalking", true);
        }

        // 키보드에서 손을 떼면 이동을 멈춤
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            stop = true;
            animator.SetBool("isWalking", false);
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.C) && jumpCount < 2 ) {
            inputJump = true;
            animator.SetBool("isJumping", true);
            jumpCount++;
        }
    }

    void FixedUpdate()  // 물리 계산은 FixedUpdate에서 하는게 좋다
    {
        // 좌우 이동 처리
        if (inputRight) {
            inputRight = false;
            rigid.AddForce(Vector2.right * maxSpeed, ForceMode2D.Impulse);
        }
        if (inputLeft) {
            inputLeft = false;
            rigid.AddForce(Vector2.left * maxSpeed, ForceMode2D.Impulse);
        }
        if (stop) {
            stop = false;
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        // 최대속도 초과 시 속도를 최대 속도로 제한
        if (rigid.velocity.x > maxSpeed) // 오른쪽 최대 속도
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽 최대속도
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        // 점프 처리
        if (inputJump) {
            inputJump = false;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        // 착지 처리
        if(rigid.velocity.y < 0){
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f) {
                    animator.SetBool("isJumping", false);
                    jumpCount = 0;
                }
            }
        }
    }
}
