using UnityEngine;
using Gemier.GridSpace;

namespace Gemier.Gems
{
    public class Gem : MonoBehaviour
    {
        private bool isInTheBag = false;
        private GridGemController currentGrid; 

        [SerializeField] GemType gemType;

        private void OnEnable() {
            isInTheBag = false;
            currentGrid = null;
            transform.SetParent(null);
        }

        public void GetInTheBag()
        {
            isInTheBag = true;
        }

        public void SetGrid(GridGemController grid)
        {
            currentGrid = grid;
        }

        public GemType GetGemType => gemType;
        public GridGemController GetGrid => currentGrid;
        public bool IsGemInTheBag => isInTheBag;
    }
}

