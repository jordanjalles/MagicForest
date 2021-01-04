using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementProvider : LocomotionProvider
{
    public float moveForceMultiplier = 100.0f;
    public float speedSmoothing = 0.5f;
    private float currentSpeed = 0f;
    public float topWalkSpeed = 10f;

    public float gravityMultiplier = 10000.0f;
    public List<XRController> controllers = null;

    private Rigidbody rBody = null;
    private CapsuleCollider collider = null;
    private GameObject head = null;

    
    // Start is called before the first frame update
    protected override void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        head = GetComponent<XRRig>().cameraGameObject;
    }

    private void Start()
    {
        PositionCollider();
    }

    // Update is called once per frame
    void Update()
    {
        PositionCollider();
        CheckForInput();
        ApplyGravity();
    }

    private void PositionCollider()
    {
        float headHeight = Mathf.Clamp(head.transform.localPosition.y, 1, 2);
        collider.height = headHeight;

        Vector3 newCenter = Vector3.zero;
        newCenter.y = collider.height / 2;
        

        newCenter.x = head.transform.localPosition.x;
        newCenter.z = head.transform.localPosition.z;

        collider.center = newCenter;
    }

    private void CheckForInput()
    {
        foreach(XRController controller in controllers)
        {
            if (controller.enableInputActions)
            {
                CheckForMovement(controller.inputDevice);
            }
        }
    }

    private void CheckForMovement(InputDevice device)
    {
        if(device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
        {
            StartMove(position);
        }
    }

    private void StartMove(Vector2 position)
    {
        // apply the touch position to the head's forward vector
        Vector3 direction = new Vector3(position.x, 0, position.y);
        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        //rotate the input direction by the horizontal head rotation
        direction = Quaternion.Euler(headRotation) * direction;


        //Vector3 movement = direction * newSpeed;
        Vector3 movement = direction * moveForceMultiplier;


        movement *= (topWalkSpeed - rBody.velocity.magnitude)/topWalkSpeed;
        
        rBody.AddForce(movement * Time.deltaTime, ForceMode.Acceleration);
    }



    private void ApplyGravity()
    {
        Vector3 gravity = new Vector3(0, Physics.gravity.y * gravityMultiplier, 0);
        gravity.y *= Time.deltaTime;

        rBody.AddForce(gravity, ForceMode.Acceleration);
    }
}
