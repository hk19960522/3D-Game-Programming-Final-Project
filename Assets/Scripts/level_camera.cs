using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_camera : MonoBehaviour {
    private GameObject l_player;
    private Animator l_animator;
    private GameObject my_l_weapon;
    public float speed = 10.0f;
    public float rotationSpeed = 2.0f;

    float min_x = 1100, max_x = 1300,
        min_z = 1100, max_z = 1300;

    private float rotation_v = 0.0f;
    private float rotation_h = 0.0f;

    // Use this for initialization
    void Start () {
        l_player = GameObject.FindWithTag("Level_player");
        my_l_weapon = GameObject.Find("Level_Weapon").transform.GetChild(0).gameObject;
        my_l_weapon.SetActive(true);
        l_animator = my_l_weapon.GetComponent<Animator>();
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

        l_player.transform.Translate(transition_AD, 0, translation_WS);
        Vector3 p_newPos = l_player.transform.position;
        if (!(min_x <= p_newPos.x && p_newPos.x <= max_x && min_z <= p_newPos.z && p_newPos.z <= max_z)){
            l_player.transform.Translate(-transition_AD, 0, -translation_WS);
        }
        // keep y 
        Vector3 pos = l_player.transform.position;
        pos.y = 10.0f;
        l_player.transform.position = pos;

        // Rotate 
        if (rotation_v > 90) rotation_v = 90;
        if (rotation_v < -90) rotation_v = -90;
        //if (rotation_h > 90) rotation_h = 90;
        //if (rotation_h < -90) rotation_h = -90;
        l_player.transform.eulerAngles = new Vector3(-rotation_v, rotation_h, 0.0f);
        // Rotate(rotation_v, rotation_h, 0);
                
        // animation for weapon            
        if (l_animator)
        {
            AnimatorStateInfo stateInfo = l_animator.GetCurrentAnimatorStateInfo(0);

            if (Input.GetMouseButtonDown(0)) // left
            {
                l_animator.SetTrigger("left");
            }

            if (Input.GetMouseButtonDown(1)) // right
            {
                l_animator.SetTrigger("right");
            }
        }
    }
}
