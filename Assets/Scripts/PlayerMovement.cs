using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerMovement : NetworkBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string Vertical = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currSteerAngle;
    private float currBreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

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
        HandleMotor();
        HandleSterring();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }


    private void HandleMotor()
    {
        frontRight.motorTorque = verticalInput * motorForce;
        frontLeft.motorTorque = verticalInput * motorForce;
        backRight.motorTorque = verticalInput * motorForce;
        backLeft.motorTorque = verticalInput * motorForce;
        breakForce = isBreaking ? breakForce : 0f;
        if (isBreaking)
        {
            ApplyBreaking();
        }
    }

    private void ApplyBreaking()
    {
        frontRight.brakeTorque = currBreakForce;
        frontLeft.brakeTorque = currBreakForce;
        backRight.brakeTorque = currBreakForce;
        backLeft.brakeTorque = currBreakForce;
    }

    private void HandleSterring()
    {
        currSteerAngle = maxSteerAngle * horizontalInput;
        frontLeft.steerAngle = currSteerAngle;
        frontRight.steerAngle = currSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeft, frontLeftTrasnform);
        UpdateSingleWheel(frontRight, frontRightTrasnform);
        UpdateSingleWheel(backLeft, backLeftTrasnform);
        UpdateSingleWheel(backRight, backRightTrasnform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
