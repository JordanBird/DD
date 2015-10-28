using UnityEngine;
using System.Collections;

public class DDPlayerController : MonoBehaviour
{
    float speed = 5f;
    float sprintSpeed = 9f;

    float horizontalSpeed = 1;
    float verticalSpeed = 1;

    float rotationSpeed = 2;

    float jumpSpeed = 5;
    float crouchedJumpSpeed = 2.5f;

    Vector3 normalCameraPosition = new Vector3(0, 0, 0);
    Vector3 crouchCameraPosition = new Vector3(0, -0.5f, 0);

    float maxBob = 3f;
    float maxLean = 3f;

    float pitch;
    float yaw;

    bool bobUp = true;
    bool leanLeft = true;

    float currentBob = 0f;
    float currentLean = 0f;

    bool sprinting = false;
    bool crouching = false;

    int playerLayer;

    //Cached objects.
    Camera playerCamera;
    Rigidbody rigidBody;
    Collider capsuleCollider;
    GameObject head;

    // Use this for initialization
    void Start ()
    {
        playerCamera = GetComponentInChildren<Camera>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<Collider>();
        head = transform.FindChild("Head").gameObject;

        playerLayer = 1 << LayerMask.NameToLayer("Player");
        playerLayer = ~playerLayer;
    }
	
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sprinting = Input.GetKey(KeyCode.LeftShift);
        crouching = Input.GetKey(KeyCode.LeftControl);

        //Debug.Log(Physics.CheckSphere(new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y - 0.1f, capsuleCollider.bounds.center.z), 0.1f, playerLayer));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.CheckSphere(new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y - 0.1f, capsuleCollider.bounds.center.z), 0.1f, playerLayer))
            {
                if (crouching)
                    rigidBody.velocity = new Vector3(0, crouchedJumpSpeed, 0);
                else
                    rigidBody.velocity = new Vector3(0, jumpSpeed, 0);
            }
            else
            {
                if (Physics.CheckSphere(transform.position + -transform.forward, 0.1f, playerLayer))
                {
                    Debug.Log("Wall Jump");

                    rigidBody.velocity = new Vector3(jumpSpeed, jumpSpeed / 2, 0);
                }
            }
        }
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        //Debug.Log(rigidBody.velocity.y);

        //Get movement input.
        pitch += rotationSpeed * Input.GetAxis("Mouse Y");
        yaw += rotationSpeed * Input.GetAxis("Mouse X");

        //Clamp the pitch.
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Wrap yaw:
        while (yaw < 0f)
        {
            yaw += 360f;
        }
        while (yaw >= 360f)
        {
            yaw -= 360f;
        }

        // Set orientation:
        transform.eulerAngles = new Vector3(0, yaw, 0f);
        playerCamera.transform.localEulerAngles = new Vector3(-pitch, 0, 0f);

        float translation = 0;

        if (sprinting)
            translation = Input.GetAxis("Vertical") * sprintSpeed;
        else
            translation = Input.GetAxis("Vertical") * speed;

        float rotation = 0;

        if (sprinting)
            rotation = Input.GetAxis("Horizontal") * sprintSpeed;
        else
            rotation = Input.GetAxis("Horizontal") * speed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(rotation, 0, translation);

        //Crouching
        if (crouching)
        {
            playerCamera.transform.localPosition = Vector3.MoveTowards(playerCamera.transform.localPosition, crouchCameraPosition, 0.1f);
        }
        else
        {
            playerCamera.transform.localPosition = Vector3.MoveTowards(playerCamera.transform.localPosition, normalCameraPosition, 0.1f);
        }

        //Bob
        if (translation != 0)
        {
            float angle = 0;

            if (translation > 0)
                angle = Mathf.MoveTowardsAngle(head.transform.localEulerAngles.x, maxBob, 0.35f);
            else
                angle = Mathf.MoveTowardsAngle(head.transform.localEulerAngles.x, -maxBob, 0.35f);

            head.transform.localEulerAngles = new Vector3(angle, head.transform.localEulerAngles.y, head.transform.localEulerAngles.z);
        }
        else
        {
            float angle = Mathf.MoveTowardsAngle(head.transform.localEulerAngles.x, 0, 0.35f);

            head.transform.localEulerAngles = new Vector3(angle, head.transform.localEulerAngles.y, head.transform.localEulerAngles.z);
        }

        //Lean
        if (rotation != 0)
        {
            float angle = 0;

            if (rotation > 0)
                angle = Mathf.MoveTowardsAngle(head.transform.localEulerAngles.z, -maxLean, 0.35f);
            else
                angle = Mathf.MoveTowardsAngle(head.transform.localEulerAngles.z, maxLean, 0.35f);

            head.transform.localEulerAngles = new Vector3(head.transform.localEulerAngles.x, head.transform.localEulerAngles.y, angle);
        }
        else
        {
            float angle = Mathf.MoveTowardsAngle(head.transform.localEulerAngles.z, 0, 0.35f);

            head.transform.localEulerAngles = new Vector3(head.transform.localEulerAngles.x, head.transform.localEulerAngles.y, angle);
        }
    }
}