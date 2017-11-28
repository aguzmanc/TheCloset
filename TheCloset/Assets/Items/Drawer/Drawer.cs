using UnityEngine;
using System.Collections;

public class Drawer : MonoBehaviour {
    public Lock TheLock;
    public Knob TheKnob;

    void Awake () {
        TheLock.OnLockChange += Unlock;
        TheKnob.Blocked = !TheLock.IsOpen;
    }

    public void Unlock (bool isOpen, Lock theLock, Key theKey) {
        TheKnob.Blocked = !isOpen;
    }
}
