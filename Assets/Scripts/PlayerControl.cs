using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class PlayerControl : MonoBehaviour
{
    public float frictionCoefficient = 0.15f;
    [Range(0.0f, 1.0f)]
    public float dragCoefficient = 0.4f; // usually range between 0.4 to 1.0
    public float airDensity = 1.225f; // kg / m^3
    public float fullFrontalArea = 0.8f; // m^2
    public float currFrontalArea = 0.0f;
    public float sensitivity = 0.1f;
    public float maxSpeed = 3.0f;
    public float poleAcceleration = 3.0f;
    public SteamVR_Action_Boolean moveAction;
    public SteamVR_Action_Boolean resetStance;
    public LayerMask terrainLayer;
    public SetUp setup;

    // For Ski
    public float skiForceMagnitude = 100f; 
    public float skiUniDisplacementPerFrame = 0.1f;
    
    private Transform cameraRig = null;
    private Transform head = null;

    private Vector3 speed = Vector3.zero;
    public Vector3 prevPosition; 

    // For turning
    public SteamVR_Action_Pose pose;
     public SteamVR_Action_Vibration hapticMotion;
    public SteamVR_Input_Sources body;
    public SteamVR_Input_Sources leftController;

    private Vector3 normal = Vector3.zero;

    private bool poleTrigger = false;
    private Rigidbody rb;

    public Vector3 skiForce;

    private bool start = false;



    public bool testBody = false;
    private void Awake() 
    {
        prevPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        setup = GetComponent<SetUp>();
        GameObject skis = GameObject.Find("skies");
        rb.centerOfMass = new Vector3(0.0f, 0.0f, 0.0f);
        skiForce = Vector3.zero;
    }

    private void Start() 
    {
        cameraRig = SteamVR_Render.Top().origin;
        head = SteamVR_Render.Top().head;

        pose.AddOnUpdateListener(body, BodyTest);
    }

    private void Update() 
    {
        if (resetStance.stateDown) ResetStance();
        if (moveAction.stateDown) {
            hapticMotion.Execute(0, 1f, 1f, 1, leftController);
            start = true;
        }

        if (!start) return;
        CalculateSpeed();
        MoveCharacter();
        //balance();


        // HandleHead();
        // CalculateMovement();
        // HandleHeight();
    }

    void BodyTest(SteamVR_Action_Pose fromAction, SteamVR_Input_Sources fromSource)
    {
    }



    private void balance() 
    {
   
        float threshold = 0.866f;

         RaycastHit contact;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out contact, 3.0f, terrainLayer))
        {
            float shouldNormalize = Vector3.Dot(transform.up.normalized, contact.normal.normalized);
            if (shouldNormalize < threshold)
            {
                Quaternion rotation = Quaternion.FromToRotation(transform.up, contact.normal);
                transform.rotation = rotation * transform.rotation;
            }
        }

    }

    private void OnTriggerEnter(Collider other) {
        if (!moveAction.state) return;
        if (other.gameObject.tag != "terrain") return;
        Debug.Log("Move");
        poleTrigger = true;
    }

    // make sure camera does not follow the rotation of the headset 
    private void HandleHead()
    {
        // Store current
        Vector3 oldPosition = cameraRig.position;
        Quaternion oldRotation = cameraRig.rotation;

        // rotation
        transform.eulerAngles = new Vector3(0.0f, head.rotation.eulerAngles.y, 0.0f);

        // restore
        cameraRig.position = oldPosition;
        cameraRig.rotation = oldRotation;
    } 

    private void HandleHeight()
    {

    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.5f)) 
        {
            Debug.DrawLine(hitInfo.normal + hitInfo.point, hitInfo.point, Color.green);
            normal = hitInfo.normal;
            Vector3 slopeVector = velocity - Vector3.Project(velocity, hitInfo.normal.normalized);
            return slopeVector;

        }
        return velocity;
    }


    private void CalculateSpeed()
    {
        speed = (transform.position - prevPosition) / Time.deltaTime;
        if (float.IsNaN(speed.x) || float.IsNaN(speed.y) || float.IsNaN(speed.z))
        {
            speed = Vector3.zero;
        }
        prevPosition = transform.position;
    }



    private void MoveCharacter()
    {
            // gravity
            Vector3 velocity = speed;

            // pole force
            if (poleTrigger)
            {
                velocity += transform.forward.normalized * poleAcceleration * Time.deltaTime;
                poleTrigger = false;
            }

            velocity = ApplyGravity(speed);
            velocity = AdjustVelocityToSlope(velocity);
            velocity = ApplyFriction(velocity, normal);
            velocity = ApplyDragForce(velocity);
            velocity = ApplySkiForce(velocity);
            //Debug.Log("velocity after: " + velocity);

            if (float.IsNaN(velocity.x) || float.IsNaN(velocity.y) || float.IsNaN(velocity.z)) velocity = Vector3.zero;

            //projVelocity = projVelocity.magnitude > maxSpeed ? projVelocity.normalized * maxSpeed : projVelocity;

            Debug.DrawLine(transform.position, transform.position + (velocity * 5), Color.red);

            if (velocity.magnitude < 1.0f) velocity = Vector3.zero;
            
            transform.position = transform.position + velocity * Time.deltaTime;
    }

    private Vector3 ApplyGravity(Vector3 speed)
    {
        return speed + (Physics.gravity * Time.deltaTime);
    }

    private Vector3 ApplyFriction(Vector3 slopeSpeed, Vector3 normal)
    {
        float frictionalForce = frictionCoefficient * Vector3.Project(rb.mass * Physics.gravity, normal).magnitude; // miu * m * g * cos
        if (frictionalForce == float.NaN) frictionalForce = 0.0f;
        float acceleration = frictionalForce / rb.mass;
        float changeOfSpeed = acceleration * Time.deltaTime;
        
        if (changeOfSpeed >= slopeSpeed.magnitude) return Vector3.zero;
        return slopeSpeed - slopeSpeed.normalized * changeOfSpeed;
    }

    private Vector3 ApplyDragForce(Vector3 slopeSpeed)
    {
        float threshold = 1.0f; // when the speed is too low, ignore drag force
        if (slopeSpeed.magnitude < threshold) return slopeSpeed;

        currFrontalArea = head.localPosition.y / setup.HEIGHT * fullFrontalArea;
        float dragForce = dragCoefficient *  1.0f / 2.0f * airDensity * currFrontalArea * slopeSpeed.magnitude * slopeSpeed.magnitude; // 1/2 * rho * A * v^2
        float acceleration = dragForce / rb.mass;
        float changeOfSpeed = acceleration * Time.deltaTime;

        if (changeOfSpeed >= slopeSpeed.magnitude) return Vector3.zero;
        return slopeSpeed - slopeSpeed.normalized * changeOfSpeed;
    }

    private Vector3 ApplySkiForce(Vector3 slopeSpeed)
    {
        //Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        Vector3 tanSkiForce = skiForce;
        tanSkiForce = tanSkiForce - Vector3.Project(tanSkiForce, normal);

        Debug.Log("tan ski force: " + tanSkiForce);
        Vector3 acceleration = tanSkiForce / rb.mass; // opposite force
        Vector3 changeOfSpeed = acceleration * Time.deltaTime;
        Debug.Log("change of speed: " + changeOfSpeed);

        return slopeSpeed + changeOfSpeed;
    }

    public void SetSkiForce(Vector3 controllerDisplacement)
    {
        float magnitude = controllerDisplacement.magnitude;
        float ratio = magnitude / skiUniDisplacementPerFrame;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        skiForce = rotation * controllerDisplacement.normalized * skiForceMagnitude * ratio;
        //Debug.Log("ski force: " + skiForce);
    }

    public void ResetStance()
    {
        speed = Vector3.zero;
        start = false;
        transform.up = Vector3.up;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);

    }

}
