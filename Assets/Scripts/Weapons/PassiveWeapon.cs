using System;
using UnityEngine;

public class Timer
{
    public float TimeMax;
    public float TimeLeft;

    public Timer(float timeMax)
    {
        TimeMax = timeMax;
        TimeLeft = timeMax;
    }

    public void Update(float timeDelta)
    {
        TimeLeft -= timeDelta;
    }

    public bool IsDone()
    {
        return TimeLeft <= 0;
    }

    public void Reset()
    {
        TimeLeft = TimeMax;
    }
}

public class PassiveWeapon : MonoBehaviour
{
    public float DamageDelay = 0.5f;
    public int Damage = 5;

    private Timer _damageTimer;

    void Start()
    {
        _damageTimer = new Timer(DamageDelay);
    }

    void Update()
    {
        if (_damageTimer != null)
            _damageTimer.Update(Time.deltaTime);
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player" && _damageTimer.IsDone())
        {
            var player = collider.gameObject.GetComponent<PlayerHealth>();

            player.TakeDamage(Damage);

            _damageTimer.Reset();
        }
    }
}
