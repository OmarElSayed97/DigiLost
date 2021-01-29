
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField]
    float timeToDestroy;
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

   
}
