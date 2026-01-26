using System;
using System.Collections;
using UnityEngine;

public interface IBootstraper
{
    public IEnumerator Initialize(ILoadProgress progress);
}

public class GameSceneBootstrap : MonoBehaviour, IBootstraper
{
    [SerializeField] private AreaController areaController;
    [SerializeField] private GameSceneInputController gameSceneInputController;

    public IEnumerator Initialize(ILoadProgress progress)
    {
        progress.UpdateMessage("Creating areas...");
        yield return areaController.CreateAreas(progress);

        progress.UpdateMessage("Finalizing...");
        yield return null;
    }
}  
        