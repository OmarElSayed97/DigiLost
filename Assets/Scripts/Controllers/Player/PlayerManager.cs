using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [HideInInspector]
    public Vector2 playerPos;
    [SerializeField]
    GameObject gameoverPanel;
    [SerializeField]
    GameObject winningPanel;

    [HideInInspector]
    public int currentRoom;
    [HideInInspector]
    public int batteryValue;
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public bool hasWon;

    [SerializeField]
    Image batteryImage;
    private void Awake()
    {
        instance = this;
        currentRoom = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        batteryImage.fillAmount = (float)batteryValue / 100;
        
        if((batteryValue <= 0 || isDead) && !gameoverPanel.activeSelf)
        {
            gameoverPanel.SetActive(true);
        }
        if(hasWon && !winningPanel.activeSelf)
        {
            winningPanel.SetActive(true);
        }
        
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Win()
    {
        winningPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
