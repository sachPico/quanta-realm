using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeltaHoriController : PlayfieldObject
{
    Vector2 Min
    {
        get
        {
            return Playfield.instance.min;
        }
    }

    Vector2 Max
    {
        get
        {
            return Playfield.instance.max;
        }
    }

    [Range(1f,80f)]
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

    void UpdateRelativePos()
    {
        relativePos.x = Mathf.Clamp(RelativePos.x, Min.x, Max.x);
        relativePos.y = Mathf.Clamp(RelativePos.y, Min.y, Max.y);

        RelativePos = new Vector3(Mathf.Clamp(RelativePos.x + input.x, Min.x, Max.x), Mathf.Clamp(relativePos.y + input.y, Min.y, Max.y), 0);

        /*if (relativePos.x > min.x || relativePos.x < max.x || relativePos.y > min.y || relativePos.y < max.y)
        {
            RelativePos += input;
        }*/
        
    }

    void Start()
    {
        ControlSetup();
    }

    void FixedUpdate()
    {
        UpdateRelativePos();
        if (isShooting)
        {
            WeaponUtility.instance.Shoot();
        }
        else
        {
            WeaponUtility.instance.ResetFireCounter();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Qbe"))
        {
            WeaponUtility.instance.QbeFragmentAdd(other.GetComponent<QbeFrag>().val);
            AudioSourcesHandler.PlaySFX((int) AudioType.ITEM_SFX, 1);
            other.gameObject.SetActive(false);
        }
    }
}
