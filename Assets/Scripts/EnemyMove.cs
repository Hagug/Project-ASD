using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Think();
    }

    void FixedUpdate()
    {
        // 기본 움직임
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 지형 체크. 낭떠러지 감지
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.5f, rigid.position.y); // 진행방향의 살짝 앞쪽 위치
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1.5f, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1.5f, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null) {
            Turn();
        }
    }

    void Think()
    {
        // 다음 활동 전하기
        nextMove = Random.Range(-1, 2);

        // 애니메이션 상태 전환
        anim.SetInteger("walkSpeed", nextMove);
        
        //  방향전환
        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        // 재귀함수
        float nextThinkTime = Random.Range(2f, 5f);
        // Invoke : 딜레이를 주고 함수를 실행
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke(); // 현재 인보크를 취소
        Invoke("Think", 2);
    }
}
