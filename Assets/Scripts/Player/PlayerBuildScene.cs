using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildScene : MonoBehaviour {

    private string m_ItemHash;

	// Use this for initialization
	void Start () {
        m_ItemHash = "Bed001";
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 hitPoint, newPoint;
        if (RayCast(out hitPoint, out newPoint))
        {

        }
	}

    private bool RayCast(out Vector3 hitPoint, out Vector3 newPoint)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        hitPoint = newPoint = Vector3.zero;
        if (Physics.Raycast(ray, out hit, 50, LayerMask.NameToLayer("SceneItem")))
        {
            Vector3 direction = ray.direction.normalized;
            hitPoint = hit.point + direction * 0.1f;
            newPoint = hit.point - direction * 0.1f;
            return true;
        }

        return false;
    }
}
