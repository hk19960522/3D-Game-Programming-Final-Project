using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class my_camera : MonoBehaviour {

    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    // float horizontalSpeed = 2.0f;
    // float verticalSpeed = 2.0f;

    public static Vector3 player_pos;
    public static GameObject player;

    // Use this for initialization
    void Start () {
		player = GameObject.FindWithTag("Player");
        DontDestroyOnLoad(player);
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
        player.transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        player.transform.Rotate(0, rotation, 0);

        player_pos = player.transform.position; // + Camera.main.transform.position;
        Debug.Log("update: " + player_pos);
    }

    void OnCollisionEnter(Collision collider)
    {
        Debug.Log("player collide something");
        if (collider.gameObject.CompareTag("Level_sub"))
        {
            SceneManager.LoadScene("level");
        }        
    }
}
