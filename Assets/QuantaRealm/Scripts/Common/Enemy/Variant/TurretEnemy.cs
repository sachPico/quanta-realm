using Sachet.Experimental;
using Sachet.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : EnemyBase, IEnemyProfile
{
    [System.Serializable]
    public struct TurretProperty
    {
        public Transform rotatingObject;
        public float minRotation;
        public float maxRotation;
        public float maxOmega;
        public float degreeOffset;
        public float seekingDuration;
        public float shootingDuration;
        public float silentDuration;
    }

    public enum TurretVariant { SeekStrike, SeekCounter };

    public static string officialName = "T7-BA Turret";

    public TurretProperty turretProperty;
    public GFSM<TurretEnemy> turretSFM;

    [HideInInspector] public PlayfieldObject currentPlayfieldObject;

    Vector3 playerPos;

    private void Start()
    {
        Begin();
    }

    public override void Begin()
    {
        turretSFM = new GFSM<TurretEnemy>(new State<TurretEnemy>[] { new TurretSeek(this), new TurretShoot(this), new TurretSilent(this) });
        turretSFM.AddFlow(0, 1, o => true);
        turretSFM.AddFlow(1, 2, o => true);
        turretSFM.AddFlow(2, 0, o => true);
    }

    public void Initialize(object _property)
    {
        turretProperty = (TurretProperty)_property;
    }

    public object GetProfile()
    {
        return turretProperty;
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
        turretSFM.FixedUpdate();
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(playerPos, 10f);
    //}

    #region FSM

    public class TurretSeek : State<TurretEnemy>
    {
        Timer timer;

        Vector3 projectedPlayerPos;
        Vector3 localEuler = Vector3.zero;

        float currentTargetRotation = 0f;
        float lastTargetRotation = 0f;

        public TurretSeek(TurretEnemy _enemy)
        {
            relatedObject = _enemy;
        }

        void TrackPlayer()
        {
            projectedPlayerPos = GetProjectedPlayerPos();
            currentTargetRotation = Mathf.Clamp(Utility.ConvertDegreeLongitude180(relatedObject.turretProperty.degreeOffset + Mathf.Atan2(projectedPlayerPos.z, projectedPlayerPos.y) * Mathf.Rad2Deg), relatedObject.turretProperty.minRotation, relatedObject.turretProperty.maxRotation);
            localEuler.x += Mathf.Clamp(currentTargetRotation - localEuler.x, -relatedObject.turretProperty.maxOmega, relatedObject.turretProperty.maxOmega);

            relatedObject.turretProperty.rotatingObject.localEulerAngles = localEuler;

            lastTargetRotation = currentTargetRotation;
        }

        Vector3 GetProjectedPlayerPos()
        {
            Vector3 result = Vector3.zero;

            result = relatedObject.transform.InverseTransformPoint(StageManager.ActivePlayer.transform.position);
            result.x = 0f;

            return result;
        }

        public override void Enter()
        {
            base.Enter();

            timer = new Timer(relatedObject.turretProperty.seekingDuration, () => relatedObject.turretSFM.Next(), false);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void FixedUpdate()
        {
            TrackPlayer();
            timer?.Update();
        }
    }

    public class TurretShoot : State<TurretEnemy>
    {
        Timer timer;

        public TurretShoot(TurretEnemy _enemy)
        {
            relatedObject = _enemy;
        }

        public override void Enter()
        {
            base.Enter();;
            timer = new Timer(relatedObject.turretProperty.shootingDuration, () => relatedObject.turretSFM.Next(), false);
        }

        public override void Exit()
        {
            base.Exit();
            foreach (var t in relatedObject.turrets)
            {
                t.Reset();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (relatedObject.turrets.Count > 0)
            {
                foreach (var t in relatedObject.turrets)
                {
                    t.CalculateShooting();
                }
            }

            timer?.Update();
        }
    }

    public class TurretSilent : State<TurretEnemy>
    {
        Timer timer;

        public TurretSilent(TurretEnemy _enemy)
        {
            relatedObject = _enemy;
        }

        public override void Enter()
        {
            base.Enter();

            timer = new Timer(relatedObject.turretProperty.silentDuration, () => relatedObject.turretSFM.Next(), false);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer?.Update();
        }
    }


    #endregion
}
