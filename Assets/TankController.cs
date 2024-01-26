using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public static TankController instance;
    public Animator Animator;
    public bool isAttack = false;
    public float AttackRange;
    public LayerMask enemyLayer;
    public Rigidbody2D body;
    private GameObject target;
    [SerializeField] BulletPooling BulletPoolingControll;
    [SerializeField] Transform GunTip;
    public void Idle()
    {
        Animator.SetBool("Idle", true);
        Animator.SetBool("Move", false);
        Animator.SetBool("Destroy", false);
        Animator.SetBool("Shotting", false);
    }

    public void Move()
    {
        isAttack = false;
        Animator.SetBool("Idle", false);
        Animator.SetBool("Move", true);
        Animator.SetBool("Destroy", false);
        Animator.SetBool("Shotting", false);
    }

    public void Destroy()
    {
        Animator.SetBool("Idle", false);
        Animator.SetBool("Move", false);
        Animator.SetBool("Destroy", true);
        Animator.SetBool("Shotting", false);
    }

    public void Shot()
    {
        isAttack = true;
        Debug.Log("Shot");
        Animator.SetBool("Idle", false);
        Animator.SetBool("Move", false);
        Animator.SetBool("Shotting", true);
        Animator.SetBool("Destroy", false);
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Move();
    }

    public void Update()
    {
        if (isAttack == false)
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
                        Debug.Log("?");
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

    public void OnAttack()
    {
        isAttack = true;
        //StartCoroutine(PlayerMove(false));
        //PlayAnimationCharacter(0, "shoot", true);

        //PlayAnimationCharacter(1, "aim", false);
        Shot();
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
            yield return new WaitForSeconds(0.633f);
            BulletShotting();
        }
    }

    public void BulletShotting()
    {
        var bulletControll = BulletPoolingControll.GetObjectInPooling();
        bulletControll.transform.position = GunTip.position;
        
        bulletControll.SetTarget(target.transform);
        bulletControll.FlyToTheTarget();
        bulletControll.enabled = true;
    }
}
