﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject BulletOrigin;
    public GameObject Bullet;


    bool Firing = false;

    IEnumerator Fire()
    {
        var t = BulletOrigin.gameObject.transform;
        var sound = GetComponent<AudioSource>();

        while (Input.GetMouseButton(0))
        {
            Instantiate(Bullet, t.position, t.rotation * Bullet.transform.rotation);
            sound.Play();
            yield return new WaitForSeconds(0.2f);
        }

        Firing = false;

    }

	void Update()
    {
        if (Input.GetMouseButton(0) && !Firing)
        {
            Firing = true;
            StartCoroutine(Fire());
        }
	}
}
