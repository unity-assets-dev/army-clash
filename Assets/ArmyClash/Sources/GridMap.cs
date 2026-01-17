using UnityEngine;

public class GridMap : MonoBehaviour {
    
    [SerializeField] private Vector2Int _gridSize = Vector2Int.one;
    [SerializeField] private float _cellSize = 1f;

    private float HalfWidth => _gridSize.x * _cellSize * .5f - HalfCell;
    private float HalfHeight => _gridSize.y * _cellSize * .5f - HalfCell;
    private float HalfCell => _cellSize * .5f;
    public int CellsCount => _gridSize.x * _gridSize.y;
    
    public Vector3 GetPosition(int index) {
        var x = index % _gridSize.x;
        var y = index / _gridSize.x;
        
        return new Vector3(
            x * _cellSize - HalfWidth,
            0,
            y * _cellSize - HalfHeight);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;

        for (var i = 0; i < CellsCount; i++) {
            var position = GetPosition(i);
            Gizmos.DrawWireCube(
                    transform.position + position, 
                    Vector3.one * _cellSize - Vector3.up * _cellSize * .5f);
        }
    }
}
