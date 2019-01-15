using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftRule : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject m_InventoryManager;
    public GameObject m_BlockPanel;
    private Inventory m_Inventory;
    private Image m_Image;

    private List<CraftItem> m_RawItem;
    private List<CraftItem> m_ResultItem;


    // Use this for initialization
    void Start () {
        m_Inventory = m_InventoryManager.GetComponent<Inventory>();
        m_Image = GetComponent<Image>();

        if (m_RawItem == null)
        {
            m_RawItem = new List<CraftItem>();
        }
        if (m_ResultItem == null)
        {
            m_ResultItem = new List<CraftItem>();
        }
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetRule(CraftRuleInfo info)
    {
        m_RawItem = new List<CraftItem>(info.rawItems);
        //Debug.Log("Raw num: " + m_RawItem.Count);
        m_ResultItem = new List<CraftItem>(info.resultItems);
        if (m_Image == null) m_Image = GetComponent<Image>();
        m_Image.sprite = Resources.Load<Sprite>(info.imagePath);

        UpdateCraftableState();
    }

    public void UpdateCraftableState()
    {
        bool state = true;
        foreach (CraftItem item in m_RawItem)
        {
            Debug.Log(item.hash + " " + SceneManager.Instance.GetItemQuantity(item.hash));
            if (SceneManager.Instance.GetItemQuantity(item.hash) < item.quantity) 
            {
                state = false;
                break;
            }
        }
        
        m_BlockPanel.SetActive(!state);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        UpdateCraftableState();
        if (!m_BlockPanel.activeInHierarchy)
        {
            Debug.Log("Can Craft");

            foreach (CraftItem item in m_RawItem)
            {
                SceneManager.Instance.PlayerInventoryUpdate
                    (item.hash, -item.quantity);
            }
            foreach (CraftItem item in m_ResultItem)
            {
                SceneManager.Instance.PlayerInventoryUpdate
                    (item.hash, item.quantity);
            }
            UpdateCraftableState();
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {

    }
}
