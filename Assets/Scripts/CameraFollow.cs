using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    PlayerManager player;
    Vector3 target;
    void Start()
    {
        player = PlayerManager.instance;
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target = new Vector3(player.playerPos.x,player.playerPos.y,-1.5f);
        transform.position = target;
    }
}
