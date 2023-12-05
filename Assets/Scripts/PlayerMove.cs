using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();   
    }

    void Update() // 단발적인 키 입력은 Update에서 하는게 좋다.
    {
        // 키보드에서 손을 떼면 이동을 멈춤
        if(Input.GetButtonUp("Horizontal"))
        {
            // rigid.velocity.normalized : 벡터 정규화. 방향은 유지, 크기는 1로 변경
            rigid.velocity  = new Vector2(0, rigid.velocity.y);
        }

        // 방향 전환
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // 이동 애니메이션 전환
        if (rigid.velocity.normalized.x == 0)
            animator.SetBool("isWalking", false);
        else
            animator.SetBool("isWalking", true);
    }

    void FixedUpdate()  // 물리 계산은 FixedUpdate에서 하는게 좋다
    {
        // 좌우 키 입력 받기. 왼쪽 : -1, 오른쪽 : 0 반환
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        

        // 최대속도 초과 시 속도를 최대 속도로 제한
        if (rigid.velocity.x > maxSpeed) // 오른쪽 최대 속도
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽 최대속도
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }
}
