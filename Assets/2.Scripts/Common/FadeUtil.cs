using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class FadeUtil
{
    // ----------------------------
    // CanvasGroup
    // ----------------------------

    public static Tween FadeIn(CanvasGroup cg, float duration = 0.15f, bool enableInput = true, bool unscaledTime = true)
    {
        if (!cg) return null;

        // 트윈이 돌려면 활성 상태여야 함
        if (!cg.gameObject.activeSelf) cg.gameObject.SetActive(true);

        cg.DOKill(); // 중복 트윈 방지

        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        var t = cg.DOFade(1f, duration);
        if (unscaledTime) t.SetUpdate(true);

        t.OnComplete(() =>
        {
            if (!cg) return;
            cg.interactable = enableInput;
            cg.blocksRaycasts = enableInput;
        });

        return t;
    }

    public static Tween FadeOut(CanvasGroup cg, float duration = 0.15f, bool disableGameObject = true, bool unscaledTime = true)
    {
        if (!cg) return null;

        cg.DOKill();

        cg.interactable = false;
        cg.blocksRaycasts = false;

        var t = cg.DOFade(0f, duration);
        if (unscaledTime) t.SetUpdate(true);

        t.OnComplete(() =>
        {
            if (!cg) return;
            if (disableGameObject) cg.gameObject.SetActive(false);
        });

        return t;
    }

    public static void SetVisibleInstant(CanvasGroup cg, bool visible, bool input = true)
    {
        if (!cg) return;

        cg.DOKill();
        if (visible)
        {
            if (!cg.gameObject.activeSelf) cg.gameObject.SetActive(true);
            cg.alpha = 1f;
            cg.interactable = input;
            cg.blocksRaycasts = input;
        }
        else
        {
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            cg.gameObject.SetActive(false);
        }
    }

    // CanvasGroup 없으면 붙여서 반환
    public static CanvasGroup GetOrAddCanvasGroup(Component c)
    {
        if (!c) return null;
        var cg = c.GetComponent<CanvasGroup>();
        return cg ? cg : c.gameObject.AddComponent<CanvasGroup>();
    }

    // ----------------------------
    // UGUI Graphic (Image, Text, TMP UGUI 등 대부분 포함)
    // ----------------------------

    public static Tween FadeTo(Graphic g, float alpha, float duration = 0.15f, bool unscaledTime = true)
    {
        if (!g) return null;

        if (!g.gameObject.activeSelf) g.gameObject.SetActive(true);

        g.DOKill();

        var t = g.DOFade(alpha, duration);
        if (unscaledTime) t.SetUpdate(true);
        return t;
    }

    public static Tween FadeIn(Graphic g, float duration = 0.15f, bool unscaledTime = true)
        => FadeTo(g, 1f, duration, unscaledTime);

    public static Tween FadeOut(Graphic g, float duration = 0.15f, bool disableGameObject = false, bool unscaledTime = true)
    {
        if (!g) return null;

        g.DOKill();

        var t = g.DOFade(0f, duration);
        if (unscaledTime) t.SetUpdate(true);

        if (disableGameObject)
        {
            t.OnComplete(() =>
            {
                if (!g) return;
                g.gameObject.SetActive(false);
            });
        }

        return t;
    }

    // ----------------------------
    // TextMeshPro (3D TMP 포함 가능)
    // - TextMeshProUGUI는 Graphic 경로로도 가능하지만,
    //   TMP_Text 전용으로 쓰고 싶으면 아래 사용
    // ----------------------------

    public static Tween FadeTo(TMP_Text tmp, float alpha, float duration = 0.15f, bool unscaledTime = true)
    {
        if (!tmp) return null;

        if (!tmp.gameObject.activeSelf) tmp.gameObject.SetActive(true);

        tmp.DOKill();

        var t = tmp.DOFade(alpha, duration);
        if (unscaledTime) t.SetUpdate(true);
        return t;
    }

    public static Tween FadeIn(TMP_Text tmp, float duration = 0.15f, bool unscaledTime = true)
        => FadeTo(tmp, 1f, duration, unscaledTime);

    public static Tween FadeOut(TMP_Text tmp, float duration = 0.15f, bool disableGameObject = false, bool unscaledTime = true)
    {
        if (!tmp) return null;

        tmp.DOKill();

        var t = tmp.DOFade(0f, duration);
        if (unscaledTime) t.SetUpdate(true);

        if (disableGameObject)
        {
            t.OnComplete(() =>
            {
                if (!tmp) return;
                tmp.gameObject.SetActive(false);
            });
        }

        return t;
    }

    // ----------------------------
    // SpriteRenderer (월드 스프라이트)
    // ----------------------------

    public static Tween FadeTo(SpriteRenderer sr, float alpha, float duration = 0.15f, bool unscaledTime = true)
    {
        if (!sr) return null;

        if (!sr.gameObject.activeSelf) sr.gameObject.SetActive(true);

        sr.DOKill();

        var t = sr.DOFade(alpha, duration);
        if (unscaledTime) t.SetUpdate(true);
        return t;
    }

    public static Tween FadeIn(SpriteRenderer sr, float duration = 0.15f, bool unscaledTime = true)
        => FadeTo(sr, 1f, duration, unscaledTime);

    public static Tween FadeOut(SpriteRenderer sr, float duration = 0.15f, bool disableGameObject = false, bool unscaledTime = true)
    {
        if (!sr) return null;

        sr.DOKill();

        var t = sr.DOFade(0f, duration);
        if (unscaledTime) t.SetUpdate(true);

        if (disableGameObject)
        {
            t.OnComplete(() =>
            {
                if (!sr) return;
                sr.gameObject.SetActive(false);
            });
        }

        return t;
    }
}