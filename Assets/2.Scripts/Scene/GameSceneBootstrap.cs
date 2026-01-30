using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBootstraper
{
    public IEnumerator Initialize(ILoadProgress progress);
}

public class GameSceneBootstrap : MonoBehaviour, IBootstraper
{
    [SerializeField] private InteractionController interactionController;
    [SerializeField] private AreaController areaController;
    [SerializeField] private UiController uiController;

    public IEnumerator Initialize(ILoadProgress progress)
    {
        progress.UpdateMessage("Setting Data...");
        uiController.Init(areaController, interactionController);

        progress.UpdateMessage("Creating areas...");
        yield return areaController.CreateAreas(progress);

        progress.UpdateMessage("Finalizing...");
        LoadData();
    }

    private void LoadData()
    {
    }
}  
        