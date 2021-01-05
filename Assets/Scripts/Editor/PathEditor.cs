using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayfieldPath))]
public class PathEditor : Editor
{
    public PlayfieldPath _playfieldPath;
    Vector3 nodePos;

    void Edit()
    {
        Event guiEvent = Event.current;
        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        //Add new node
        //if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.control)
        //{
        //    _playfieldPath.pathNodes.Add(mousePos);
        //    if(_playfieldPath.pathNodes.Count>1)
        //    {
        //        _playfieldPath.camDefForwardVectors.Add(Vector3.up);
        //    }
        //}

        //Check the content of Cam Default Forward Direction
        //if(_playfieldPath.camDefForwardVectors.Count+1 != _playfieldPath.pathNodes.Count)
        //{
        //    for(int i=0; i<_playfieldPath.pathNodes.Count - _playfieldPath.camDefForwardVectors.Count - 1; i++)
        //    {
        //        _playfieldPath.camDefForwardVectors.Add(Vector3.up);
        //    }
        //}
    }

    void Draw()
    {
        if(_playfieldPath.showGizmos)
        {
            Event guiEvent = Event.current;
            //Draw poly-lines
            Handles.color = Color.black;
            Handles.DrawPolyLine(_playfieldPath.nodePos.ToArray());

            EditorCurveBinding[] ecb = AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation);
            Keyframe[] k = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys;
            //Handle SphereHandleCap at all nodes
            if (_playfieldPath.nodePos.Count != k.Length)
            {
                
            }
            for (int i = 0; i < k.Length; i++)
            {
                Handles.color = Color.white;
                nodePos = Handles.FreeMoveHandle(i + 200, _playfieldPath.nodePos[i], Quaternion.identity, 20f, Vector3.zero, Handles.SphereHandleCap);
                //_playfieldPath.nodePos[i] = nodePos;

                if (guiEvent.button == 0 && GUIUtility.hotControl == i + 200)
                {
                    _playfieldPath.nodePos[i] = nodePos;
                    /*Keyframe[] l = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys;
                    l[i].value = nodePos.x;
                    AnimationCurve ac = new AnimationCurve(l);
                    AnimationUtility.SetEditorCurve(_playfieldPath.stageAnimation, ecb[0], AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]));

                    l = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[1]).keys;
                    l[i].value = nodePos.y;
                    ac = new AnimationCurve(l);
                    AnimationUtility.SetEditorCurve(_playfieldPath.stageAnimation, ecb[1], AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[1]));

                    l = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[2]).keys;
                    l[i].value = nodePos.z;
                    ac = new AnimationCurve(l);
                    AnimationUtility.SetEditorCurve(_playfieldPath.stageAnimation, ecb[2], AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[2]));*/
                }
            }
        }
    }

    public void Refresh()
    {
        EditorCurveBinding[] ecb = AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation);
        Keyframe[] k = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys;
        _playfieldPath.nodePos = new List<Vector3>(new Vector3[k.Length]);
        for (int i = 0; i < AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys.Length; i++)
        {
            _playfieldPath.nodePos[i] = new Vector3(
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys[i].value,
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[1]).keys[i].value,
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[2]).keys[i].value
                );
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Hubla"))
        {
            foreach(var something in AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation))
            {
                Debug.Log(something.propertyName);
                Debug.Log(AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, something).keys.Length);
            }
        }
        if (GUILayout.Button("Refresh"))
        {
            Refresh();
        }
    }

    public void OnEnable()
    {
        _playfieldPath = (PlayfieldPath)target;
    }

    public void OnSceneGUI()
    {
        Edit();
        Draw();
    }
}
