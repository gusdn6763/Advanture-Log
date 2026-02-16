using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridMover : MonoBehaviour
{
    public GridMap2D grid;
    [Min(0.1f)] public float moveSpeed = 4f;

    private Coroutine _moveCo;
    public Vector2Int CurrentCell { get; private set; }

    private void Start()
    {
        if (grid == null) Debug.LogError("PlayerGridMover: grid reference missing.");
        else CurrentCell = grid.WorldToCell(transform.position);
    }

    public void MoveAlong(List<Vector2Int> pathCells)
    {
        if (grid == null || pathCells == null || pathCells.Count == 0) return;

        if (_moveCo != null) StopCoroutine(_moveCo);
        _moveCo = StartCoroutine(MoveCo(pathCells));
    }

    private IEnumerator MoveCo(List<Vector2Int> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int cell = path[i];
            Vector3 target = grid.CellCenterWorld(cell);

            while ((transform.position - target).sqrMagnitude > 0.0001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = target;
            CurrentCell = cell;
        }

        _moveCo = null;
    }
}