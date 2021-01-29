using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullerRobot : Robot
{

    [SerializeField]
    [Range(1, 5)]
    public int pullRange;

   
    public PullerRobot()
    {
        robotType = RobotType.PULLER;
        speed = Random.Range(0.35f, 1f);
        batteryCapacity = Random.Range(100, 501) ;
        batteryValue = batteryCapacity * Random.Range(0.1f, 0.99f); ;
        batteryDischargeRate = Random.Range(1, 51);
        transferRate = Random.Range(1, 4);
        abilityCDR = 5;
        ///Unique Abilities
        pullRange = Random.Range(1, 6);
        
    }
}
