using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaHoriController : MonoBehaviour
{
    public float moveSpeed;
    public float camRotateRange;
    public Vector3 horiMove, vertMove;
    public Playfield _playfield;
    
    public Vector3 newPos;

    public bool isShooting = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        vertMove = Vector3.up * Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        horiMove = Vector3.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        newPos = transform.localPosition + (Vector3.up * Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime) + (Vector3.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime);
        newPos.x = Mathf.Clamp(newPos.x, _playfield.leftBottomBorder.x, _playfield.rightUpperBorder.x);
        newPos.y = Mathf.Clamp(newPos.y, _playfield.leftBottomBorder.y, _playfield.rightUpperBorder.y);
        transform.localPosition = newPos;
        // if((newPos.y < _playfield.rightUpperBorder.y && newPos.y > _playfield.leftBottomBorder.y) || (newPos.x < _playfield.rightUpperBorder.x && newPos.x > _playfield.leftBottomBorder.x))
        // {
        //     _playfield.mainCamPivot.transform.rotation = Quaternion.AngleAxis(camRotateRange * transform.localPosition.y / _playfield.rightUpperBorder.y, _playfield.transform.right);
        // }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && transform.GetChild(1).GetComponent<MaterialBehaviour>().isAnimating == false)
        {
            gameObject.transform.GetChild(1).GetComponent<Animator>().SetTrigger("tr_shoot");
            // isShooting = true;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            //Debug.Log("ASW");
            isShooting = false;
            gameObject.transform.GetChild(1).GetComponent<Animator>().SetTrigger("tr_release");
        }
    }
}
