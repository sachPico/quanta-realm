using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialBehaviour : MonoBehaviour
{
    [Range(2, 16)]
    public int numOfRayCasts;
    public float laserWidth;
    public Material _mat;
    public Vector4[] hitInfos = new Vector4[16];

    Ray laserRays, gLaserRays;
    RaycastHit rayHitInfo = new RaycastHit();
    ComputeBuffer matBuffer;

    // Update is called once per frame
    [ExecuteInEditMode]
    void Update()
    {
        //hitInfos = new Vector4[numOfRayCasts];
        for (int i = 0; i < numOfRayCasts; i++)
        {
            hitInfos[i] = Vector4.zero;
            Vector3 startPoint = -transform.right * 5f + transform.position - (transform.forward * laserWidth / 2) + (transform.forward * (2 * i + 1) * laserWidth / (2 * numOfRayCasts));
            laserRays = new Ray(startPoint, transform.right);
            if (Physics.Raycast(laserRays, out rayHitInfo))
            {
                hitInfos[i] = new Vector4(rayHitInfo.point.x, rayHitInfo.point.y, rayHitInfo.point.z, 1);
            }
            else
            {
                hitInfos[i] = Vector4.zero;
            }
            // hitInfos[i] = tmpInfo;
        }

        matBuffer = new ComputeBuffer(hitInfos.Length, sizeof(float) * 4);
        //Graphics.ClearRandomWriteTargets();
        //_mat.SetPass(0);
        //_mat.SetBuffer("hitInfoBuffer", matBuffer);
        //Graphics.SetRandomWriteTarget(1, matBuffer, false);
        _mat.SetVector("_BottomLimit", transform.position - new Vector3(5, 5, 0));
        _mat.SetVector("_LaserForwardDirection", transform.right.normalized);
        _mat.SetVector("_LaserUpDirection", transform.forward.normalized);
        _mat.SetVector("_FirstHitInfo", hitInfos[0]);
        _mat.SetVectorArray("_HitInfos", hitInfos);
        _mat.SetFloat("laserWidth", laserWidth);
        _mat.SetInt("numOfRayCasts", numOfRayCasts);

        matBuffer.Dispose();
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < numOfRayCasts; i++)
        {
            Vector3 startPoint = -transform.right * 5f + transform.position - (transform.forward * laserWidth / 2) + (transform.forward * (2 * i + 1) * laserWidth / (2 * numOfRayCasts));
            gLaserRays = new Ray(startPoint, transform.right);
            if (Physics.Raycast(gLaserRays, out rayHitInfo))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(gLaserRays.origin, rayHitInfo.point);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(gLaserRays.origin, gLaserRays.origin + (transform.right * 500f));
            }
        }
    }
}
