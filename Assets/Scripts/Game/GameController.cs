using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreProject.Singleton;
using System;
using CoreProject.Resource;
using CoreProject.Pool;
using CoreProject.Data;
using BiggerDemo.Data;
using BiggerDemo.Creation;

namespace BiggerDemo.Game
{
    public class GameController : SingletonComponent<GameController>
    {
        IDataStoreStrategy _dataStoreStrategy = new PlayerPrefsStrategy();
        int currentLevel = 0;
        int maxLevel = 0;

        void Start()
        {
            PoolerManager.Instance.Initialize();
            ReadLevel();
            OpenLevel();
        }

        private void ReadLevel()
        {
            string levelString = (string)_dataStoreStrategy.Read("Level");
            currentLevel = Int32.Parse((String.IsNullOrEmpty(levelString) ? "0" : levelString));
            maxLevel = ResourceManager.Instance.GetResource<Levels>("Levels").LevelDatas.Count - 1;
        }

        private void SaveLevel(int level)
        {
            _dataStoreStrategy.Write("Level", level.ToString());
        }

        public void NextLevel()
        {
            if (currentLevel == maxLevel)
            {
                currentLevel = 0;
            }
            else
            {
                currentLevel++;
            }
            SaveLevel(currentLevel);
            OpenLevel();
        }

        private void OpenLevel()
        {
            LevelFactory.Instance.CreateLevel(currentLevel);
        }
    }
}
