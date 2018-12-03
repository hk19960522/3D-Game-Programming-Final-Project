using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SceneManager : MonoBehaviour {

    private static SceneManager s_Instance;

    [SerializeField]
    private GameObject m_PreviewItem;

    private Dictionary<int, GameObject> m_SceneItem = null;

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
class SceneInfo
{
    public List<SceneItemPlacement> sceneItems;

    public SceneInfo()
    {
        sceneItems = new List<SceneItemPlacement>();
    }
}