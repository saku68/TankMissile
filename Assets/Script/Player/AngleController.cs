using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AngleController : MonoBehaviour
{
	/// <summary>
	/// 感度
	/// </summary>
	[SerializeField, Range(0.01F, 5.0F), Tooltip("感度")]
	private float sensitivity = 0.25F;

	/// <summary>
	/// 砲身のオブジェクト
	/// </summary>
	[SerializeField, Tooltip("砲身のオブジェクト")]
	private GameObject barrelObject;
	// void Update ()
	// {
	//     if (Input.GetKey(KeyCode.A))
	//     {
	//         this.transform.Rotate(new Vector3(0F, -1.0F * sensitivity, 0F));
	//     }
	//     if (Input.GetKey(KeyCode.D))
	//     {
	//         this.transform.Rotate(new Vector3(0F, 1.0F * sensitivity, 0F));
	//     }

	//     if (Input.GetKey(KeyCode.W))
	//     {
	//         barrelObject.transform.Rotate(new Vector3(-1.0F * sensitivity, 0F, 0F));
	//     }
	//     if (Input.GetKey(KeyCode.S) && barrelObject.transform.localRotation.eulerAngles.x < 90f)
	//     {
	//         barrelObject.transform.Rotate(new Vector3(1.0F * sensitivity, 0F, 0F));
	//     }
	// }

	[SerializeField]
	private float minAngleY;

	[SerializeField]
	private float maxAngleY;
	[SerializeField]
	private float minAngleX;

	[SerializeField]
	private float maxAngleX;

	void Update()
	{
		// 左右キーの入力を取得
		float horizontal = Input.GetAxis("Horizontal");
		// 現在のGameObjectのY軸方向の角度を取得
		float currentYAngle = transform.eulerAngles.y;

		// 上下キーの入力を取得
		float vertical = Input.GetAxis("Vertical");
		// 現在のBurrelのX軸方向の角度を取得
		float currentXAngle = barrelObject.transform.eulerAngles.x;

		// 現在の角度が180より大きい場合
		if (currentYAngle > 180)
		{
			// デフォルトでは角度は0～360なので-180～180となるように補正
			currentYAngle = currentYAngle - 360;
		}

		// 現在の角度が180より大きい場合
		if (currentXAngle > 180)
		{
			// デフォルトでは角度は0～360なので-180～180となるように補正
			currentXAngle = currentXAngle - 360;
		}

		// (現在の角度が最小角度以上かつキー入力が0未満(左キー押下)) または (現在の角度が最大角度以下かつキー入力が0より大きい(右キー押下))の時
		if ((currentYAngle >= minAngleY && horizontal < 0) || (currentYAngle <= maxAngleY && horizontal > 0))
		{
			// Y軸を基準に回転させる
			transform.Rotate(new Vector3(0, horizontal * sensitivity, 0));
		}

		// (現在の角度が最小角度以上かつキー入力が0未満(左キー押下)) または (現在の角度が最大角度以下かつキー入力が0より大きい(右キー押下))の時
		if ((currentXAngle >= minAngleX && -vertical < 0) || (currentXAngle <= maxAngleX && -vertical > 0))
		{
			// Y軸を基準に回転させる
			barrelObject.transform.Rotate(new Vector3(-vertical * sensitivity, 0, 0));
		}
	}
}

