using UnityEngine;
using Gemier.Gems;

namespace Gemier.Managers
{
    public class GemManager : MonoBehaviour
    {
        public static GemManager Instance { get; private set;}
        [SerializeField] private GemType[] gemsArray;
        private void Awake() 
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }


        public GemType GetRandomGem()
        {
            return gemsArray[Random.Range(0, gemsArray.Length)];
        }

        public string GetRandomGemTag()
        {
            return gemsArray[Random.Range(0, gemsArray.Length)].gemSO.gemName;
        }
    }
}

