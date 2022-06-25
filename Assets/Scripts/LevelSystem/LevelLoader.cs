using UnityEngine;

namespace LevelSystem
{
    public class LevelLoader : MonoBehaviour, ILevelLoader
    {
        public void LoadData(LevelData data)
        {
            FindObjectOfType<MovableEnvironment>().ChangeEnvironment(data.environmentGameObject);

            GameManager.Instance.scoreToEndRound = data.scoreToCompleteLevel;
            
            FindObjectOfType<TileCreator>().SetCountTopTiles(data.columnCount);
        }
    }
}