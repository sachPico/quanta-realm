using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//TODO: Provide a system where user can provide int/ hex value to get a bullet object
//1st digit: type
//2nd digit: color variation

public enum SubPatternType {Straight, Fan}

[System.Serializable]
public class SubPatternBase
{
    public PlayfieldObject enemyPlayfieldObject;
    public virtual void OnDrawCustomInspector()
    {

    }
    public virtual void CalculateSubPattern(Turret _turret)
    {

    }

    public static SubPatternBase GetSubPattern(SubPatternType _subPatternType)
    {
        SubPatternBase result = null;
        
        switch(_subPatternType)
        {
            case SubPatternType.Fan:
                result = new FanPatternProfile();
                break;
            case SubPatternType.Straight:
                result = new StraightPatternProfile();
                break;
        }

        return result;
    }
}

#region STRAIGHT
[System.Serializable]
public struct StraightPatternProperty
{
    public float fireInterval;
    public float speed;
    public int bulletIndex;
}

[System.Serializable]
public class StraightPatternProfile : SubPatternBase
{
    [SerializeField]
    public StraightPatternProperty property;

    public override void OnDrawCustomInspector()
    {
        property.fireInterval = EditorGUILayout.FloatField("Fire interval", property.fireInterval);
        property.speed = EditorGUILayout.FloatField("Speed", property.speed);
        property.bulletIndex = EditorGUILayout.IntField("Bullet index", property.bulletIndex);
    }

    public override void CalculateSubPattern(Turret _turret)
    {
        BulletProperty bulletProperty = PoolHandler.instance.RequestObject<BulletProperty>("Straight_E1", true);
        bulletProperty.Reset();
        bulletProperty.Turret = _turret.stayInLocalSpace ? _turret : null;
        bulletProperty.RelativePos = _turret.RelativePos;
        bulletProperty.Velocity = _turret.RelativeForward * property.speed;
    }
}
#endregion

#region FAN
[System.Serializable]
public struct FanPatternProperty
{
    public float degreeRange;
    public int density;
    public float fireInterval;
    public float speed;

    public int bulletIndex;
}

[System.Serializable]
public class FanPatternProfile : SubPatternBase
{
    [SerializeField]
    public FanPatternProperty property;

    public override void OnDrawCustomInspector()
    {
        property.degreeRange = EditorGUILayout.FloatField("Degree Range", property.degreeRange);
        property.density = EditorGUILayout.IntField("Density", property.density);
        property.fireInterval = EditorGUILayout.FloatField("Fire interval", property.fireInterval);
        property.speed = EditorGUILayout.FloatField("Speed", property.speed);
        property.bulletIndex = EditorGUILayout.IntField("Bullet index", property.bulletIndex);
    }

    //public override void CalculateSubPattern(Vector3 _spawnRelativePos, Vector3 _spawnDirection)
    //{
    //    base.CalculateSubPattern(_spawnRelativePos, _spawnDirection);


    //}

    public override void CalculateSubPattern(Turret _turret)
    {
        if (property.density > 0)
        {
            float theta = 0f;
            float thetaRad = 0f;
            float theta_1 = property.degreeRange / -2;
            float delta = property.degreeRange / (property.density - 1);

            Vector3 defaultDirection = _turret.RelativeForward;
            Vector3 direction;
            for (int i = 0; property.density > 0 && i < property.density; i++)
            {
                theta = theta_1 + delta * i;
                thetaRad = theta * Mathf.Deg2Rad;
                direction = defaultDirection;
                direction.x = (direction.x * Mathf.Cos(thetaRad)) - (direction.y * Mathf.Sin(thetaRad));
                direction.y = (direction.x * Mathf.Sin(thetaRad)) + (direction.y * Mathf.Cos(thetaRad));
                direction = direction.normalized;
                direction *= property.speed;

                BulletProperty bulletProperty = PoolHandler.instance.RequestObject<BulletProperty>("Straight_E1", true);
                bulletProperty.Reset();
                bulletProperty.Turret = _turret;
                bulletProperty.RelativePos = _turret.RelativePos;
                bulletProperty.Velocity = direction;
            }
        }
    }
}
#endregion