using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    const float MoveSpeed = 2f;
    const float BaseDistance = 1f;
    const float PlayerDistance = 3f;

    GameObject Player;
    GameObject Base;

    int Health = 3;

    enum MoveModes
    {
        ToBase,
        ToPlayer,
        Attack,
    };

    MoveModes MoveMode = MoveModes.ToBase;

    public delegate void KilledCB();
    KilledCB Killed;


    public void Activate(GameObject Player, GameObject Base, KilledCB Killed)
    {
        this.Player = Player;
        this.Base = Base;
        this.Killed = Killed;

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
            Destroy(this.gameObject);
        }
    }
}
