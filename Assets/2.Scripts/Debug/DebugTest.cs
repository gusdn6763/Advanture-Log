using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class DebugTest : MonoBehaviour, ILoadProgress
{
    [SerializeField] public GameSceneBootstrap b;
    public AreaSo startArea;

    private void Awake()
    {
#if DEBUG
        //StartCoroutine(GameStart());
#else
        gameObject.SetActive(false);
#endif
    }

    public void UpdateMessage(string message)
    {
        Debug.Log(message);
    }

    public void UpdateProgress(float value01)
    {

    }

    public BaseEntitySo appleSo;
    public void CreateApple() => CreateEntity(appleSo, _ => { });

    public BaseEntitySo goblinSo;
    public void CreateGoblin() => CreateEntity(goblinSo, _ => { });

    public BaseEntitySo player;
    public void CreatePlayer() => CreateEntity(player, _ => { });

    public void CreateEntity(BaseEntitySo so, System.Action<BaseEntity> onCreated)
    {
        BaseEntity entity = Managers.Object.Spawn<BaseEntity>(Vector2.zero, so.Id);

        onCreated?.Invoke(entity);
    }



    public Image aa;   // 기존 GameObject 대신 Image로 받기
    public Image bb;

    [Range(0.01f, 5f)]
    public float duration = 0.3f;

    // 0~255 기준 알파 목표
    private const float TargetA = 0.5f;

    public void TT()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        // 시작 상태 세팅
        aa.gameObject.SetActive(true);
        bb.gameObject.SetActive(true);

        SetAlpha(aa, TargetA); // A: 128
        SetAlpha(bb, 0f);      // B: 0

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / duration);

            // A: 128 -> 0
            SetAlpha(aa, Mathf.Lerp(TargetA, 0f, u));
            // B: 0 -> 128
            SetAlpha(bb, Mathf.Lerp(0f, TargetA, u));

            yield return null;
        }

        // 마무리 고정
        SetAlpha(aa, 0f);
        SetAlpha(bb, TargetA);

        aa.gameObject.SetActive(false); // 완전히 사라지면 비활성
        // bb는 계속 활성 유지 (이미 true)
    }

    private void SetAlpha(Image img, float a01)
    {
        Color c = img.color;
        c.a = a01;
        img.color = c;
    }






}