using UnityEngine;
using System.Collections;

public class LockedLid : MonoBehaviour {
    public Lock TheLock;

    private OVRGrabbable _grabbable;
    private OPGrabber _opGrabbable;

    void Awake () {
        _opGrabbable = GetComponent<OPGrabber>();
        _grabbable = GetComponent<OVRGrabbable>();

        _opGrabbable.enabled = false;
        _grabbable.enabled = false;
        TheLock.OnLockChange += Unlock;
    }

    public void Unlock (bool isOpen, Lock theLock, Key theKey) {
        _grabbable.enabled = isOpen;
        _opGrabbable.enabled = isOpen;
        Rigidbody body = GetComponent<Rigidbody>();
        body.isKinematic = false;
        body.useGravity = true;
    }
}
