using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AngleController : MonoBehaviour
{
    [SerializeField, Range(0.01F, 5.0F), Tooltip("感度")]
    private float sensitivity = 0.6F;

    [SerializeField, Tooltip("砲身のオブジェクト")]
    private GameObject barrelObject;

    [SerializeField] private float minAngleX = -60f;
    [SerializeField] private float maxAngleX = 45f;

    private float normalizedXAngle;

    private void Update()
    {
        normalizedXAngle = NormalizeAngle(barrelObject.transform.eulerAngles.x);
    }

    // 角度を正規化する
    private float NormalizeAngle(float angle)
    {
        if (angle > 180) angle -= 360;
        return angle;
    }

    // 上下左右の入力を受け取り、角度を更新する
    public void UpdateAngles(float horizontalInput, float verticalInput)
    {
        if (IsWithinVerticalLimits(-verticalInput))
        {
            barrelObject.transform.Rotate(new Vector3(-verticalInput * sensitivity, 0, 0));
        }
    }

    // 左右の回転を更新
    public void Rotate(float rotationInput)
    {
        transform.Rotate(new Vector3(0, rotationInput * sensitivity, 0));
    }

    // 上下の角度制限をチェック
    private bool IsWithinVerticalLimits(float verticalInput)
    {
        float newAngle = normalizedXAngle + verticalInput * sensitivity;
        return newAngle >= minAngleX && newAngle <= maxAngleX;
    }

    // 感度の変更
    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = Mathf.Clamp(newSensitivity, 0.01f, 5.0f);
    }
}