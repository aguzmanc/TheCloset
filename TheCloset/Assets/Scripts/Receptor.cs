using UnityEngine;
using System.Collections;

public abstract class Receptor : MonoBehaviour {
    public abstract void ReceiveInteraction (GameObject item);
}
