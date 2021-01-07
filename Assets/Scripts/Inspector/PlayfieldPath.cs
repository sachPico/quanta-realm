using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldPath : MonoBehaviour
{
    //[SerializeField]
    //public List<Vector3> pathNodes = new List<Vector3>();
    //public List<Vector3> camDefForwardVectors = new List<Vector3>();
    public string playerGameObjectName;

    public float maxSpeed;

    public Playfield _playfield;
    public DeltaHoriController _dhc;

    public AnimationClip stageAnimation;
    public List<Vector3> nodePos;

    /*public void InitCamForwards()
    {
        for(int i=0; i<camDefForwardVectors.Count; i++)
        {
            camDefForwardVectors[i] = new Vector3(-(pathNodes[i+1]-pathNodes[i]).z, 0, (pathNodes[i+1]-pathNodes[i]).x).normalized;
        }
    }*/

    void Start()
    {

        _playfield = this.gameObject.GetComponent<Playfield>();
        _dhc = GameObject.Find(playerGameObjectName).GetComponent<DeltaHoriController>();

        if (_playfield == null)
        {
            Debug.Log("PF NOT FOUND");
            Debug.Break();
        }
        if (_dhc == null)
        {
            Debug.Log("DHC NOT FOUND");
            Debug.Break();
        }
        //GetComponent<Animation>().Play("stage1_a");
        // transform.GetChild(0).rotation = Quaternion.LookRotation(camDefForwardVectors[0]);
    }

    /*void FixedUpdate()
    {
        interpolant += 1f / (playfieldMoveSpeed * Vector3.Magnitude(pathNodes[nodeIndex+1]-pathNodes[nodeIndex]));
        qInterpolant += playfieldRotateSpeed * Time.deltaTime;
        qInterpolant = Mathf.Clamp(qInterpolant, 0, 1);
        if(interpolant >= 1)
        {
            time = 0;
            interpolant = 0;
            qInterpolant = 0;
            if(nodeIndex<pathNodes.Count-1)
            {
                nodeIndex++;
            }
        }
        transform.position = Vector3.Lerp(pathNodes[nodeIndex], pathNodes[nodeIndex+1], interpolant);
    }*/
    /*void Update()
    {
        Vector3 origin;
        time += Time.deltaTime;
        // // if(isChangePivotDir)
        // // {
        // //     transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(_dhc.camRotateRange * _dhc.transform.localPosition.y/ _playfield.rightUpperBorder.y,0,0);
        // //     // Camera.main.transform.rotation = Quaternion.LookRotation(transform.GetChild(0).forward, transform.GetChild(0).up);
        // // }
        // else
        // {
        //     transform.GetChild(0).localPosition = new Vector3(0, pivotMaxYOffset * _dhc.transform.localPosition.y/ _playfield.rightUpperBorder.y,0);
        // }
        if(nodeIndex-1<0)
        {
            origin = transform.right;
        }
        else
        {
            origin = pathNodes[nodeIndex] - pathNodes[nodeIndex-1];
            transform.GetChild(0).localRotation = Quaternion.LookRotation(Vector3.Slerp(camDefForwardVectors[nodeIndex-1], camDefForwardVectors[nodeIndex], qInterpolant));
        }
    }*/
}
