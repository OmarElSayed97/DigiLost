using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockerRobot : Robot
{
    [SerializeField]
    [Range(2,6)]
    public int shockStunTime;
    [SerializeField]
    [Range(50, 150)]
    public int shockChargePower;
    [SerializeField]
    [Range(0.2f, 2.5f)]
    public float shockRange;
    public ShockerRobot()
    {
        robotType = RobotType.SHOCKER;
        speed = Random.Range(0.5f, 1.3f);
        batteryCapacity = Random.Range(100, 501);
        batteryValue = batteryCapacity * Random.Range(0.1f,0.99f);
        batteryDischargeRate = Random.Range(1, 51);
        transferRate = Random.Range(1, 4);
        abilityCDR = 1;
        ///Unique Abilities
        shockStunTime = Random.Range(2, 7);
        shockChargePower = Random.Range(50, 151);
        shockRange = Random.Range(0.2f, 2.6f);
    }
}
