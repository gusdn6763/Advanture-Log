using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Area : MonoBehaviour
{
    public AreaSo AreaData { get; private set; }

    private SpriteRenderer spriteRenderer;

    private List<AsyncOperationHandle> defaultInstanceHandles = new List<AsyncOperationHandle>();

    public void Init(AreaSo data)
    {
        AreaData = data;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator CreateDefaultObjects()
    {
        // Background
        AsyncOperationHandle<Sprite> bagkgroundHandle = AreaData.BackgroundSpriteRef.LoadAssetAsync<Sprite>();
        yield return bagkgroundHandle;

        defaultInstanceHandles.Add(bagkgroundHandle);
        spriteRenderer.sprite = bagkgroundHandle.Result;

        // Default Objects
        for (int i = 0; i < AreaData.CreateObjectRefs.Count; i++)
        {
            AssetReferenceGameObject aref = AreaData.CreateObjectRefs[i];

            AsyncOperationHandle<GameObject> instHandle = aref.InstantiateAsync(transform.position, Quaternion.identity, transform);

            yield return instHandle;

            defaultInstanceHandles.Add(instHandle);
        }
    }

    private void OnDestroy()
    {
        Release();
    }

    public void Release()
    {
        int count = defaultInstanceHandles.Count;
        for (int i = 0; i < count; i++)
            Addressables.ReleaseInstance(defaultInstanceHandles[i]);

        defaultInstanceHandles.Clear();

        spriteRenderer.sprite = null;
    }
}