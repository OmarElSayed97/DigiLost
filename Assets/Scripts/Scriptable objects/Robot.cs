using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public class Robot 
{
    [SerializeField]
    public RobotType robotType;

    [SerializeField]
    [Range(0.1f,3f)]
    public float speed;

    [SerializeField]
    [Range(100,500)]
    public int batteryCapacity;

    [SerializeField]
    public float batteryValue;

    [SerializeField]
    [Range(1,50)]
    public int batteryDischargeRate;


    [SerializeField]
    [Range(1,3)]
    public int transferRate;

    [Range(0.2f, 1.5f)]
    public float abilityCDR;
    public Robot()
    {

    }



  
   



}
