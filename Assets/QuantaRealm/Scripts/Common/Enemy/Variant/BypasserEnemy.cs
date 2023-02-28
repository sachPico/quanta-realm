using Sachet.Experimental;
using Sachet.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BypasserEnemy : EnemyBase, IEnemyProfile
{
    [System.Serializable]
    public struct BypasserProperty
    {
        public float shootInterval;
    }

    //public enum TurretVariant { SeekStrike, SeekCounter };

    public static string officialName = "aCMa-BP-S Bypasser";

    public BypasserProperty bypasserProperty;
    public GFSM<BypasserEnemy> bypasserSFM;

    [HideInInspector] public PlayfieldObject currentPlayfieldObject;

    Vector3 playerPos;

    private void Start()
    {
        Begin();
    }

    public override void Begin()
    {
    }

    public void Initialize(object _property)
    {
        bypasserProperty = (BypasserProperty)_property;
    }

    public object GetProfile()
    {
        return bypasserProperty;
    }

    public string GetOfficialName()
    {
        return officialName;
    }

    public void GetVariantProperty()
    {

    }

    public void OnDrawCustomInspector()
    {

    }

    public Vector3 GetRelativePosition(PlayfieldObject _playfieldObject)
    {
        return Vector3.zero;
    }

    private void FixedUpdate()
    {
        //bypasserSFM.FixedUpdate();
    }

    class BypasserShoot : State<BypasserEnemy>
    {
        Timer shootTimer;

        public BypasserShoot(BypasserEnemy _relatedObject)
        {
            relatedObject = _relatedObject;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}
