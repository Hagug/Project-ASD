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
        // �⺻ ������
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // ���� üũ. �������� ����
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.5f, rigid.position.y); // ��������� ��¦ ���� ��ġ
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1.5f, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1.5f, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null) {
            Turn();
        }
    }

    void Think()
    {
        // ���� Ȱ�� ���ϱ�
        nextMove = Random.Range(-1, 2);

        // �ִϸ��̼� ���� ��ȯ
        anim.SetInteger("walkSpeed", nextMove);
        
        //  ������ȯ
        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        // ����Լ�
        float nextThinkTime = Random.Range(2f, 5f);
        // Invoke : �����̸� �ְ� �Լ��� ����
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke(); // ���� �κ�ũ�� ���
        Invoke("Think", 2);
    }
}
