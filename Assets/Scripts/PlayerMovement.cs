using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float turnSpeed = 1000f;
    private Rigidbody rb;

    public int X { get; private set; }
    public int Z { get; private set; }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        X = Mathf.RoundToInt(transform.position.x / 4);
        Z = Mathf.RoundToInt(-transform.position.z / 4);
    }
    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * turnSpeed);
            var newX = Mathf.RoundToInt(transform.position.x / 4);
            var newZ = Mathf.RoundToInt(-transform.position.z / 4);
            if ((X != newX) || (Z != newZ))
            {
                EventManager.Instance.OnPlayerMoved?.Invoke();
            }
            X = newX;
            Z = newZ;
        }

        rb.velocity = movement * moveSpeed;
    }
}
