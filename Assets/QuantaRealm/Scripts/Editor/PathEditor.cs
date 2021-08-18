﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayfieldPath))]
public class PathEditor : Editor
{
    public PlayfieldPath _playfieldPath;
    Vector3 nodePos;

    /*void Edit()
    {
        Event guiEvent = Event.current;
        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.control)
        {
            if(_playfieldPath.NodePos.Count > 0)
            {
                mousePos.y=_playfieldPath.NodePos[_playfieldPath.NodePos.Count-1].y;
            }
            _playfieldPath.NodePos.Add(mousePos);
        }
    }

    void Draw()
    {
        Event guiEvent = Event.current;
        Handles.color = Color.black;
        Handles.DrawAAPolyLine(_playfieldPath.NodePos.ToArray());
        
        for (int i = 0; i < _playfieldPath.NodePos.Count; i++)
        {
            Handles.color = Color.white;
            nodePos = Handles.FreeMoveHandle(i + 200, _playfieldPath.NodePos[i], Quaternion.identity, 20f, Vector3.zero, Handles.SphereHandleCap);
            if (guiEvent.button == 0 && GUIUtility.hotControl == i + 200)
            {
                _playfieldPath.NodePos[i] = nodePos;
            }
        }
    }

    void AdjustAnimationClip()
    {
        Keyframe[] newKeyframe = new Keyframe[_playfieldPath.NodePos.Count];
        EditorCurveBinding[] ecb = AnimationUtility.GetCurveBindings(_playfieldPath.StageAnimation);
        
        //Handles Playfield's locomotion adjustment in AnimationClip
        for(int i=0; i<newKeyframe.Length; i++)
        {
            newKeyframe[i].value = _playfieldPath.NodePos[i].x;
            if(i > 0)
            {
                newKeyframe[i].time = newKeyframe[i-1].time + (Vector3.Magnitude(_playfieldPath.NodePos[i] - _playfieldPath.NodePos[i-1])) / _playfieldPath.maxSpeed;
            }
            else
            {
                newKeyframe[i].time = 0;
            }
        }
        AnimationUtility.SetEditorCurve(_playfieldPath.StageAnimation, ecb[0], new AnimationCurve(newKeyframe));

        for(int i=0; i<newKeyframe.Length; i++)
        {
            newKeyframe[i].value = _playfieldPath.NodePos[i].y;
            if(i > 0)
            {
                newKeyframe[i].time = newKeyframe[i-1].time + (Vector3.Magnitude(_playfieldPath.NodePos[i] - _playfieldPath.NodePos[i-1])) / _playfieldPath.maxSpeed;
            }
            else
            {
                newKeyframe[i].time = 0;
            }
        }
        AnimationUtility.SetEditorCurve(_playfieldPath.StageAnimation, ecb[1], new AnimationCurve(newKeyframe));

        for(int i=0; i<newKeyframe.Length; i++)
        {
            newKeyframe[i].value = _playfieldPath.NodePos[i].z;
            if(i > 0)
            {
                newKeyframe[i].time = newKeyframe[i-1].time + (Vector3.Magnitude(_playfieldPath.NodePos[i] - _playfieldPath.NodePos[i-1])) / _playfieldPath.maxSpeed;
            }
            else
            {
                newKeyframe[i].time = 0;
            }
        }
        AnimationUtility.SetEditorCurve(_playfieldPath.StageAnimation, ecb[2], new AnimationCurve(newKeyframe));


        //Handles EnemySpawner adjustment in AnimationClip
        List<AnimationEvent> ae = new List<AnimationEvent>();
        for(int j=0; j<_playfieldPath.StageEnemyProperties.Length; j++)
        {
            ae.Add(new AnimationEvent());
            ae[j].functionName = "Spawn";
            ae[j].time = _playfieldPath.StageEnemyProperties[j].spawnTime;
        }
        AnimationUtility.SetAnimationEvents(_playfieldPath.StageAnimation, ae.ToArray());
    }*/

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        /*if(GUILayout.Button("Show identity quaternion"))
        {
            Debug.Log(new Quaternion(_playfieldPath.x, _playfieldPath.y, _playfieldPath.z, _playfieldPath.w));
        }

        if (GUILayout.Button("Show X of identity quaternion"))
        {
            Debug.Log(_playfieldPath.x);
        }

        if (GUILayout.Button("Show Y of identity quaternion"))
        {
            Debug.Log(_playfieldPath.y);
        }

        if (GUILayout.Button("Show Z of identity quaternion"))
        {
            Debug.Log(_playfieldPath.z);
        }

        if (GUILayout.Button("Show W of identity quaternion"))
        {
            Debug.Log(_playfieldPath.w);
        }

        if(GUILayout.Button("Set Player's forward direction"))
        {
            _playfieldPath.player.rotation = new Quaternion(_playfieldPath.x, _playfieldPath.y, _playfieldPath.z, _playfieldPath.w);
        }*/

        /*
                if (GUILayout.Button("Check Curve Bindings"))
                {
                    foreach(var b in AnimationUtility.GetCurveBindings(_playfieldPath.StageAnimation))
                    {
                        Debug.Log(b.propertyName);
                    }
                }

                if (GUILayout.Button("Adjust Curve Bindings"))
                {
                    AdjustAnimationClip();
                }*/
    }

    public void OnEnable()
    {
        _playfieldPath = (PlayfieldPath)target;
    }

    public void OnSceneGUI()
    {
        /*Edit();
        Draw();*/
    }
}
