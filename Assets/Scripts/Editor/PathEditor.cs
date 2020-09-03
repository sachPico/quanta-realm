using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayfieldPath))]
public class PathEditor : Editor
{
    public PlayfieldPath _playfieldPath;

    void Edit()
    {
        Event guiEvent = Event.current;
        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        //Add new node
        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.control)
        {
            _playfieldPath.pathNodes.Add(mousePos);
            if(_playfieldPath.pathNodes.Count>1)
            {
                _playfieldPath.camDefForwardVectors.Add(Vector3.up);
            }
        }

        //Check the content of Cam Default Forward Direction
        if(_playfieldPath.camDefForwardVectors.Count+1 != _playfieldPath.pathNodes.Count)
        {
            for(int i=0; i<_playfieldPath.pathNodes.Count - _playfieldPath.camDefForwardVectors.Count - 1; i++)
            {
                _playfieldPath.camDefForwardVectors.Add(Vector3.up);
            }
        }
    }

    void Draw()
    {
        if(_playfieldPath.showGizmos)
        {
            Event guiEvent = Event.current;
            //Draw poly-lines
            Handles.color = Color.black;
            Handles.DrawPolyLine(_playfieldPath.pathNodes.ToArray());

            //Draw Camera Default Forward Direction
            Vector3 midPoint;
            Handles.color = new Color(1,0,1,1);
            for(int i=0; i<_playfieldPath.camDefForwardVectors.Count; i++)
            {
                midPoint = (_playfieldPath.pathNodes[i+1]-_playfieldPath.pathNodes[i])/2+_playfieldPath.pathNodes[i];
                Handles.DrawLine(midPoint, midPoint+(_playfieldPath.camDefForwardVectors[i]*50));
            }

            //Handle SphereHandleCap at all nodes
            for(int i=0; i<_playfieldPath.pathNodes.Count; i++)
            {
                Handles.color = Color.white;
                Vector3 nodePos = Handles.FreeMoveHandle(i+200, _playfieldPath.pathNodes[i], Quaternion.identity, 20f, Vector3.zero, Handles.SphereHandleCap);
                if(guiEvent.button == 0 && GUIUtility.hotControl == i+200)
                {
                    // if(_playfieldPath.pathNodes[i] != nodePos)
                    // {
                        _playfieldPath.pathNodes[i] = nodePos;
                    // }
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("Initialize All Camera Forward Directions"))
        {
            _playfieldPath.InitCamForwards();
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
