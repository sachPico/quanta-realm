using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialBehaviour : MonoBehaviour
{
    public bool isLineRenderer;
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
<<<<<<< HEAD:Assets/Scripts/MaterialBehaviour.cs
            Vector3 startPoint = -transform.right * 5f + transform.position - (transform.forward * laserWidth / 2) + (transform.forward * (2 * i + 1) * laserWidth / (2 * numOfRayCasts));
=======
            Vector3 startPoint = transform.position - (transform.up*laserWidth/2) + (transform.up*(2*i+1)*laserWidth/(2*numOfRayCasts));
>>>>>>> fb9e56b4841fa7f0b8863166a6ea8b01ed5dc8cc:Assets/MaterialBehaviour.cs
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

<<<<<<< HEAD:Assets/Scripts/MaterialBehaviour.cs
        matBuffer = new ComputeBuffer(hitInfos.Length, sizeof(float) * 4);
        //Graphics.ClearRandomWriteTargets();
        //_mat.SetPass(0);
        //_mat.SetBuffer("hitInfoBuffer", matBuffer);
        //Graphics.SetRandomWriteTarget(1, matBuffer, false);
        _mat.SetVector("_BottomLimit", transform.position - new Vector3(5, 5, 0));
        _mat.SetVector("_LaserForwardDirection", transform.right.normalized);
        _mat.SetVector("_LaserUpDirection", transform.forward.normalized);
        _mat.SetVector("_FirstHitInfo", hitInfos[0]);
=======
        matBuffer = new ComputeBuffer(hitInfos.Length, sizeof(float)*4);
        Graphics.ClearRandomWriteTargets();
        _mat.SetPass(0);
        _mat.SetBuffer("hitInfoBuffer", matBuffer);
        Graphics.SetRandomWriteTarget(1, matBuffer, false);
        _mat.SetVector("_BottomLimit",transform.position);// - (transform.right*5) - (transform.up*5));
        _mat.SetVector("_LaserForwardDirection",transform.right.normalized);
        _mat.SetVector("_LaserUpDirection",transform.up.normalized);
        _mat.SetVector("_FirstHitInfo",hitInfos[0]);
>>>>>>> fb9e56b4841fa7f0b8863166a6ea8b01ed5dc8cc:Assets/MaterialBehaviour.cs
        _mat.SetVectorArray("_HitInfos", hitInfos);
        _mat.SetFloat("laserWidth", laserWidth);
        _mat.SetInt("numOfRayCasts", numOfRayCasts);
        if(isLineRenderer)
        {
            _mat.SetInt("_IsLineRenderer", 1);
        }
        else
        {
            _mat.SetInt("_IsLineRenderer", 0);
        }

        matBuffer.Dispose();
<<<<<<< HEAD:Assets/Scripts/MaterialBehaviour.cs
=======

        //_mat.SetBuffer("_hitPosInfo", _shaderHitInfo);
        // if(isCopyFromCompBuffer)
        // {
        //     _shaderHitInfo.GetData(copyHitInfos);
        // }
        LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        if(lr!=null)
        {
            AnimationCurve c = new AnimationCurve();
            c.AddKey(0f, laserWidth/10f);
            c.AddKey(30f, laserWidth/10f);
            
            lr.startWidth = laserWidth;
            lr.endWidth = laserWidth;
            // lr.widthCurve = c;
            // lr.widthMultiplier = laserWidth;
        }
>>>>>>> fb9e56b4841fa7f0b8863166a6ea8b01ed5dc8cc:Assets/MaterialBehaviour.cs
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < numOfRayCasts; i++)
        {
<<<<<<< HEAD:Assets/Scripts/MaterialBehaviour.cs
            Vector3 startPoint = -transform.right * 5f + transform.position - (transform.forward * laserWidth / 2) + (transform.forward * (2 * i + 1) * laserWidth / (2 * numOfRayCasts));
=======
            Vector3 startPoint = transform.position - (transform.up*laserWidth/2) + (transform.up*(2*i+1)*laserWidth/(2*numOfRayCasts));
>>>>>>> fb9e56b4841fa7f0b8863166a6ea8b01ed5dc8cc:Assets/MaterialBehaviour.cs
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
<<<<<<< HEAD:Assets/Scripts/MaterialBehaviour.cs
=======

    void OnDestroy()
    {
        // matBuffer.Dispose();
    }

    // public void SetWidth(float width)
    // {
    //     LineRenderer lr = gameObject.GetComponent<LineRenderer>();
    //     if(lr!=null)
    //     {
    //         AnimationCurve c = new AnimationCurve();
    //         c.AddKey(0f, width/10f);
    //         c.AddKey(30f, width/10f);
            
    //         lr.startWidth = width;
    //         lr.endWidth = width;
    //         // lr.widthCurve = c;
    //         // lr.widthMultiplier = laserWidth;
    //     }
    // }
    // [ExecuteInEditMode]
    // void OnValidate()
    // {
        // LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        // if(lr!=null)
        // {
        //     AnimationCurve c = new AnimationCurve();
        //     c.AddKey(0f, laserWidth/10f);
        //     c.AddKey(30f, laserWidth/10f);
            
        //     lr.startWidth = laserWidth;
        //     lr.endWidth = laserWidth;
        //     // lr.widthCurve = c;
        //     // lr.widthMultiplier = laserWidth;
        // }
    // }
>>>>>>> fb9e56b4841fa7f0b8863166a6ea8b01ed5dc8cc:Assets/MaterialBehaviour.cs
}
