using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Panels;
    int index;
    private void Update()
    {
        if(index>0 && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            Next();
        }
    }

    public void Next()
    {
        index++;
        if(index == Panels.Length)
        {
            SceneManager.LoadScene(1);
        }
        foreach(GameObject go in Panels)
        {
            go.SetActive(false);
        }

        Panels[index].SetActive(true);
    }
}
