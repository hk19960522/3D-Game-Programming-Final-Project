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
    private int m_SelectItem;

    private void Start()
    {
        
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
                    GameObject pickItemManager = new GameObject("PickItemManager");
                    s_Instance = pickItemManager.AddComponent<PickItemManager>();
                }
            }

            return s_Instance;
        }
    }

    public string GetSelectItemHash()
    {
        return m_ItemTags[m_SelectItem];
    }

    private void SetSelectItemPosition()
    {
        m_SelectItemImage.gameObject.transform.position = m_ItemImages[m_SelectItem].gameObject.transform.position;
    }
}
