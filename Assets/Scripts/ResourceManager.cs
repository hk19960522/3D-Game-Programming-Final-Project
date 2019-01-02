using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ResourceManager : MonoBehaviour {

    private static ResourceManager s_Instance;


    public GameObject item;

    [SerializeField]
    private BasicItemList m_BasicItemList;

    private Dictionary<string, string> m_PrefabPath;

    private GameObject m_ItemPoolRoot;
    private Dictionary<string, GameObject> m_ItemPool = null;

    private bool m_IsLoadComplete;

    private void Start()
    {
        m_PrefabPath = new Dictionary<string, string>();
        m_ItemPool = new Dictionary<string, GameObject>();

        m_ItemPoolRoot = new GameObject("Item Pool Root");
        m_IsLoadComplete = false;
        Load();
        m_IsLoadComplete = true;
    }

    public static ResourceManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(ResourceManager)) as ResourceManager;

                if (s_Instance == null)
                {
                    GameObject resourceManager = new GameObject("ResourceManager");
                    s_Instance = resourceManager.AddComponent<ResourceManager>();
                }
            }

            return s_Instance;
        }
    }

    public bool IsLoadComplete()
    {
        return m_IsLoadComplete;
    }

    private void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Jsons/BasicItemList");

        string data = textAsset.text;
        m_BasicItemList = new BasicItemList();
        m_BasicItemList = JsonUtility.FromJson<BasicItemList>(data);
        LoadSceneItem();
        //Save(m_BasicItemList);
    }

    private void LoadSceneItem()
    {
        try
        {
            foreach (SceneItemInfo itemInfo in m_BasicItemList.sceneItemInfos)
            {
                // Record prefab path

                GameObject obj = Instantiate(Resources.Load<GameObject>(itemInfo.prefabPath));

                SceneItem sceneItem = obj.GetComponent<SceneItem>();
                if (sceneItem == null)
                {
                    sceneItem = obj.AddComponent<SceneItem>();
                }
                
                sceneItem.Set(itemInfo);

                SetItemPool(obj, "Scene Item");
                m_ItemPool.Add(itemInfo.hashTag, obj);

                obj.SetActive(false);
            }
        }
        catch
        {

        }
    }

    /// <summary>
    /// Set the game object under the "ItemPool" in scene.
    /// It will let the hierarchy more clear.
    /// </summary>
    /// <param name="item">The object that want to put in the ItemPool.</param>
    /// <param name="type">Item type.</param>
    private void SetItemPool(GameObject item, string type)
    {
        if (m_ItemPoolRoot == null)
        {
            m_ItemPoolRoot = GameObject.Find("/Item Pool Root");
            if (m_ItemPoolRoot == null)
            {
                m_ItemPoolRoot = new GameObject("Item Pool Root");
                m_ItemPoolRoot.transform.position = Vector3.zero;
            }
        }

        GameObject root = GameObject.Find("/Item Pool Root/" + type);
        if (root == null)
        {
            root = new GameObject(type);
            root.transform.position = Vector3.zero;
            root.transform.parent = m_ItemPoolRoot.transform;
        }

        item.transform.parent = root.transform;
    }

    public void Save(BasicItemList basic)
    { 
        StreamWriter file = new StreamWriter("Assets/ResourseList.json");
        file.Write(JsonUtility.ToJson(basic, true));
        file.Close();
    }

    public GameObject GetItemByHash(string hash)
    {
        GameObject obj = null;
        
        if (!m_ItemPool.ContainsKey(hash))
        {
            if (!m_PrefabPath.ContainsKey(hash))
            {
                // Object is not exist
                //Debug.LogError("Error.");
                return obj;
            } 
        }

        // Object Exist
        obj = Instantiate(m_ItemPool[hash]);
        obj.SetActive(true);
        obj.layer = LayerMask.NameToLayer("PostProcessing");

        // Check Component
        if (obj.GetComponent<SceneItem>() != null)
        {
            obj.GetComponent<SceneItem>().Set(m_ItemPool[hash].GetComponent<SceneItem>());
            //Debug.Log(obj.name);
            //Debug.Log(obj.GetComponent<SceneItem>().GetItemSize());
        }

        return obj;
    }
}

[Serializable]
public class SceneItemInfo
{
    public string hashTag;
    public List<Vector3> offset;
    public string prefabPath;

    public SceneItemInfo()
    {
        offset = new List<Vector3>();
    }
}

[Serializable]
public class BasicItemList
{
    public List<SceneItemInfo> sceneItemInfos;

    public BasicItemList()
    {
        sceneItemInfos = new List<SceneItemInfo>();
    }
}