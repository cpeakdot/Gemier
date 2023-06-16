using UnityEngine;

namespace Gemier.Gems
{
    [CreateAssetMenu(fileName = "Gem", menuName = "Gems/Create New Gem", order = 1)]
    public class GemSO : ScriptableObject
    {
        public string gemName;
        public int startingPrice;
        public Sprite gemIcon;
        public GameObject gemModel;
        public Material material;
    }
}

