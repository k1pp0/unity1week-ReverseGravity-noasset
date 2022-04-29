using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMover : MonoBehaviour
{
    public float speed = 300.0f;
    float moveX = 0f;
    float moveZ = 0f;

    Rigidbody rigidbody;

    // Start is called before the first frame update

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal") * speed;
        moveZ = Input.GetAxis("Vertical") * speed;
        Debug.Log($"{moveX}, {moveZ}");
        Vector3 direction = new Vector3(moveX, 0, moveZ);

        rigidbody.AddForce(direction, ForceMode.Impulse);
    }
}