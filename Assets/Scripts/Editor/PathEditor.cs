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

        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.control)
        {
            if(_playfieldPath.nodePos.Count > 0)
            {
                mousePos.y=_playfieldPath.nodePos[_playfieldPath.nodePos.Count-1].y;
            }
            _playfieldPath.nodePos.Add(mousePos);
        }
    }

    void Draw()
    {
        Event guiEvent = Event.current;
        Handles.color = Color.black;
        Handles.DrawAAPolyLine(_playfieldPath.nodePos.ToArray());
        
        for (int i = 0; i < _playfieldPath.nodePos.Count; i++)
        {
            Handles.color = Color.white;
            nodePos = Handles.FreeMoveHandle(i + 200, _playfieldPath.nodePos[i], Quaternion.identity, 20f, Vector3.zero, Handles.SphereHandleCap);
            if (guiEvent.button == 0 && GUIUtility.hotControl == i + 200)
            {
                _playfieldPath.nodePos[i] = nodePos;
            }
        }
    }

    void AdjustAnimationClip()
    {
        Keyframe[] newKeyframe = new Keyframe[_playfieldPath.nodePos.Count];
        EditorCurveBinding[] ecb = AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation);
        float time = 0;
        for(int i=0; i<newKeyframe.Length; i++)
        {
            newKeyframe[i].value = _playfieldPath.nodePos[i].x;
            if(i > 0)
            {
                newKeyframe[i].time = newKeyframe[i-1].time + (Vector3.Magnitude(_playfieldPath.nodePos[i] - _playfieldPath.nodePos[i-1])) / _playfieldPath.maxSpeed;
            }
            else
            {
                newKeyframe[i].time = 0;
            }
        }
        AnimationUtility.SetEditorCurve(_playfieldPath.stageAnimation, ecb[0], new AnimationCurve(newKeyframe));

        time=0;
        for(int i=0; i<newKeyframe.Length; i++)
        {
            newKeyframe[i].value = _playfieldPath.nodePos[i].y;
            if(i > 0)
            {
                newKeyframe[i].time = newKeyframe[i-1].time + (Vector3.Magnitude(_playfieldPath.nodePos[i] - _playfieldPath.nodePos[i-1])) / _playfieldPath.maxSpeed;
            }
            else
            {
                newKeyframe[i].time = 0;
            }
        }
        AnimationUtility.SetEditorCurve(_playfieldPath.stageAnimation, ecb[1], new AnimationCurve(newKeyframe));

        time=0;
        for(int i=0; i<newKeyframe.Length; i++)
        {
            newKeyframe[i].value = _playfieldPath.nodePos[i].z;
            if(i > 0)
            {
                newKeyframe[i].time = newKeyframe[i-1].time + (Vector3.Magnitude(_playfieldPath.nodePos[i] - _playfieldPath.nodePos[i-1])) / _playfieldPath.maxSpeed;
            }
            else
            {
                newKeyframe[i].time = 0;
            }
        }
        AnimationUtility.SetEditorCurve(_playfieldPath.stageAnimation, ecb[2], new AnimationCurve(newKeyframe));

        List<AnimationEvent> ae = new List<AnimationEvent>();
        for(int j=0; j<_playfieldPath.enemySpawnProperties.Count; j++)
        {
            ae.Add(new AnimationEvent());
            ae[j].functionName = "Spawn";
            ae[j].time = _playfieldPath.enemySpawnProperties[j].spawnTime;
        }
        foreach(var d in ae)
        {
            Debug.Log(d.functionName + ": "+ d.time);
        }
        AnimationUtility.SetAnimationEvents(_playfieldPath.stageAnimation, ae.ToArray());
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Check Curve Bindings"))
        {
            foreach(var b in AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation))
            {
                Debug.Log(b.propertyName);
            }
        }

        if (GUILayout.Button("Adjust Curve Bindings"))
        {
            AdjustAnimationClip();
        }

        if (GUILayout.Button("Output All Enemies Spawn Point"))
        {
            foreach(var ep in _playfieldPath.enemySpawnProperties)
            {
                Debug.Log(new Vector3
                (
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation)[0]).Evaluate(ep.spawnTime),
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation)[1]).Evaluate(ep.spawnTime),
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation)[2]).Evaluate(ep.spawnTime)
                ));
            }
        }
        if (GUILayout.Button("Get Animation Events"))
        {
            foreach(var ae in AnimationUtility.GetAnimationEvents(_playfieldPath.stageAnimation))
            {
                Debug.Log(ae.functionName+": "+ae.time);
            }
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
