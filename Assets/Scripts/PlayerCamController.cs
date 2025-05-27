using UnityEngine;

public class PlayerCamController : MonoBehaviour
{
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;
    
    [SerializeField] private Transform orientation;

    private float _rotationX;
    private float _rotationY;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY;

        _rotationY += mouseX;
        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, _rotationY, 0);
    }
}
