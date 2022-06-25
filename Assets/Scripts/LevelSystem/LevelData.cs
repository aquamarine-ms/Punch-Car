using System;
using UnityEngine;

namespace LevelSystem
{
    [Serializable]
    public class LevelData
    {
        public int columnCount;
        public GameObject environmentGameObject;
        public int scoreToCompleteLevel;
    }
}
