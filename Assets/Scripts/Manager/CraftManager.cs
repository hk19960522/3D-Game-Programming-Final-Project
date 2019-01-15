using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour {

    private static CraftManager s_Instance;

    public GameObject m_CraftRoot;
    public GameObject m_CraftRuleObject;

    private List<GameObject> m_CraftRules;

    private void Start()
    {
        m_CraftRules = new List<GameObject>();
    }

    private void Update()
    {
        
    }

    public static CraftManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(CraftManager)) as CraftManager;

                if (s_Instance == null)
                {
                    Debug.Log("HII");
                    GameObject pickItemManager = new GameObject("CraftManager");
                    s_Instance = pickItemManager.AddComponent<CraftManager>();
                }
            }

            return s_Instance;
        }
    }

    public void BuildCraftRules(List<CraftRuleInfo> rules)
    {
        foreach (CraftRuleInfo rule in rules)
        {
            GameObject obj = (GameObject)Instantiate(m_CraftRuleObject);
            obj.transform.parent = m_CraftRoot.transform;

            //obj.SetActive(true);
            obj.GetComponent<CraftRule>().SetRule(rule);
            m_CraftRules.Add(obj);

        }
        m_CraftRuleObject.SetActive(false);
    }

    public void UpdateCraftRules()
    {
        foreach (GameObject obj in m_CraftRules)
        {
            Debug.Log("HIIII");
            obj.GetComponent<CraftRule>().UpdateCraftableState();
        }
    }
}
