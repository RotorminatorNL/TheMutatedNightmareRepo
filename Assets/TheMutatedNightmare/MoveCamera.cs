using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private InputActionReference camera_ia_mouseposition;
    [SerializeField] private InputActionReference camera_ia_move;
    [SerializeField] private InputActionReference camera_ia_rightclick;
    [SerializeField] private InputActionReference camera_ia_shiftpress;
    [SerializeField] private float camAngleMoveSpeed = 0.4f;
    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private float shiftMoveSpeed = 0.04f;

    private Vector2 previousMousePos;
    private bool prevMousePosCorrect = false;

    void Update()
    {
        if (!camera_ia_rightclick.action.IsPressed()) 
        {
            if (prevMousePosCorrect) prevMousePosCorrect = false;
            return;
        }

        ChangeCameraRot();
        ChangeCameraPos();
    }

    private void ChangeCameraRot()
    {
        Vector2 mousePos = camera_ia_mouseposition.action.ReadValue<Vector2>();

        if (prevMousePosCorrect)
        {
            float newX = mousePos.y - previousMousePos.y;
            float newY = mousePos.x - previousMousePos.x;
            Vector3 angleDirection = new Vector2(-newX, newY) * camAngleMoveSpeed;
            transform.eulerAngles += angleDirection;
        }

        previousMousePos = mousePos;
        prevMousePosCorrect = true;
    }

    private void ChangeCameraPos()
    {
        Vector3 moveDirection = camera_ia_move.action.ReadValue<Vector3>();
        Vector3 nextPos = Vector3.zero;
        if (moveDirection.z > 0) nextPos += transform.forward;
        if (moveDirection.z < 0) nextPos += -transform.forward;
        if (moveDirection.x > 0) nextPos += transform.right;
        if (moveDirection.x < 0) nextPos += -transform.right;
        if (moveDirection.y > 0) nextPos += transform.up;
        if (moveDirection.y < 0) nextPos += -transform.up;
        
        if (!camera_ia_shiftpress.action.IsPressed()) transform.position += nextPos * moveSpeed;
        else transform.position += nextPos * shiftMoveSpeed;
    }
}
