using UnityEngine;

public class GridMap : MonoBehaviour {
    
    [SerializeField] private Vector2Int _gridSize = Vector2Int.one;
    [SerializeField] private float _cellSize = 1f;

    private float HalfWidth => _gridSize.x * _cellSize * .5f;
    private float HalfHeight => _gridSize.y * _cellSize * .5f;

    public Vector3 GetPosition(int x, int y) {
        
        return new Vector3(
            x * _cellSize - HalfWidth - _cellSize * .5f,
            0,
            y * _cellSize - HalfHeight - _cellSize * .5f);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;

        for (var x = 0; x < _gridSize.x; x++) {
            for (var y = 0; y < _gridSize.y; y++) {
                var position = GetPosition(x, y);
                Gizmos
                    .DrawWireCube(position, Vector3.one * _cellSize - Vector3.up * _cellSize * .5f);
            }
        }
    }
}
