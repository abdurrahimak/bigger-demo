using BiggerDemo.Data;
using BiggerDemo.Game;
using CoreProject.Resource;
using CoreProject.Singleton;
using UnityEngine;

namespace BiggerDemo.Creation
{
    public class LevelFactory : SingletonClass<LevelFactory>
    {
        private const string LEVEL_DIFF_INF_KEY = "LevelDifficultInformation";

        public IRandomLevelCreator CreateRandomLevelCreator()
        {
            return new RandomLevelCreator();
        }

        public Level CreateLevel(int index)
        {
            Level level = GameObject.Instantiate(ResourceManager.Instance.GetResource<GameObject>("Level")).GetComponent<Level>();
            LevelData levelData = ResourceManager.Instance.GetResource<Levels>("Levels").LevelDatas[index];
            level.Initialize(levelData);
            return level;
        }

        public LevelParameters GetLevelParameters(LevelDifficult levelDifficult)
        {
            LevelDifficultInformation levelDifficultInformation = ResourceManager.Instance.GetResource<LevelDifficultInformation>(LEVEL_DIFF_INF_KEY);
            return levelDifficultInformation.GetLevelParameters(levelDifficult);
        }
    }
}
