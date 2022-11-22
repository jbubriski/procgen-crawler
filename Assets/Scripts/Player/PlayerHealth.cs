using System.Collections;
using UnityEngine;

public class PlayerHealth : Entity
{
    private Camera _camera;
    private Transform _torch;
    private Transform _weapon;
    private Rigidbody _rigidBody;
    private LevelDirector _levelDirector;
    private Transform _levelDirectorExtras;

    public int Health = 100;

    public Transform HealthDamagePrefab;

    void Start()
    {
        _camera = Camera.main;
        _torch = transform.Find("Torch");
        _weapon = transform.Find("Weapon");
        _rigidBody = GetComponent<Rigidbody>();
        _levelDirector = FindObjectOfType<LevelDirector>();
        _levelDirectorExtras = _levelDirector.transform.Find("Extras");

        _camera.GetComponent<FollowPlayer>().SetPlayer(transform);
    }

    void Update()
    {

    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "EnemyWeapon")
        {
            var weapon = collider.gameObject.GetComponentInParent<Weapon>();

            TakeDamage(weapon.Damage);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        Instantiate(HealthDamagePrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);

        if (Health <= 0)
            Kill();
    }

    public void Kill()
    {
        if (IsDead)
            return;

        if (Health > 0)
            Health = 0;

        IsDead = true;

        _torch.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _torch.GetComponent<Rigidbody>().freezeRotation = false;
        _torch.GetComponent<CapsuleCollider>().enabled = true;
        _torch.transform.parent = _levelDirectorExtras;

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

        ResetGame();
    }

    protected void ResetGame()
    {
        var coroutine = ResetGameInternal();
        StartCoroutine(coroutine);
    }

    private IEnumerator ResetGameInternal()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);

        _levelDirector.ResetAndSetup();
    }
}
