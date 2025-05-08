using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    //https://www.youtube.com/watch?v=f473C43s8nE
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;


    private DataManager _dataManager;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _dataManager = DataManager.Instance;
    }

    void Update()
    {
        //if (_dataManager.IsPlayerMovementDisabled()) return;


        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
