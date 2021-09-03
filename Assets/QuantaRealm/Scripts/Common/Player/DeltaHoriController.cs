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

    public bool canMove = true;
    public bool canShoot = true;

    bool isShooting = false;
    Animation animation;
    Collider collider;
    GameObject model;

    public void Respawn()
    {
        PlayerManager.Lives--;
    }

    public void ToggleCollider()
    {
        StartCoroutine(TogglingCollider());
    }

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
        if (canMove)
        {
            input = ((Vector3.right * context.ReadValue<Vector2>().x) + (Vector3.up * context.ReadValue<Vector2>().y));
            input = input.normalized * speed * Time.fixedDeltaTime;
        }
    }

    void UpdateRelativePos()
    {
        relativePos.x = Mathf.Clamp(RelativePos.x, Min.x, Max.x);
        relativePos.y = Mathf.Clamp(RelativePos.y, Min.y, Max.y);

        RelativePos = new Vector3(Mathf.Clamp(RelativePos.x + input.x, Min.x, Max.x), Mathf.Clamp(relativePos.y + input.y, Min.y, Max.y), 0);
    }

    void Start()
    {
        ControlSetup();
        animation = GetComponent<Animation>();
        collider = GetComponent<Collider>();
        model = transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        UpdateRelativePos();
        if (canShoot)
        {
            if (isShooting)
            {
                WeaponUtility.instance.Shoot();
            }
            else
            {
                WeaponUtility.instance.ResetFireCounter();
            }
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
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("A");
            input = Vector3.zero;
            model.SetActive(false);
            canMove = false;
            canShoot = false;
            collider.enabled = false;
            StartCoroutine(Shot());
        }
    }

    IEnumerator Shot()
    {
        yield return new WaitForSeconds(3f);
        if(PlayerManager.Lives != 0)
        {
            animation.Play("pl_respawn");
            Respawn();
        }
        else
        {
            PlayerManager.StartGameOverSequence();
            //GAME OVER
        }
        yield break;
    }

    IEnumerator TogglingCollider()
    {
        yield return new WaitForSeconds(3f);
        collider.enabled = true;
        yield break;
    }
}
