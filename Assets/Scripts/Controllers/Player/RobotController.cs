using Enums;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RobotController : MonoBehaviour
{

    [SerializeField]
    public bool isActive = false;
    [SerializeField]
    public Robot robot;
    [SerializeField]
    TextMeshProUGUI batteryText;

    [HideInInspector]
    Animator animator;
    [SerializeField]
    public GameObject fireAnimation;
    [SerializeField]
    GameObject abilityAnimation;

    bool isOtherRobotInRange;
    RobotController otherRobotInRange;
    float timestamp;
    Vector2 movement;
    Vector3 mousePos;
    Rigidbody2D rb;

    PlayerManager playerInstance;

    int robotsLayerMask;
    int enemyLayerMask;
    void Start()
    {
        
       
        Debug.Log(transform.childCount);
       
        StartRobot();
        animator = GetComponent<Animator>();
        if (CompareTag("CurrentRobot"))
        {
            fireAnimation.SetActive(true);
            animator.enabled = true;
        }
        rb = GetComponent<Rigidbody2D>();
        robotsLayerMask = 1 << 8;
        enemyLayerMask = 1 << 9;
        playerInstance = PlayerManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            CheckIfActivationIsValid();
            GetAxisInput();
            UseAbility();
            HandleBattery();
        }
      
       

    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            DetectOtherRobots();
            Move();
        }
       
    }


    public void Activate(RobotController robotInControl)
    {
        otherRobotInRange = null;
        isOtherRobotInRange = false;
        robotInControl.animator.enabled = false;
        robotInControl.fireAnimation.SetActive(false);
        robotInControl.isActive = false;
        isActive = true;
        animator.enabled = true;
        fireAnimation.SetActive(true);
        tag = "CurrentRobot";
        DetectOtherRobots();
        robotInControl.tag = "Untagged";

    }
    
    void CheckIfActivationIsValid()
    {
        if (isOtherRobotInRange)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(otherRobotInRange != null)
                {
                    otherRobotInRange.Activate(this);
                }     
            }
        }
    }
   

    void DetectOtherRobots()

    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f, robotsLayerMask);
        List<Collider2D>  nearestRobots = new List<Collider2D>();
        float smallestDistance;
        if (colliders.Length > 1)
        {
            isOtherRobotInRange = true;
            if (otherRobotInRange == null)
            {
                foreach(Collider2D collider in colliders)
                {
                    Debug.Log(collider.tag);
                    if (collider.CompareTag("Untagged"))
                    {
                        nearestRobots.Add(collider);
                        //otherRobotInRange = collider.GetComponent<RobotController>();
                    }
                }
                smallestDistance = Mathf.Infinity;
                foreach(Collider2D collider1 in nearestRobots)
                {
                    if(Vector2.Distance(transform.position,collider1.transform.position) < smallestDistance)
                    {
                        otherRobotInRange = collider1.GetComponent<RobotController>();
                    }
                }
                
               
            }

        }
        else
        {
            isOtherRobotInRange = false;
            otherRobotInRange = null;
        }
    }



    //MOVEMENT
    void GetAxisInput()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


    
           

    }

    void Move()
    {
        rb.MovePosition(rb.position + movement * robot.speed *Time.fixedDeltaTime);
        playerInstance.playerPos = rb.position;
       
        Vector2 lookDir = (Vector2)mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

    }

    //ABILITIES
    void UseAbility()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (timestamp <= Time.time))
        {
            if (robot.robotType == RobotType.SHOCKER)
            {
                GameObject shock = Instantiate(abilityAnimation, transform.position, Quaternion.identity);
                shock.transform.rotation = transform.rotation;
                shock.transform.parent = transform;

                RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, transform.up, 2f,enemyLayerMask);
                
                if (raycastHit)
                {
                    EnemyController enemy = raycastHit.transform.GetComponent<EnemyController>();
                    enemy.Stun(5);
                }
                
                RaycastHit2D raycastHit2 = Physics2D.Raycast(transform.position + (transform.up * 0.3f), transform.up, 2.5f, robotsLayerMask);
                if (raycastHit2)
                {
                    
                    RobotController robot = raycastHit2.transform.GetComponent<RobotController>();
                    robot.robot.batteryValue += 50;
                }

            }
            else if(robot.robotType == RobotType.PULLER)
            {
                
                Vector2 posToInstantiate = transform.position + (transform.up * 0.5f);
                GameObject magneticPull = Instantiate(abilityAnimation, posToInstantiate, Quaternion.identity);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(magneticPull.transform.position, 4f, enemyLayerMask);
                if(colliders.Length > 0)
                {
                    foreach(Collider2D collider in colliders)
                    {
                        EnemyController enemy = collider.transform.GetComponent<EnemyController>();
                        enemy.Pull(magneticPull.transform)
;                    }

                }
            }

            else if(robot.robotType == RobotType.DISTURBER)
            {
                GameObject wave = Instantiate(abilityAnimation, transform.position, Quaternion.identity);
                wave.transform.parent = transform;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 4f, enemyLayerMask);
                if (colliders.Length > 0)
                {
                    foreach (Collider2D collider in colliders)
                    {
                        EnemyController enemy = collider.transform.GetComponent<EnemyController>();
                        enemy.Slow();
;
                    }

                }
            }


            timestamp = Time.time + robot.abilityCDR;
        }
      
    }


    void HandleBattery()
    {
       

        robot.batteryValue -= Time.fixedDeltaTime * 4;
        batteryText.text = Mathf.Round((robot.batteryValue /robot.batteryCapacity) * 100).ToString() + "%";
        if (robot.batteryValue <= 0)
        {
            Destroy(gameObject);
        }
        else if (robot.batteryValue > robot.batteryCapacity)
            robot.batteryValue = robot.batteryCapacity;
    }

    void StartRobot()
    {
        if (robot.robotType == RobotType.SHOCKER)
        {
            robot = new ShockerRobot();
           

        }
        else if (robot.robotType == RobotType.DISTURBER)
        {
            robot = new DisturberRobot();

        }
        else if (robot.robotType == RobotType.PULLER)
        {
            robot = new PullerRobot();
          
        }


    }



}
