﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpiritPetMaster;

public class Pet01_Controller : Pet {
    [SerializeField]
    private Animator animator;
    private Rigidbody2D rb;
    private float timerJump = 0.6f;
    private int Dir = 1;
    private float timerRecover = 0;
    private float timerAttackfire = 0;
    private float HP, MP;
    private bool isJump = false;
    private bool isDoubleJump = false;
    private bool isFalling = false;

    public GameObject Attackfire;
    public Slider PlayerHP, PlayerMP;
    public GameStageController gamestage;




    void Start () {
        //change to read file here 
        //LoadPet(502);
        Speed = 2;
        MaxHP = 100;
        MaxMP = 100;
        MPRecover = 0.01f;
        HPRecover = 0.01f;
        PetfireAttack = 100;
        PetwaterAttack = 100;
        PetwindAttack = 100;
        

        rb = GetComponent<Rigidbody2D>();
        Dir = 1;
        MP = MaxMP;
        HP = MaxHP;
        isJump = false;
        isDoubleJump = false;
}

    void FixedUpdate()
    {
        timerJump += Time.deltaTime;
        timerRecover += Time.deltaTime;
        timerAttackfire += Time.deltaTime;

        if (HP <= 0)
        {
            HP = 0;
            animator.SetInteger("Dead", 0);
            gamestage.Gameover = 2;//lose
            return;
        }

        //move
        float moveHorizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));
        float moveZ = moveHorizontal * Speed;
        moveZ *= Time.deltaTime;
        transform.Translate(moveZ, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump)//如果还在跳跃中，则不重复执行 
            {
                rb.AddForce(Vector3.up * 250f);
                isJump = true;
                animator.SetBool("isJumping", true);
            }
            else
            {
                if (isDoubleJump)//判断是否在二段跳  
                {
                    return;//否则不能二段跳  
                }
                else
                {
                    isDoubleJump = true;
                    rb.AddForce(Vector3.up * 250f);
                    animator.SetBool("isJumping", true);
                }
            }
            /*rb.AddForce(Vector3.up * 350.0f);
            //transform.Translate(Vector3.up * 10.0f);
            timerJump = 0;
            animator.SetBool("isJumping", true);*/
        }
        else animator.SetBool("isJumping", false);

        

        //animation
        Vector2 currentVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        if(moveHorizontal * transform.localScale.x < 0)
        {
            transform.localScale = new Vector2 ( -transform.localScale.x, transform.localScale.y);
        }
        if (moveHorizontal < 0 && currentVelocity.x <= 0)
        {
            // animator.SetInteger("DirectionX", -1);
            Dir = -1;
            //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(currentVelocity.x - 0.1f, currentVelocity.y);// for ice
        }
        else if (moveHorizontal > 0 && currentVelocity.x >= 0)
        {
            Dir = 1;
            // animator.SetInteger("DirectionX", 1);
            //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(currentVelocity.x + 0.1f, currentVelocity.y);// for ice
        }
        else
        {
            // animator.SetInteger("DirectionX", 0);
        }
        isFalling = currentVelocity.y < -0.1f ? true : false;//jump falling
        if(isFalling) animator.SetInteger("isFalling", 1);

        //MPHP Recover
        if (timerRecover > 1)
        {
            if (MP + MP * MPRecover < MaxMP) MP +=  MP * MPRecover;
            if (HP + HP * HPRecover < MaxHP) HP += HP * HPRecover;
            timerRecover = 0;
        }

        //MPHP UI
        PlayerHP.value = HP / MaxHP;
        PlayerMP.value = MP / MaxMP;

        //attack
        if (Input.GetKeyDown(KeyCode.Q) && MP - 10 > 0 && timerAttackfire > 1f)
        {
            MP -= 10;
            Quaternion rot;
            if (Dir == 1) rot = Quaternion.Euler(0, 0, 125);
            else rot = Quaternion.Euler(0, 0, -45);
            GameObject fires =  Instantiate(Attackfire, transform.position, rot);
            fires.GetComponent<Attack_far>().fire = 1;
            fires.GetComponent<Attack_far>().Attacknum = PetfireAttack * 0.1f;
            fires.GetComponent<Attack_far>().AttackDir = Dir;
            animator.SetBool("isAttacking", true);
            timerAttackfire = 0;
        }
        else animator.SetBool("isAttacking", false);




    }

    //Hitted
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            HP -= other.gameObject.GetComponent<Monster01_Controller>().Attacknum;
            float Dist = Mathf.Abs(gameObject.transform.position.x - other.transform.position.x);
            float moveHorizontal = (gameObject.transform.position.x - other.transform.position.x) / Dist;
            rb.velocity = (new Vector2(1, 0) * moveHorizontal * 3);
            StartCoroutine("Damage");
            // animator.SetInteger("Hitted", 1);
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            HP -= other.gameObject.GetComponent<Boss01_Controller>().Attacknum;
            float Dist = Mathf.Abs(gameObject.transform.position.x - other.transform.position.x);
            float moveHorizontal = (gameObject.transform.position.x - other.transform.position.x) / Dist;
            rb.velocity = (new Vector2(1, 0) * moveHorizontal * 3);
            StartCoroutine("Damage");
            // animator.SetInteger("Hitted", 1);
        }
        else if (other.gameObject.CompareTag("Plane")) {//碰撞的是Plane  
            isJump = false;
            isDoubleJump = false;
        }
 //    else animator.SetInteger("Hitted", 0);
    }

    IEnumerator Damage()//無敵
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
        int count = 10;
        while (count > 0)
        {
            GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.05f);
            GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.05f);
            count--;
        }
        gameObject.layer = LayerMask.NameToLayer("Default");
    }


}


