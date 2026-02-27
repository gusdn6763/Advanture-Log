using System;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Localization;


namespace Data
{
    #region 스탯
    [Serializable]
    public class MainStatSaveData
    {
        public MainStatType mainType;
        public float baseValue;
    }
    #endregion

    #region 리소스
    [Serializable]
    public class SubStatSaveData
    {
        public SubStatType subType;
        public float baseValue;
    }
    #endregion

    #region 개체
    [Serializable]
    public class BaseSaveData
    {
        public string Id;              //오브젝트 찾기용
        public Vector2 position;
    }

    [Serializable]
    public class ActorSaveData : BaseSaveData
    {
        public List<MainStatSaveData> mainStat = new List<MainStatSaveData>();
        public List<SubStatSaveData> subStat = new List<SubStatSaveData>();

        public ActorSaveData() { }
        public ActorSaveData(BaseSaveData BaseSaveData)
        {
            Id = BaseSaveData.Id;
            position = BaseSaveData.position;
        }
    }

    [Serializable]
    public class MobSaveData : ActorSaveData
    {
        public MobSaveData() { }
        public MobSaveData(ActorSaveData actorData) : base(actorData)
        {
        }
    }

    [Serializable]
    public class PlayerSaveData : ActorSaveData
    {
        public int currentHunger;
        public float current;
    }
    #endregion

    #region 설정
    [Serializable]
    public class SettingSaveData
    {
        #region 노래 데이터
        public bool bgmOn;
        public bool soundOn;

        public float bgmVolume;
        public float soundVolume;
        #endregion

        public Locale locale;
        public FullScreenMode fullScreenMode;
    }
    #endregion

    #region 지역
    [Serializable]
    public class WorldSaveData
    {
        public int currentLocationId;

        public List<LocationSaveData> Locations;
    }

    [Serializable]
    public class LocationSaveData
    {
        public List<BaseSaveData> articles = new();
        public List<ActorSaveData> mobs = new();
        public PlayerSaveData player;
    }
    #endregion

    [Serializable]
    public class GameSaveData
    {
        public int Gold = 0;

        public SettingSaveData SettingData;
        //public CurrentTime TimeData;

        public WorldSaveData WorldData;

        //public List<ItemSaveData> Items = new List<ItemSaveData>();
        //public List<QuestSaveData> AllQuests = new List<QuestSaveData>();
    }
}