
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
    [SerializeField]
    int roomInAction;
    PlayerManager playerInstance;
    AudioManager audioManager;
    bool isStunned;
    bool isPulled;
    bool isSlowed;
    bool isDestroying;
    Transform magneticPull;
    Rigidbody2D rb;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("CurrentRobot").transform.position;
        playerInstance = PlayerManager.instance;
        audioManager = AudioManager.Instance;
        rb = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned && !isPulled && (roomInAction == playerInstance.currentRoom))
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
        Vector2 lookDir = target - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("CurrentRobot"))
        {
            if (!isDestroying)
            {
                audioManager.Play("Explode");
                StartCoroutine(DestroyPlayer(collision.gameObject));
            }
          

        }
        else if (collision.CompareTag("Enemy"))
        {
            if (isPulled)
            {
               
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("CurrentRobot"))
        {
            Debug.Log("IS DYING");
            audioManager.Play("Explode");
            StartCoroutine(DestroyPlayer(collision.gameObject));
        }
        else if (collision.transform.CompareTag("Enemy"))
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

    IEnumerator DestroyPlayer(GameObject player)
    {
        Debug.Log("IS DYING");
        isDestroying = true;
        yield return new WaitForSeconds(0.7f);
        if(player!= null)
        {
            if (player.CompareTag("CurrentRobot"))
            {
                playerInstance.isDead = true;
                Destroy(player);
            }
        }
      
        isDestroying = false;
    }



}
