using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarGridPathfinder
{
    private int width;
    private int height;

    private int[] gCost;
    private int[] cameFrom;
    private bool[] closed;

    private readonly List<int> touched = new List<int>(256);
    private readonly MinHeap openHeap = new MinHeap(256);

    public void Resize(Vector2Int size)
    {
        int newWidth = Mathf.Max(1, size.x);
        int newHeight = Mathf.Max(1, size.y);

        if (newWidth == width && newHeight == height && gCost != null)
            return;

        width = newWidth;
        height = newHeight;

        int length = width * height;
        gCost = new int[length];
        cameFrom = new int[length];
        closed = new bool[length];

        for (int i = 0; i < length; i++)
        {
            gCost[i] = int.MaxValue;
            cameFrom[i] = -1;
            closed[i] = false;
        }

        touched.Clear();
        openHeap.Clear();
    }

    public bool TryFindPath(
        Vector2Int start,
        Vector2Int goal,
        Func<Vector2Int, bool> isWalkable,
        List<Vector2Int> outPath)
    {
        outPath.Clear();

        if (!InBounds(start) || !InBounds(goal))
            return false;

        // 목표가 막혀있으면(바위/고블린 등) 경로 없음 처리(요구사항: 이동 불가 장애물)
        if (!isWalkable(goal))
            return false;

        int startIndex = ToIndex(start);
        int goalIndex = ToIndex(goal);

        if (startIndex == goalIndex)
        {
            outPath.Add(start);
            return true;
        }

        PrepareSearch();

        Touch(startIndex);
        gCost[startIndex] = 0;
        cameFrom[startIndex] = -1;

        int startH = Heuristic(startIndex, goalIndex);
        openHeap.Push(startIndex, startH);

        while (openHeap.Count > 0)
        {
            int currentIndex = openHeap.PopMinIndex();

            if (closed[currentIndex])
                continue;

            closed[currentIndex] = true;

            if (currentIndex == goalIndex)
            {
                ReconstructPath(goalIndex, outPath);
                CleanupSearch();
                return true;
            }

            int currentG = gCost[currentIndex];

            int cx = currentIndex % width;
            int cy = currentIndex / width;

            // 4방향
            TryRelaxNeighbor(cx + 1, cy, currentIndex, currentG, goalIndex, isWalkable);
            TryRelaxNeighbor(cx - 1, cy, currentIndex, currentG, goalIndex, isWalkable);
            TryRelaxNeighbor(cx, cy + 1, currentIndex, currentG, goalIndex, isWalkable);
            TryRelaxNeighbor(cx, cy - 1, currentIndex, currentG, goalIndex, isWalkable);
        }

        CleanupSearch();
        return false;
    }

    private void TryRelaxNeighbor(
        int nx,
        int ny,
        int fromIndex,
        int fromG,
        int goalIndex,
        Func<Vector2Int, bool> isWalkable)
    {
        if (nx < 0 || ny < 0 || nx >= width || ny >= height)
            return;

        int neighborIndex = nx + ny * width;

        if (closed[neighborIndex])
            return;

        Vector2Int neighborCell = new Vector2Int(nx, ny);
        if (!isWalkable(neighborCell))
            return;

        int tentativeG = fromG + 1;

        if (gCost[neighborIndex] <= tentativeG)
            return;

        Touch(neighborIndex);
        gCost[neighborIndex] = tentativeG;
        cameFrom[neighborIndex] = fromIndex;

        int f = tentativeG + Heuristic(neighborIndex, goalIndex);
        openHeap.Push(neighborIndex, f);
    }

    private void ReconstructPath(int goalIndex, List<Vector2Int> outPath)
    {
        int current = goalIndex;
        while (current >= 0)
        {
            int x = current % width;
            int y = current / width;
            outPath.Add(new Vector2Int(x, y));
            current = cameFrom[current];
        }
        outPath.Reverse();
    }

    private int Heuristic(int fromIndex, int toIndex)
    {
        int fx = fromIndex % width;
        int fy = fromIndex / width;
        int tx = toIndex % width;
        int ty = toIndex / width;
        return Mathf.Abs(fx - tx) + Mathf.Abs(fy - ty);
    }

    private bool InBounds(Vector2Int c)
    {
        if (c.x < 0 || c.y < 0) return false;
        if (c.x >= width || c.y >= height) return false;
        return true;
    }

    private int ToIndex(Vector2Int c)
    {
        return c.x + c.y * width;
    }

    private void PrepareSearch()
    {
        openHeap.Clear();
        // touched / closed는 CleanupSearch에서 되돌림
    }

    private void CleanupSearch()
    {
        for (int i = 0; i < touched.Count; i++)
        {
            int idx = touched[i];
            gCost[idx] = int.MaxValue;
            cameFrom[idx] = -1;
            closed[idx] = false;
        }
        touched.Clear();
        openHeap.Clear();
    }

    private void Touch(int index)
    {
        if (gCost[index] == int.MaxValue && cameFrom[index] == -1 && closed[index] == false)
        {
            touched.Add(index);
            return;
        }

        // 이미 touched 되었을 가능성이 있으나, 중복 add는 비용이므로 조건을 단순화:
        // gCost가 MaxValue일 때만 add하도록 위에서 처리
        if (gCost[index] == int.MaxValue)
            touched.Add(index);
    }

    // ---------------------------------------------------------
    // Minimal Binary Heap (index + priority)
    // ---------------------------------------------------------
    private sealed class MinHeap
    {
        private int[] indices;
        private int[] priorities;
        private int count;

        public int Count { get { return count; } }

        public MinHeap(int capacity)
        {
            int cap = Mathf.Max(16, capacity);
            indices = new int[cap];
            priorities = new int[cap];
            count = 0;
        }

        public void Clear()
        {
            count = 0;
        }

        public void Push(int index, int priority)
        {
            EnsureCapacity(count + 1);

            int i = count;
            indices[i] = index;
            priorities[i] = priority;
            count++;

            HeapifyUp(i);
        }

        public int PopMinIndex()
        {
            int minIndex = indices[0];

            count--;
            if (count > 0)
            {
                indices[0] = indices[count];
                priorities[0] = priorities[count];
                HeapifyDown(0);
            }

            return minIndex;
        }

        private void HeapifyUp(int i)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (priorities[parent] <= priorities[i])
                    break;

                Swap(i, parent);
                i = parent;
            }
        }

        private void HeapifyDown(int i)
        {
            while (true)
            {
                int left = i * 2 + 1;
                int right = i * 2 + 2;
                int smallest = i;

                if (left < count && priorities[left] < priorities[smallest])
                    smallest = left;
                if (right < count && priorities[right] < priorities[smallest])
                    smallest = right;

                if (smallest == i)
                    break;

                Swap(i, smallest);
                i = smallest;
            }
        }

        private void Swap(int a, int b)
        {
            int tmpIndex = indices[a];
            indices[a] = indices[b];
            indices[b] = tmpIndex;

            int tmpPriority = priorities[a];
            priorities[a] = priorities[b];
            priorities[b] = tmpPriority;
        }

        private void EnsureCapacity(int needed)
        {
            if (needed <= indices.Length)
                return;

            int newCap = indices.Length * 2;
            while (newCap < needed)
                newCap *= 2;

            int[] newIndices = new int[newCap];
            int[] newPriorities = new int[newCap];

            Array.Copy(indices, newIndices, count);
            Array.Copy(priorities, newPriorities, count);

            indices = newIndices;
            priorities = newPriorities;
        }
    }
}