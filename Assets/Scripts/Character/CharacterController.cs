using UnityEngine;
using DG.Tweening;
using Gemier.Gems;
using Gemier.Managers;
using System.Collections.Generic;
using cpeak.cPool;

namespace Gemier.Character
{
    public class CharacterController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private FloatingJoystick floatingJoystick;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform bagTransform;
        [SerializeField] private cPool cpool;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private GameManager gameManager;

        [Header("Values")]
        [SerializeField] private float movementSpeed = 1f;
        [SerializeField] private float rotationDuration = .2f;
        private List<Gem> gemsInTheBagList = new ();
        private float sellGemDuration = .1f;
        private float sellGemTimer = 0f;
        private const string COLLECT_PARTICLE_TAG = "CollectParticle";

        private void Awake() 
        {
            sellGemTimer = sellGemDuration;    
        }

        private void Update() 
        {
            Vector3 movementDir = GetMovementDirection();

            Movement(movementDir);

            Rotation(movementDir);

            Animation(movementDir);
        }

        private void Movement(Vector3 movementDir)
        {
            transform.position += movementDir * movementSpeed * Time.deltaTime;
        }

        private void Rotation(Vector3 movementDir)
        {
            if(movementDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDir);

                transform.DORotateQuaternion(targetRotation, rotationDuration);
            }
        }

        private void Animation(Vector3 movementDir)
        {
            animator.SetBool("IsRunning", (movementDir == Vector3.zero) ? false : true);
        }

        private Vector3 GetMovementDirection()
        {
            var direction = (Vector3.forward * floatingJoystick.Vertical) + (Vector3.right * floatingJoystick.Horizontal);

            return direction;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.TryGetComponent(out Gem gem))
            {
                if(!gem.GetGrid.IsGemHarvestable() || gem.IsGemInTheBag)
                {
                    return;
                }
                if(gemsInTheBagList.Count == 0)
                {
                    gem.transform.position = bagTransform.position;
                }
                else
                {
                    float spaceBetweenGems = gemsInTheBagList[^1].transform.lossyScale.y - 0.1f;

                    gem.transform.position = gemsInTheBagList[^1].transform.position + (Vector3.up * spaceBetweenGems);
                }

                GameObject collectParticle = cpool.GetPoolObject(COLLECT_PARTICLE_TAG, transform.position, Quaternion.identity, true, 1f);

                if(collectParticle.TryGetComponent(out ParticleSystemRenderer particle))
                {
                    Material gemMaterial = gem.GetGemType.gemSO.material;
                    particle.sharedMaterial = gemMaterial;
                }

                gemsInTheBagList.Add(gem);

                gem.transform.SetParent(bagTransform);

                gem.GetInTheBag();

                gem.GetGrid.HarvestGem(out string gemTag);
            }   
            
        }

        private void OnTriggerStay(Collider other) 
        {
            if(other.CompareTag("SellArea") && gemsInTheBagList.Count > 0)
            {
                sellGemTimer += Time.deltaTime;
                if(sellGemTimer >= sellGemDuration)
                {
                    Gem gem = gemsInTheBagList[^1];

                    int gemSellAmount = (int)(gem.transform.localScale.x * 100);

                    string gemTag = gem.GetGemType.gemSO.gemName;

                    cpool.ReleaseObject(gemTag, gem.gameObject);

                    inventoryManager.AddItem(gem.GetGemType);

                    gemsInTheBagList.Remove(gem);

                    sellGemTimer = 0f;

                    gameManager.AddMoney(gemSellAmount);
                }
            }
        }
    }
}
