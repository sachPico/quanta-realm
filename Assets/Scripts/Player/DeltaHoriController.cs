using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaHoriController : PlayfieldObject
{
    public float fireCounter;
    public Vector2 min, max;
    public Vector3 playerPivotRelativePos;
    [Range(1f,20f)]
    public float speed;

    protected override void UpdateRelativePos()
    {
        relativePos+= ((Vector3.right * speed * Input.GetAxisRaw("Horizontal")) + (Vector3.up * speed * Input.GetAxisRaw("Vertical"))) * Time.deltaTime;
        relativePos.x = Mathf.Clamp(relativePos.x, min.x, max.x);
        relativePos.y = Mathf.Clamp(relativePos.y, min.y, max.y);

        base.UpdateRelativePos();
    }

    void FixedUpdate()
    {
        UpdateRelativePos();

        if(Input.GetKey(KeyCode.X))
        {
            WeaponUtility.instance.Shoot();
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            WeaponUtility.instance.ResetFireCounter();
        }
    }

    void LateUpdate()
    {
        Playfield.instance.qCamPivot.transform.localRotation = Quaternion.AngleAxis(Playfield.instance.camPivotRotateRange * (relativePos.y / max.y), Vector3.right);
        Camera.main.transform.localPosition = Vector3.forward * (-35f + Playfield.instance.qCamPivot.GetChild(1).InverseTransformPoint(transform.position).z - Playfield.instance.tertiaryPivot.GetChild(1).InverseTransformPoint(transform.position).z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Qbe"))
        {
            WeaponUtility.instance.QbeFragmentAdd(other.GetComponent<QbeFrag>().val);
            other.gameObject.SetActive(false);
        }
    }
}
