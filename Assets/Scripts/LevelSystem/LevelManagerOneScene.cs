using UnityEngine;

namespace LevelSystem
{
    public class LevelManagerOneScene : MonoBehaviour
    {
        [SerializeField] private LevelData[] levels;

        private ILevelLoader loader;
        
        void Start()
        {
            Initialize();
            
            LoadLevel();
        }

        private void Initialize()
        {
            loader = GetComponent<ILevelLoader>();
        }

        //Load random level
        public void LoadLevel()
        {
            int index = Random.Range(0, levels.Length);
            
            Load(index);
        }
        
        //Load level with index
        public void LoadLevel(int index)
        {
            Load(index);
        }

        private void Load(int index)
        {
            loader.LoadData(levels[index]);
            Debug.Log("Level " + index + " loaded");
        }
    }
}
