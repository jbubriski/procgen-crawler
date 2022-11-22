using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public Transform BreakAnimation;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "PlayerWeapon")
        {
            Break();
        }
    }

    public void Break()
    {
        Instantiate(BreakAnimation, transform.position + new Vector3(0, 1, 0), Quaternion.identity);

        Destroy(gameObject);
    }
}
