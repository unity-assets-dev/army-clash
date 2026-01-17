using System.Linq;
using UnityEngine;

public class CameraDriver : MonoBehaviour {
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed = 15;
    [SerializeField] private UnitFactory _unitFactory;

    private Vector3 _middlePosition;

    
    private void LateUpdate() {
        _middlePosition = Vector3.zero;
        var count = _unitFactory.Count();

        if (count == 0) return;
        
        foreach (var actor in _unitFactory) {
            _middlePosition += actor.transform.position;
        }
        _middlePosition /= count;
        
        transform.position = Vector3.Lerp(transform.position, _middlePosition, Time.deltaTime * _speed);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_middlePosition, 0.5f);
    }
}
