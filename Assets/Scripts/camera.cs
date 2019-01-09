using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class camera : MonoBehaviour {

    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    // float horizontalSpeed = 2.0f;
    // float verticalSpeed = 2.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move "speed" meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("my P");
            for (int i = 0; i < GameObject.Find("PanelWithButtons").transform.childCount; i++)
            {
                // GameObject.Find("PanelWithButtons").transform.getChild(i).gameObject.SetActive(true);
            }
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        Debug.Log("hit");
        if (collider.gameObject.CompareTag("levels"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("level");
        }
    }
}
