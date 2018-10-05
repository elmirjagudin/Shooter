using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    const float HitRate = 1.5f; /* hits per second */
    const float MoveSpeed = 2f;
    const float BaseDistance = 1f;
    const float PlayerDistance = 3f;

    public AudioClip HitSfx;
    public AudioClip AttackSfx;
    public AudioClip DeadSfx;

    GameObject Player;
    GameObject Base;

    int Health = 3;

    float nextHit = 0;

    enum MoveModes
    {
        ToBase,
        ToPlayer,
        Attack,
    };

    MoveModes MoveMode = MoveModes.ToBase;

    public delegate void Callback();
    Callback Killed;
    Callback PlayerHit;

    AudioSource Sound;

    public void Activate(GameObject Player, GameObject Base,
                         Callback Killed, Callback PlayerHit)
    {
        this.Player = Player;
        this.Base = Base;
        this.Killed = Killed;
        this.PlayerHit = PlayerHit;

        Sound = gameObject.AddComponent<AudioSource>();

        gameObject.SetActive(true);
    }

	void Update()
    {
        switch (MoveMode)
        {
            case MoveModes.ToBase:
                MoveMode = MoveToBase();
                break;
            case MoveModes.ToPlayer:
                MoveMode = MoveToPlayer();
                break;
            case MoveModes.Attack:
                MoveMode = Attack();
                break;
        }

        CheckPlayerHit();
	}

    void GetDirectionDistanceTo(GameObject go, out Vector3 direction, out float distance)
    {
        var t = gameObject.transform;
        var moveDir = go.transform.position - t.position;

        /* we only care about XZ-plane */
        moveDir.y = 0;

        distance = moveDir.magnitude;
        direction = moveDir.normalized;
    }

    void CheckPlayerHit()
    {
        Vector3 dir;
        float distance;

        GetDirectionDistanceTo(Player, out dir, out distance);

        if (distance > PlayerDistance)
        {
            /* no hit */
            return;
        }

        var now = Time.time;
        if (now >= nextHit)
        {
            gameObject.GetComponent<Animator>().SetTrigger("attack");
            Sound.PlayOneShot(AttackSfx);
            PlayerHit();

            nextHit = now + HitRate;
        }

    }

    MoveModes MoveToBase()
    {
        Vector3 dir;
        float distance;

        GetDirectionDistanceTo(Base, out dir, out distance);
        if (distance < BaseDistance)
        {
            return MoveModes.ToPlayer;
        }

        gameObject.transform.position += dir * MoveSpeed * Time.deltaTime;
        gameObject.transform.rotation = Quaternion.LookRotation(dir);

        return MoveModes.ToBase;
    }

    MoveModes MoveToPlayer()
    {
        Vector3 dir;
        float distance;

        GetDirectionDistanceTo(Player, out dir, out distance);

        if (distance < PlayerDistance)
        {
            return MoveModes.Attack;
        }

        gameObject.transform.position += dir * MoveSpeed * Time.deltaTime;
        gameObject.transform.rotation = Quaternion.LookRotation(dir);

        return MoveModes.ToPlayer;
    }

    MoveModes Attack()
    {
        Vector3 dir;
        float distance;

        GetDirectionDistanceTo(Player, out dir, out distance);

        if (distance >= PlayerDistance)
        {
            return MoveModes.ToPlayer;
        }

        return MoveModes.Attack;
    }

    void OnTriggerEnter(Collider other)
    {
        Health -= 1;

        if (Health <= 0)
        {
            Killed();
            Sound.PlayOneShot(DeadSfx);
            Destroy(this.gameObject, 1f);
            return;
        }

        gameObject.GetComponent<Animator>().SetTrigger("hit");
        Sound.PlayOneShot(HitSfx);
    }
}
