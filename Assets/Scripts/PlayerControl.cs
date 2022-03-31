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
    public float sensitivity = 0.01f;
    public float maxSpeed = 3.0f;
    public float poleAcceleration = 3.0f;
    public SteamVR_Action_Boolean moveAction;
    public SteamVR_Action_Boolean resetStance;
    public LayerMask terrainLayer;

    // For Ski
    public float mass = 1.0f;
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

    public Vector3 normal = Vector3.zero;


    public Vector3 skiForce;

    // for start
    public bool start = false;
    private Skis skis;

    // ski pole 
    public float skiPoleSpeed = 20.0f;
    private bool skiPolePushed = false;

    // sound
    public AudioSource skiBackgroundAudio;
    public AudioSource skiPushAudio;


    // movement
    public float gravityAngle = 40.0f;
    private CharacterController characterController;


    public bool testBody = false;
    private void Awake() 
    {
        prevPosition = transform.position;
        skis = GameObject.Find("skis").GetComponent<Skis>();
        skiForce = Vector3.zero;
        
        characterController = GetComponent<CharacterController>();
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
            Debug.Log("STARTTTTT");
            // hapticMotion.Execute(0, 1f, 1f, 1, leftController);
            if (skis != null) 
            {
                skis.ResetSkiPosition();
            }
            start = true;
        }

        if (!start) return;
        MoveCharacter();
        CalculateSpeed();
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

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10.0f, terrainLayer)) 
        {
            Debug.DrawLine(hitInfo.normal + hitInfo.point, hitInfo.point, Color.green);
            normal = hitInfo.normal;
            Debug.Log("normal: " + normal);
            Vector3 slopeVector = velocity - Vector3.Project(velocity, hitInfo.normal.normalized);
            return slopeVector;

        }
        else 
        {
            Debug.Log("no normal detected");
        }
        return velocity;
    }


    private void CalculateSpeed()
    {
        speed = (transform.position - prevPosition) / Time.fixedDeltaTime;
        if (float.IsNaN(speed.x) || float.IsNaN(speed.y) || float.IsNaN(speed.z))
        {
            speed = Vector3.zero;
        }
        prevPosition = transform.position;

        if (speed.magnitude > maxSpeed)
        {
            speed = speed.normalized * maxSpeed;
        }

        if (speed.magnitude > 0.5f && !skiBackgroundAudio.isPlaying) 
        {
            skiBackgroundAudio.Play();
        } 

        if (speed.magnitude <= 0.5f && skiBackgroundAudio.isPlaying) 
        {
            skiBackgroundAudio.Stop();
        }
    }



    private void MoveCharacter()
    {
        // cancel out y directional speed if the character is grounded
        if (characterController.isGrounded)
        {
            speed.y = 0.0f;
        }

        // gravity
        Vector3 velocity = speed;

        velocity = AdjustVelocityToSlope(velocity);
        velocity = ApplyGravity(speed);

        if (!characterController.isGrounded)
        {
            Debug.Log("should not reach here");
            velocity = AdjustVelocityToSlope(velocity);
        }
        velocity = ApplySkiPoleForce(velocity);
        velocity = ApplyFriction(velocity, normal);

        velocity = ApplyDragForce(velocity);
        //velocity = ApplySkiForce(velocity);
        //Debug.Log("velocity after: " + velocity);

        if (float.IsNaN(velocity.x) || float.IsNaN(velocity.y) || float.IsNaN(velocity.z)) velocity = Vector3.zero;

        //projVelocity = projVelocity.magnitude > maxSpeed ? projVelocity.normalized * maxSpeed : projVelocity;

        Debug.DrawLine(transform.position, transform.position + (velocity * 5), Color.red);

        if (velocity.magnitude < 1.0f) velocity = Vector3.zero;
        
        characterController.Move(velocity * Time.deltaTime);
        //transform.position = transform.position + velocity * Time.deltaTime;
    }

    private Vector3 ApplyGravity(Vector3 speed)
    {
        if (Vector3.Dot(normal.normalized, Vector3.up.normalized) > Mathf.Cos(gravityAngle / 180.0f * Mathf.PI))
        {
            return speed + (Physics.gravity * Time.deltaTime);
        } 

        if (!characterController.isGrounded) 
        {
            return speed + (Physics.gravity * Time.deltaTime);
        }

        return speed;
    }

    private Vector3 ApplyFriction(Vector3 slopeSpeed, Vector3 normal)
    {

        float frictionalForce = frictionCoefficient * Vector3.Project(mass * Physics.gravity, normal).magnitude; // miu * m * g * cos
        if (frictionalForce == float.NaN) {
            frictionalForce = 0.0f;
        }

        float acceleration = frictionalForce / mass;
        float changeOfSpeed = acceleration * sensitivity;
        
        if (changeOfSpeed >= slopeSpeed.magnitude) return Vector3.zero;
        return slopeSpeed - slopeSpeed.normalized * changeOfSpeed;
    }

    private Vector3 ApplyDragForce(Vector3 slopeSpeed)
    {
        float threshold = 1.0f; // when the speed is too low, ignore drag force
        if (slopeSpeed.magnitude < threshold) return slopeSpeed;

        currFrontalArea = head.localPosition.y / SetUp.HEIGHT * fullFrontalArea;
        float dragForce = dragCoefficient *  1.0f / 2.0f * airDensity * currFrontalArea * slopeSpeed.magnitude * slopeSpeed.magnitude; // 1/2 * rho * A * v^2
        float acceleration = dragForce / mass;
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
        Vector3 acceleration = tanSkiForce / mass; // opposite force
        Vector3 changeOfSpeed = acceleration * Time.deltaTime;
        Debug.Log("change of speed: " + changeOfSpeed);

        return slopeSpeed + changeOfSpeed;
    }

    private Vector3 ApplySkiPoleForce(Vector3 slopeSpeed) {
        if (!skiPolePushed) return slopeSpeed;
        skiPolePushed = false;

        if (skiPushAudio.isPlaying) return slopeSpeed;

        Debug.Log("NOW apply ski pole force NOW!!!");
        skiPushAudio.Play();
        //Vector3 forceDir = (skis.transform.forward - Vector3.Project(skis.transform.forward, normal)).normalized;
        Vector3 forceDir = (skis.transform.forward - Vector3.Project(skis.transform.forward, Vector3.up)).normalized;
        Debug.Log("push speed: " + skiPoleSpeed * forceDir * sensitivity);

        Vector3 newSpeed = slopeSpeed + skiPoleSpeed * forceDir * sensitivity;

        if (newSpeed.magnitude > maxSpeed) 
        {
            newSpeed = newSpeed.normalized * maxSpeed;
        }

        return newSpeed;
    }

    public void SetSkiForce(Vector3 controllerDisplacement)
    {
        float magnitude = controllerDisplacement.magnitude;
        float ratio = magnitude / skiUniDisplacementPerFrame;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        skiForce = rotation * controllerDisplacement.normalized * skiForceMagnitude * ratio;
    }

    public void ResetStance()
    {
        speed = Vector3.zero;
        start = false;
        transform.up = Vector3.up;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        skis.ResetSkiPosition();

    }

    public void ExertSkiPoleForce()
    {
        Debug.Log("exert ski pole force");
        skiPolePushed = true;
    }

}
