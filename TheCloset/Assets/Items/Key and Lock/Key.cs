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

        void Start () {
            _grabbable = transform.parent.GetComponent<OVRGrabbable>();
        }

        void Update () {
            if (!DebugMode) {
                if (!_grabbable.isGrabbed && _forcedDrop) {
                    ReturnToParent();
                }
            } else {
                if (!DebugGrab && _forcedDrop) {
                    ReturnToParent();
                }
            }
        }

        void OnTriggerEnter (Collider c) {
            _currentLock = c.GetComponent<Lock>();
            _forcedDrop = false;

            if (!_forcedDrop &&
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
    }
}
