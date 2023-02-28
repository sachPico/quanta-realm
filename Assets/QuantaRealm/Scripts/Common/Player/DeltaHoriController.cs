using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeltaHoriController : PlayfieldObject
{
    static DeltaHoriController instance;
    public static DeltaHoriController Instance
    { 
        get
        {
            if(instance==null)
            {
                instance = FindObjectOfType<DeltaHoriController>();
            }
            return instance;
        }
    }


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

    public float overheatCounter = 0;
    private int maxOverheat = 1000;

    public InputActionMap actionMap;

    public bool canMove = true;
    public bool canShoot = true;
    public Collider enemyCollisionHandler;

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

    public void AddOverheat(float _value)
    {
        overheatCounter = Mathf.Clamp(overheatCounter+_value, 0, maxOverheat);

        //Debug.Log(_value);
        //Debug.Log(overheatCounter);

        PlayerManager.Overheat = overheatCounter;
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
        Vector3 r = RelativePos;

        r.x = Mathf.Clamp(RelativePos.x, Min.x, Max.x);
        r.y = Mathf.Clamp(RelativePos.y, Min.y, Max.y);

        RelativePos = r;

        RelativePos = new Vector3(Mathf.Clamp(RelativePos.x + input.x, Min.x, Max.x), Mathf.Clamp(RelativePos.y + input.y, Min.y, Max.y), 0);
    }

    void Start()
    {
        ControlSetup();
        animation = GetComponent<Animation>();
        collider = GetComponent<Collider>();
        model = transform.GetChild(0).gameObject;
        overheatCounter = 0;
        PlayerManager.Overheat = overheatCounter;
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

                AddOverheat(-1f / StageManager.Instance.ambientHeatMultiplier);
            }
        }
    }

    public void InvokeGetQbe(Collider other)
    {
        if (other.CompareTag("Qbe"))
        {
            WeaponUtility.instance.QbeFragmentAdd(other.GetComponent<QbeFrag>().val);
            AudioSourcesHandler.PlaySFX((int)AudioType.ITEM_SFX, 1);
            other.gameObject.SetActive(false);
        }
    }

    public void InvokeShot(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            input = Vector3.zero;
            model.SetActive(false);
            canMove = false;
            canShoot = false;
            other.enabled = false;
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
        enemyCollisionHandler.enabled = true;
        yield break;
    }
}
