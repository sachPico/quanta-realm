using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct KamikazeProperty
{
    public Vector3 velocity;
}

public class KamikazeEnemy : EnemyBase, IEnemyProfile
{
    public enum KamikazeVariant { A, B, C };

    public static string officialName = "aCMi-008 Kamikaze";

    [SerializeField] public KamikazeProperty kamikazeProperty;
    //public KamikazeVariant kamikazeVariant;

    #region METHODS
    #region OVERRIDDEN
    public object GetProfile()
    {
        return kamikazeProperty;
    }

    public string GetOfficialName()
    {
        return officialName;
    }

    public Vector3 GetRelativePosition(PlayfieldObject _playfieldObject)
    {
        Vector3 value;

        value = kamikazeProperty.velocity * Time.fixedDeltaTime + _playfieldObject.RelativePos;

        return value;
    }

    public void GetVariantProperty()
    {
        // switch(kamikazeVariant)
        // {
        //     case KamikazeVariant.A:
        //         kamikazeProperty.velocity = Vector3.left * 30f;
        //         break;
        //     case KamikazeVariant.B:
        //         kamikazeProperty.velocity = Vector3.left * 20f;
        //         break;
        //     case KamikazeVariant.C:
        //         kamikazeProperty.velocity = Vector3.left * 10f;
        //         break;
        // }
    }

    public void Initialize(object _kamikazeProperty)
    {
        KamikazeProperty newProperty = (KamikazeProperty) _kamikazeProperty;

        kamikazeProperty.velocity = newProperty.velocity;
    }

    public void OnDrawCustomInspector()
    {
        //TODO: Proses getvariantproperty jadiin satu di base method
        // kamikazeVariant = (KamikazeVariant)UnityEditor.EditorGUILayout.EnumPopup("Variant", kamikazeVariant);
        // GetVariantProperty();
        kamikazeProperty.velocity = UnityEditor.EditorGUILayout.Vector3Field("Velocity", kamikazeProperty.velocity);
    }

    private void FixedUpdate()
    {
        RelativePos = GetRelativePosition(this);
    }
    #endregion
    #endregion
}
