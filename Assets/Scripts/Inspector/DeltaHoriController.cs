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
        playerPivotRelativePos += ((Vector3.right * speed * Input.GetAxisRaw("Horizontal")) + (Vector3.up * speed * Input.GetAxisRaw("Vertical"))) * Time.deltaTime;
        // playerPivotRelativePos.x = Mathf.Clamp(playerPivotRelativePos.x, 0f, 1f);
        // playerPivotRelativePos.y = Mathf.Clamp(playerPivotRelativePos.y, 0f, 1f);
        // _playfield.playerPivot.position = Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 35f));
        transform.position = _playfield.playerPivot.TransformPoint(playerPivotRelativePos);
        // viewportPos += ((Vector3.right * speed * Input.GetAxisRaw("Horizontal")) + (Vector3.up * speed * Camera.main.aspect * Input.GetAxisRaw("Vertical"))) * Time.deltaTime;
        // viewportPos.x = Mathf.Clamp(viewportPos.x, 0f, 1f);
        // viewportPos.y = Mathf.Clamp(viewportPos.y, 0f, 1f);
        // transform.position = Camera.main.ViewportToWorldPoint(viewportPos);

        _playfield.qCamPivot.transform.localRotation = Quaternion.AngleAxis(_playfield.camPivotRotateRange * (playerPivotRelativePos.y / 13f), Vector3.right);

        if(Input.GetKey(KeyCode.X))
        {
            if(fireCounter >= WeaponUtility.activeFireRate)
            {
                foreach(var spawners in bulletSpawners)
                {
                    WeaponUtility.Shoot(spawners.position, spawners.localEulerAngles.z);
                    //Debug.Log(spawners.name+": "+spawners.right);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach(var spawner in bulletSpawners)
        {
            Gizmos.DrawLine(transform.position, transform.position + (Camera.main.transform.right * Mathf.Cos(Mathf.Deg2Rad * spawner.localEulerAngles.z)) + (Camera.main.transform.up * Mathf.Sin(Mathf.Deg2Rad * spawner.localEulerAngles.z)));
        }
    }
    // public float fireCounter;
    // public float moveSpeed;
    // public float camRotateRange;
    // public Vector3 horiMove, vertMove;
    // public Playfield _playfield;
    
    // public Vector3 newPos;
    // public List<Transform> bulletSpawners;

    // public bool isShooting = false;
    // // Update is called once per frame

    // void Start()
    // {
    //     fireCounter = WeaponUtility.activeFireRate;
    //     for(int i=1; i<transform.childCount; i++)
    //     {
    //         bulletSpawners.Add(transform.GetChild(i));
    //     }
    //     //Debug.Log("bulletSpawners' size is: "+bulletSpawners.Count);
    // }

    // void FixedUpdate()
    // {
    //     vertMove = Vector3.up * Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
    //     horiMove = Vector3.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
    //     newPos = transform.localPosition + (Vector3.up * Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime) + (Vector3.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime);
    //     newPos.x = Mathf.Clamp(newPos.x, _playfield.leftBottomBorder.x, _playfield.rightUpperBorder.x);
    //     newPos.y = Mathf.Clamp(newPos.y, _playfield.leftBottomBorder.y, _playfield.rightUpperBorder.y);
    //     transform.localPosition = newPos;
    //     // if((newPos.y < _playfield.rightUpperBorder.y && newPos.y > _playfield.leftBottomBorder.y) || (newPos.x < _playfield.rightUpperBorder.x && newPos.x > _playfield.leftBottomBorder.x))
    //     // {
    //     //     _playfield.mainCamPivot.transform.rotation = Quaternion.AngleAxis(camRotateRange * transform.localPosition.y / _playfield.rightUpperBorder.y, _playfield.transform.right);
    //     // }
    //     if(Input.GetKey(KeyCode.X))
    //     {
    //         if(fireCounter >= WeaponUtility.activeFireRate)
    //         {
    //             foreach(var spawners in bulletSpawners)
    //             {
    //                 WeaponUtility.Shoot(spawners.position, spawners.right);
    //                 //Debug.Log(spawners.name+": "+spawners.right);
    //             }
    //             fireCounter = 0;
    //         }
    //         else
    //         {
    //             fireCounter += Time.deltaTime;
    //         }
    //     }
    //     if(Input.GetKeyUp(KeyCode.X))
    //     {
    //         fireCounter = WeaponUtility.activeFireRate;
    //     }
    // }

    // void Update()
    // {   
    // }
}
