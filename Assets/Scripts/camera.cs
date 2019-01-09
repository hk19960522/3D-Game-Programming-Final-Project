using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class camera : MonoBehaviour {

    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    // float horizontalSpeed = 2.0f;
    // float verticalSpeed = 2.0f;

    public static Vector3 player_pos;
    public static GameObject player;
    public Button m_Setting, m_Back, m_Save, m_Home;

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        DontDestroyOnLoad(player);
        initialize_UI();
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

        // UI
        // pause panel show
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause_panel_show_hide(true);
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("levels"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("level");
        }
    }

    void initialize_UI()
    {
        // setactive false of pause panel
        pause_panel_show_hide(false);

        // pause panels' buttons pressed
        m_Setting.onClick.AddListener(click_setting);
        m_Back.onClick.AddListener(click_back);
        m_Save.onClick.AddListener(click_save);
        m_Home.onClick.AddListener(click_home);
    }

    void click_setting()
    {

    }

    void click_back()
    {
        pause_panel_show_hide(false);
    }

    void click_save()
    {

    }

    void click_home()
    {

    }

    void pause_panel_show_hide(bool show)
    {
        for (int i = 0; i < GameObject.Find("Pause_menu").transform.childCount; i++)
        {
            GameObject.Find("Pause_menu").transform.GetChild(i).gameObject.SetActive(show);
        }
    }
}