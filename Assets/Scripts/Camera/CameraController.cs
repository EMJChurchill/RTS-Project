using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;


//Commentted out code in this script is for mouse to screen edge camera movement, this should be moved to an optional code enable once game has settings (but off my default).
public class CameraController : NetworkBehaviour
{
    [SerializeField] private Transform playerCameraTransform = null;
    [SerializeField] private float speed = 20f;
    //[SerializeField] private float screenBorderThickness = 10f;
    [SerializeField] private Vector2 screenXLimit = Vector2.zero;
    [SerializeField] private Vector2 screenZLimit = Vector2.zero;

    private Vector2 previousInput;

    private Controls controls;

    public override void OnStartAuthority()
    {
        playerCameraTransform.gameObject.SetActive(true);

        controls = new Controls();

        controls.Player.CameraMovement.performed += SetPreviousInput;
        controls.Player.CameraMovement.canceled += SetPreviousInput;

        controls.Enable();
    }

    [ClientCallback]
    private void Update()
    {
        if(!hasAuthority || !Application.isFocused) { return; }

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 pos = playerCameraTransform.position;

        /*
        if(previousInput == Vector2.zero)
        {
            //Vector3 cursorMovement = Vector3.zero;

            //Vector2 cursorPosition = Mouse.current.position.ReadValue();

            /*
            if(cursorPosition.y >= Screen.height - screenBorderThickness)
            {
                cursorMovement.z += 1;
            }
            else if(cursorPosition.y <= screenBorderThickness)
            {
                cursorMovement.z -= 1;
            }
            if (cursorPosition.x >= Screen.width - screenBorderThickness)
            {
                cursorMovement.x += 1;
            }
            else if (cursorPosition.x <= screenBorderThickness)
            {
                cursorMovement.x -= 1;
            }
            

            //pos += cursorMovement.normalized * speed * Time.deltaTime;
        }
        else
        {
            pos += new Vector3(previousInput.x, 0, previousInput.y) * speed * Time.deltaTime;
        }
        */

        //Remove the below line of code (Line 78) once moved screen edge camera movement to optional game panning setting (off by default).
        pos += new Vector3(previousInput.x, 0, previousInput.y) * speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, screenXLimit.x, screenXLimit.y);
        pos.z = Mathf.Clamp(pos.z, screenZLimit.x, screenZLimit.y);

        playerCameraTransform.position = pos;
    }

    private void SetPreviousInput(InputAction.CallbackContext ctx)
    {
        previousInput = ctx.ReadValue<Vector2>();
    }
}