using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArc : MonoBehaviour
{
    /// <summary>
    /// 放物線の描画ON/OFF
    /// </summary>
    //[SerializeField]
    private bool drawArc = true;

    /// <summary>
    /// 放物線を構成する線分の数
    /// </summary>
    //[SerializeField]
    private int segmentCount = 60;

    /// <summary>
    /// 放物線を何秒分計算するか
    /// </summary>
    private float predictionTime = 6.0F;

    /// <summary>
    /// 放物線のMaterial
    /// </summary>
    [SerializeField, Tooltip("放物線のマテリアル")]
    private Material arcMaterial;

    /// <summary>
    /// 放物線の幅
    /// </summary>
    [SerializeField, Tooltip("放物線の幅")]
    private float arcWidth = 0.02F;

    /// <summary>
    /// 放物線を構成するLineRenderer
    /// </summary>
    private LineRenderer[] lineRenderers;

    /// <summary>
    /// 弾の初速度や生成座標を持つコンポーネント
    /// </summary>
    private ShootBullet shootBullet;

    /// <summary>
    /// 弾の初速度
    /// </summary>
    private Vector3 initialVelocity;

    /// <summary>
    /// 放物線の開始座標
    /// </summary>
    private Vector3 arcStartPosition;

    /// <summary>
    /// 着弾マーカーオブジェクトのPrefab
    /// </summary>
    [SerializeField, Tooltip("着弾地点に表示するマーカーのPrefab")]
    private GameObject pointerPrefab;

    /// <summary>
    /// 着弾点のマーカーのオブジェクト
    /// </summary>
    private GameObject pointerObject;

    // Start is called before the first frame update
    void Start()
    {
        // 放物線のLineRendererオブジェクトを用意
        CreateLineRendererObjects();

        // マーカーのオブジェクトを用意
        pointerObject = Instantiate(pointerPrefab, Vector3.zero, Quaternion.identity);
        pointerObject.SetActive(false);

        // 弾の初速度や生成座標を持つコンポーネント
        shootBullet = gameObject.GetComponent<ShootBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        // 初速度と放物線の開始座標を更新
        initialVelocity = shootBullet.ShootVelocity;
        arcStartPosition = shootBullet.InstantiatePosition;

        if (drawArc)
        {
            // 放物線を表示
            float timeStep = predictionTime / segmentCount;
            bool draw = false;
            float hitTime = float.MaxValue;
            for (int i = 0; i < segmentCount; i++)
            {
                // 線の座標を更新
                float startTime = timeStep * i;
                float endTime = startTime + timeStep;
                SetLineRendererPosition(i, startTime, endTime, !draw);

                // 衝突判定
                if (!draw)
                {
                    hitTime = GetArcHitTime(startTime, endTime);
                    if (hitTime != float.MaxValue)
                    {
                        draw = true; // 衝突したらその先の放物線は表示しない
                    }
                }
            }

            // マーカーの表示
            if (hitTime != float.MaxValue)
            {
                Vector3 hitPosition = GetArcPositionAtTime(hitTime);
                ShowPointer(hitPosition);
            }
        }
    }

    /// <summary>
    /// 指定時間に対するアーチの放物線上の座標を返す
    /// </summary>
    /// <param name="time">経過時間</param>
    /// <returns>座標</returns>
     private Vector3 GetArcPositionAtTime(float time)
    {
        return (arcStartPosition + ((initialVelocity * time) + (0.5f * time * time) * Physics.gravity));
    }
    /// <summary>
    /// LineRendererの座標を更新
    /// </summary>
    /// <param name="index"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    private void SetLineRendererPosition(int index, float startTime, float endTime, bool draw = true)
    {
        lineRenderers[index].SetPosition(0, GetArcPositionAtTime(startTime));
        lineRenderers[index].SetPosition(1, GetArcPositionAtTime(endTime));
        lineRenderers[index].enabled = draw;
    }

    private void CreateLineRendererObjects()
    {
        // 親オブジェクトを作り、LineRendererを持つ子オブジェクトを作る
        GameObject arcObjectsParent = new GameObject("ArcObject");

        lineRenderers = new LineRenderer[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newObject = new GameObject("LineRenderer_" + i);
            newObject.transform.SetParent(arcObjectsParent.transform);
            lineRenderers[i] = newObject.AddComponent<LineRenderer>();

            // 光源関連を使用しない
            lineRenderers[i].receiveShadows = false;
            lineRenderers[i].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            lineRenderers[i].lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            lineRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            // 線の幅とマテリアル
            lineRenderers[i].material = arcMaterial;
            lineRenderers[i].startWidth = arcWidth;
            lineRenderers[i].endWidth = arcWidth;
            lineRenderers[i].numCapVertices = 5;
            lineRenderers[i].enabled = false;
        }
    }

    private void ShowPointer(Vector3 position)
    {
        pointerObject.transform.position = position;
        pointerObject.SetActive(true);
    }

    private float GetArcHitTime(float startTime, float endTime)
    {
        Vector3 startPosition = GetArcPositionAtTime(startTime);
        Vector3 endPosition = GetArcPositionAtTime(endTime);

        RaycastHit hitInfo;
        if (Physics.Linecast(startPosition, endPosition, out hitInfo))
        {
            float distance = Vector3.Distance(startPosition, endPosition);
            return startTime + (endTime - startTime) * (hitInfo.distance / distance);
        }
        return float.MaxValue;
    }

    public void OffDrawArc()
    {
        drawArc = false;
        pointerObject.SetActive(false);
        Debug.Log("放物線停止");
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].enabled = false;
        }
    }
}

