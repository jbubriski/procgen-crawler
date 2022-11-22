using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Entity
{
    private Rigidbody _rigidBody;
    private PlayerHealth _playerHealth;
    private Transform _mesh;
    private Weapon _weapon;

    private bool _isAttacking;
    public EnemyHealth Target;

    public VirtualJoystick VirtualJoystickMove;
    public VirtualJoystick VirtualJoystickLook;

    public float Speed = 8;
    public float TurnSpeed = 8;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _playerHealth = GetComponent<PlayerHealth>();
        _mesh = transform.Find("Model");
        _weapon = GetComponentInChildren<Weapon>();

        VirtualJoystickMove = GameObject.Find("VirtualJoystickMove").GetComponent<VirtualJoystick>();
        VirtualJoystickLook = GameObject.Find("VirtualJoystickLook").GetComponent<VirtualJoystick>();
    }

    void Update()
    {
        if (_playerHealth.IsDead)
            return;

        var xMove = VirtualJoystickMove.Horizontal();
        var yMove = VirtualJoystickMove.Vertical();

        var xLook = VirtualJoystickLook.Horizontal();
        var yLook = VirtualJoystickLook.Vertical();

        var fire2 = Input.GetButton("Fire2");
        var lockOn = Input.GetButtonDown("LockOn");

        // Movement
        if (xMove != 0 || yMove != 0)
        {
            _rigidBody.velocity = new Vector3(xMove * Speed, _rigidBody.velocity.y, yMove * Speed);
        }

        // Aiming
        if (Target != null)
        {
            TurnTowardsTarget(Target.transform, TurnSpeed, true);
        }
        else if (xLook != 0 || yLook != 0)
        {
            TurnTowardsVector(transform, new Vector3(xLook, 0, yLook));
        }

        // Attacking
        if (_isAttacking)
        {
            if (_weapon.IsReadyToAttack())
            {
                _isAttacking = false;
            }
        }
        else if (fire2)
        {
            if (_weapon != null)
            {
                _weapon.Attack();

                _isAttacking = true;
            }
        }


        // Lock On
        if (Target != null && Target.IsDead)
        {
            Target = null;
        }
        else if (lockOn)
        {
            if (Target != null)
            {
                Target = null;
            }
            else
            {
                var newTarget = LevelDirector.Instance.Enemies.Closest(transform);

                var enemyHealth = newTarget.GetComponent<EnemyHealth>();

                if (!enemyHealth.IsDead)
                    Target = enemyHealth;
            }
        }
    }
}

public static class TransformExtensions
{
    public static Transform Closest(this List<Transform> transforms, Transform sourceTransform)
    {
        Transform target = null;
        var targetLength = 0f;

        for (var i = 0; i < transforms.Count; i++)
        {
            var transform = transforms[i];

            if (transform == null)
                continue;

            var enemyHealth = transform.GetComponent<EnemyHealth>();

            if (enemyHealth.IsDead)
                continue;

            if (target == null)
            {
                target = transform;
                targetLength = Vector3.Distance(sourceTransform.position, transform.position);
            }
            else
            {
                var newTargetLength = Vector3.Distance(sourceTransform.position, transform.position);

                if (newTargetLength < targetLength)
                {
                    target = transform;
                    targetLength = newTargetLength;
                }
            }
        }

        return target;
    }
}
