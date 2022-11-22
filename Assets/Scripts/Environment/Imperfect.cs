using UnityEngine;
using System.Collections;

public class Imperfect : MonoBehaviour
{
    public float RotationRange = 2;

    void Start()
    {
        var meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (meshRenderer != null)
        {
            var zRotation = 90 * Random.Range(0, 3);

            meshRenderer.transform.Rotate(Random.Range(-RotationRange, RotationRange), Random.Range(-RotationRange, RotationRange), Random.Range(-RotationRange, RotationRange) + zRotation);
        }
    }

    void Update()
    {

    }
}
