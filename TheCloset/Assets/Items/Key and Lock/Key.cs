using UnityEngine;
using System.Collections;

namespace Vg {
    public class Key : MonoBehaviour {
        public bool DebugMode = false;
        public bool DebugGrab = false;

        public Lock OwnerLock;
        public float DroppingTreshold = 1;

        public delegate void ForcedDropDelegate ();
        public event ForcedDropDelegate OnForcedDrop;

        public bool _forcedDrop = false;
        public Lock _currentLock;
        private OVRGrabbable _grabbable;
        private bool _physicsReconfiguredDrop = false;
        private bool _physicsReconfiguredGrab = false;
        private Vector3 _anchoredPosition;
        private float _exitTime = 0;

        void Start () {
            _grabbable = transform.parent.GetComponent<OVRGrabbable>();
        }

        void Update () {
            if (IsGrabbed()) {
                _physicsReconfiguredDrop = false;
                if (!_physicsReconfiguredGrab) {
                    _physicsReconfiguredGrab = true;
                    Rigidbody body = transform.parent.GetComponent<Rigidbody>();
                    body.isKinematic = true;
                    body.useGravity = false;
                }
            } else {
                _physicsReconfiguredGrab = false;
                transform.parent.position = transform.position;
                transform.parent.rotation = transform.rotation;

                Rigidbody body = transform.parent.GetComponent<Rigidbody>();
                if (_forcedDrop) {
                    ReturnToParent();
                } else if (_currentLock != null) {
                    if (!_physicsReconfiguredDrop) {
                        _physicsReconfiguredDrop = true;
                        body.isKinematic = true;
                        body.useGravity = false;
                        _anchoredPosition = transform.position;
                    }

                    transform.parent.position = _anchoredPosition;
                    transform.parent.rotation = _currentLock.Model.transform.rotation;
                } else if (!_physicsReconfiguredDrop){
                    _physicsReconfiguredDrop = true;
                    body.isKinematic = false;
                    body.useGravity = true;
                }

            }
        }

        void OnTriggerEnter (Collider c) {
            _currentLock = c.GetComponent<Lock>();
            _forcedDrop = false;

            if (!_forcedDrop && (Time.time - _exitTime) > 1 &&
                (Vector3.Angle(transform.forward, OwnerLock.EnterAngle.forward) > 80 ||
                 Vector3.Angle(transform.up, OwnerLock.EnterAngle.up) > Tresholds.Rotation)) {
                ForceDrop();
            }
        }

        void OnTriggerStay (Collider c) {
            if (_currentLock != null && OwnerLock != _currentLock) {
                transform.rotation = OwnerLock.EnterAngle.rotation;
            } else if (!_forcedDrop) {
                if (Vector3.Angle(transform.up, OwnerLock.UnlockAngle.up) < Tresholds.Rotation) {
                    _currentLock.ReceiveInteraction(this.gameObject);
                }

                Vector3 r = _currentLock.Model.transform.eulerAngles;
                _currentLock.Model.transform.rotation =
                    transform.rotation =
                    Quaternion.Euler(r.x, r.y, transform.rotation.eulerAngles.z);

                Transform t = _currentLock.Model.transform;
                transform.position = _currentLock.Model.transform.position + t.forward *
                    Vector3.Dot((transform.parent.position - t.position), t.forward);

                if (Vector3.Distance(transform.position, transform.parent.position) > DroppingTreshold) {
                    ForceRegrap();
                }
            }
        }

        void OnTriggerExit (Collider c) {
            if (!_forcedDrop) {
                _exitTime = Time.time;
                ForceRegrap();
            }
        }

        public void ForceRegrap () {
            Rigidbody body = gameObject.GetComponent<Rigidbody>();
            if (body != null) Destroy(body);

            _forcedDrop = false;
            transform.parent = _grabbable.transform;
            transform.position = transform.parent.transform.position;
            transform.rotation = transform.parent.transform.rotation;
        }

        public void ForceDrop () {
            _forcedDrop = true;
            transform.parent = null;
            gameObject.AddComponent(typeof(Rigidbody));

            if (OnForcedDrop != null) OnForcedDrop();
        }

        public void ReturnToParent () {
            _forcedDrop = true;
            _currentLock = null;
            Destroy(GetComponent<Rigidbody>());
            _grabbable.transform.position = transform.position;
            _grabbable.transform.rotation = transform.rotation;
            transform.parent = _grabbable.transform;
        }

        public bool IsGrabbed () {
            return (_grabbable.enabled == true && _grabbable.isGrabbed) || (DebugMode && DebugGrab);
        }
    }
}
