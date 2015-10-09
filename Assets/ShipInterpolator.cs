using UnityEngine;
using System.Collections;

public class ShipInterpolator : MonoBehaviour
{

    private Vector3 _lastPos;
    private Vector3 _nextPos;
    private float _lerpTime = 0.1f;
    private float _currentTime;

    public void setNextPosition(Vector3 pos)
    {
        _lastPos = this.transform.position;
        _nextPos = pos;
        _currentTime = 0;
    }

    void Update ()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _lerpTime)
        {
            _currentTime = _lerpTime;
        }

        float perc = _currentTime / _lerpTime;
        this.transform.position = Vector3.Lerp(_lastPos, _nextPos, perc);
    }
}
