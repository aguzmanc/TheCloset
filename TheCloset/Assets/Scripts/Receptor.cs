using UnityEngine;
using System.Collections;

namespace Vg {
    public abstract class Receptor : MonoBehaviour {
        public abstract void ReceiveInteraction (GameObject item);
    }
}
