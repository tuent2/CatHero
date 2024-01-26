using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heath : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] float MaxHeath = 300;
    [SerializeField] float CurrentHeath = 300;
    [SerializeField] Slider HeathSlider;
    //[SerializeField] int score = 50;
    //[SerializeField] ParticleSystem hitEffect;
   // CameraShake cameraShake;
   // AudioPlayer audioPlayer;
   // ScoreKeeper scoreKeeper;
   // LevelManager levelManager;
    [SerializeField] bool applyCameraShake;
    void Awake()
    {
        //cameraShake = Camera.main.GetComponent<CameraShake>();
        //audioPlayer = FindObjectOfType<AudioPlayer>();
        //scoreKeeper = FindObjectOfType<ScoreKeeper>();
        //levelManager = FindObjectOfType<LevelManager>();
        
    }
    private void OnEnable()
    {
        CurrentHeath = MaxHeath;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        BulletControll bulletControll = other.GetComponent<BulletControll>();
        if (bulletControll != null)
        {
            TakeDame(bulletControll.GetDamage());
            //playHitEffect();
            //ShakeCamera();
            //audioPlayer.PlayerGetDamageAudio();
            // heath -=damage.GetDamage();
            bulletControll.Hit();
        }
    }

    void TakeDame(float damageValue)
    {
        CurrentHeath -= damageValue;
        if (CurrentHeath <= 0)
        {
            if (!isPlayer)
            {
                gameObject.SetActive(false);
               // Player.Instance.OnStopAttack();
                TankController.instance.Move();
            }
            else
            {
               // levelManager.LoadGameOver();
            }
            //Destroy(gameObject);

        }
    }

    void Update()
    {
        if (HeathSlider.value != CurrentHeath/MaxHeath)
        {
            HeathSlider.value = CurrentHeath/MaxHeath;
        }
    }

    //void playHitEffect()
    //{
    //    if (hitEffect != null)
    //    {
    //        ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
    //        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    //    }
    //}

    //void ShakeCamera()
    //{
    //    if (cameraShake != null && applyCameraShake)
    //    {
    //        cameraShake.PlayCameraShake();
    //    }
    //}

    public float getHeath()
    {
        return CurrentHeath;
    }


}
