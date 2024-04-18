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

    void Update ()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(new Vector3(0F, -1.0F * sensitivity, 0F));
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(new Vector3(0F, 1.0F * sensitivity, 0F));
        }

        if (Input.GetKey(KeyCode.W))
        {
            barrelObject.transform.Rotate(new Vector3(-1.0F * sensitivity, 0F, 0F));
        }
        if (Input.GetKey(KeyCode.S))
        {
            barrelObject.transform.Rotate(new Vector3(1.0F * sensitivity, 0F, 0F));
        }

    }
}
