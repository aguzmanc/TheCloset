using UnityEngine;
using System.Collections;

public class Knob : MonoBehaviour {
    public GameObject DrawerModel;
    public Transform MaxDistance;
    public Transform MinDistance;
    public bool Blocked = false;
    public bool Closed = true;

    private Vector3 _offset;
    private OPGrabber _opGrabber;
    private bool _backToPlace = false;
    private float _cachedTotalOpenningGap;

    void Start () {
        _cachedTotalOpenningGap = Vector3.Distance(MaxDistance.position, MinDistance.position);
        _offset = DrawerModel.transform.position - transform.position;
        _opGrabber = GetComponent<OPGrabber>();
    }
    
    void Update () {
        if (_opGrabber.IsGrabbed()) {
            _backToPlace = false;
            if (!Blocked || (Blocked && !Closed)) {

                Closed = Vector3.Distance(DrawerModel.transform.position,
                                          MinDistance.position) <
                    _cachedTotalOpenningGap * 0.1f;

                if (Vector3.Dot(transform.position - MaxDistance.position, MaxDistance.forward) < 0) {
                    DrawerModel.transform.position = MaxDistance.position;
                } else if (Vector3.Dot(transform.position - MinDistance.position,
                                       MinDistance.forward) > 0) {
                    DrawerModel.transform.position = MinDistance.position;
                } else {
                    DrawerModel.transform.position += DrawerModel.transform.forward *
                        Vector3.Dot(transform.position - DrawerModel.transform.position,
                                    DrawerModel.transform.forward);
                
                }
            }
        } else if (!_backToPlace) {
            ResetPosition();
        }
    }

    public void ResetPosition () {
        _backToPlace = true;
        transform.position = DrawerModel.transform.position + _offset;
    }
}
