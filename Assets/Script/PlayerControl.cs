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
    public SkeletonAnimation texasAnimation;
    public AnimationReferenceAsset idle, attack;
    private string currentState;
    private string currentAnimation;
    bool isAttack;

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
        isAttack = false;
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
        //ִ�й���
        if (Input.GetKeyDown(KeyCode.F))
        {
            //�ж��Ƿ��Ѿ�
            if(this.GetComponent<SkeletonAnimation>().AnimationName == "Attack_Loop")
            {
                return;
            }
            currentAnimation = null;
            //���currentAnimation���⶯����ѭ��
            Debug.Log(Input.GetKeyDown(KeyCode.F));
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
        //�ж϶���״̬��ִ����ȷ����
        if (currentAnimation == "Attack_Loop") 
        {
            if(isAttack == false)
            {
                texasAnimation.state.AddAnimation(0, animation, loop, 0).TimeScale = timeScale;
                isAttack = true;
                Debug.Log(isAttack);
            }   
            return;
        }
        else if(animation.name.Equals(currentAnimation))
        {
            return;
        }
        texasAnimation.state.SetAnimation(0,animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
        isAttack = false;
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
