using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    CharacterController cc;

    DataManager dataManager;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        dataManager = DataManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        //Apply gravity at all times.
        var moveVector = Vector3.zero;
        if (cc.isGrounded == false)
        {
            //Add our gravity Vecotr
            moveVector += Physics.gravity;
        }
        cc.Move(moveVector * Time.deltaTime);


        //if (dataManager.IsPlayerMovementDisabled()) return;

        MyInput();
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        cc.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }
}
