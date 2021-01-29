using Enums;
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
        robotInControl.isActive = false;
        isActive = true;
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
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, robotsLayerMask);
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
                        otherRobotInRange = collider.GetComponent<RobotController>();
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
                RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, transform.up, 1,enemyLayerMask);
                Debug.DrawLine(transform.position, transform.up * 2,Color.red);
                if (raycastHit)
                {
                    EnemyController enemy = raycastHit.transform.GetComponent<EnemyController>();
                    enemy.Stun(5);
                }
                
                RaycastHit2D raycastHit2 = Physics2D.Raycast(transform.position + (transform.up * 0.3f), transform.up, 5, robotsLayerMask);
                if (raycastHit2)
                {
                    
                    RobotController robot = raycastHit2.transform.GetComponent<RobotController>();
                    robot.robot.batteryValue += 50;
                }

            }
            else if(robot.robotType == RobotType.PULLER)
            {
                
                Vector2 posToInstantiate = transform.position + (transform.up * 0.5f);
                GameObject magneticPull = Instantiate(Resources.Load("MagneticPuller") as GameObject, posToInstantiate, Quaternion.identity);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(magneticPull.transform.position, 5f, enemyLayerMask);
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
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f, enemyLayerMask);
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


            timestamp = Time.time + 3;
        }
      
    }


    void HandleBattery()
    {
       
        Debug.Log(name + "  " + robot.batteryValue);
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
