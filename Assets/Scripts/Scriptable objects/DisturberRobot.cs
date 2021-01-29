using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturberRobot : Robot
{

    [SerializeField]
    [Range(1, 5)]
    public int waveRange;
    public DisturberRobot()
    {
        robotType = RobotType.DISTURBER;
        speed = Random.Range(0.8f, 1.75f);
        batteryCapacity = Random.Range(100, 501);
        batteryValue = batteryCapacity * Random.Range(0.1f, 0.99f); ;
        batteryDischargeRate = Random.Range(1, 51);
        transferRate = Random.Range(1, 4);
        abilityCDR = 2.5f;
        ///Unique Abilities
        waveRange = Random.Range(1, 6);
        
    }


}
