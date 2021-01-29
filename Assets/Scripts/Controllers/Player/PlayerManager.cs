using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [HideInInspector]
    public Vector2 playerPos;
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
