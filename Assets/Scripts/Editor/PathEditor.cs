using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayfieldPath))]
public class PathEditor : Editor
{
    public PlayfieldPath _playfieldPath;
    Vector3 nodePos;
    EditorCurveBinding[] ecb;
    Keyframe[] k;
    AnimationCurve ac;

    void Edit()
    {
        Event guiEvent = Event.current;
        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
    }

    void Draw()
    {
        Event guiEvent = Event.current;

        //Draw poly-lines
        //Handles.color = Color.black;
        //Handles.DrawPolyLine(_playfieldPath.nodePos.ToArray());

        ecb = AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation);
        k = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys;

        /*for (int i = 0; i < k.Length; i++)
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
                AnimationUtility.SetEditorCurve(_playfieldPath.stageAnimation, ecb[2], AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[2]));
            }
        }*/
    }

    public void Refresh()
    {
        ecb = AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation);
        k = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys;
        _playfieldPath.nodePos = new List<Vector3>(new Vector3[k.Length]);
        for (int i = 0; i < k.Length; i++)
        {
            _playfieldPath.nodePos[i] = new Vector3(
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys[i].value,
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[1]).keys[i].value,
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[2]).keys[i].value
                );
        }
    }

    public void Readjust()
    {
        float dst, time;
        ecb = AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation);
        k = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys;
        int limit = k.Length;
        for (int i = limit-1; i > 0; i--)
        {
            dst = Vector3.Magnitude
            (
                new Vector3
                (
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys[i].value,
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[1]).keys[i].value,
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[2]).keys[i].value
                ) -
                new Vector3
                (
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys[i - 1].value,
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[1]).keys[i - 1].value,
                    AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[2]).keys[i - 1].value
                )
            );
            time = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys[i].time - AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys[i-1].time;
            Debug.Log(i + " to " + (i - 1) + " distance is: " + dst + ". Time required is: " + time + ". Speed is: " + dst / time);
            for(int j=0; j < ecb.Length; j++)
            {
                ac = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[j]);
                k = ac.keys;
                k[i].time = k[i-1].time + dst / _playfieldPath.maxSpeed;
                AnimationUtility.SetEditorCurve(_playfieldPath.stageAnimation, ecb[j], new AnimationCurve(k));
            }
        }
        //ac = AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]);
        //k = ac.keys;
        //k[1].time = 199f;
        //AnimationUtility.SetEditorCurve(_playfieldPath.stageAnimation, ecb[0], new AnimationCurve(k));
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
        if (GUILayout.Button("Readjust"))
        {
            Readjust();
        }
        if (GUILayout.Button("Check"))
        {
            ecb = AnimationUtility.GetCurveBindings(_playfieldPath.stageAnimation);
            Debug.Log(AnimationUtility.GetEditorCurve(_playfieldPath.stageAnimation, ecb[0]).keys.Length);
        }
    }

    public void OnEnable()
    {
        _playfieldPath = (PlayfieldPath)target;
    }

    public void OnSceneGUI()
    {
        Draw();
    }
}
