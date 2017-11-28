using UnityEngine;
using System.Collections;

public class OPGrabber : MonoBehaviour {
    public bool DebugMode = false;
    public bool DebugGrab = false;

    public bool StaysOnDrop = false;
    public OVRGrabbable Grabbable;

    private bool _reconfiguredPhysicsGrab = false;
    private bool _reconfiguredPhysicsDrop = false;
    private Vector3 _anchorPosition;
    private Quaternion _anchorRotation;

    void Update () {
        if (IsGrabbed()) {
            _reconfiguredPhysicsDrop = false;
            if (!_reconfiguredPhysicsGrab) {
                _reconfiguredPhysicsGrab = true;
                Rigidbody body = Grabbable.GetComponent<Rigidbody>();
                body.isKinematic = true;
                body.useGravity = false;
            }
        } else {
            _reconfiguredPhysicsGrab = false;
            if (!_reconfiguredPhysicsDrop) {
                _reconfiguredPhysicsDrop = true;
                Rigidbody body = Grabbable.GetComponent<Rigidbody>();
                if (StaysOnDrop) {
                    body.isKinematic = true;
                    body.useGravity = false;

                    Debug.Log(transform.position);
                    Grabbable.transform.position =
                        _anchorPosition = transform.position;
                    Grabbable.transform.rotation =
                        _anchorRotation = transform.rotation;
                } else {
                    Grabbable.transform.position = transform.position;
                    Grabbable.transform.rotation = transform.rotation;
                    body.isKinematic = false;
                    body.useGravity = true;
                }
            }

            if (StaysOnDrop) {
                Grabbable.transform.position = _anchorPosition;
                Grabbable.transform.rotation = _anchorRotation;
            }
        }
    }

    public bool IsGrabbed () {
        return (Grabbable.enabled == true && Grabbable.isGrabbed) || (DebugMode && DebugGrab);
    }
}
