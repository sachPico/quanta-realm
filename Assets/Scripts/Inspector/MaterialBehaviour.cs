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
    public float laserLength;
    public Material _mat;
    public Vector4[] hitInfos = new Vector4[16];
    public bool isAnimating = false;

    Ray laserRays, gLaserRays;
    RaycastHit rayHitInfo = new RaycastHit();
    ComputeBuffer matBuffer;

    void Start()
    {
    }

    // Update is called once per frame
    [ExecuteInEditMode]
    void Update()
    {
        //hitInfos = new Vector4[numOfRayCasts];
        for(int i=0; i<numOfRayCasts; i++)
        {
            hitInfos[i] = Vector4.zero;
            Vector3 startPoint = transform.position - (transform.up*laserWidth/2) + (transform.up*(2*i+1)*laserWidth/(2*numOfRayCasts));
            laserRays = new Ray(startPoint, transform.right);
            if(Physics.Raycast(laserRays, out rayHitInfo))
            {
                hitInfos[i] = new Vector4(rayHitInfo.point.x, rayHitInfo.point.y, rayHitInfo.point.z, 1);
            }
            else
            {
                hitInfos[i] = Vector4.zero;
            }
            // hitInfos[i] = tmpInfo;
        }

        matBuffer = new ComputeBuffer(hitInfos.Length, sizeof(float)*4);
        Graphics.ClearRandomWriteTargets();
        _mat.SetPass(0);
        _mat.SetBuffer("hitInfoBuffer", matBuffer);
        Graphics.SetRandomWriteTarget(1, matBuffer, false);
        _mat.SetVector("_BottomLimit",transform.position);// - (transform.right*5) - (transform.up*5));
        _mat.SetVector("_LaserForwardDirection",transform.right.normalized);
        _mat.SetVector("_LaserUpDirection",transform.up.normalized);
        _mat.SetVector("_FirstHitInfo",hitInfos[0]);
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

        //_mat.SetBuffer("_hitPosInfo", _shaderHitInfo);
        // if(isCopyFromCompBuffer)
        // {
        //     _shaderHitInfo.GetData(copyHitInfos);
        // }
        LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        if(lr!=null)
        {

            lr.startWidth = laserWidth;
            lr.endWidth = laserWidth;
            lr.SetPosition(1, new Vector3(laserLength, 0, 0));
            // lr.widthCurve = c;
            // lr.widthMultiplier = laserWidth;
        }
    }

    void OnDrawGizmos()
    {
        for(int i=0; i<numOfRayCasts; i++)
        {
            Vector3 startPoint = transform.position - (transform.up*laserWidth/2) + (transform.up*(2*i+1)*laserWidth/(2*numOfRayCasts));
            gLaserRays = new Ray(startPoint, transform.right);
            if(Physics.Raycast(gLaserRays, out rayHitInfo))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(gLaserRays.origin, rayHitInfo.point);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(gLaserRays.origin, gLaserRays.origin + (transform.right*laserLength));
            }
        }
    }

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

    public void SetTrueDoneShoot()
    {
        gameObject.GetComponent<Animator>().SetBool("tr_doneShoot", true);
    }
    public void SetFalseDoneShoot()
    {
        gameObject.GetComponent<Animator>().SetBool("tr_doneShoot", false);
    }
}
