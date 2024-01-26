using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] float AttackRange;
    [SerializeField] float MoveSpeed;
   
    [SerializeField] bool isAttack;
    SkeletonAnimation skeletonAnimation;
    Rigidbody2D body;
    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        body = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        PlayAnimationCharacter(0, "flying", true);
        //OnMove();
       // body.velocity = new Vector2(-MoveSpeed, 0f);
    }
    private void OnDisable()
    {
        isAttack = false;
    }

    private void OnEnable()
    {
        PlayAnimationCharacter(0, "flying", true);
        body.velocity = new Vector2(-MoveSpeed, 0f);
    }

    private void Update()
    {
        if (isAttack == false)
        {
            
            float distance = Vector2.Distance(gameObject.transform.position, TankController.instance.gameObject.transform.position);
            if (distance <= AttackRange)
            {
                isAttack = true;
                OnAttack();
            }
            
        }
        
    }

   


    void OnAttack()
    {
        body.velocity = new Vector2(0f, 0f);

        //PlayAnimationCharacter(0, "flying", false);
    }

    public void PlayAnimationCharacter(int index, string animation, bool isLoop)
    {
        skeletonAnimation.AnimationState.SetAnimation(index, animation, isLoop);
    }
}
