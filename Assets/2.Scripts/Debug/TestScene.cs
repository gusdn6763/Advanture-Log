using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    [Header("Grid Settings (Bottom-Left origin)")]
    [Min(1)] public int width = 16;
    [Min(1)] public int height = 9;

    // cell (1,1) 크기
    public Vector2 cellSize = Vector2.one;

    // cell(0,0)의 월드 좌하단 좌표
    public Vector2 originWorld = Vector2.zero;

    [Header("Line Settings")]
    [Min(0.001f)] public float lineWidth = 0.03f;
    public bool drawInPlayModeOnly = false;

    [Header("Z depth (2D sorting)")]
    public float z = 0f;

    // 내부 관리
    private readonly List<LineRenderer> _lines = new();
    private bool _dirty = true;

    private void OnEnable()
    {
        MarkDirty();
        RedrawIfNeeded(force: true);
    }

    private void OnDisable()
    {
        // 에디터에서 컴포넌트 껐다 켤 때 깔끔하게 유지하고 싶으면 해제
        //ClearAllLines();
    }

    private void OnValidate()
    {
        // Inspector 값 바뀔 때마다 재드로우
        MarkDirty();
        RedrawIfNeeded();
    }

    private void Update()
    {
        if (drawInPlayModeOnly && !Application.isPlaying)
            return;

        RedrawIfNeeded();
    }

    public void MarkDirty() => _dirty = true;

    private void RedrawIfNeeded(bool force = false)
    {
        if (!force && !_dirty) return;
        _dirty = false;

        // 입력 값 방어
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);
        cellSize.x = Mathf.Max(0.0001f, cellSize.x);
        cellSize.y = Mathf.Max(0.0001f, cellSize.y);
        lineWidth = Mathf.Max(0.001f, lineWidth);

        int verticalCount = width + 1;
        int horizontalCount = height + 1;
        int neededLines = verticalCount + horizontalCount;

        EnsureLineCount(neededLines);

        // 그리드 월드 영역
        float xMin = originWorld.x;
        float yMin = originWorld.y;
        float xMax = originWorld.x + width * cellSize.x;
        float yMax = originWorld.y + height * cellSize.y;

        int idx = 0;

        // 세로 라인들 (x 고정)
        for (int x = 0; x < verticalCount; x++)
        {
            float wx = xMin + x * cellSize.x;

            var lr = _lines[idx++];
            SetLine(lr,
                new Vector3(wx, yMin, z),
                new Vector3(wx, yMax, z)
            );
        }

        // 가로 라인들 (y 고정)
        for (int y = 0; y < horizontalCount; y++)
        {
            float wy = yMin + y * cellSize.y;

            var lr = _lines[idx++];
            SetLine(lr,
                new Vector3(xMin, wy, z),
                new Vector3(xMax, wy, z)
            );
        }

        // 남는 라인은 비활성화
        for (int i = idx; i < _lines.Count; i++)
            if (_lines[i] != null) _lines[i].gameObject.SetActive(false);
    }

    private void EnsureLineCount(int count)
    {
        // 필요한 만큼 생성
        while (_lines.Count < count)
        {
            var go = new GameObject($"GridLine_{_lines.Count}");
            go.transform.SetParent(transform, false);

            var lr = go.AddComponent<LineRenderer>();
            SetupLineRendererDefaults(lr);

            _lines.Add(lr);
        }

        // 필요한 만큼 활성화 + 기본값 갱신
        for (int i = 0; i < count; i++)
        {
            var lr = _lines[i];
            if (lr == null) continue;

            lr.gameObject.SetActive(true);
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
        }
    }

    private void SetupLineRendererDefaults(LineRenderer lr)
    {
        // 머티리얼은 프로젝트 상황에 따라 다를 수 있음
        // 가장 무난: Sprites/Default (2D에서 잘 보임)
        var mat = new Material(Shader.Find("Sprites/Default"));
        lr.material = mat;

        lr.useWorldSpace = true;
        lr.positionCount = 2;

        // 색은 필요하면 Inspector로 빼도 됨 (여기선 기본 흰색)
        lr.startColor = Color.white;
        lr.endColor = Color.white;

        // 코너/캡 둥글게
        lr.numCapVertices = 2;
        lr.numCornerVertices = 2;

        // 정렬: 2D에서는 Sorting Layer가 중요할 수 있음
        lr.sortingOrder = 1000;
    }

    private void SetLine(LineRenderer lr, Vector3 a, Vector3 b)
    {
        if (lr == null) return;

        lr.positionCount = 2;
        lr.SetPosition(0, a);
        lr.SetPosition(1, b);

        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
    }

    public void ClearAllLines()
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            if (_lines[i] == null) continue;
            if (Application.isPlaying)
                Destroy(_lines[i].gameObject);
            else
                DestroyImmediate(_lines[i].gameObject);
        }
        _lines.Clear();
    }
}