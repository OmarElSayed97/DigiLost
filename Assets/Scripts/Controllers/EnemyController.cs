using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Vector2 target;
    [SerializeField]
    public float speed = 0.5f;
    PlayerManager playerInstance;
    bool isStunned;
    bool isPulled;
    bool isSlowed;
    Transform magneticPull;
    Rigidbody2D rb;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("CurrentRobot").transform.position;
        playerInstance = PlayerManager.instance;
       
        rb = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned && !isPulled)
        {
            TrackTarget();
        }
        else if (isPulled)
        {
            TrackMagnet(magneticPull);
        }

        
      
    }

    private void TrackMagnet(Transform magneticPull)
    {
        if(magneticPull != null)
        {
           
            transform.position = Vector2.MoveTowards(transform.position, magneticPull.position, Time.deltaTime);
        }
    }

    private void TrackTarget()
    {
        target = playerInstance.playerPos;
        transform.position = Vector2.MoveTowards(transform.position, target,speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("CurrentRobot"))
        {
            Destroy(collision.gameObject, 1.5f);
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (isPulled)
            {
                Destroy(gameObject);
            }
        }
    }


    public void Stun(float timeToWait)
    {
        if (!isStunned)
        {
            StartCoroutine(StartStunning(timeToWait));
        }
        
    }

    public void Pull(Transform magneticPuller)
    {
        if (!isPulled)
        {
            magneticPull = magneticPuller;
            StartCoroutine(StartPulling());
        }
       
    }

    public void Slow()
    {
        if (!isSlowed)
        {
            StartCoroutine(StartSlowing());
        }
    }


    IEnumerator StartStunning(float timeToWait)
    {
        isStunned = true;
        yield return new WaitForSeconds(timeToWait);
        isStunned = false;
    }

    IEnumerator StartPulling()
    {
        isPulled = true;
        yield return new WaitForSeconds(3.5f);
        isPulled = false;
        magneticPull = null;
    }

    IEnumerator StartSlowing()
    {
        isSlowed = true;
        speed = speed / 2.5f;
        yield return new WaitForSeconds(3.5f);
        speed = speed * 2.5f;
        isSlowed = false;
    }



}
