using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletControll : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] Transform target = null;
    [SerializeField] Rigidbody2D rb;
    public void SetTarget(Transform targetEnemy)
    {
        // Lưu vị trí của targetEnemy tại thời điểm gọi SetTarget
        if (targetEnemy != null)
        {
            target = targetEnemy;
            // Lưu vị trí của targetEnemy
            Vector3 targetPosition = targetEnemy.position;
            // Bạn có thể làm bất kỳ điều gì với vị trí này, ví dụ:
           // Debug.Log("Target position: " + targetPosition);
        }
        else
        {
            // Nếu targetEnemy là null, đặt target là null
            target = null;
        }
    }

   
    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public void Hit()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {

            
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
     
    public void FlyToTheTarget()
    {
        
        Vector3 direction = target.position - transform.position;
        //transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        rb.velocity = new Vector2(direction.x, direction.y).normalized * 5f;


        if (direction.magnitude < 0.1f)
         

            gameObject.SetActive(false); // Tắt script sau khi đến điểm đến
    }
}





