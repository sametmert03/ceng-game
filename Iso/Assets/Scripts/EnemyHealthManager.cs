﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{
    public GameObject hurtZone;
    CapsuleCollider capsule;
    public AudioSource mAudioSrc;
    public int health;
    private int currentHealth;
    public bool isDead = false;
    public int damage = 2;
    PlayerHealthManager playerHealth;
    public int gainedHealth = 5;
    public int gainedAmmo = 20;
    GunController playerAmmo;
    public Text gammoText;
    public Text ghealthText;
    float nextFireTime = 5f;
    float cooldownTime = 5f;

    public static int score;
    public int scc;
    int scoreValue = 10;
    public Text scoreText;


    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        mAudioSrc = GetComponent<AudioSource>();
        currentHealth = health;
        mAudioSrc.Play();
        scoreText = GameObject.Find("ScoreText").GetComponentInChildren<Text>();

    }
    void Update()
    {
        

        scoreText.text = "Score: " + score.ToString();
        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                ScoreUpdate();
                gameObject.GetComponent<EnemyController>().die();
                isDead = true;
                mAudioSrc.Stop();
                capsule.enabled = false;
                damage = 0;
                hurtZone.SetActive(false);
            }
            else
                damage = 2;
        }

        if (Time.time > nextFireTime)
        {
            gammoText.text = "";
            ghealthText.text = "";
            nextFireTime = Time.time + cooldownTime;
        }
    }
    void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealthManager>();
        playerAmmo = FindObjectOfType<GunController>();
    }
    public void HurtEnemy(int damage)
    {
        currentHealth -= damage;
    }

    public void ScoreUpdate()
    {
        
        scoreText = GameObject.Find("Score").GetComponentInChildren<Text>();
        gammoText = GameObject.Find("GainedAmmo").GetComponentInChildren<Text>();
        ghealthText = GameObject.Find("GainedHealth").GetComponentInChildren<Text>();
        score += scoreValue;

        //If score is multiple of 100, player will gain some health 
        if (score != 0 && score % 50 == 0)
        {
            GainHealth();
            GainAmmo();
            playerAmmo.showAmmo();
        }
    }
    public void GainHealth()
    {
        ghealthText.text = null;
        if (playerHealth.currentHealth+gainedHealth < playerHealth.startingHealth)
        {
            playerHealth.currentHealth += gainedHealth;
            ghealthText.text = "+ " + gainedHealth + " Health";
        }
        else
        {
            if (playerHealth.currentHealth < playerHealth.startingHealth)
            {
                ghealthText.text = "+ " + (playerHealth.startingHealth - playerHealth.currentHealth) + " Health";
                playerHealth.currentHealth = playerHealth.startingHealth;
            }
        }
    }
    public void GainAmmo()
    {
            playerAmmo.maxAmmo += gainedAmmo;
            gammoText.text = "+ " + gainedAmmo + " Ammo";
    }


}
