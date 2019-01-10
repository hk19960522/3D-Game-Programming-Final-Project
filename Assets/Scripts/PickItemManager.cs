using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickItemManager : MonoBehaviour {

    private static PickItemManager s_Instance;

    private int m_ItemCapacity = 6;
    [SerializeField]
    private Image[] m_ItemImages;
    [SerializeField]
    private string[] m_ItemTags;
    [SerializeField]
    private Image m_SelectItemImage;
    private int m_SelectItem;

    
    
    public RectTransform m_CanvasRect;
    public float padding = 10;

    public GameObject m_ImageRoot;
    public GameObject m_ImageObject;
    private RectTransform m_ImageObjectRect;

    private void Start()
    {
        m_ItemTags = new string[m_ItemCapacity];
        for (int i = 0; i < m_ItemCapacity; i++)
        {
            m_ItemTags[i] = "NULL";
        }
        InitImagePosition();
    }

    private void Update()
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
        if (m_ItemTags.Length == 0)
        {
            m_ItemTags = new string[m_ItemCapacity];
            for (int i = 0; i < m_ItemCapacity; i++)
            {
                m_ItemTags[i] = "NULL";
            }
        }
        return m_ItemTags[m_SelectItem];
    }

    private void SetSelectItemPosition()
    {
        m_SelectItemImage.gameObject.transform.position = m_ItemImages[m_SelectItem].gameObject.transform.position;
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
            GameObject obj = (GameObject)Instantiate(m_ImageObject);
            obj.transform.transform.SetParent(m_ImageRoot.transform);
            obj.GetComponent<RectTransform>().localPosition = imagePos;
            imagePos.x += offset;

            m_ItemImages[i] = obj.GetComponent<Image>();

            
            obj.SetActive(true);
        }
    }
}
