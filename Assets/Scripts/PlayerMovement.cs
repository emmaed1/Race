using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    //private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float positionRange = 10f;

    public override void OnNetworkSpawn()
    {
        transform.position = new Vector3(Random.Range(positionRange, -positionRange), 0, Random.Range(positionRange, -positionRange));
    }

    private void Update()
    {
       
        if (!IsOwner) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }


    /*private void Update()
    {
        if(!IsOwner) return;
        HandleMovement();
    }
    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDirection.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDirection.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }*/
    /*public Rigidbody rb;
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTrasnform;
    [SerializeField] Transform frontLeftTrasnform;
    [SerializeField] Transform backRightTrasnform;
    [SerializeField] Transform backLeftTrasnform;

    public float accel = 700f;
    public float brakeForce = 250f;
    public float maxTurnAngle = 30f;

    private float currAccel = 0f;
    private float currBrake = 0f;
    private float currTurnAngle = 0f;

    private void FixedUpdate()
    {
        *//*if (!IsOwner) return;*//*

        currAccel = accel * Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currBrake = brakeForce;
        }
        else
        {
            currBrake = 0f;
        }

        frontRight.motorTorque = currAccel;
        frontLeft.motorTorque = currAccel;
        backRight.motorTorque = currAccel;
        backLeft.motorTorque = currAccel;

        frontRight.brakeTorque = currBrake;
        frontLeft.brakeTorque = currBrake;
        backRight.brakeTorque = currBrake;
        backLeft.brakeTorque = currBrake;

        currTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currTurnAngle;
        frontRight.steerAngle = currTurnAngle;

        updateWheel(frontRight, frontRightTrasnform);
        updateWheel(frontLeft, frontLeftTrasnform);
        updateWheel(backRight, backRightTrasnform);
        updateWheel(backLeft, backLeftTrasnform);
    }
    
    public void updateWheel (WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);
        
        trans.position = position;
        trans.rotation = rotation;
    }*/
}
