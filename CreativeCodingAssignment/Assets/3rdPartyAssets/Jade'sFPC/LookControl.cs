using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookControl : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    [SerializeField] Transform player; //Must be the main player object

    float xRotation = 0f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to the screen
    }


    void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked; //Lock cursor to screen
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime; // these variables are essentially getting how much we *want* to move the 
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime; // camera by multiplying how much we moved the mouse by our sensitivity.

        xRotation -= mouseY; // Remember, x rotation is actually the vertical look. Because looking left and right is spinning the player on the y axis.
        xRotation = Mathf.Clamp(xRotation, -90, 90f); // This stops us from flipping the camera around 360 in the vertical direction.

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // This points the camera up and down
        player.Rotate(Vector3.up * mouseX); // This rotates the player left and right
    }

}
