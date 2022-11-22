using UnityEngine;

public class EnemyMovementRandom : Entity
{
    private Rigidbody _rigidBody;
    private EnemyHealth _enemyHealth;
    private Weapon _weapon;
    private Seeker _seeker;

    private float _time = 0;

    public float TurnSpeed = 5;
    public float MoveSpeed = 6;

    public void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _weapon = GetComponentInChildren<Weapon>();
    }

    public void Update()
    {
        if (_enemyHealth.IsDead)
            return;

        _time += Time.deltaTime / 1;

        var x = Mathf.Cos(_time) * MoveSpeed;
        var y = Mathf.Sin(_time) * MoveSpeed;

        MoveTowardsTarget(_rigidBody, new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y), 0, MoveSpeed);
    }
}
