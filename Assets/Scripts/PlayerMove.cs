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

    void Update() // �ܹ����� Ű �Է��� Update���� �ϴ°� ����.
    {
        // �¿� Ű �Է� �ޱ�. 
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

        // Ű���忡�� ���� ���� �̵��� ����
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            stop = true;
            animator.SetBool("isWalking", false);
        }

        // ����
        if (Input.GetKeyDown(KeyCode.C) && jumpCount < 2 ) {
            inputJump = true;
            animator.SetBool("isJumping", true);
            jumpCount++;
        }
    }

    void FixedUpdate()  // ���� ����� FixedUpdate���� �ϴ°� ����
    {
        // �¿� �̵� ó��
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

        // �ִ�ӵ� �ʰ� �� �ӵ��� �ִ� �ӵ��� ����
        if (rigid.velocity.x > maxSpeed) // ������ �ִ� �ӵ�
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // ���� �ִ�ӵ�
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        // ���� ó��
        if (inputJump) {
            inputJump = false;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        // ���� ó��
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
