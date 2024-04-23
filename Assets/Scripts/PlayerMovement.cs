using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerMovement : NetworkBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    public float accel = 700f;
    public float brakeForce = 250f;
    public float maxTurnAngle = 30f;

    private float currAccel = 0f;
    private float currBrake = 0f;
    private float currTurnAngle = 0f;

    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTrasnform;
    [SerializeField] Transform frontLeftTrasnform;
    [SerializeField] Transform backRightTrasnform;
    [SerializeField] Transform backLeftTrasnform;
    [SerializeField] public Rigidbody rb;

    public override void OnNetworkSpawn()
    {
        rb.GetComponent<Rigidbody>();
        Debug.Log("Spawned");
    }
    
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        GetInput();
        if (Input.GetKey(KeyCode.Space))
        {
            currBrake = brakeForce;
        }
        else
        {
            currBrake = 0f;
        }
        currAccel = accel * verticalInput;
        frontRight.motorTorque = currAccel;
        frontLeft.motorTorque = currAccel;
        //backRight.motorTorque = currAccel;
        //backLeft.motorTorque = currAccel;
        
        HandleSterring();
        UpdateWheels(frontLeft, frontLeftTrasnform);
        UpdateWheels(frontRight, frontRightTrasnform);
        UpdateWheels(backLeft, backLeftTrasnform);
        UpdateWheels(backRight, backRightTrasnform);
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void HandleSterring()
    {
        currTurnAngle = maxTurnAngle * horizontalInput;
        frontLeft.steerAngle = currTurnAngle;
        frontRight.steerAngle = currTurnAngle;
    }

    private void UpdateWheels(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
