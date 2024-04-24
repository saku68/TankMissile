using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AngleController : MonoBehaviour
{
    [SerializeField, Range(0.01F, 5.0F), Tooltip("感度")]
    private float sensitivity = 0.25F;

    [SerializeField, Tooltip("砲身のオブジェクト")]
    private GameObject barrelObject;

    [SerializeField] private float minAngleY;
    [SerializeField] private float maxAngleY;
    [SerializeField] private float minAngleX;
    [SerializeField] private float maxAngleX;

    // 上下左右の入力を受け取り、角度を更新する
    public void UpdateAngles(float horizontalInput, float verticalInput)
    {
        float currentYAngle = transform.eulerAngles.y;
        float currentXAngle = barrelObject.transform.eulerAngles.x;

        if (currentYAngle > 180) currentYAngle -= 360;
        if (currentXAngle > 180) currentXAngle -= 360;

        if ((currentYAngle >= minAngleY && horizontalInput < 0) || (currentYAngle <= maxAngleY && horizontalInput > 0))
        {
            transform.Rotate(new Vector3(0, horizontalInput * sensitivity, 0));
        }

        if ((currentXAngle >= minAngleX && -verticalInput < 0) || (currentXAngle <= maxAngleX && -verticalInput > 0))
        {
            barrelObject.transform.Rotate(new Vector3(-verticalInput * sensitivity, 0, 0));
        }
    }
}