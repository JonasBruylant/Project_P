using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float sprintMultiplier = 1.5f;
    private bool _isSprinting = false;


    public bool controlEnabled = true;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controlEnabled) return;

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

        if(_isSprinting)
        {
            cc.Move(moveDirection.normalized * moveSpeed * sprintMultiplier * Time.deltaTime);
            return;
        }
        cc.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }
}
