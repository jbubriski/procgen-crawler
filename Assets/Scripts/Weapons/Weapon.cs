using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    private Animator _animator;

    public int Damage = 10;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if (_animator != null)
            _animator.SetTrigger("Ready");
    }

    void Update()
    {

    }

    public void Ready()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Ready");
        }
    }

    public void Attack()
    {
        if (_animator == null)
            return;

        if (Random.Range(0f, 2f) > 1)
        {
            _animator.SetTrigger("Attack1");
        }
        else
        {
            _animator.SetTrigger("Attack2");
        }
    }

    public bool IsReadyToAttack()
    {
        if (_animator == null)
            return true;

        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || _animator.GetCurrentAnimatorStateInfo(0).IsName("Ready");
    }
}
