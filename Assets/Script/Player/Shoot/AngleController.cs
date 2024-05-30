using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AngleController : MonoBehaviour
{
    [SerializeField, Range(0.01F, 5.0F), Tooltip("感度")]
    private float sensitivity = 0.4F;

    [SerializeField, Tooltip("砲身のオブジェクト")]
    private GameObject barrelObject;

    [SerializeField] private float minAngleX;
    [SerializeField] private float maxAngleX;

    // 上下左右の入力を受け取り、角度を更新する
    public void UpdateAngles(float horizontalInput, float verticalInput)
    {
        float currentXAngle = barrelObject.transform.eulerAngles.x;

        if (currentXAngle > 180) currentXAngle -= 360;

        if ((currentXAngle >= minAngleX && -verticalInput < 0) || (currentXAngle <= maxAngleX && -verticalInput > 0))
        {
            barrelObject.transform.Rotate(new Vector3(-verticalInput * sensitivity, 0, 0));
        }
    }

    // 左右の回転を更新
    public void Rotate(float rotationInput)
    {
        transform.Rotate(new Vector3(0, rotationInput * sensitivity, 0));
    }
    //左右感度の変更
    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = Mathf.Clamp(newSensitivity, 0.01f, 5.0f);
    }
}