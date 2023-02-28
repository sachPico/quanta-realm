using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float walkVelocity = 2.5f;
    [SerializeField] private float turnSpeed = 10f;

    private Vector2 input;
    private Vector3 input3;
    private float angle;
    private float moveVelocity;
    private PlayerInput playerInput;
    private Quaternion targetRotation;

    void Start()
    {
        moveVelocity = walkVelocity;
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        GetInput();

        if (Mathf.Abs(input.x) < 0.01 && Mathf.Abs(input.y) < 0.01)
        {
            return;
        }
        else
        {
            moveVelocity = walkVelocity;
        }

        CalculateDirection();
        //Rotate();
        Move();
    }

    private void GetInput()
    {
        Vector2 inputValue = playerInput.actions["Move"].ReadValue<Vector2>();
        input.x = inputValue.x;
        input.y = inputValue.y;

        Debug.Log(input);

        input3.x = input.x;
        input3.y = input.y;
    }

    private void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
    }

    private void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        //Debug.Log(input3);
        transform.position += input3.normalized * moveVelocity * Time.deltaTime;
    }
}