using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType {Einhander, Delta};

public class CameraControl : MonoBehaviour
{
    public ControlType cameraControlType;
    public static CameraControl instance;

    public PlayfieldObject target;
    public Transform cameraTransform;
    public float smoothDampTime;

    private Vector3 track;
    private float targetValue;
    private float _ratio;

    delegate void UpdateCamera();
    event UpdateCamera UpdateCameraEvent;

    float velocity;

    public float ratio
    {
        get
        {
            return _ratio;
        }
        set
        {
            _ratio = value;
        }
    }

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        track = Vector3.zero;

        switch(cameraControlType)
        {
            case ControlType.Delta:
                UpdateCameraEvent = DeltaCamera; break;
            case ControlType.Einhander:
                UpdateCameraEvent = EinhanderCamera; break;
        }
    }

    void DeltaCamera()
    {
        _ratio = Mathf.Clamp(target.RelativePos.y / 23f, -1f, 1f);
        targetValue = -Playfield.instance.camPivotRotateRange * _ratio;
        track.x = Mathf.SmoothDampAngle(track.x, targetValue, ref velocity, smoothDampTime);
        cameraTransform.localEulerAngles = track;
    }

    void EinhanderCamera()
    {
        _ratio = Mathf.Clamp(target.RelativePos.y / 10f, -1f, 1f)  * 0.1f;
        targetValue = 13f * _ratio;
        track.y = targetValue;
        cameraTransform.localPosition = track;
    }

    private void LateUpdate()
    {
        UpdateCameraEvent();
    }
}
