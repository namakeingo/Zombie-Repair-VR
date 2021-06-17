using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Enemy : MonoBehaviour
{
    private Transform target;
    private Transform player;
    private PlayerHandler playerHandler;

    public float speed = 0.2f;
    public float damage = 1.0f;
    public float hp = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Find Player
        player = GameObject.Find("Player")?.transform;
        if (player)
        {
            target = player.Find("SteamVRObjects")?.transform.Find("BodyCollider");
            if (target)
            {
                playerHandler = player.GetComponent<PlayerHandler>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Move enemy toword the player
        if (target)
        {
            var directlyTowardsTarget = target.position - transform.position;
            var towardsTarget = Vector3.ProjectOnPlane(directlyTowardsTarget, Vector3.up);
            var towardsTargetRot = Quaternion.LookRotation(towardsTarget.normalized);

            transform.rotation = towardsTargetRot;
            transform.Translate( speed * Time.deltaTime * Vector3.forward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //On enemy collision with player, kill player
        if (other.CompareTag("Player"))
        {
            playerHandler.TakeDamage(damage);
        } 
        //On enemy collision with bullet, take damage
        else if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();

            if (bullet)
            {
                hp -= bullet.damage;
            }

            Destroy(other.gameObject);

            //If enemy hp is at 0 then die
            if (hp <= 0)
            {
                Spawner spawner = FindObjectOfType<Spawner>();
                Die();
            }
        }
    }

    public void Die(bool isGameOver = false)
    {
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.aliveEnemies.Remove(this.gameObject);

        //count enemy death if the Die is not called from gameover
        if (!isGameOver)
        {
            spawner.enemiesKilled += 1;
        }

        Destroy(gameObject);
    }
}
