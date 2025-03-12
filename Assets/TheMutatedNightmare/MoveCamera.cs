using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InputActionReference camera_ia_mouseposition;
    [SerializeField] private InputActionReference camera_ia_move;
    [SerializeField] private InputActionReference camera_ia_rightclick;
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float camAngleMoveSpeed = 1;

    private Vector3 moveDirection;

    private Vector2 previousMousePos = new(10000, 10000);
    private Quaternion angleDirection;

    void Update()
    {
        if (!camera_ia_rightclick.action.IsPressed()) return;

        moveDirection = transform.forward + camera_ia_move.action.ReadValue<Vector3>() * moveSpeed;

        Vector2 mousePos = camera_ia_mouseposition.action.ReadValue<Vector2>();

        if (previousMousePos != new Vector2(10000, 10000))
        {
            float newValue = transform.rotation.z + (mousePos.y - previousMousePos.y);
            angleDirection = new Quaternion(transform.rotation.x, transform.rotation.y, newValue, transform.rotation.w);
        }

        previousMousePos = mousePos;
    }

    private void FixedUpdate()
    {
        if (!camera_ia_rightclick.action.IsPressed()) return;

        //rb.linearVelocity = moveDirection;
        //transform.position = transform.position + moveDirection;
        transform.rotation = Quaternion.Lerp(transform.rotation, angleDirection, camAngleMoveSpeed);
    }
}
