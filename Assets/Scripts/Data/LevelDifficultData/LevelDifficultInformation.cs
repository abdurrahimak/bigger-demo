using System.Collections.Generic;
using UnityEngine;

namespace BiggerDemo.Data
{
    [CreateAssetMenu(fileName = "LevelDifficultInformation.asset", menuName = "BiggerDemo/Create Level Difficult Information")]
    public class LevelDifficultInformation : ScriptableObject
    {
        public List<LevelDifficultData> LevelDifficultDatas;

        public LevelParameters GetLevelParameters(LevelDifficult levelDifficult)
        {
            foreach (var levelDifficultDatas in LevelDifficultDatas)
            {
                if (levelDifficultDatas.LevelDifficult == levelDifficult)
                {
                    return levelDifficultDatas.LevelParameters;
                }
            }
            return LevelParameters.CreatePrimitive();
        }
    }
}
