using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TMP_ShatterOnClick2D : MonoBehaviour
{
    [Header("Mode")]
    public bool randomShape = true;

    [SerializeField] float shardLife = 0.35f;
    [SerializeField] float shardSpeed = 1.4f;
    [SerializeField] float shardUpBias = 0.6f;
    [SerializeField] float shardAngularSpeed = 360f;
    [SerializeField] float gravity = -6f;
    [SerializeField] bool pixelSnapMovement = true;
    [SerializeField] float pixelsPerUnit = 16f;

    TMP_Text tmp;
    bool[] broken;
    bool isShattering;

    int shatterSerial = 0;
    const int baseSeed = 12345;

    class Shard
    {
        public Transform tr;
        public Mesh mesh;
        public Color32[] baseColors; // 3개
        public Vector3 vel;
        public float angVel;
    }

    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// 외부에서 호출: 텍스트 전체를 한 번에 깨뜨리고 사라지게 합니다.
    /// (UnityEvent/버튼/트리거 등에서 이 함수만 호출하면 됨)
    /// </summary>
    public void ShatterAll()
    {
        if (!isActiveAndEnabled) return;
        if (isShattering) return;
        StartCoroutine(ShatterAllCoroutine());
    }

    IEnumerator ShatterAllCoroutine()
    {
        isShattering = true;

        EnsureState();
        var ti = tmp.textInfo;

        int callSeed = randomShape ? unchecked((int)System.DateTime.UtcNow.Ticks ^ (shatterSerial++ * 1103515245)) : baseSeed;
        var allShards = new List<Shard>(ti.characterCount * 8);

        // 텍스트 전체의 모든 "보이는 글자"를 깨뜨림
        for (int charIndex = 0; charIndex < ti.characterCount; charIndex++)
        {
            var ch = ti.characterInfo[charIndex];
            if (!ch.isVisible) continue;
            if (broken[charIndex]) continue;

            int matIndex = ch.materialReferenceIndex;
            int vIndex = ch.vertexIndex;

            Vector3[] verts = ti.meshInfo[matIndex].vertices;
            Vector2[] uvs = ti.meshInfo[matIndex].uvs0;
            Color32[] cols = ti.meshInfo[matIndex].colors32;

            Vector3 bl = verts[vIndex + 0];
            Vector3 tl = verts[vIndex + 1];
            Vector3 tr = verts[vIndex + 2];
            Vector3 br = verts[vIndex + 3];

            Vector2 uvBL = uvs[vIndex + 0];
            Vector2 uvTL = uvs[vIndex + 1];
            Vector2 uvTR = uvs[vIndex + 2];
            Vector2 uvBR = uvs[vIndex + 3];

            Color32 cBL = cols[vIndex + 0];
            Color32 cTL = cols[vIndex + 1];
            Color32 cTR = cols[vIndex + 2];
            Color32 cBR = cols[vIndex + 3];

            Vector3 localCenter = (bl + tl + tr + br) * 0.25f;

            var rng = new System.Random(callSeed);

            var shards = SpawnShards2x2(
                bl, tl, tr, br,
                uvBL, uvTL, uvTR, uvBR,
                cBL, cTL, cTR, cBR,
                ti.meshInfo[matIndex].material,
                localCenter,
                rng,
                randomShape
            );

            allShards.AddRange(shards);

            // 원본 글자 숨김(알파 0)
            broken[charIndex] = true;
            HideCharacterVertexAlpha(charIndex, 0);
        }

        // 색상 배열 변경 반영
        tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        // 조각 애니메이션
        yield return AnimateShards(allShards);

        isShattering = false;
    }

    void EnsureState()
    {
        tmp.ForceMeshUpdate();

        int n = Mathf.Max(1, tmp.textInfo.characterCount);
        if (broken == null || broken.Length != n)
            broken = new bool[n];

        // ForceMeshUpdate가 메쉬를 재생성하므로, 깨진 글자는 다시 숨김 처리
        ReapplyBrokenMask();
        tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    void ReapplyBrokenMask()
    {
        var ti = tmp.textInfo;
        for (int i = 0; i < ti.characterCount && i < broken.Length; i++)
        {
            if (broken[i] && ti.characterInfo[i].isVisible)
                HideCharacterVertexAlpha(i, 0);
        }
    }

    void HideCharacterVertexAlpha(int charIndex, byte alpha)
    {
        var ti = tmp.textInfo;
        if (charIndex >= ti.characterCount) return;

        var ch = ti.characterInfo[charIndex];
        if (!ch.isVisible) return;

        int matIndex = ch.materialReferenceIndex;
        int vIndex = ch.vertexIndex;
        var cols = ti.meshInfo[matIndex].colors32;

        for (int i = 0; i < 4; i++)
        {
            var c = cols[vIndex + i];
            c.a = alpha;
            cols[vIndex + i] = c;
        }
    }

    // ---------- 랜덤 형태 분할 (true일 때) ----------
    // 기본은 2x2(8조각)인데, randomShape=true면
    // 분할 비율(tx,ty,cx,cy) + 각 사각형의 대각선을 랜덤으로 선택해서 "형태"가 바뀜
    List<Shard> SpawnShards2x2(
        Vector3 bl, Vector3 tl, Vector3 tr, Vector3 br,
        Vector2 uvBL, Vector2 uvTL, Vector2 uvTR, Vector2 uvBR,
        Color32 cBL, Color32 cTL, Color32 cTR, Color32 cBR,
        Material mat,
        Vector3 localCenter,
        System.Random rng,
        bool randomize)
    {
        float tx = randomize ? NextFloat(rng, 0.32f, 0.68f) : 0.5f;
        float ty = randomize ? NextFloat(rng, 0.32f, 0.68f) : 0.5f;

        float cx = randomize ? NextFloat(rng, 0.35f, 0.65f) : 0.5f;
        float cy = randomize ? NextFloat(rng, 0.35f, 0.65f) : 0.5f;

        Vector3 x = br - bl;
        Vector3 y = tl - bl;

        Vector2 uvX = uvBR - uvBL;
        Vector2 uvY = uvTL - uvBL;

        Vector3 bm = bl + x * tx;
        Vector3 tm = tl + (tr - tl) * tx;
        Vector3 lm = bl + y * ty;
        Vector3 rm = br + (tr - br) * ty;
        Vector3 ce = bl + x * cx + y * cy;

        Vector2 uvBM = uvBL + uvX * tx;
        Vector2 uvTM = uvTL + (uvTR - uvTL) * tx;
        Vector2 uvLM = uvBL + uvY * ty;
        Vector2 uvRM = uvBR + (uvTR - uvBR) * ty;
        Vector2 uvCE = uvBL + uvX * cx + uvY * cy;

        Color32 cBM = LerpColor32(cBL, cBR, tx);
        Color32 cTM = LerpColor32(cTL, cTR, tx);
        Color32 cLM = LerpColor32(cBL, cTL, ty);
        Color32 cRM = LerpColor32(cBR, cTR, ty);
        Color32 cCE = AvgColor32(cBL, cTL, cTR, cBR);

        var list = new List<Shard>(8);

        AddQuad(list, bl, lm, ce, bm, uvBL, uvLM, uvCE, uvBM, cBL, cLM, cCE, cBM, mat, localCenter, rng, randomize);
        AddQuad(list, bm, ce, rm, br, uvBM, uvCE, uvRM, uvBR, cBM, cCE, cRM, cBR, mat, localCenter, rng, randomize);
        AddQuad(list, lm, tl, tm, ce, uvLM, uvTL, uvTM, uvCE, cLM, cTL, cTM, cCE, mat, localCenter, rng, randomize);
        AddQuad(list, ce, tm, tr, rm, uvCE, uvTM, uvTR, uvRM, cCE, cTM, cTR, cRM, mat, localCenter, rng, randomize);

        return list;
    }

    void AddQuad(
        List<Shard> list,
        Vector3 a, Vector3 b, Vector3 c, Vector3 d,
        Vector2 uva, Vector2 uvb, Vector2 uvc, Vector2 uvd,
        Color32 ca, Color32 cb, Color32 cc, Color32 cd,
        Material mat,
        Vector3 localCenter,
        System.Random rng,
        bool randomize)
    {
        bool diagAC = randomize ? NextBool(rng) : true;

        if (diagAC)
        {
            AddTri(list, a, b, c, uva, uvb, uvc, ca, cb, cc, mat, localCenter, rng);
            AddTri(list, a, c, d, uva, uvc, uvd, ca, cc, cd, mat, localCenter, rng);
        }
        else
        {
            AddTri(list, a, d, b, uva, uvd, uvb, ca, cd, cb, mat, localCenter, rng);
            AddTri(list, b, d, c, uvb, uvd, uvc, cb, cd, cc, mat, localCenter, rng);
        }
    }

    void AddTri(
        List<Shard> list,
        Vector3 a, Vector3 b, Vector3 c,
        Vector2 uva, Vector2 uvb, Vector2 uvc,
        Color32 ca, Color32 cb, Color32 cc,
        Material mat,
        Vector3 localCenter,
        System.Random rng)
    {
        var go = new GameObject("TMP_Shard");
        go.transform.SetParent(transform, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        var mf = go.AddComponent<MeshFilter>();
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = mat;

        var r = GetComponent<Renderer>();
        if (r != null)
        {
            mr.sortingLayerID = r.sortingLayerID;
            mr.sortingOrder = r.sortingOrder + 1;
        }

        var mesh = new Mesh();
        mesh.vertices = new[] { a, b, c };
        mesh.uv = new[] { uva, uvb, uvc };
        mesh.triangles = new[] { 0, 1, 2 };
        mesh.colors32 = new[] { ca, cb, cc };
        mesh.RecalculateBounds();
        mf.sharedMesh = mesh;

        Vector3 triCenterLocal = (a + b + c) / 3f;
        Vector3 dirLocal = (triCenterLocal - localCenter).normalized;
        if (dirLocal.sqrMagnitude < 0.0001f) dirLocal = Vector3.right;

        Vector3 dirWorld = transform.TransformDirection(dirLocal);
        Vector3 upWorld = transform.up;

        float spdMul = NextFloat(rng, 0.85f, 1.15f);
        float ang = NextFloat(rng, -shardAngularSpeed, shardAngularSpeed);

        var shard = new Shard
        {
            tr = go.transform,
            mesh = mesh,
            baseColors = new[] { ca, cb, cc },
            vel = (dirWorld * shardSpeed * spdMul) + (upWorld * shardUpBias * shardSpeed * spdMul),
            angVel = ang
        };

        // z-fighting 완화
        shard.tr.position += -transform.forward * 0.01f;

        list.Add(shard);
    }

    IEnumerator AnimateShards(List<Shard> shards)
    {
        float life = Mathf.Max(0.05f, shardLife);
        float t = 0f;

        while (t < life)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / life);

            for (int i = shards.Count - 1; i >= 0; i--)
            {
                var s = shards[i];
                if (s.tr == null) { shards.RemoveAt(i); continue; }

                s.vel += (Vector3.up * gravity) * Time.deltaTime;
                var pos = s.tr.position + s.vel * Time.deltaTime;
                if (pixelSnapMovement) pos = PixelSnapWorld(pos);
                s.tr.position = pos;

                s.tr.Rotate(0f, 0f, s.angVel * Time.deltaTime, Space.Self);

                var cols = s.mesh.colors32;
                cols[0].a = (byte)Mathf.RoundToInt(s.baseColors[0].a * (1f - k));
                cols[1].a = (byte)Mathf.RoundToInt(s.baseColors[1].a * (1f - k));
                cols[2].a = (byte)Mathf.RoundToInt(s.baseColors[2].a * (1f - k));
                s.mesh.colors32 = cols;
            }

            yield return null;
        }

        foreach (var s in shards)
            if (s.tr != null) Destroy(s.tr.gameObject);
    }

    Vector3 PixelSnapWorld(Vector3 w)
    {
        if (pixelsPerUnit <= 0f) return w;
        w.x = Mathf.Round(w.x * pixelsPerUnit) / pixelsPerUnit;
        w.y = Mathf.Round(w.y * pixelsPerUnit) / pixelsPerUnit;
        return w;
    }

    static int HashSeed(int a, int b)
    {
        unchecked
        {
            int h = 17;
            h = h * 31 + a;
            h = h * 31 + b * 19349663;
            return h;
        }
    }

    static float NextFloat(System.Random rng, float min, float max)
        => (float)(min + (max - min) * rng.NextDouble());

    static bool NextBool(System.Random rng) => rng.Next(0, 2) == 0;

    static Color32 LerpColor32(Color32 a, Color32 b, float t)
    {
        Color ca = a; Color cb = b;
        return (Color32)Color.Lerp(ca, cb, t);
    }

    static Color32 AvgColor32(Color32 a, Color32 b, Color32 c, Color32 d)
    {
        int r = a.r + b.r + c.r + d.r;
        int g = a.g + b.g + c.g + d.g;
        int bl = a.b + b.b + c.b + d.b;
        int al = a.a + b.a + c.a + d.a;
        return new Color32((byte)(r / 4), (byte)(g / 4), (byte)(bl / 4), (byte)(al / 4));
    }
}