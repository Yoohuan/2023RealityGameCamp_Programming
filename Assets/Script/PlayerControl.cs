using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //�����ٶȱ���
    public float runSpeed;
    public float jumpSpeed;

    //���ö���
    //�ǵ����ã�����������������
    public SkeletonAnimation chooseAnimation;
    public AnimationReferenceAsset idle, attack;
    private string currentState;
    bool attackFinish;

    private Rigidbody2D myRigidbody;
    private BoxCollider2D myFeet;
    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        //��ȡPlayer����ģ��
        myRigidbody = GetComponent<Rigidbody2D>();
        myFeet = GetComponent<BoxCollider2D>();

        //��ʼ������
        currentState = "Idle";
        SetCharacterState(currentState);
        attackFinish = false;
    }

    // Update is called once per frame
    void Update()
    {
        OnGround();
        Move();
        Jump();
        Flip();
        Attack();
    }

    void Move()
    {
        //��������ƶ�
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidbody.velocity.y);
        //ͨ���ı��ٶȽ����ƶ�
        myRigidbody.velocity = playerVel;
        if(moveDir != 0)
        {
            SetCharacterState("Idle");
        }
        else
        {
            SetCharacterState("Idle");
        }

    }

    void Attack()
    {
        //ִ�й���("J"��)
        if (Input.GetButtonDown("Attack"))
        {
            //�ж��Ƿ����ڽ��й���
            if(chooseAnimation.AnimationName == "Attack_Loop")
            {
                return;
            }
            SetCharacterState("Attack");
        }       
    }

    void Flip()
    {
        //�ж�player�ٶȷ����Ҹı��泯����
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasXAxisSpeed)
        {
            if(myRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);

            }

            if (myRigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);

            }
        }
    }

    void Jump()
    {
        if(isOnGround)
        {
            //ִ����Ծ
            if (Input.GetButtonDown("Jump"))
            {
                Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
                myRigidbody.velocity = Vector2.up * jumpVel;
            }
        }
        
    }


    void OnGround()
    {
        //�ж��Ƿ��ڵ�����
        isOnGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void AnimationSet(AnimationReferenceAsset animation,bool loop,float timeScale)
    {
        //�ж϶���״̬��ִ��/�л���ȷ����
        if (chooseAnimation.AnimationName == "Attack_Loop") 
        {
            if(attackFinish == false)
            {
                //���������л�Ϊԭ����
                chooseAnimation.state.AddAnimation(0, animation, loop, 0).TimeScale = timeScale;
                attackFinish = true;
            }   
            return;
        }
        //����ÿ֡�ظ���ʼ���Ŷ���
        else if(animation.name.Equals(chooseAnimation.AnimationName))
        {
            return;
        }
        chooseAnimation.state.SetAnimation(0,animation, loop).TimeScale = timeScale;
        attackFinish = false;
    }

    void SetCharacterState(string state)
    {
        if(state.Equals("Idle"))
        {
            AnimationSet(idle, true, 1f);
        }
        else if (state.Equals("Attack"))
        {
            AnimationSet(attack, false, 1f);
        }
    }
}
