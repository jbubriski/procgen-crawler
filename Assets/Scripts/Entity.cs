﻿using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool IsDead = false;
    public float DestroyTime = 5f;

    public bool IsLookingTowardsTarget(Transform target, float range = 0.9f)
    {
        var dirFromAtoB = (target.transform.position - transform.position).normalized;
        var dotProd = Vector3.Dot(dirFromAtoB, transform.forward);

        return (dotProd > range);
    }

    public bool IsLookingTowardsTarget(Vector3 targetPosition, float range = 0.9f)
    {
        var dirFromAtoB = (targetPosition - transform.position).normalized;
        var dotProd = Vector3.Dot(dirFromAtoB, transform.forward);

        return (dotProd > range);
    }

    public void TurnTowardsVector(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void TurnTowardsVector(Transform transform, Vector3 direction)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 0.5f);
    }

    public void TurnTowardsTarget(Transform target, float turnSpeed, bool overrideY = false)
    {
        transform.rotation = RotationTowardsTarget(target, turnSpeed, overrideY);
    }

    public void TurnTowardsTarget(Vector3 targetPosition, float turnSpeed)
    {
        transform.rotation = RotationTowardsTarget(targetPosition, turnSpeed);
    }

    public Quaternion RotationTowardsTarget(Transform target, float turnSpeed, bool overrideY = false)
    {
        var oldQuaternion = transform.rotation;
        var oldRotation = transform.eulerAngles;

        transform.LookAt(target.position);
        var newRotation = Quaternion.Euler(oldRotation.x, transform.eulerAngles.y, overrideY ? 0 : oldRotation.z);
        return Quaternion.Slerp(oldQuaternion, newRotation, 0.1f);
    }

    public Quaternion RotationTowardsTarget(Vector3 targetPosition, float turnSpeed)
    {
        var oldQuaternion = transform.rotation;
        var oldRotation = transform.eulerAngles;

        transform.LookAt(targetPosition);
        var newRotation = Quaternion.Euler(oldRotation.x, transform.eulerAngles.y, oldRotation.z);
        return Quaternion.Slerp(oldQuaternion, newRotation, 0.1f);
    }

    public void MoveTowardsTarget(Rigidbody rigidbody, Transform target, float distanceToStop, float speed)
    {
        var direction = (target.position - transform.position);

        if (direction.magnitude > distanceToStop)
        {
            rigidbody.velocity = new Vector3(direction.normalized.x * speed, rigidbody.velocity.y, direction.normalized.z * speed);
        }
    }

    public void MoveTowardsTarget(Rigidbody rigidbody, Vector3 targetPosition, float distanceToStop, float speed)
    {
        var direction = (targetPosition - transform.position);

        if (direction.magnitude > distanceToStop)
        {
            rigidbody.velocity = new Vector3(direction.normalized.x * speed, rigidbody.velocity.y, direction.normalized.z * speed);
        }
    }

    protected void DelayDestroy()
    {
        var coroutine = DelayDestroyInternal();
        StartCoroutine(coroutine);
    }

    private IEnumerator DelayDestroyInternal()
    {
        yield return new WaitForSeconds(DestroyTime);

        Destroy(gameObject);
    }
}
