using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaHoriController : MonoBehaviour
{
    public float fireCounter;
    public Vector3 viewportPos;
    public Vector3 playerPivotRelativePos;
    [Range(1f,20f)]
    public float speed;
    public List<Transform> bulletSpawners;

    Playfield _playfield;

    void Awake()
    {
        _playfield = FindObjectOfType<Playfield>();
    }

    void Start()
    {
        playerPivotRelativePos = _playfield.playerPivot.InverseTransformPoint(transform.position);
    }

    void FixedUpdate()
    {
        //float z = Camera.main.WorldToViewportPoint(transform.position).z;
        playerPivotRelativePos += ((Vector3.right * speed * Input.GetAxisRaw("Horizontal")) + (Vector3.up * speed * Input.GetAxisRaw("Vertical"))) * Time.deltaTime;
        playerPivotRelativePos.x = Mathf.Clamp(playerPivotRelativePos.x, -23f, 21f);
        playerPivotRelativePos.y = Mathf.Clamp(playerPivotRelativePos.y, -13f, 13f);
        transform.position = _playfield.playerPivot.TransformPoint(playerPivotRelativePos);

        _playfield.qCamPivot.transform.localRotation = Quaternion.AngleAxis(_playfield.camPivotRotateRange * (playerPivotRelativePos.y / 13f), Vector3.right);
        Camera.main.transform.localPosition = Vector3.forward * (-35f + _playfield.qCamPivot.GetChild(1).InverseTransformPoint(transform.position).z - _playfield.tertiaryPivot.GetChild(1).InverseTransformPoint(transform.position).z);

        if(Input.GetKey(KeyCode.X))
        {
            if(fireCounter >= WeaponUtility.activeFireRate)
            {
                foreach(var spawners in bulletSpawners)
                {
                    WeaponUtility.Shoot(spawners.position, spawners.localEulerAngles.z);
                }
                fireCounter = 0;
            }
            else
            {
                fireCounter += Time.deltaTime;
            }
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            fireCounter = WeaponUtility.activeFireRate;
        }
    }
}
