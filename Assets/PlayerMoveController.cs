using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

[RequireComponent(typeof(CharacterController))]

public class PlayerMoveController : NetworkBehaviour
{
    public float walkingSpeed = 1.8f;
    public float runningSpeed = 3.6f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public GameObject CamComponentModel;
    public List<GameObject> BodyAnimatingParts;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 75.0f;
    // [SerializeField] private GameObject playerModel;
    public Animator animatorSYNC;
    public Animator animatorHands;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        // playerModel.SetActive(false);
        // Dont Lock cursor
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;

        if (!isLocalPlayer)//if not me, for other players
        {
            playerCamera.gameObject.SetActive(false);
            //disable camera component
            CamComponentModel.SetActive(false);

        }
        else //if thats me
        {

            foreach (GameObject item in BodyAnimatingParts)
            {
                item.SetActive(false);
            }

        }
        setPos();
    }

    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (!isLocalPlayer) { return; }
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            // Press Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            moveDirection = Vector3.ClampMagnitude(moveDirection, walkingSpeed);


            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.fixedDeltaTime;
            }

            // Debug.Log("SpeedX: " + curSpeedX + " SpeedY: " + curSpeedY);
            if (curSpeedX != 0 || curSpeedY != 0)
            {
                animatorSYNC.SetBool("IsMoving", true);
                // animatorHands.SetBool("Walking", true);
            }
            else
            {
                // animatorSYNC.SetBool("IsMoving", false);
                // animatorHands.SetBool("Walking", false);
            }
            // Move the controller

            animatorSYNC.SetFloat("SpeedX", curSpeedX / walkingSpeed, 0.1f, Time.fixedDeltaTime);
            animatorSYNC.SetFloat("SpeedY", curSpeedY / walkingSpeed, 0.1f, Time.fixedDeltaTime);
            animatorHands.SetFloat("Speed", curSpeedX);

            characterController.Move(moveDirection * Time.fixedDeltaTime);


            // Player and Camera rotation
            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }

        }

    }
    private void setPos()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(-5, 5));
    }

}