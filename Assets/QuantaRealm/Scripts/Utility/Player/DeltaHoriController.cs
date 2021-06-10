using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeltaHoriController : PlayfieldObject
{
    public Vector2 min, max;
    [Range(1f,20f)]
    public float speed;

    public InputActionMap actionMap;

    bool isShooting = false;

    void ControlSetup()
    {
        actionMap["move"].performed += GetMoveInput;
        actionMap["move"].canceled += GetMoveInput;

        actionMap["shoot"].performed += _ => isShooting = true;
        actionMap["shoot"].canceled += _ => isShooting = false;

        actionMap.Enable();
    }

    void GetMoveInput(InputAction.CallbackContext context)
    {
        input = ((Vector3.right * speed * context.ReadValue<Vector2>().x) + (Vector3.up * speed * context.ReadValue<Vector2>().y)) * Time.fixedDeltaTime;
    }

    protected override void UpdateRelativePos()
    {
        if(input.x != 0 || input.y != 0)
        {
            relativePos += input;
            relativePos.x = Mathf.Clamp(relativePos.x, min.x, max.x);
            relativePos.y = Mathf.Clamp(relativePos.y, min.y, max.y);

            base.UpdateRelativePos();
        }
    }

    void Start()
    {
        ControlSetup();
    }

    void FixedUpdate()
    {
        UpdateRelativePos();
        if(isShooting)
        {
            WeaponUtility.instance.Shoot();
        }
        else
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
