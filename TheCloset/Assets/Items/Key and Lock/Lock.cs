using UnityEngine;
using System.Collections;

public class Lock : Receptor {
    public bool IsOpen = false;
    public GameObject Model;

    public Transform EnterAngle;
    public Transform UnlockAngle;

    public delegate void LockChangeDelegate (bool isOpen, Lock theLock, Key theKey);
    public event LockChangeDelegate OnLockChange; 

    public override void ReceiveInteraction (GameObject item) {
        IsOpen = true;

        if (OnLockChange != null) OnLockChange(IsOpen, this, item.GetComponent<Key>());
    }
}
