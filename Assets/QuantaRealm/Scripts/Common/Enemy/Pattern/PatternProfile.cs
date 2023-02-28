using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SubPatternProperty
{
    public SubPatternType type;
    [SerializeReference]
    public SubPatternBase profile;

    public SubPatternProperty()
    {
        type = SubPatternType.Straight;
        profile = SubPatternBase.GetSubPattern(type);
    }
}

[CreateAssetMenu(fileName = "pp_{enemyName}_1", menuName = "Amnisium/Pattern/PatternProfile")]
public class PatternProfile : ScriptableObject
{
    [SerializeField]
    public List<SubPatternProperty> subPatterns = new List<SubPatternProperty>();
}

[CustomEditor(typeof(PatternProfile)), CanEditMultipleObjects]
public class PatternProfileEditor : Editor
{
    PatternProfile dst;

    public override void OnInspectorGUI()
    {
        SubPatternType typeTrack;
        for(int i=0; i<dst.subPatterns.Count; i++)
        {
            typeTrack = dst.subPatterns[i].type;
            dst.subPatterns[i].type = (SubPatternType)EditorGUILayout.EnumPopup("Sub-Pattern Type", dst.subPatterns[i].type);

            if(typeTrack != dst.subPatterns[i].type)
            {
                dst.subPatterns[i].profile = SubPatternBase.GetSubPattern(dst.subPatterns[i].type);
            }
            dst.subPatterns[i].profile.OnDrawCustomInspector();
            
            if(GUILayout.Button("Delete Sub-Pattern Profile"))
            {
                dst.subPatterns.Remove(dst.subPatterns[i]);
            }
        }

        if(GUILayout.Button("Add Sub-Pattern Profile"))
        {
            dst.subPatterns.Add(new SubPatternProperty());
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        dst = (PatternProfile)target;

        EditorUtility.SetDirty(dst);
    }
}
