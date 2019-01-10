using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public GameObject m_Inventory;
    private bool m_InventoryEnable;

    public GameObject m_SlotHolder;
    public int m_MaxSlot;
    private GameObject[] m_Slots;


    public GameObject m_ItemDescription;
    private bool m_DescriptionEnable;

    public GameObject m_PopingUI;
    private bool m_PopingUIEnable;
    private bool m_SwapState;
    private int m_ClickItemIndex;

	// Use this for initialization
	void Start () {
        m_InventoryEnable = false;
        m_DescriptionEnable = false;
        m_PopingUIEnable = false;
        m_SwapState = false;

        m_Slots = new GameObject[m_MaxSlot];
        for (int i = 0; i < m_MaxSlot; i++)
        {
            m_Slots[i] = m_SlotHolder.transform.GetChild(i).gameObject;
            m_Slots[i].GetComponent<ItemData>().index = i;
        }

        m_Inventory.SetActive(false);
        m_ItemDescription.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I))
        {
            m_InventoryEnable = !m_InventoryEnable;
            m_Inventory.SetActive(m_InventoryEnable);
            SceneManager.Instance.PlayerInventoryUpdate();
        }

        if (m_DescriptionEnable && !m_PopingUIEnable)
        {
            //Debug.Log(Input.mousePosition);
            m_ItemDescription.transform.position = Input.mousePosition;
        }

        // Test
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.Instance.PlayerInventoryUpdate("Bed001", 30);
        }
	}

    public void SetInventory(int index, string hash, int quantity)
    {
        if (index >= 0 && index < m_MaxSlot)
        {
            m_Slots[index].GetComponent<ItemData>().
                SetItemData(hash, quantity, ResourceManager.Instance.GetSpriteByHash(hash));
        }
    }

    public void RemoveInventory(int index)
    {
        if (index >= 0 && index < m_MaxSlot)
        {
            m_Slots[index].GetComponent<ItemData>().ResetItemData();
        }
    }

    public void ResetInventory()
    {
        for (int i = 0; i < m_MaxSlot; i++)
        {
            m_Slots[i].GetComponent<ItemData>().ResetItemData();
        }
    }

    public void ItemClick(int index, string hash)
    {
        if ((hash == "NULL" && !m_SwapState) || index < 0 || index >= m_MaxSlot)
        {
            if (m_PopingUI.activeInHierarchy)
            {
                m_PopingUIEnable = !m_PopingUIEnable;
                m_PopingUI.SetActive(m_PopingUIEnable);
            }
            return;
        }

        if (m_SwapState)
        {
            int index_1 = index;
            int index_2 = m_ClickItemIndex;

            string hash_1 = hash;
            string hash_2 = m_Slots[index_2].GetComponent<ItemData>().itemHash;

            SceneManager.Instance.PlayerInventorySwapPlace(hash_1, hash_2, index_1, index_2);
            m_SwapState = false;
            return;
        }

        m_PopingUIEnable = !m_PopingUIEnable;
        m_PopingUI.SetActive(m_PopingUIEnable);

        if (m_PopingUI.activeInHierarchy)
        {
            m_PopingUI.transform.position = Input.mousePosition;
            m_ClickItemIndex = index;
        }
    }

    public void ItemClickRemove()
    {
        //Debug.Log("REMOVE");
        SceneManager.Instance.PlayerInventoryUpdate(
            m_Slots[m_ClickItemIndex].GetComponent<ItemData>().itemHash,
            -m_Slots[m_ClickItemIndex].GetComponent<ItemData>().quantity);
        m_PopingUIEnable = !m_PopingUIEnable;
        m_PopingUI.SetActive(m_PopingUIEnable);
    }

    public void ItemClickSwap()
    {
        m_SwapState = true;
        m_PopingUIEnable = !m_PopingUIEnable;
        m_PopingUI.SetActive(m_PopingUIEnable);
    }

    public void PanelClick()
    {
        if (m_PopingUI.activeInHierarchy)
        {
            m_PopingUIEnable = !m_PopingUIEnable;
            m_PopingUI.SetActive(m_PopingUIEnable);
        }
        m_SwapState = false;
    }

    public void MoveInItem(string hash)
    {
        if (hash == "NULL" || m_PopingUIEnable)
        {
            return;
        }
        m_DescriptionEnable = true;
        m_ItemDescription.SetActive(true);
        m_ItemDescription.GetComponentInChildren<Text>().text =
            ResourceManager.Instance.GetDescriptionByHash(hash);
    }

    public void MoveOutItem()
    {
        m_DescriptionEnable = false;
        m_ItemDescription.SetActive(false);
    }
}
