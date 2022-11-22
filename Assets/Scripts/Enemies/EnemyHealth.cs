using System.Collections;
using UnityEngine;

public class EnemyHealth : Entity
{
    private Rigidbody _rigidBody;
    private Transform _weapon;
    private GameObject _levelDirector;
    private Transform _levelDirectorExtras;

    public Transform HitEffect;
    public Transform DeathEffect;

    public int Health = 100;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _weapon = transform.Find("Weapon");
        _levelDirector = GameObject.Find("LevelDirector");
        _levelDirectorExtras = _levelDirector.transform.Find("Extras");
    }

    void Update()
    {

    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "PlayerWeapon")
        {
            var weapon = collider.gameObject.GetComponentInParent<Weapon>();

            TakeDamage(weapon.Damage);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        Instantiate(HitEffect, transform.position + new Vector3(0, 2), Quaternion.identity);

        if (Health <= 0 && !IsDead)
            Kill();
    }

    public void Kill()
    {
        if (Health > 0)
            Health = 0;

        Instantiate(DeathEffect, transform.position + new Vector3(0, 2), Quaternion.identity);

        IsDead = true;

        if (_weapon != null)
        {
            _weapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            _weapon.GetComponent<Rigidbody>().freezeRotation = false;
            var animator = _weapon.GetComponentInChildren<Animator>();
            
            if (animator != null)
                animator.enabled = false;

            var colliders = _weapon.GetComponentsInChildren<Collider>();

            foreach (var collider in colliders)
            {
                collider.enabled = true;
                collider.isTrigger = false;
            }

            _weapon.transform.parent = _levelDirectorExtras;
        }

        _rigidBody.constraints = RigidbodyConstraints.None;

        _rigidBody.AddForce(30, 100, 30);

        DelayDestroy();
    }
}
