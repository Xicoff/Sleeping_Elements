﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy2 : MonoBehaviour
{

    private const float waypointOffset = 0.05f;

    private Rigidbody2D rb;
    private GameObject player;

    [SerializeField] public GameObject enemyBullet;
    private GameObject bullet;
    private float bulletStartOffset = 0.5f;
    private float shootTimer = 0f;
    private float cooldownTime = 1f;
    private float health = 2f;
    private float dmgTaken = 0f;

    private Vector2 playerPos;

    //Enemy main variables
    private string state = "Patrol";
    private float viewRange = 5f;
    private float moveSpeed = 3f;

    //Patrol variables
    [SerializeField] private Transform[] waypoints = new Transform[1];
    private int nextWaypoint;

    //Keeping the distance
    
    private float retreatDist = 3f;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = new Vector2(0, 0);

        rb.gravityScale = 0;

    }

    void Update()
    {

        playerPos = player.transform.position;
        if (health <= 0)
        {

            Destroy(gameObject);

        }

        //Patrolling: goes from wapoint to waypoint
        if (Vector2.Distance(transform.position, waypoints[nextWaypoint].position) <= waypointOffset * moveSpeed)
            nextWaypoint++;

        if (nextWaypoint >= waypoints.Length)
            nextWaypoint = 0;

        transform.position = Vector2.MoveTowards(transform.position, waypoints[nextWaypoint].position, moveSpeed * Time.deltaTime);

        //Switch Case responsible for the behaviour of the enemy
        switch (state)
        {
            case "Patrol":

                /*//Patrolling: goes from wapoint to waypoint
                if (Vector2.Distance(transform.position, waypoints[nextWaypoint].position) <= waypointOffset * moveSpeed)
                    nextWaypoint++;

                if (nextWaypoint >= waypoints.Length)
                    nextWaypoint = 0;

                transform.position = Vector2.MoveTowards(transform.position, waypoints[nextWaypoint].position, moveSpeed * Time.deltaTime);*/

                

                //Changing the state to Chase
                if (Vector2.Distance(transform.position, player.transform.position) < viewRange)
                    state = "Chase";

                break;

            case "Chase":

               /* //Patrolling: goes from wapoint to waypoint
                if (Vector2.Distance(transform.position, waypoints[nextWaypoint].position) <= waypointOffset * moveSpeed)
                    nextWaypoint++;

                if (nextWaypoint >= waypoints.Length)
                    nextWaypoint = 0;

                transform.position = Vector2.MoveTowards(transform.position, waypoints[nextWaypoint].position, moveSpeed * Time.deltaTime);*/

                if (player)
                {

                    if (shootTimer <= 0)
                    {

                        Vector3 direction = player.transform.position - Camera.main.WorldToScreenPoint(transform.position);
                        direction.x += bulletStartOffset;
                        
                        GameObject bullet = Instantiate(enemyBullet, transform.position, Quaternion.identity);

                        shootTimer = cooldownTime;
                    }

                    if (shootTimer > 0)
                        shootTimer -= Time.deltaTime;


                    if (Vector2.Distance(transform.position, playerPos) < retreatDist)
                    {

                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPos.x, transform.position.y - playerPos.y), -moveSpeed * 1.5f * Time.deltaTime);

                    }

                    /* else if(Vector2.Distance(transform.position, playerPos) < viewRange  && Vector2.Distance(transform.position, playerPos) > retreatDist) {

                         transform.position = this.transform.position;

                     }*/


                    if (Vector2.Distance(transform.position, player.transform.position) > viewRange)
                    {

                        transform.position = Vector2.MoveTowards(transform.position, waypoints[nextWaypoint].position, moveSpeed * Time.deltaTime);
                        state = "Patrol";
                    }
                        

                }

                break;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Bullet")
        {

            dmgTaken = GameObject.FindWithTag("Gun").GetComponent<gunshoots>().dmg;

            health = health - dmgTaken;

        }
    }
}
