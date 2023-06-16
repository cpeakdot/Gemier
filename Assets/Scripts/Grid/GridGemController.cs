using cpeak.cPool;
using Gemier.Gems;
using Gemier.Managers;
using UnityEngine;

namespace Gemier.GridSpace
{
    public class GridGemController : MonoBehaviour
    {
        private const float GROWTH_DURATION = 5f;
        private const float HARVESTABLE_WHEN = GROWTH_DURATION / 4; 
        private float growthTimer = 0f;
        private Gem currentGem = null;
        private GemManager gemManager;
        private cPool cpool;

        private void Start() 
        {
            gemManager = GemManager.Instance;

            cpool = cPool.instance;   

            SpawnGem();
        }

        private void Update() 
        {
            if(HasGem())
            {
                if(growthTimer < GROWTH_DURATION)
                {
                    growthTimer += Time.deltaTime;

                    currentGem.transform.localScale = Vector3.one * (growthTimer / GROWTH_DURATION); 
                }
            }
        }

        private void SpawnGem()
        {
            if(currentGem != null)
            {
                /// Already has a gem.
                return;
            }

            if(gemManager == null)
            {
                Debug.LogWarning("Gem Manager is null!");
            }

            growthTimer = 0f;
            
            string randomGemTag = gemManager.GetRandomGemTag();

            var newGem = cpool.GetPoolObject(randomGemTag, transform.position, Quaternion.identity);

            if(newGem.TryGetComponent(out Gem gem))
            {
                currentGem = gem;
            }
            else
            {
                Debug.LogError("Couldn't get component GEM", newGem.gameObject);
                return;
            }

            currentGem.SetGrid(this);

            currentGem.transform.localScale = Vector3.zero;

        }

        public void HarvestGem(out string GemTag)
        {
            var gemTag = currentGem.GetGemType.gemSO.gemName;

            currentGem = null;

            SpawnGem();

            GemTag = gemTag;
        }

        public bool HasGem() => currentGem != null;
        public bool IsGemHarvestable() => HasGem() && growthTimer >= HARVESTABLE_WHEN;
       
    }
}

