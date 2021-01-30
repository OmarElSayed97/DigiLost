using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraFollow : MonoBehaviour
{
    PlayerManager player;
    [SerializeField]
    Transform[] anchors;
    int currentRoom;
    bool isLerping;
    void Start()
    {
        player = PlayerManager.instance;
        currentRoom = player.currentRoom;
       
    }

    private void Update()
    {
        if (currentRoom != player.currentRoom)
            ChangeRooms();
    }
    // Update is called once per frame
    IEnumerator EaseMove()
    {
        float t = 0;
        while (t <= 1.0)
        {
            t += Time.deltaTime / 0.2f;
            transform.position = Vector3.Lerp(transform.position, anchors[player.currentRoom - 1].localPosition, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        isLerping = false;
    }
    void ChangeRooms()
    {
        isLerping = true;
        currentRoom = player.currentRoom;
        StartCoroutine(EaseMove());
    }
}
