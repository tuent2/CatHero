using NaughtyAttributes;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Unity.Burst.Intrinsics;


public class Player : MonoBehaviour
{
    public static Player Instance;

    public bool isAttack = false;

    
    public SkeletonAnimation skeletonAnimation;

    [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;
    public Camera cam;

    public float AttackRange;
    public LayerMask enemyLayer;
    public Rigidbody2D body;

    // public bool isMoveOnDone;
    Bone bone;
    [SerializeField] Transform GunTip;
    private GameObject target;

    [SerializeField] BulletPooling BulletPoolingControll;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bone = skeletonAnimation.Skeleton.FindBone(boneName);
        OnApear();
        //OnStopAttack();
    }
    public void PlayAnimationCharacter(int index, string animation, bool isLoop)
    {
        skeletonAnimation.AnimationState.SetAnimation(index, animation, isLoop);
    }

    public void OnApear()
    {
        StartCoroutine(HandleOnApear());
    }

    IEnumerator HandleOnApear()
    {
        
        PlayAnimationCharacter(0, "portal", false);
        yield return new WaitForSeconds(2f);
        OnStopAttack();
    }


    public void OnAttack()
    {
        isAttack = true;
        StartCoroutine(PlayerMove(false));
        PlayAnimationCharacter(0, "shoot", true);
       
        PlayAnimationCharacter(1, "aim", false);
        //Vector3 position = target.transform.position;
        //Vector3 worldMousePosition = cam.ScreenToWorldPoint(position);

        //Vector3 skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(worldMousePosition);
        //skeletonSpacePoint.x *= skeletonAnimation.Skeleton.ScaleX;
        //skeletonSpacePoint.y *= skeletonAnimation.Skeleton.ScaleY;
        //bone.SetLocalPosition(target.transform.position);
        StartCoroutine(ContinuousBulletShotting());
    }

    private IEnumerator ContinuousBulletShotting()
    {
        while (isAttack) 
        {
            yield return new WaitForSeconds(0.16f);
            BulletShotting();
        }
    }

    public void BulletShotting()
    {
        var bulletControll = BulletPoolingControll.GetObjectInPooling();
        bulletControll.transform.position = GunTip.position;
        bulletControll.SetTarget(target.transform);
        bulletControll.enabled = true;
       
    }


    [Button("StopAttack")]
    public void OnStopAttack()
    {
        isAttack = false;
        StartCoroutine(PlayerMove(true));
        skeletonAnimation.state.AddEmptyAnimation(1, 0.5f, 0.1f);
        //skeletonAnimation.state.AddEmptyAnimation(2, 0.5f, 0.1f);
        skeletonAnimation.AnimationState.SetAnimation(0, "run", true);
        //PlayAnimationCharacter(0, "run", true);
       
    }

    public void Update()
    {
        if(isAttack == false)
        {
            //float distance = Vector3.Distance(gameObject.transform.position, );
            //if (distance <= AttackRange)
            //{
            //    isAttack = true;
            //    OnAttack();
            //}

            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, AttackRange, enemyLayer);
            
            foreach (Collider2D enemyCollider in nearbyEnemies)
            {
                //Debug.Log(nearbyEnemies.Length);
                if (enemyCollider.CompareTag("Enemy"))
                {
                    
                    float distanceToEnemy = Vector2.Distance(transform.position, enemyCollider.transform.position);

                    //Debug.Log(distanceToEnemy);
                    if (distanceToEnemy <= AttackRange)
                    {
                        //Debug.Log("?");
                        target = enemyCollider.gameObject;
                        OnAttack();

                    }
                } 
            }
        }
        if (isAttack == true)
        {
            // Vector3 position = target.transform.position;
            //Vector3 worldMousePosition = cam.ScreenToWorldPoint(position);

            //Vector3 skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(worldMousePosition);
            //skeletonSpacePoint.x *= skeletonAnimation.Skeleton.ScaleX;
            //skeletonSpacePoint.y *= skeletonAnimation.Skeleton.ScaleY;
            //bone.SetLocalPosition(target.transform.position);
        }
    }

    IEnumerator PlayerMove(bool isMoveUp)
    {
        //if (isMoveUp == false)
        //{
        //    body.velocity = new Vector2(-0.5f, 0f);
        //}
        //if (isMoveUp == true)
        //{
        //    body.velocity = new Vector2(+0.5f, 0f);
        //}

        yield return new WaitForSeconds(1.2f);

        body.velocity = Vector2.zero;
        
        
    }
    

    
}
