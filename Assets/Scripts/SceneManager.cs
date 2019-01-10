using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SceneManager : MonoBehaviour {

    private static SceneManager s_Instance;

    [SerializeField]
    private GameObject m_PreviewItem;

    [SerializeField]
    private Dictionary<int, GameObject> m_SceneItem = null;

    public Inventory m_Inventory;
    [SerializeField]
    private Dictionary<string, PlayerInventoryInfo> m_PlayerInventory = null;

    private bool m_IsLoadComplete;

    public static int MAX_X, MAX_Y, MAX_Z;
    public static int MIN_X, MIN_Y, MIN_Z;
    public static int HASH_UNIT;
    public static int HASH_OFFSET;

    private void Start()
    {
        MAX_X = MAX_Y = MAX_Z = 25;
        MIN_X = MIN_Y = MIN_Z = -25;
        HASH_UNIT = 1000;
        HASH_OFFSET = 100;
        m_IsLoadComplete = false;

        m_SceneItem = new Dictionary<int, GameObject>();
        m_PlayerInventory = new Dictionary<string, PlayerInventoryInfo>();

        //Save();
    }

    private void Update()
    {
        if (!m_IsLoadComplete && ResourceManager.Instance.IsLoadComplete())
        {
            Load("Map001");
            m_IsLoadComplete = true;
        }
        
    }

    public static SceneManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(SceneManager)) as SceneManager;

                if (s_Instance == null)
                {
                    GameObject resourceManager = new GameObject("ResourceManager");
                    s_Instance = resourceManager.AddComponent<SceneManager>();
                }
            }

            return s_Instance;
        }
    }

    public bool IsPositionLegal(Vector3 pos)
    {
        return IsPositionLegal(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z));
    }

    public bool IsPositionLegal(int x, int y, int z)
    {
        return 
            x >= MIN_X && x <= MAX_X && 
            y >= MIN_Y && y <= MAX_Y && 
            z >= MIN_Z && z <= MAX_Z;
    }

    public int GetPositionHash(Vector3 pos)
    {
        return GetPositionHash(
            Mathf.RoundToInt(pos.x), 
            Mathf.RoundToInt(pos.y), 
            Mathf.RoundToInt(pos.z));
    }

    public int GetPositionHash(int x, int y, int z)
    {
        if (!IsPositionLegal(x, y, z))
        {
            Debug.LogError(new Vector3(x, y, z));
            Debug.LogError("Wrong Position");
            return 0;
        }

        return 
            (x + HASH_OFFSET) * HASH_UNIT * HASH_UNIT +
            (y + HASH_OFFSET) * HASH_UNIT +
            (z + HASH_OFFSET);
    }

    public bool IsLegal(Vector3 pos)
    {
        return IsLegal(GetPositionHash(pos));
    }

    public bool IsLegal(int pos)
    {
        return !m_SceneItem.ContainsKey(pos);
    }

    public bool AddPosition(Vector3 pos, GameObject obj)
    {
        return AddPosition(GetPositionHash(pos), obj);
    }

    public bool AddPosition(int pos, GameObject obj)
    {
        if (!IsLegal(pos)) return false;

        m_SceneItem.Add(pos, obj);
        return true;
    }

    public void PutItem()
    {
        if (m_PreviewItem != null)
        {
            // Set game object's component in right value
            SceneItem componment = m_PreviewItem.GetComponent<SceneItem>();

            // Set game object's layer
            m_PreviewItem.layer = LayerMask.NameToLayer("SceneItem");

            // Add position into Dictionary
            int size = componment.GetItemSize();
            for (int i = 0; i < size; i++)
            {
                Vector3 pos;
                if (componment.GetPosition(i, out pos))
                {
                    if (!AddPosition(pos, m_PreviewItem))
                    {
                        Debug.LogError("Same position.");
                    }
                }
                else
                {
                    Debug.LogError("Wrong index.");
                }
            }

            m_PreviewItem = null;
        }
    }

    public void PutItemPreview(Vector3 putPoint, Vector3 placePoint, string itemHash, int direction)
    {
        int hitPointHash = GetPositionHash(putPoint);
        int placePointHash = GetPositionHash(placePoint);

        if (m_PreviewItem != null && m_PreviewItem.GetComponent<SceneItem>().GetItemType() == itemHash)
        {
            Vector3 prePosition = Vector3.zero;
            SceneItem sceneItem = m_PreviewItem.GetComponent<SceneItem>();
            sceneItem.GetPosition(0, out prePosition);
            if (GetPositionHash(prePosition) == placePointHash)
            {
                // Check rotation
                SceneItem.Direction itemDirection = SceneItem.Direction.FRONT;
                if (direction == 0)
                {
                    // Do nothing
                    itemDirection = sceneItem.GetDirection();
                }
                else if (direction == 1)
                {
                    itemDirection = sceneItem.NextDirection(true);
                }
                else if (direction == 2)
                {
                    itemDirection = sceneItem.NextDirection(false);
                }
                else
                {
                    Debug.LogError("Wrong Direction.\n");
                }

                int itemSize = sceneItem.GetItemSize();
                bool isLegalPlace = true;
                //Debug.Log(m_PreviewItem.GetComponent<SceneItem>().GetItemSize());
                for (int i = 0; i < itemSize; i++)
                {
                    Vector3 pos = Vector3.zero;
                    if (sceneItem.GetPosition(i, out pos, itemDirection))
                    {
                        //Debug.Log(pos);
                        if (!IsLegal(GetPositionHash(pos)))
                        {
                            isLegalPlace = false;
                            break;
                        }
                    }
                }

                if (isLegalPlace)
                {
                    sceneItem.SetRotate(itemDirection);
                }
                else
                {
                    Destroy(m_PreviewItem);
                    m_PreviewItem = null;
                }
            }
            else
            {
                int itemSize = sceneItem.GetItemSize();
                bool isLegalPlace = true;
                sceneItem.SetPosition(placePoint);

                for (int i = 0; i < itemSize; i++)
                {
                    Vector3 pos = Vector3.zero;
                    if (sceneItem.GetPosition(i, out pos))
                    {
                        if (!IsLegal(GetPositionHash(pos)))
                        {
                            isLegalPlace = false;
                            break;
                        }
                    }
                }

                if (isLegalPlace)
                {
                    
                }
                else
                {
                    Destroy(m_PreviewItem);
                    m_PreviewItem = null;
                }
            }
        }
        else
        {
            if (m_PreviewItem != null)
            {
                Destroy(m_PreviewItem);
                m_PreviewItem = null;
            }
            if (IsLegal(placePointHash))
            {
                bool isLegalPlace = true;
                GameObject obj = ResourceManager.Instance.GetItemByHash(itemHash);
                if (obj == null)
                {
                    return;
                }
                //Debug.Log(obj.GetComponent<SceneItem>().GetItemSize());

                if (obj == null || obj.GetComponent<SceneItem>() == null)
                {
                    Debug.LogError("Wrong item hash.");
                }

                SceneItem sceneItem = obj.GetComponent<SceneItem>();
                sceneItem.SetPosition(placePoint);

                int itemSize = sceneItem.GetItemSize();
                for (int i = 0; i < itemSize; i++)
                {
                    Vector3 pos = Vector3.zero;
                    if (sceneItem.GetPosition(i, out pos))
                    {
                        if (!IsLegal(GetPositionHash(pos)))
                        {
                            isLegalPlace = false;
                            break;
                        }
                    }
                }

                if (isLegalPlace)
                {
                    m_PreviewItem = obj;
                    //Debug.Log(sceneItem.GetItemSize() + "    777777777777777");
                }
                else
                {
                    Destroy(obj);
                }
            }
        }
    }

    public void Save()
    {
        string filepath = Application.persistentDataPath + "/Maps/Map001.json";
        try
        {
            
        }
        catch
        {

        }
    }

    public void Load(string filename)
    {
        string filepath = Application.persistentDataPath + "/Maps/" + filename + ".json";
        try
        {
            string fileContext;
            if (!File.Exists(filepath))
            {
                // Load Scene Json from Resources folder
                TextAsset textAsset = Resources.Load<TextAsset>("Jsons/" + filename);
                fileContext = textAsset.text;
            }
            else
            {
                //Load Scene Json from persistenDataPath
                StreamReader reader = new StreamReader(filepath);
                fileContext = reader.ReadToEnd();
                reader.Close();
            }

            SceneInfo itemList = JsonUtility.FromJson<SceneInfo>(fileContext);
            //LoadBlockItem(itemList.blockItems);

            LoadSceneItem(itemList.sceneItems);
            LoadPlayerInventory(itemList.inventoryInfos);
        }
        catch
        {

        }
    }

    private void LoadSceneItem(List<SceneItemPlacement> placements)
    {
        foreach (SceneItemPlacement sceneItem in placements)
        {
            GameObject obj = ResourceManager.Instance.GetItemByHash(sceneItem.hash);

            if (obj == null)
            {
                Debug.LogError("Wrong hash.");
            }

            // Set game object's component in right value
            SceneItem componment = obj.GetComponent<SceneItem>();
            componment.Set(sceneItem);

            // Set game object's layer
            obj.layer = LayerMask.NameToLayer("SceneItem");

            // Add position into Dictionary
            int size = componment.GetItemSize();
            for (int i = 0; i < size; i++)
            {
                Vector3 pos;
                if (componment.GetPosition(i, out pos))
                {
                    if (!AddPosition(pos, obj))
                    {
                        Debug.LogError("Same position.");
                    }
                }
                else
                {
                    Debug.LogError("Wrong index.");
                }
            }
        }
    }

    private void LoadPlayerInventory(List<PlayerInventoryInfo> inventoryInfos)
    {
        foreach (PlayerInventoryInfo info in inventoryInfos)
        {
            m_PlayerInventory.Add(info.hash, info);
            m_Inventory.SetInventory(info.index, info.hash, info.quantity);
        }
    }

    public void PlayerInventoryUpdate()
    {
        m_Inventory.ResetInventory();
        foreach (PlayerInventoryInfo info in m_PlayerInventory.Values)
        {
            m_Inventory.SetInventory(info.index, info.hash, info.quantity);
        }
    }

    public void PlayerInventoryUpdate(string hash, int delta)
    {
        if (m_PlayerInventory.ContainsKey(hash))
        {
            m_PlayerInventory[hash].quantity += delta;
            if (m_PlayerInventory[hash].quantity <= 0)
            {
                m_Inventory.RemoveInventory(m_PlayerInventory[hash].index);
                m_PlayerInventory.Remove(hash);
            }
            //else
            //{
            //    m_Inventory.SetInventory(
            //        m_PlayerInventory[hash].index, 
            //        m_PlayerInventory[hash].hash, 
            //        m_PlayerInventory[hash].quantity);
            //}
        }
        else
        {
            if (delta > 0 && ResourceManager.Instance.CheckItemByHash(hash))
            {
                // Add new item
                bool[] record = new bool[100];
                foreach(PlayerInventoryInfo info in m_PlayerInventory.Values)
                {
                    record[info.index] = true;
                }

                int fittestIndex = -1;
                while (record[++fittestIndex]) ;

                PlayerInventoryInfo newInfo = new PlayerInventoryInfo();
                newInfo.hash = hash;
                newInfo.index = fittestIndex;
                newInfo.quantity = delta;

                m_PlayerInventory.Add(hash, newInfo);
                //m_Inventory.SetInventory(newInfo.index, newInfo.hash, newInfo.quantity);
            }
        }
        PlayerInventoryUpdate();
    }

    public void PlayerInventorySwapPlace(string hash_1, string hash_2, int index_1, int index_2)
    {
        if (m_PlayerInventory.ContainsKey(hash_1) && m_PlayerInventory.ContainsKey(hash_2))
        {
            m_PlayerInventory[hash_1].index = index_2;
            m_PlayerInventory[hash_2].index = index_1;
        }
        else if(m_PlayerInventory.ContainsKey(hash_1))
        {
            m_PlayerInventory[hash_1].index = index_2;
        }
        else if (m_PlayerInventory.ContainsKey(hash_2))
        {
            m_PlayerInventory[hash_2].index = index_1;
        }
        PlayerInventoryUpdate();
    }
}

[Serializable]
public class SceneItemPlacement
{
    public string hash;
    public Vector3 position;
    public SceneItem.Direction direction;
    public SceneItem.BreakState breakState;
}

[Serializable]
public class PlayerInventoryInfo
{
    public string hash;
    public int quantity;
    public int index;
}

[Serializable]
class SceneInfo
{
    public List<SceneItemPlacement> sceneItems;
    public List<PlayerInventoryInfo> inventoryInfos;

    public SceneInfo()
    {
        sceneItems = new List<SceneItemPlacement>();
        inventoryInfos = new List<PlayerInventoryInfo>();
    }
}