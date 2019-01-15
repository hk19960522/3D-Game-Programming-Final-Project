using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickItemManager : MonoBehaviour {

    private static PickItemManager s_Instance;

    private int m_ItemCapacity = 5;
    [SerializeField]
    private Image[] m_ItemImages;
    [SerializeField]
    private string[] m_ItemTags;
    [SerializeField]
    private Image m_SelectItemImage;
    [SerializeField]
    private GameObject[] m_SelectSlot;
    private int m_SelectItem;

    
    
    public RectTransform m_CanvasRect;
    public float padding = 10;

    public GameObject m_ImageRoot;
    public GameObject m_ImageObject;
    public GameObject m_SelectObject;
    private RectTransform m_ImageObjectRect;

    private void Start()
    {
        m_SelectSlot = new GameObject[m_ItemCapacity];
        m_ItemTags = new string[m_ItemCapacity];
        for (int i = 0; i < m_ItemCapacity; i++)
        {
            m_ItemTags[i] = "NULL";
        }
        InitImagePosition();

        m_SelectObject.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.Instance.gameMode == SceneManager.GameMode.BUILD_MODE)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                m_SelectItem++;
                m_SelectItem %= m_ItemCapacity;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                m_SelectItem--;
                m_SelectItem += m_ItemCapacity;
                m_SelectItem %= m_ItemCapacity;
            }
            SetSelectItemPosition();
        } 
    }

    public static PickItemManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(PickItemManager)) as PickItemManager;

                if (s_Instance == null)
                {
                    Debug.Log("HII");
                    GameObject pickItemManager = new GameObject("PickItemManager");
                    s_Instance = pickItemManager.AddComponent<PickItemManager>();
                }
            }

            return s_Instance;
        }
    }

    public string GetSelectItemHash()
    {
        //if (m_ItemTags.Length == 0)
        //{
        //    m_ItemTags = new string[m_ItemCapacity];
        //    for (int i = 0; i < m_ItemCapacity; i++)
        //    {
        //        m_ItemTags[i] = "NULL";
        //    }
        //}
        if (m_SelectSlot.Length <= m_SelectItem || m_SelectItem < 0)
        {
            return "NULL";
        }
        return m_SelectSlot[m_SelectItem].GetComponent<ItemData>().itemHash;
    }

    private void SetSelectItemPosition()
    {
        m_SelectItemImage.gameObject.transform.position = m_SelectSlot[m_SelectItem].gameObject.transform.position;
    }

    private void InitImagePosition()
    {
        m_ImageObjectRect = m_ImageObject.GetComponent<RectTransform>();
        m_ItemImages = new Image[m_ItemCapacity];
        Vector3 imagePos =
            new Vector3(0, (m_ImageObjectRect.rect.height - m_CanvasRect.rect.height) * 0.5f, 0);

        float offset = m_ImageObjectRect.rect.width + padding;
        float offsetTime = 0;
        offsetTime = m_ItemCapacity * 0.5f - 0.5f;

        imagePos.x -= offset * offsetTime;

        for (int i = 0; i < m_ItemCapacity; i++)
        {
            //GameObject obj = (GameObject)Instantiate(m_ImageObject);
            //obj.transform.transform.SetParent(m_ImageRoot.transform);
            //obj.GetComponent<RectTransform>().localPosition = imagePos;
            //imagePos.x += offset;

            //m_ItemImages[i] = obj.GetComponent<Image>();


            //obj.SetActive(true);
            m_SelectSlot[i] = (GameObject)Instantiate(m_SelectObject);
            m_SelectSlot[i].transform.SetParent(m_ImageRoot.transform);
            m_SelectSlot[i].GetComponent<RectTransform>().localPosition = imagePos;
            imagePos.x += offset;

            //m_SelectSlot[i].SetActive(true);
        }
    }

    public void UpdateSelectSlot(int index, string hash, int quantity)
    {
        if (index >= 0 && index < m_ItemCapacity)
        {
            m_SelectSlot[index].GetComponent<ItemData>().
                SetItemData(hash, quantity, ResourceManager.Instance.GetSpriteByHash(hash));
        }
    }

    public void RemoveSelectSlot(int index)
    {
        if (index >= 0 && index < m_ItemCapacity)
        {
            m_SelectSlot[index].GetComponent<ItemData>().ResetItemData("UIMask");
        }
    }
}
