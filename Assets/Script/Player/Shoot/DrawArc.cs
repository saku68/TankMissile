using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArc : MonoBehaviour
{
    private bool drawArc = true;
    private int segmentCount = 60;
    private float predictionTime = 6.0F;

    [SerializeField, Tooltip("放物線のマテリアル")]
    private Material arcMaterial;

    [SerializeField, Tooltip("放物線の幅")]
    private float arcWidth = 0.02F;

    private LineRenderer[] lineRenderers;
    private ShootBullet shootBullet;

    private Vector3 initialVelocity;
    private Vector3 arcStartPosition;

    void Start()
    {
        CreateLineRendererObjects();
        shootBullet = gameObject.GetComponent<ShootBullet>();
    }

    void Update()
    {
        initialVelocity = shootBullet.ShootVelocity;
        arcStartPosition = shootBullet.InstantiatePosition;

        if (drawArc)
        {
            float timeStep = predictionTime / segmentCount;
            for (int i = 0; i < segmentCount; i++)
            {
                float startTime = timeStep * i;
                float endTime = startTime + timeStep;
                SetLineRendererPosition(i, startTime, endTime);
            }
        }
    }

    private Vector3 GetArcPositionAtTime(float time)
    {
        return (arcStartPosition + ((initialVelocity * time) + (0.5f * time * time) * Physics.gravity));
    }

    private void SetLineRendererPosition(int index, float startTime, float endTime)
    {
        lineRenderers[index].SetPosition(0, GetArcPositionAtTime(startTime));
        lineRenderers[index].SetPosition(1, GetArcPositionAtTime(endTime));
    }

    private void CreateLineRendererObjects()
    {
        GameObject arcObjectsParent = new GameObject("ArcObject");

        lineRenderers = new LineRenderer[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newObject = new GameObject("LineRenderer_" + i);
            newObject.transform.SetParent(arcObjectsParent.transform);
            lineRenderers[i] = newObject.AddComponent<LineRenderer>();

            lineRenderers[i].receiveShadows = false;
            lineRenderers[i].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            lineRenderers[i].lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            lineRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            lineRenderers[i].material = arcMaterial;
            lineRenderers[i].startWidth = arcWidth;
            lineRenderers[i].endWidth = arcWidth;
            lineRenderers[i].numCapVertices = 5;
            lineRenderers[i].enabled = true;
        }
    }

    public void OffDrawArc()
    {
        drawArc = false;
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].enabled = false;
        }
    }
}

