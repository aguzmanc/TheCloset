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
    private Vector3 _offset;

    private Transform _dynamicAnchor = null;

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

                    if (_dynamicAnchor != null) {
                        _offset = _dynamicAnchor.transform.position - transform.position;
                    } else {
                        Grabbable.transform.position =
                            _anchorPosition = transform.position;
                    }
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
                if (_dynamicAnchor != null) {
                    Grabbable.transform.position = _dynamicAnchor.position - _offset;
                } else {
                    Grabbable.transform.position = _anchorPosition;
                }
                Grabbable.transform.rotation = _anchorRotation;
            }
        }
    }

    public void SetDynamicAnchor(Transform anchor) {
        _dynamicAnchor = anchor;
    }

    public void UnsetDynamicAnchor () {
        _dynamicAnchor = null;
    }

    public bool IsGrabbed () {
        return (Grabbable.enabled == true && Grabbable.isGrabbed) || (DebugMode && DebugGrab);
    }
}
