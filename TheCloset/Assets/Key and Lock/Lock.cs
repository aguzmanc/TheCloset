using UnityEngine;
using System.Collections;

namespace Vg {
    public class Lock : Receptor {
        public bool IsOpen = false;
        public GameObject Model;

        public Transform EnterAngle;
        public Transform UnlockAngle;

        public override void ReceiveInteraction (GameObject item) {
            IsOpen = true;
        }
    }
}
