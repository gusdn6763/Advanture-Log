using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PathParticleRenderer : MonoBehaviour
{
    private ParticleSystem _ps;

    [Header("Emit")]
    public float zOffset = -0.1f; // 바닥 위에 보이게

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();

        // 권장 설정(인스펙터에서 해도 됨):
        // - Simulation Space: World
        // - Emission: Rate over Time 0
        // - Start Speed 0
        // - 적당한 Start Lifetime(0.5~2s) / Size
    }

    public void Clear() => _ps.Clear(true);

    public void RenderPath(IReadOnlyList<Vector3> points)
    {
        _ps.Clear(true);
        if (points == null || points.Count == 0) return;

        var ep = new ParticleSystem.EmitParams();
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 p = points[i];
            p.z = zOffset;

            ep.position = p;
            _ps.Emit(ep, 1);
        }
    }
}