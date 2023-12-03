using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public enum State
    {
        stateStand,
        stateMove,
        stateJump,
        stateAttack,
        stadeDie,
        stateUpdate,
        stateUpdateStand,
        stateUpdateMove,
        stateUpdateJump,
        stateUpdateAttack,
        stateUpdateDie
    }
    
    public static State playerState;


    public GameObject battlePlayer;
    public GameObject buildingPlayer;
    public GameObject updateBattlePlayer;
    public GameObject updateBuildingPlayer;

    //�����ٶȱ���
    public float runSpeed;
    public float jumpSpeed;

    //���ö���
    public SkeletonAnimation battleAnimation, buildingAnimation, updateBattleAnimation, updateBuildingAnimation;
    public AnimationReferenceAsset idle, attack, move, die, update, updateIdle, updateAttack, updateMove, updateDie;
    private string currentState;

    //���ù���HitBox
    public GameObject AtkHitBox;
    public GameObject UpdateHitBox;
    public float startAtk;
    public float endAtk;
    public float updateStartAtk;
    public float updateEndAtk;

    private Rigidbody2D myRigidbody;
    private BoxCollider2D myFeet;
    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        //��ʼ��״̬��
        buildingPlayer.SetActive(false);
        updateBattlePlayer.SetActive(false);
        updateBuildingPlayer.SetActive(false);

        //��ȡPlayer����ģ��
        myRigidbody = GetComponent<Rigidbody2D>();
        myFeet = GetComponent<BoxCollider2D>();

        playerState = State.stateStand;

    }


    // Update is called once per frame
    void Update()
    {
        Control();
        OnGround();
    }

    //ͨ��״̬��������Ҫ����
    void Control()
    {
        switch (playerState) 
        {
            case State.stateStand:
                buildingPlayer.SetActive(true);
                battlePlayer.SetActive(false);
                updateBattlePlayer.SetActive(false);
                updateBuildingPlayer.SetActive(false);
                AnimationSet(buildingAnimation,idle, true, 1f);
                GetAnimation(buildingAnimation);
                Move();
                Jump();
                Attack();
                Flip();
                if (!isOnGround)
                {
                    playerState = State.stateJump;
                }
                else if(myRigidbody.velocity.x != 0)
                {
                    playerState= State.stateMove;
                }
                break; 
            case State.stateMove:
                buildingPlayer.SetActive(true);
                battlePlayer.SetActive(false);
                updateBattlePlayer.SetActive(false);
                updateBuildingPlayer.SetActive(false);
                AnimationSet(buildingAnimation, move, true, 1f);
                GetAnimation(buildingAnimation);
                Move();
                Jump();
                Attack();
                Flip();
                if (!isOnGround)
                {
                    playerState = State.stateJump;
                }
                else if (myRigidbody.velocity.x == 0)
                {
                    playerState = State.stateStand;
                }
                break;
            case State.stateJump:
                buildingPlayer.SetActive(true);
                battlePlayer.SetActive(false);
                updateBattlePlayer.SetActive(false);
                updateBuildingPlayer.SetActive(false);
                AnimationSet(buildingAnimation, idle, true, 1f);
                GetAnimation(buildingAnimation);
                Move();
                Attack();
                Flip();
                if (isOnGround)
                {
                    playerState = State.stateStand;
                }
                break;
            case State.stateAttack:
                battlePlayer.SetActive(true);
                buildingPlayer.SetActive(false);
                updateBattlePlayer.SetActive(false);
                updateBuildingPlayer.SetActive(false);
                AnimationSet(battleAnimation, attack, false, 1f);
                GetAnimation(battleAnimation);
                if (isOnGround)
                {
                    Vector2 playerAttackVel = new Vector2(0, myRigidbody.velocity.y);
                    myRigidbody.velocity = playerAttackVel;
                }
                if(battleAnimation.AnimationState.GetCurrent(0).IsComplete)
                {
                    playerState = State.stateStand;
                }
                break;
            case State.stadeDie:
                break;
            case State.stateUpdate:
                updateBattlePlayer.SetActive(true);
                battlePlayer.SetActive(false);
                buildingPlayer.SetActive(false);
                updateBuildingPlayer.SetActive(false);
                AnimationSet(updateBattleAnimation, update, false, 1f);
                GetAnimation(updateBattleAnimation);
                GetComponent<AudioSource>().Play();
                if (isOnGround)
                {
                    Vector2 playerAttackVel = new Vector2(0, myRigidbody.velocity.y);
                    myRigidbody.velocity = playerAttackVel;
                }
                if (updateBattleAnimation.AnimationState.GetCurrent(0).IsComplete)
                {
                    playerState = State.stateUpdateStand;
                }
                break;
            case State.stateUpdateStand:
                updateBuildingPlayer.SetActive(true);
                buildingPlayer.SetActive(false);
                battlePlayer.SetActive(false);
                updateBattlePlayer.SetActive(false);
                AnimationSet(updateBuildingAnimation, updateIdle, true, 1f);
                GetAnimation(updateBuildingAnimation);
                Move();
                Jump();
                UpdateAttack();
                Flip();
                if (!isOnGround)
                {
                    playerState = State.stateUpdateJump;
                }
                else if (myRigidbody.velocity.x != 0)
                {
                    playerState = State.stateUpdateMove;
                }
                break;
            case State.stateUpdateMove:
                updateBuildingPlayer.SetActive(true);
                buildingPlayer.SetActive(false);
                battlePlayer.SetActive(false);
                updateBattlePlayer.SetActive(false);
                AnimationSet(updateBuildingAnimation, updateMove, true, 1f);
                GetAnimation(updateBuildingAnimation);
                Move();
                Jump();
                UpdateAttack();
                Flip();
                if (!isOnGround)
                {
                    playerState = State.stateUpdateJump;
                }
                else if (myRigidbody.velocity.x == 0)
                {
                    playerState = State.stateUpdateStand;
                }
                break;
            case State.stateUpdateJump:
                updateBuildingPlayer.SetActive(true);
                buildingPlayer.SetActive(false);
                battlePlayer.SetActive(false);
                updateBattlePlayer.SetActive(false);
                AnimationSet(updateBuildingAnimation, updateIdle, true, 1f);
                GetAnimation(updateBuildingAnimation);
                Move();
                UpdateAttack();
                Flip();
                if (isOnGround)
                {
                    playerState = State.stateUpdateStand;
                }
                break;
            case State.stateUpdateAttack:
                updateBattlePlayer.SetActive(true);
                battlePlayer.SetActive(false);
                buildingPlayer.SetActive(false);
                updateBuildingPlayer.SetActive(false);
                AnimationSet(updateBattleAnimation, updateAttack, false, 1f);
                GetAnimation(updateBattleAnimation);
                if (isOnGround)
                {
                    Vector2 playerAttackVel = new Vector2(0, myRigidbody.velocity.y);
                    myRigidbody.velocity = playerAttackVel;
                }
                if (updateBattleAnimation.AnimationState.GetCurrent(0).IsComplete)
                {
                    playerState = State.stateUpdateStand;
                }
                break;
            case State.stateUpdateDie:
                break;
        }
    }

    void Move()
    {
        //��������ƶ�
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidbody.velocity.y);
        //ͨ���ı��ٶȽ����ƶ�
        myRigidbody.velocity = playerVel;
    }

    void Attack()
    {
        //ִ�й���("J"��)
        if (Input.GetButtonDown("Attack"))
        {
            playerState = State.stateAttack;
            StartCoroutine(StartAtk(AtkHitBox, startAtk, endAtk));
        }
    }

    void UpdateAttack()
    {
        //ִ�й���("J"��)
        if (Input.GetButtonDown("Attack"))
        {
            playerState = State.stateUpdateAttack;
            StartCoroutine(StartAtk(UpdateHitBox, updateStartAtk, updateEndAtk));
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

    void GetAnimation(SkeletonAnimation skeletonAnimation)
    {
        //��ȡ���ڲ��ŵĶ���
        currentState = skeletonAnimation.AnimationName;
    }
    void AnimationSet(SkeletonAnimation skeletonAnimation,AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentState))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    //ͨ��Э�̿���ǰҡ�ͺ�ҡ
    IEnumerator StartAtk(GameObject HitBox, float startTime, float endTime)
    {
        yield return new WaitForSeconds(startTime);
        HitBox.SetActive(true);
        StartCoroutine(EndAtk(HitBox, endTime));
    }

    IEnumerator EndAtk(GameObject HitBox, float endTime)
    {
        yield return new WaitForSeconds(endTime);
        HitBox.SetActive(false);
    }
}
