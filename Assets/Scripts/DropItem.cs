using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour {

    public float rotateSpeed = 50.0f;
    public float moveSpeed = 2.0f;

    private string hash;
    private bool beDestory = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime, Space.Self);
	}

    public void SetItem(string itemHash)
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.position += new Vector3(0, 3, 0);

        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 3.0f;

        hash = itemHash;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !beDestory)
        {
            transform.Translate((other.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime, Space.World);
            if ((transform.position - other.transform.position).sqrMagnitude <= 0.01f)
            {
                SceneManager.Instance.PlayerInventoryUpdate(hash, 1);
                beDestory = true;
                Destroy(gameObject);
            }
        }
    }
}
