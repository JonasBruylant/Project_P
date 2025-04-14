using UnityEngine;

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
        if (dataManager.IsPlayerMovementDisabled()) return;

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
