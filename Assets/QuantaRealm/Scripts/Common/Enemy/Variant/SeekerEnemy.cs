using Sachet.Experimental;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerEnemy : EnemyBase, IEnemyProfile 
{
    [System.Serializable]
    public struct SeekerProperty
    {
        public float detectionRange;
        public float seekSpeed;
        public float strikeSpeed;
        //[SerializeField] public StraightPatternProperty patternProfile;
    }

    public enum SeekerVariant {SeekStrike, SeekCounter};

    public static string officialName = "aCMh-S1 Seeker";

    public SeekerProperty seekerProperty;
    public GFSM<SeekerEnemy> seekerFSM;

    [HideInInspector] public PlayfieldObject currentPlayfieldObject;
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public Vector3 initRelativePos;
    [HideInInspector] public Vector3 capturedPlayerPos;

    public override void Begin()
    {
        initRelativePos = RelativePos;
        seekerFSM = new GFSM<SeekerEnemy>(new State<SeekerEnemy>[] { new SeekerSeek(this), new SeekerStrike(this) });
        seekerFSM.AddFlow(0, 1, o => true);
    }

    public void Initialize(object _property)
    {
        seekerProperty = (SeekerProperty)_property;
        velocity = Vector3.left * 20f;
    }

    public object GetProfile()
    {
        return seekerProperty;
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
        Vector3 value;

        if (currentPlayfieldObject == null)
        {
            currentPlayfieldObject = _playfieldObject;
        }

        seekerFSM.FixedUpdate();
        value = velocity * Time.fixedDeltaTime + _playfieldObject.RelativePos;

        return value;
    }

    private void FixedUpdate()
    {
        RelativePos = GetRelativePosition(this);
    }

    #region FSM

    public class SeekerSeek : State<SeekerEnemy>
    {
        public float amplitude = 3f;
        public float frequency = 100f * Mathf.Deg2Rad * Time.fixedDeltaTime;

        bool detected = false;
        float degree = 0f;
        Vector2 velocity;

        public SeekerSeek (SeekerEnemy _enemy)
        {
            relatedObject = _enemy;
        }

        public bool IsInRange(Vector3 worldPosition)
        {
            return Vector3.Distance(worldPosition, StageManager.ActivePlayer.transform.position) < relatedObject.seekerProperty.detectionRange;
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
            if (relatedObject.currentPlayfieldObject != null)
            {
                if (!IsInRange(relatedObject.currentPlayfieldObject.transform.position))
                {
                    velocity.x = -10f;
                    velocity.y = Mathf.Sin(degree) * amplitude;

                    degree += frequency;
                    velocity = velocity.normalized * relatedObject.seekerProperty.seekSpeed;

                    relatedObject.velocity = velocity;
                }
                else
                {
                    detected = true;

                    relatedObject.capturedPlayerPos = StageManager.ActivePlayer.RelativePos;
                    relatedObject.seekerFSM.Next();
                }
            }
        }
    }

    public class SeekerStrike : State<SeekerEnemy>
    {
        float steerForce = 20f;

        Vector3 targetVelocity;

        public SeekerStrike(SeekerEnemy _enemy)
        {
            relatedObject = _enemy;
        }

        public override void Enter()
        {
            base.Enter();

            targetVelocity = (relatedObject.capturedPlayerPos - relatedObject.RelativePos).normalized * relatedObject.seekerProperty.strikeSpeed;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            relatedObject.velocity += (targetVelocity - relatedObject.velocity).normalized * steerForce * Time.fixedDeltaTime;
        }
    }
    #endregion
}
