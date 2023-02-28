using Sachet.Experimental;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaniteEnemy : EnemyBase, IEnemyProfile
{
    [System.Serializable]
    public struct CaniteProperty
    {
        public float detectionRange;
        [SerializeField] public StraightPatternProperty patternProfile;
    }

    public enum CaniteVariant { Scout, Trooper, Grenadier };

    public static string officialName = "aCMh-A3 Kynos";

    #region PROPERTY_FIELDS
    public bool stateMachineInitialized;
    [SerializeField] public CaniteProperty caniteProperty;
    //public CaniteVariant caniteVariant;
    [HideInInspector] public GFSM<CaniteEnemy> caniteFSM;
    [HideInInspector] public PlayfieldObject currentPlayfieldObject;
    [HideInInspector] public Vector3 velocity;
    #endregion

    #region METHODS
    #region OVERRIDDEN
    public string GetOfficialName()
    {
        return officialName;
    }

    public void GetVariantProperty()
    {
        // switch(caniteVariant)
        // {
        //     case CaniteVariant.Scout:
        //         break;
        //     case CaniteVariant.Trooper:
        //         break;
        //     case CaniteVariant.Grenadier:
        //         break;
        // }
    }

    public override void Begin()
    {
        base.Begin();

        //TODO: Jangan pake new terus
        caniteFSM = new GFSM<CaniteEnemy>(new State<CaniteEnemy>[] { new CaniteStrike(this), new CaniteEvasive(this) });
        caniteFSM.AddFlow(0, 1, o => true);
        velocity = Vector3.left * 20f;
    }

    public void Initialize(object _caniteProperty)
    {
        caniteProperty = (CaniteProperty)_caniteProperty;
        velocity = Vector3.left * 20f;
    }

    public void OnDrawCustomInspector()
    {
        //TODO: Proses getvariantproperty jadiin satu di base method
        // caniteVariant = (CaniteVariant)UnityEditor.EditorGUILayout.EnumPopup("Variant", caniteVariant);
        // GetVariantProperty();

        stateMachineInitialized = UnityEditor.EditorGUILayout.Toggle("State Machine Initialized", stateMachineInitialized);

        //velocity = UnityEditor.EditorGUILayout.Vector3Field("Velocity", velocity);

        UnityEditor.EditorGUILayout.LabelField("Canite Property", UnityEditor.EditorStyles.boldLabel);
        caniteProperty.detectionRange = UnityEditor.EditorGUILayout.FloatField("Detection range", caniteProperty.detectionRange);
        caniteProperty.patternProfile.fireInterval = UnityEditor.EditorGUILayout.FloatField("Fire interval", caniteProperty.patternProfile.fireInterval);
        caniteProperty.patternProfile.speed = UnityEditor.EditorGUILayout.FloatField("Speed", caniteProperty.patternProfile.speed);
    }

    //TODO: Assign playfieldObject jangan di sini
    public Vector3 GetRelativePosition(PlayfieldObject _playfieldObject)
    {
        Vector3 value;

        if (currentPlayfieldObject == null)
        {
            currentPlayfieldObject = _playfieldObject;
        }

        caniteFSM.FixedUpdate();
        value = velocity * Time.fixedDeltaTime + _playfieldObject.RelativePos;

        return value;
    }
    #endregion

    public object GetProfile()
    {
        return caniteProperty;
    }

    private void FixedUpdate()
    {
        RelativePos = GetRelativePosition(this);
    }
    #endregion

    public class CaniteStrike : State<CaniteEnemy>
    {
        public CaniteStrike(CaniteEnemy _relatedObject)
        {
            relatedObject = _relatedObject;
        }

        private bool IsInRange(Vector3 currentWorldPosition)
        {
            return Vector3.Distance(currentWorldPosition, StageManager.ActivePlayer.transform.position) < relatedObject.caniteProperty.detectionRange;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            //TODO: SetActiveState jangan dipanggil di sini, bikin overflow
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (relatedObject.currentPlayfieldObject != null)
            {
                if (!IsInRange(relatedObject.currentPlayfieldObject.transform.position))
                {
                    relatedObject.velocity = relatedObject.velocity;
                }
                else
                {
                    relatedObject.caniteFSM.Next();
                }
            }
        }
    }

    //This class implement evasive and steering behavior
    public class CaniteEvasive : State<CaniteEnemy>
    {
        public CaniteEvasive(CaniteEnemy _relatedObject)
        {
            relatedObject = _relatedObject;
        }

        #region VARIABLE
        public float maxSpeed = 20f;
        public float steeringMaxDegree = 15f;
        public float steeringMaxForce = 10f;

        private SubPatternSpawner patternSpawner = new SubPatternSpawner(new StraightPatternProfile());
        private Vector3 steeringTarget;
        private Vector3 steeringForce;
        private Vector3 desiredVelocity;
        #endregion

        #region METHODS
        #region OVERRIDDEN
        public override void Enter()
        {
            base.Enter();

            patternSpawner.subPatternProfile.enemyPlayfieldObject = relatedObject.currentPlayfieldObject;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Vector3 relativePos = relatedObject.currentPlayfieldObject.RelativePos;

            steeringTarget = StageManager.ActivePlayer.RelativePos;
            steeringTarget.x = 22f;

            desiredVelocity = steeringTarget - relativePos;
            desiredVelocity = desiredVelocity.normalized * maxSpeed;

            steeringForce = desiredVelocity - relatedObject.velocity;
            steeringForce = steeringForce.normalized * steeringMaxForce;

            relatedObject.velocity += steeringForce * Time.fixedDeltaTime;
            relatedObject.velocity = relatedObject.velocity.normalized * Mathf.Clamp(relatedObject.velocity.magnitude, 0f, 20f);

            //Movement avoiding playfield vertical border
            if (IntersectionCheck(Vector3.up * -26f, 10f) || IntersectionCheck(Vector3.up * 26f, 10f))
            {
                if (IntersectionCheck(Vector3.up * -26f, 10f))
                {
                    desiredVelocity = relatedObject.velocity + Vector3.up * (Mathf.Abs(relativePos.y - 26f));
                    //desiredVelocity = desiredVelocity.normalized * maxSpeed;
                }
                else if (IntersectionCheck(Vector3.up * 26f, 10f))
                {
                    desiredVelocity = relatedObject.velocity - Vector3.up * (Mathf.Abs(relativePos.y + 26f));
                    //desiredVelocity = desiredVelocity.normalized * maxSpeed;
                }

                steeringForce = desiredVelocity - relatedObject.velocity;
                //steeringForce = steeringForce.normalized * steeringMaxForce;

                relatedObject.velocity += steeringForce * Time.fixedDeltaTime;
                relatedObject.velocity = relatedObject.velocity.normalized * Mathf.Clamp(relatedObject.velocity.magnitude, 0f, 20f);
            }

            if (relatedObject.turrets.Count > 0)
            {
                foreach(var t in relatedObject.turrets)
                {
                    t.CalculateShooting();
                }
                //foreach (var p in relatedObject.turrets[0].patternProfile.subPatterns)
                //{
                //    p.profile.CalculateSubPattern(relatedObject.turrets[0]);
                //}
            }

            //relatedObject.turrets[0].patternProfile..subPatternProfile.CalculateSubPattern();
        }
        #endregion

        bool IntersectionCheck(Vector3 targetVector, float range)
        {
            if (Mathf.Abs(relatedObject.currentPlayfieldObject.RelativePos.y - targetVector.y) > range)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}