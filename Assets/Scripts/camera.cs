using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class camera : MonoBehaviour {

    public float speed = 10.0f;
    public float rotationSpeed = 2.0f;
    private float rotation_v = 0.0f;
    private float rotation_h = 0.0f;

    float min_x = -250, max_x = 250,
        min_z = -250, max_z = 250;

    public static Vector3 player_pos;
    public static GameObject player;
    public Button m_Setting, m_Back, m_Save, m_Home;

    private Animator animator;
    private GameObject my_weapon;
    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        DontDestroyOnLoad(player);
        initialize_UI();
        my_weapon = GameObject.Find("Weapon").transform.GetChild(0).gameObject;
        animator = my_weapon.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        float translation_WS = Input.GetAxis("Vertical") * speed,
            transition_AD = Input.GetAxis("Horizontal") * speed;

        rotation_h += Input.GetAxis("Mouse X") * rotationSpeed;
        rotation_v += Input.GetAxis("Mouse Y") * rotationSpeed;

        translation_WS *= Time.deltaTime;
        transition_AD *= Time.deltaTime;
        // rotation_h *= Time.deltaTime;
        // rotation_v *= Time.deltaTime;

        player.transform.Translate(transition_AD, 0, translation_WS);
        Vector3 p_newPos = player.transform.position;
        if (!(min_x <= p_newPos.x && p_newPos.x <= max_x && min_z <= p_newPos.z && p_newPos.z <= max_z))
        {
            player.transform.Translate(-transition_AD, 0, -translation_WS);
        }
        // keep y 
        Vector3 pos = player.transform.position;
        pos.y = 10.0f;
        player.transform.position = pos;

        // Rotate 
        if (rotation_v > 90) rotation_v = 90;
        if (rotation_v < -90) rotation_v = -90;
        player.transform.eulerAngles = new Vector3(-rotation_v, rotation_h, 0.0f);

        // UI
        // pause panel show
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause_panel_show_hide(true);
        }

        if (if_player_in_forest())
        {
            my_weapon.SetActive(true);
            /*
            // animation for weapon            
            if (animator)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

                if (Input.GetMouseButtonDown(0)) // left
                {
                    animator.SetTrigger("left");
                }

                if (Input.GetMouseButtonDown(1)) // right
                {
                    animator.SetTrigger("right");                 
                }
            }*/            
        }
        else
        {
            my_weapon.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("levels"))
        {
            Destroy(collider.gameObject);
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

    bool if_player_in_forest()
    {
        Vector3 pos = player.transform.position;        
        if (-50 < pos.x && pos.x < 50 && -50 < pos.z && pos.z < 50)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}