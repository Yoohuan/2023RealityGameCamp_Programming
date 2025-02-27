using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyNasbr : MonoBehaviour
{

    public enum State
    {
        statePatrol,
        statePursuit,
        stateAttack,
        stateDie
    }
    private State nasbrState;

    //设置属性
    public int health;
    public int damage;
    public float speed;
    public float pursuitSpeed;
    public float waitTime;
    public float atkDistance;
    public Transform[] movePos;

    //设置Hitbox
    public GameObject hitBox;
    public float startAtk;
    public float endAtk;

    GameObject player;

    //设置动画
    public SkeletonAnimation enemyAnimation;
    public AnimationReferenceAsset idle, move, attack, die;
    private string currentState;

    private int i = 0;//定位数组的游标

    //控制巡逻方向和等待时间
    private bool moveRight;
    private float wait;

    private Rigidbody2D enemyRigidbody;

    private bool isAtk;

    private enum Direction
    {
        Right = 1,
        Left = -1,
        Stop = 0
    }

    private Direction moveDir;

    // Start is called before the first frame update
    void Start()
    {
        //初始化数据
        nasbrState = State.statePatrol;
        moveRight = true;
        moveDir = Direction.Right;
        wait = waitTime;
        enemyRigidbody = GetComponent<Rigidbody2D>();
        isAtk = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetAnimation(enemyAnimation);
        Control();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && health > 0)
        {
            nasbrState = State.statePursuit;
            player = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            nasbrState = State.statePatrol;
        }
    }

    void Control()
    {
        switch(nasbrState)
        {
            case State.statePatrol:
                Patrol();
                break; 
            case State.statePursuit:
                Pursuit();
                break;
            case State.stateAttack:
                Attack();
                break; 
            case State.stateDie:
                if (enemyAnimation.AnimationState.GetCurrent(0).IsComplete)
                {
                    Destroy(gameObject);
                }
                AnimationSet(enemyAnimation, die, false, 1f);
                break;
        }
    }

    void Patrol()
    {
        Vector2 enemyVel;
        switch (moveDir)
        {
            case Direction.Right:
                enemyVel = new Vector2( 1 * speed, 0f);
                enemyRigidbody.velocity = enemyVel;
                AnimationSet(enemyAnimation, move, true, 1f);
                break;
            case Direction.Left:
                enemyVel = new Vector2( -1 * speed, 0f);
                enemyRigidbody.velocity = enemyVel;
                AnimationSet(enemyAnimation, move, true, 1f);
                break;
            case Direction.Stop:
                enemyVel = new Vector2(0f, 0f);
                enemyRigidbody.velocity = enemyVel;
                AnimationSet(enemyAnimation, idle, true, 1f);
                break;
        }

        if (Vector2.Distance(transform.position, movePos[i].position) < 0.1f)
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                moveDir = Direction.Stop;
            }
            else
            {
                if (moveRight)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    moveRight = false;
                    moveDir = Direction.Left;
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    moveRight = true;
                    moveDir = Direction.Right;
                }
                if (i == 0)
                {
                    i = 1;
                }
                else
                {
                    i = 0;
                }
                waitTime = wait;
            }

        }
        else//处理超出巡逻范围的情况
        {
            switch (moveDir)
            {
                case Direction.Left:
                    if (Vector2.Distance(transform.position, movePos[0].position) < Vector2.Distance(transform.position, movePos[1].position))
                    {
                        break;
                    }
                    if (Vector2.Distance(transform.position, movePos[0].position) > Vector2.Distance(movePos[0].position, movePos[1].position))
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        moveRight = true;
                        moveDir = Direction.Right;
                    }
                    break; 
                case Direction.Right:
                    if (Vector2.Distance(transform.position, movePos[1].position) < Vector2.Distance(transform.position, movePos[0].position))
                    {
                        break;
                    }
                    if (Vector2.Distance(transform.position, movePos[1].position) > Vector2.Distance(movePos[0].position, movePos[1].position))
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        moveRight = false;
                        moveDir = Direction.Left;
                    }
                    break;
            }
        }

    }

    void Pursuit()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < atkDistance)
        {
            nasbrState = State.stateAttack;
            return;
        }
        Vector2 enemyVel;
        if (player.transform.position.x < transform.position.x)//向左追击
        {
            moveDir = Direction.Left;
            if (moveRight)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                moveRight = false;
            }
            enemyVel = new Vector2(-1 * pursuitSpeed, 0f);
            enemyRigidbody.velocity = enemyVel;
            AnimationSet(enemyAnimation, move, true, 1f);
        }
        else if (player.transform.position.x > transform.position.x)//向右追击
        {
            moveDir = Direction.Right;
            if (!moveRight)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                moveRight = true;
            }
            enemyVel = new Vector2(1 * pursuitSpeed, 0f);
            enemyRigidbody.velocity = enemyVel;
            AnimationSet(enemyAnimation, move, true, 1f);
        }
    }

    void Attack()
    {
        if (isAtk)
        {
            return;
        }
        if (Vector2.Distance(player.transform.position, transform.position) > atkDistance)
        {
            nasbrState = State.statePursuit;
            return;
        }
        if (player.transform.position.x < transform.position.x)//向左攻击
        {
            if (moveRight)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                moveRight = false;
            }
        }
        else if (player.transform.position.x > transform.position.x)//向右攻击
        {
            if (!moveRight)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                moveRight = true;
            }
        }
        Vector2 enemyVel;
        enemyVel = new Vector2(0f, 0f);
        enemyRigidbody.velocity = enemyVel;
        moveDir = Direction.Stop;
        AnimationSet(enemyAnimation, attack, true, 1f);
        isAtk = true;
        StartCoroutine(StartAtk(hitBox, startAtk, endAtk));//开启协程控制前后摇
    }

    void GetAnimation(SkeletonAnimation skeletonAnimation)
    {
        //获取正在播放的动画
        currentState = skeletonAnimation.AnimationName;
    }
    void AnimationSet(SkeletonAnimation skeletonAnimation, AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentState))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    //受伤
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            nasbrState = State.stateDie;
        }
    }

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
        isAtk = false;
        nasbrState = State.statePursuit;
    }
}
