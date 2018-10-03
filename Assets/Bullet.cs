using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float LifeTime;
    public float Speed;

	void Start ()
    {
        Destroy(this.gameObject, LifeTime);
	}

	// Update is called once per frame
	void Update ()
    {
        var t = gameObject.transform;
        t.position += t.rotation * Vector3.up * Speed * Time.deltaTime;
	}
}
