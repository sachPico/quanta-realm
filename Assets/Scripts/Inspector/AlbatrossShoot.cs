using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class AlbatrossShoot : MonoBehaviour, IShoot
{
    public Material laserMaterial;
    public Vector3 laserOffset;
    public float laserWidthMax;
    public float laserHeight;
    public int laserLOD;
    private Ray[] laserRay;
    private List<GameObject> laserAnchors;

    private GameObject newLaserAnchor;
    private RaycastHit hitInfo;
    private Vector3 newLaserAnchorPosition;
    private LineRenderer tmpLineRenderer;
    
    public float laserWidth = 0;
    public float laserExtendingRate;
    public void Start()
    {
        laserRay = new Ray[laserLOD];
        laserAnchors = new List<GameObject>();
        for(int i=0; i<laserLOD; i++)
        {
            newLaserAnchor = new GameObject("laserAnchor"+i);
            newLaserAnchor.AddComponent<AlbatrossAnchorHandler>();

            /*newLaserAnchor.AddComponent<Rigidbody>().useGravity = false;
            newLaserAnchor.GetComponent<Rigidbody>().isKinematic = true;*/
            SphereCollider newCollider = newLaserAnchor.AddComponent<SphereCollider>();
            newCollider.isTrigger = true;
            newCollider.radius = 1f;
                
            newLaserAnchor.transform.SetParent(transform);

            //newLaserAnchorPosition = new Vector3(0, -(laserHeight/2)+(laserHeight/laserLOD*i),0);
            newLaserAnchorPosition = transform.position - (transform.up*laserHeight/2) + (transform.up*(2*i+1)*laserHeight/(2*laserLOD)) + transform.TransformPoint(laserOffset);
            newLaserAnchor.transform.position = newLaserAnchorPosition;
            laserAnchors.Add(newLaserAnchor);
            //laserRay[i] = new Ray(newLaserAnchorPosition, transform.right);
            
            tmpLineRenderer = newLaserAnchor.AddComponent<LineRenderer>();
            tmpLineRenderer.shadowCastingMode = ShadowCastingMode.Off;
            tmpLineRenderer.useWorldSpace = false;
            tmpLineRenderer.alignment = LineAlignment.TransformZ;
            tmpLineRenderer.startWidth = laserHeight/laserLOD;
            tmpLineRenderer.endWidth = laserHeight/laserLOD;
            tmpLineRenderer.material = laserMaterial;
            tmpLineRenderer.positionCount = 2;
        }
    }
    public void Shoot()
    {
        if (laserWidth < laserWidthMax)
        {
            laserWidth += laserExtendingRate * Time.deltaTime;
        }
        //Debug.Log(laserRay[0].origin);
    }

    public void Update()
    {
        for (int i = 0; i < laserLOD; i++)
        {
            laserRay[i].origin = laserAnchors[i].transform.position;
            //laserRay[i].origin = laserAnchors[i].transform.position - (laserAnchors[i].transform.up*laserHeight/2) + (laserAnchors[i].transform.up*(2*i+1)*laserHeight/(2*laserLOD));
            laserRay[i].direction = transform.right;
            if (!laserAnchors[i].GetComponent<AlbatrossAnchorHandler>().isCollide)
            {
                if (Physics.Raycast(laserRay[i], out hitInfo, laserWidth))
                {
                    laserAnchors[i].GetComponent<LineRenderer>().SetPosition(1,
                        laserAnchors[i].transform.InverseTransformPoint(hitInfo.point));
                }
                else
                {
                    laserAnchors[i].GetComponent<LineRenderer>().SetPosition(1, Vector3.right * laserWidth);
                }
            }
        }

        if(Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            laserWidth=0;
        }
    }

    public void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            for(int i=0; i<laserLOD; i++)
            {
                if(Physics.Raycast(laserRay[i], out hitInfo, laserWidth))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawSphere(laserRay[i].origin, 0.5f);
                //Gizmos.DrawLine(laserRay[i].origin, laserRay[i].origin + laserRay[i].direction*100f);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(laserRay[i].origin, laserRay[i].origin + laserRay[i].direction*100f);
            }
        }
    }
}
