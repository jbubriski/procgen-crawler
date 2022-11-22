using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour
{
    void Start()
    {
        var meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (meshRenderer != null)
        {
            meshRenderer.transform.localScale = new Vector3(3, 0.1f, 3);
        }
    }

    void Update()
    {

    }
}
