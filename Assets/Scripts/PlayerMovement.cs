using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public Rigidbody rb;
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

    private void Start()
    {
        /*if (IsLocalPlayer)
        {
            this.transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = true; ;
        }
        else
        {
            this.transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = false; ;
        }*/
    }
    private void FixedUpdate()
    {
        currAccel = accel * Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
        {
            currBrake = brakeForce;
        }
        else
        {
            currBrake = currBrake = 0f;
        }
        frontRight.motorTorque = currAccel;
        frontLeft.motorTorque = currAccel;

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
    }
}
