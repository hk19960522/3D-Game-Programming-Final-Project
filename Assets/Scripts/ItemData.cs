using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemData : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject m_InventoryManager;
    private Inventory m_Inventory;
    private Text m_Text = null;
    private Image m_Image = null;

    public int index { get; set; }
    public string itemHash { get; set; }
    public int quantity { get; set; }
    public bool allSet { get; set; }

	// Use this for initialization
	void Start () {
        m_Inventory = m_InventoryManager.GetComponent<Inventory>();
        itemHash = "NULL";
        m_Text = GetComponentInChildren<Text>();
        m_Image = GetComponentInChildren<Image>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetItemData(string hash, int quan, Sprite sprite)
    {
        itemHash = hash;
        quantity = quan;
        m_Image.sprite = sprite;
        m_Text.text = quan.ToString();
    }

    public void ResetItemData()
    {
        itemHash = "NULL";
        quantity = 0;
        if (m_Image == null) m_Image = GetComponent<Image>();
        if (m_Text == null) m_Text = GetComponent<Text>();
        m_Image.sprite = ResourceManager.Instance.GetSpriteByHash("NULL");
        m_Text.text = "";
    }

    public void ResetItemData(string type)
    {
        itemHash = "NULL";
        quantity = 0;
        if (m_Image == null) m_Image = GetComponent<Image>();
        if (m_Text == null) m_Text = GetComponent<Text>();
        m_Image.sprite = ResourceManager.Instance.GetSpriteByHash(type);
        m_Text.text = "";
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        m_Inventory.ItemClick(index, itemHash);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        m_Inventory.MoveInItem(itemHash);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        m_Inventory.MoveOutItem();
    }

}
