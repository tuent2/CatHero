using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScolls : MonoBehaviour
{
    [SerializeField] Vector2 scollerMoveSpeed;
    Vector2 offset;
    Material material;
    void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }


    void Update()
    {
        if ( TankController.instance.isAttack == false) 
        {
            offset = scollerMoveSpeed * Time.deltaTime;
            
            material.mainTextureOffset += offset;
        }
        
    }
}
