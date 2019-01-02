using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildScene : MonoBehaviour {

    private string m_ItemHash;
    private int m_RotateType = 0; // 0 for nothing, 1 for clockwise, 2 for opposite

	// Use this for initialization
	void Start () {
        m_ItemHash = "Block002";
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 hitPoint, newPoint;
        if (Input.GetKeyDown(KeyCode.Q)) m_RotateType = 2;
        if (Input.GetKeyDown(KeyCode.E)) m_RotateType = 1;

        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.Instance.PutItem();
        }
        if (RayCast(out hitPoint, out newPoint))
        {
            SceneManager.Instance.PutItemPreview(
                new Vector3(Mathf.Round(hitPoint.x), Mathf.Round(hitPoint.y), Mathf.Round(hitPoint.z)),
                new Vector3(Mathf.Round(newPoint.x), Mathf.Round(newPoint.y), Mathf.Round(newPoint.z)),
                PickItemManager.Instance.GetSelectItemHash(), m_RotateType);
            
            m_RotateType = 0;
        }
	}

    private bool RayCast(out Vector3 hitPoint, out Vector3 newPoint)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        hitPoint = newPoint = Vector3.zero;
        //
        if (Physics.Raycast(ray, out hit, 50, 1 << LayerMask.NameToLayer("SceneItem")))
        {
            //Debug.Log(hit.collider.gameObject.layer);
            Vector3 direction = ray.direction.normalized;
            hitPoint = hit.point + direction * 0.1f;
            newPoint = hit.point - direction * 0.1f;
            return true;
        }
        
        return false;
    }
}
