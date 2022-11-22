using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
public class EnemyMovement : Entity
{
    private Rigidbody _rigidBody;
    private EnemyHealth _enemyHealth;
    private Weapon _weapon;
    private Seeker _seeker;

    private Transform _target;

    // The calculated path
    public Path _path;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float NextWaypointDistance = 1;
    // The waypoint we are currently moving towards
    private int _currentWaypoint = 0;

    // How often to recalculate the path (in seconds)
    public float RepathRate = 0.5f;
    private float _lastRepath = -9999;

    public float TurnSpeed = 5;
    public float MoveSpeed = 6;
    public float ViewDistance = 5;
    public float AttackDistance = 3;
    public float SpotDistance = 16;
    public float LookVariation = 0.9f;

    private bool _isAttacking = false;

    public void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _weapon = GetComponentInChildren<Weapon>();

        //Get a reference to the Seeker component we added earlier
        _seeker = GetComponent<Seeker>();

        if (_seeker != null)
        {
            //OnPathComplete will be called every time a path is returned to this seeker
            //So now we can omit the callback parameter for StartPath
            _seeker.pathCallback += OnPathComplete;
        }
    }

    public void Update()
    {
        if (_enemyHealth.IsDead)
            return;

        // Find new target
        if (_target == null)
        {
            var newTarget = FindObjectOfType<PlayerHealth>();

            if (newTarget != null)
            {
                _target = newTarget.transform;

                if (_seeker != null)
                    _seeker.StartPath(transform.position, _target.position);
            }
        }

        // No target, bail
        if (_target == null && _seeker == null)
        {
            return;
        }

        // Still attacking, don't do anything else
        if (_isAttacking)
        {
            if (_weapon.IsReadyToAttack())
            {
                _isAttacking = false;
            }

            return;
        }

        // Move/Attack checks
        var distanceToTarget = Vector3.Distance(_target.position, transform.position);
        var isLookingTowardsTarget = IsLookingTowardsTarget(_target, LookVariation);

        if (isLookingTowardsTarget && distanceToTarget < AttackDistance)
        {
            if (_weapon != null)
            {
                _weapon.Attack();

                _isAttacking = true;
            }
        }
        else if (distanceToTarget < SpotDistance && _seeker != null)
        {
            if (Time.time - _lastRepath > RepathRate && _seeker.IsDone())
            {
                //Debug.Log("Repathing");
                _lastRepath = Time.time + Random.value * RepathRate * 0.5f;
                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                _seeker.StartPath(transform.position, _target.position, OnPathComplete);
            }
            if (_path == null)
            {
                //Debug.Log("No path");
                // We have no path to follow yet, so don't do anything
                return;
            }
            if (_currentWaypoint > _path.vectorPath.Count) return;
            if (_currentWaypoint == _path.vectorPath.Count)
            {
                //Debug.Log("End Of Path Reached");
                _currentWaypoint++;
                return;
            }

            // Original turn/movement code
            //TurnTowardsTarget(_target.transform, TurnSpeed);

            //if (isLookingTowardsTarget)
            //{
            //    MoveTowardsTarget(_rigidBody, _target.transform, 1, MoveSpeed);
            //}

            var isLookingTowardsWaypoint = IsLookingTowardsTarget(_path.vectorPath[_currentWaypoint], LookVariation);

            TurnTowardsTarget(_path.vectorPath[_currentWaypoint], TurnSpeed);

            if (isLookingTowardsWaypoint)
            {
                //Debug.Log("Moving towards target");
                MoveTowardsTarget(_rigidBody, _path.vectorPath[_currentWaypoint], 1, MoveSpeed);
            }
            else
            {
                //Debug.Log("Not yet looking at target");
            }
        }

        if (_path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Direction to the next waypoint
        //Vector3 dir = (_path.vectorPath[currentWaypoint] - transform.position).normalized;
        //dir *= speed;
        //// Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
        //controller.SimpleMove(dir);

        // The commented line is equivalent to the one below, but the one that is used
        // is slightly faster since it does not have to calculate a square root
        //if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
        if ((transform.position - _path.vectorPath[_currentWaypoint]).sqrMagnitude < NextWaypointDistance * NextWaypointDistance)
        {
            _currentWaypoint++;
            return;
        }



        ////Calculate desired velocity
        //Vector3 dir = CalculateVelocity(GetFeetPosition());

        //if (canMove)
        //{
        //    if (rvoController != null)
        //    {
        //        rvoController.Move(dir);
        //    }
        //    else
        //if (controller != null)
        //    {
        //        controller.SimpleMove(dir);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("No NavmeshController or CharacterController attached to GameObject");
        //    }
        //}
    }

    public void OnPathComplete(Path p)
    {
        //Debug.Log("A path was calculated. Did it fail with an error? " + p.error);
        if (!p.error)
        {
            _path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            _currentWaypoint = 0;
        }
    }

    public void OnDisable()
    {
        if (_seeker != null)
            _seeker.pathCallback -= OnPathComplete;
    }
}
