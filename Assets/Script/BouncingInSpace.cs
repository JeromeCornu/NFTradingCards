using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingInSpace : MonoBehaviour
{
    Rigidbody rb;
    Vector3 LastVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        LastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var speed = LastVelocity.magnitude;
        var direction = Vector3.Reflect(LastVelocity.normalized, collision.contacts[0].normal);
        rb.velocity = direction * Mathf.Max(speed, 0f);
    }
}
