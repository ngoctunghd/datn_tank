﻿using UnityEngine;
using System.Collections;

public class AIEnemyAttack : MonoBehaviour {

    public GameObject Bullet;
    public GameObject bulletPos;
    public float speedBullet;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip clipShoot;

    private string isMusic;
    private bool colupPlayer, colupStone, colupBrick;
    [SerializeField]
    private Transform startPos, upPos;
    private float shootTimer = 1f;
    float cooldown = 0;

    // Use this for initialization
    void Start()
    {
        isMusic = PlayerPrefs.GetString("Music");
        if (isMusic == "Off")
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }
        //StartCoroutine(Attack());
        //        GetComponent<PhotonView>().RPC("Shoot", PhotonTargets.All); 
    }

    void Update()
    {
        
        if (GetComponent<EnemyDefence>().isDead)
        {
            return;
        }
        //colupStone = Physics2D.Linecast(startPos.position, upPos.position, 1 << LayerMask.NameToLayer("stone"));
        //colupBrick = Physics2D.Linecast(startPos.position, upPos.position, 1 << LayerMask.NameToLayer("brick"));
        //colupPlayer = Physics2D.Linecast(startPos.position, upPos.position, 1 << LayerMask.NameToLayer("player"));
        //colupPlayer = Physics2D.Raycast(startPos.position, upPos.position,5f,  1 << LayerMask.NameToLayer("player"));
        //Debug.DrawLine(startPos.position, upPos.position, Color.red);
        cooldown -= Time.deltaTime;

        float distance = 10f;

        if (transform.localScale.y < 0)
        {
            Debug.Log(transform.localScale.y);
            distance = -10f;
        }

        RaycastHit2D hitwall = Physics2D.Raycast(transform.position, Vector2.up, distance, 1 << LayerMask.NameToLayer("stone"));
        RaycastHit2D hitbrick = Physics2D.Raycast(transform.position, Vector2.up, distance, 1 << LayerMask.NameToLayer("brick"));
        
        Debug.DrawRay(transform.position, transform.up * distance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up , distance, 1 << LayerMask.NameToLayer("player"));
        if (hitwall || hitbrick)
        {
            return;
        }
        if (hit)
        {
            AttackNew();
        }

        //    if (colupPlayer)
        //{
        //    AttackNew();
        //}
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(Random.Range(2f, 4f));
        var bullet = (GameObject)Instantiate(Bullet, bulletPos.transform.position, bulletPos.transform.rotation);
        var bulletBody = (Rigidbody2D)bullet.GetComponentInChildren(typeof(Rigidbody2D));
        //            bulletBody.AddForce( bulletPos.transform.up * speedBullet, ForceMode2D.Impulse);
        bulletBody.velocity = bulletPos.transform.up * 10;

        if (transform.localScale.y < 0)
        {
            bulletBody.velocity = bulletPos.transform.up * -10;
        }

        audioSource.PlayOneShot(clipShoot);
        StartCoroutine(Attack());
    }

    void AttackNew()
    {
        if (cooldown > 0)
        {
            return;
        }
        else
        {

            GameObject bullet = ObjectPooler.instance.GetPooledBulletEnemy();
            if (bullet == null) return;

            bullet.transform.position = bulletPos.transform.position;
            bullet.transform.rotation = bulletPos.transform.rotation;
            bullet.SetActive(true);

            //var bullet = (GameObject)Instantiate(Bullet, bulletPos.transform.position, bulletPos.transform.rotation);
            var bulletBody = (Rigidbody2D)bullet.GetComponentInChildren(typeof(Rigidbody2D));
            //            bulletBody.AddForce( bulletPos.transform.up * speedBullet, ForceMode2D.Impulse);
            bulletBody.velocity = bulletPos.transform.up * speedBullet;

            if (transform.localScale.y < 0)
            {
                bulletBody.velocity = bulletPos.transform.up * -speedBullet;
            }

            audioSource.PlayOneShot(clipShoot);
            cooldown = shootTimer;
        }
       
    }

}
