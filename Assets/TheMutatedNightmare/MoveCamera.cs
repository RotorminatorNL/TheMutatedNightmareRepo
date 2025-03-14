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
    [SerializeField] private int stepsSpeedStabilization = 20;
    [SerializeField] private int stepsDirectionStabilization = 5;
    [SerializeField] private float speedIncrease = 0.01f;
    [SerializeField] private float shiftSpeedIncrease = 0.04f;

    private Vector2 previousMousePos;
    private bool prevMousePosCorrect = false;

    private int currentStepSpeedStab = 0;
    private float currentSpeed = 0f;
    private float currentShiftSpeed = 0f;

    void Update()
    {
        if (!camera_ia_rightclick.action.IsPressed()) 
        {
            prevMousePosCorrect = false;
            currentStepSpeedStab = 0;
            currentSpeed = 0f;
            currentShiftSpeed = 0f;
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
        Vector3 nextPos = GetNextPos();
        if (nextPos == Vector3.zero)
        {
            currentStepSpeedStab = 0;
            currentSpeed = 0f;
            currentShiftSpeed = 0f;
        }

        currentSpeed += speedIncrease;
        currentShiftSpeed += shiftSpeedIncrease;

        Vector3 nextPosCorrection;
        if (currentStepSpeedStab <= stepsSpeedStabilization)
        {
            float currentSpeedCorrection = currentSpeed / stepsSpeedStabilization * currentStepSpeedStab;
            float currentSpeedShiftCorrection = currentShiftSpeed / stepsSpeedStabilization * currentStepSpeedStab;

            if (camera_ia_shiftpress.action.IsPressed()) nextPosCorrection = currentSpeedShiftCorrection * Time.deltaTime * nextPos;
            else nextPosCorrection = currentSpeedCorrection * Time.deltaTime * nextPos;
            
            currentStepSpeedStab++;
        }
        else
        {
            if (camera_ia_shiftpress.action.IsPressed()) nextPosCorrection = currentShiftSpeed * Time.deltaTime * nextPos;
            else nextPosCorrection = currentSpeed * Time.deltaTime * nextPos;
        }

        transform.position += nextPosCorrection;
    }

    private Vector3 GetNextPos()
    {
        Vector3 moveDirection = camera_ia_move.action.ReadValue<Vector3>();
        if (moveDirection == Vector3.zero) return Vector3.zero;

        Vector3 returnValue = Vector3.zero;
        if (moveDirection.z > 0) returnValue += transform.forward;
        if (moveDirection.z < 0) returnValue += -transform.forward;
        if (moveDirection.x > 0) returnValue += transform.right;
        if (moveDirection.x < 0) returnValue += -transform.right;
        if (moveDirection.y > 0) returnValue += transform.up;
        if (moveDirection.y < 0) returnValue += -transform.up;
        return returnValue;
    }
}