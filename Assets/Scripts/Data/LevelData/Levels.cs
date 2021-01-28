using System;
using System.Collections.Generic;
using UnityEngine;

namespace BiggerDemo.Data
{
    [CreateAssetMenu(fileName = "Levels.asset", menuName = "BiggerDemo/Create Levels Asset")]
    [Serializable]
    public class Levels : ScriptableObject
    {
        public List<LevelData> LevelDatas;

        public LevelData GetLevel(int index)
        {
            return LevelDatas[index];
        }
    }
}
