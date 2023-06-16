using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Gemier.Gems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gemier.Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set;}
        private GameManager gameManager;
        private Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();
        [SerializeField] private RectTransform listParentTransform;
        [SerializeField] private GameObject listObjectPrefab;
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

            var inventorySave = LoadInventory();
            itemDictionary = (inventorySave != null) ? inventorySave : new Dictionary<string, Item>();
        }

        private void Start() 
        {
            gameManager = GameManager.Instance;

            gameManager.OnPause += SaveInventory;   

            gameManager.OnQuit += SaveInventory; 
        } 

        public void AddItem(GemType gemType)
        {
            Item itemCursor = null;

            if(itemDictionary.ContainsKey(gemType.gemSO.gemName))
            {
                itemCursor = itemDictionary[gemType.gemSO.gemName];
            }
            else
            {
                SerializableSprite icon = new SerializableSprite(gemType.gemSO.gemIcon);

                var item = new Item(0, icon);

                itemDictionary.Add(gemType.gemSO.gemName, item);

                itemCursor = itemDictionary[gemType.gemSO.gemName];
            }
            
            if(itemCursor == null)
            {
                Debug.Log("Item Cursor is null! " + itemDictionary.Count);
            }
            itemCursor.collectedAmount++;
        }

        public Dictionary<string, Item> GetAllItems()
        {
            return itemDictionary;
        }

        public void EnableUIList()
        {
            foreach (Transform item in listParentTransform)
            {
                Destroy(item.gameObject);
            }

            foreach (var item in itemDictionary)
            {
                var listObject = Instantiate(listObjectPrefab, Vector3.zero, Quaternion.identity, listParentTransform);
                listObject.GetComponentInChildren<TextMeshProUGUI>().text = "Gem Type : " + item.Key + "<br> Collected Amount : " + item.Value.collectedAmount;
                Sprite icon = item.Value.icon.ToSprite();
                listObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite = icon;
            }
        }

        public void SaveInventory()
        {
            try
            {
                string filePath = Path.Combine(Application.persistentDataPath, "save.json");

                BinaryFormatter formatter = new BinaryFormatter();

                FileStream stream = new FileStream(filePath, FileMode.Create);

                formatter.Serialize(stream, itemDictionary);

                stream.Close();

                Debug.Log($"<color=green> Progress Saved. </color>");
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            
        }

        public Dictionary<string, Item> LoadInventory()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "save.json");
            if(!File.Exists(filePath))
            {
                return null;
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                FileStream stream = new FileStream(filePath, FileMode.Open);

                Dictionary<string, Item> itemDic = (Dictionary<string, Item>)formatter.Deserialize(stream);

                stream.Close();
                
                Debug.Log($"<color=green> Progress Loaded. </color>");

                return itemDic;
            }
            catch (System.Exception e)
            {
                Debug.Log($"<color=red> Progress Not Loaded. Error message : </color>" + e);
                throw;
            }
        }

        public void DeleteSave()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "save.json");
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"<color=yellow> Save deleted. </color>");
            }
            else
            {
                Debug.Log($"<color=yellow> No file do delete </color>");
            }
        }
    }

    [System.Serializable]
    public class Item
    {
        public int collectedAmount;
        public SerializableSprite icon;

        public Item (int collectedAmount, SerializableSprite icon)
        {
            this.collectedAmount = collectedAmount;
            this.icon = icon;
        }
    }

    [System.Serializable]
    public class SerializableSprite
    {
        public SerializableTexture2D textureData;
        
        public SerializableSprite(Sprite sprite)
        {
            Texture2D texture = sprite.texture;
            textureData = new SerializableTexture2D(texture);
        }
        
        public Sprite ToSprite()
        {
            Texture2D texture = textureData.ToTexture2D();
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }

    [System.Serializable]
    public class SerializableTexture2D
    {
        public byte[] textureData;
        
        public SerializableTexture2D(Texture2D texture)
        {
            textureData = texture.EncodeToPNG();
        }
        
        public Texture2D ToTexture2D()
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(textureData);
            return texture;
        }
    }
}

