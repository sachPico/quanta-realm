using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CurveMovementAnimationGenerator : MonoBehaviour
{
    public Animation animationContainer;
    public AnimationClip animationClip;
    public Bezier bezier;
    public GameObject target;
    public bool overwriteCurvesWhenGenerating;

    [Range(1, 600)] public int frameBetweenPoints;

    float t;
    AnimationCurve xAnimationCurve;
    AnimationCurve yAnimationCurve;
    AnimationCurve zAnimationCurve;

    EditorCurveBinding xPosBinding = new EditorCurveBinding();
    EditorCurveBinding yPosBinding = new EditorCurveBinding();
    EditorCurveBinding zPosBinding = new EditorCurveBinding();

    public string GetRelativePath(GameObject target, GameObject parent)
    {
        string result = "";
        GameObject currentParent = target.transform.parent.gameObject;
        List<string> parents = new List<string>();

        while(currentParent != parent)
        {
            parents.Add(currentParent.name);
            currentParent = target.transform.parent.gameObject;
        }

        for(int i=parents.Count-1; i>=0; i--)
        {
            result += parents[i]+"/";
        }
        result += target.name;

        return result;
    }

    public void Generate()
    {
        string targetPath = GetRelativePath(target, animationContainer.gameObject);

        xPosBinding.path = targetPath;
        xPosBinding.type = typeof(Transform);
        xPosBinding.propertyName = "m_LocalPosition.x";

        yPosBinding.path = targetPath;
        yPosBinding.type = typeof(Transform);
        yPosBinding.propertyName = "m_LocalPosition.y";

        zPosBinding.path = targetPath;
        zPosBinding.type = typeof(Transform);
        zPosBinding.propertyName = "m_LocalPosition.z";

        xAnimationCurve = new AnimationCurve();
        yAnimationCurve = new AnimationCurve();
        zAnimationCurve = new AnimationCurve();

        if (overwriteCurvesWhenGenerating)
        {
            animationClip.ClearCurves();
        }

        Vector3[] points = bezier.curve.evenlySpacedPoints;

        for(int i=0; i<points.Length; i++)
        {
            t = ((i + 1) * frameBetweenPoints) / 60f;
            xAnimationCurve.AddKey(t, points[i].x);
            yAnimationCurve.AddKey(t, points[i].y);
            zAnimationCurve.AddKey(t, points[i].z);
        }

        AnimationUtility.SetEditorCurve(animationClip, xPosBinding, xAnimationCurve);
        AnimationUtility.SetEditorCurve(animationClip, yPosBinding, yAnimationCurve);
        AnimationUtility.SetEditorCurve(animationClip, zPosBinding, zAnimationCurve);
    }
}

[CustomEditor(typeof(CurveMovementAnimationGenerator))]
public class MovementAnimationGenerator : Editor
{
    CurveMovementAnimationGenerator dst;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate"))
        {
            dst.Generate();
        }

        if(GUILayout.Button("Test Relative Path"))
        {
            Debug.Log(dst.GetRelativePath(dst.target, dst.animationContainer.gameObject));
        }
    }

    private void OnEnable()
    {
        dst = (CurveMovementAnimationGenerator)target;
    }
}
