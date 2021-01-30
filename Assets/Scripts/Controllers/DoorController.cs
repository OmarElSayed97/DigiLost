using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    SpriteRenderer sprite;
    BoxCollider2D boxCollider;
    PlayerManager player;
    AudioManager audioManager;
    GameObject openedDoor;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = PlayerManager.instance;
        audioManager = AudioManager.Instance;
        openedDoor = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    public void Open()
    {
        sprite.enabled = true;
        boxCollider.isTrigger = true;
        openedDoor.SetActive(true);
        audioManager.Play("DoorOpen");
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("CurrentRobot"))
        {
            player.currentRoom++;
            sprite.enabled = false;
            boxCollider.isTrigger = false;
        }
    }
}
