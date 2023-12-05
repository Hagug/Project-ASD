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

    void Update() // �ܹ����� Ű �Է��� Update���� �ϴ°� ����.
    {
        // Ű���忡�� ���� ���� �̵��� ����
        if(Input.GetButtonUp("Horizontal"))
        {
            // rigid.velocity.normalized : ���� ����ȭ. ������ ����, ũ��� 1�� ����
            rigid.velocity  = new Vector2(0, rigid.velocity.y);
        }

        // ���� ��ȯ
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // �̵� �ִϸ��̼� ��ȯ
        if (rigid.velocity.normalized.x == 0)
            animator.SetBool("isWalking", false);
        else
            animator.SetBool("isWalking", true);
    }

    void FixedUpdate()  // ���� ����� FixedUpdate���� �ϴ°� ����
    {
        // �¿� Ű �Է� �ޱ�. ���� : -1, ������ : 0 ��ȯ
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        

        // �ִ�ӵ� �ʰ� �� �ӵ��� �ִ� �ӵ��� ����
        if (rigid.velocity.x > maxSpeed) // ������ �ִ� �ӵ�
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // ���� �ִ�ӵ�
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }
}
