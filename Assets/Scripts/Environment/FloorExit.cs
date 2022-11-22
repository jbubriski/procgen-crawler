using UnityEngine;
using System.Collections;

public class FloorExit : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

     void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LevelDirector.Instance.NextFloor();
        }
    }
}
