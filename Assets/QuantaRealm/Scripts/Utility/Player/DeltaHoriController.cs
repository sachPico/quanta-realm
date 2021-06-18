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
        input = ((Vector3.right * context.ReadValue<Vector2>().x) + (Vector3.up * context.ReadValue<Vector2>().y));
        input = input.normalized * speed * Time.fixedDeltaTime;
    }

    protected override void UpdateRelativePos()
    {
            relativePos += input;
            relativePos.x = Mathf.Clamp(relativePos.x, min.x, max.x);
            relativePos.y = Mathf.Clamp(relativePos.y, min.y, max.y);

            base.UpdateRelativePos();
    }

    void Start()
    {
        ControlSetup();
    }

    void LateUpdate()
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
        float ratio = Mathf.Clamp( relativePos.y / 10f, -1f, 1f);
        
        Playfield.instance.qCamPivot.transform.localRotation = Quaternion.AngleAxis(-Playfield.instance.camPivotRotateRange * ratio, Vector3.right);
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
