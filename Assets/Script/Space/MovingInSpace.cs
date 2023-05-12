using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingInSpace : MonoBehaviour
{
    Rigidbody rb;
    private float speed = 100;
    private int direction = 20;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        int var = Random.Range(0, 3);
        //print(var);
        switch (var)
        {
            case 0:
                direction = 20;
                break;

            case 1:
                direction = 15;
                break;

            case 2:
                direction = -10;
                break;

            default:
                direction = 10;
                break;
        }

        rb.AddForce(new Vector3(direction * Time.deltaTime * speed, direction * Time.deltaTime * speed, 0f));

    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0.1f, 0.2f, 0.3f));
    }
}
