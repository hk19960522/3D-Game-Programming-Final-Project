using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class create_level_scene : MonoBehaviour {

    public GameObject[] tree = new GameObject[16];
    public GameObject[] flower = new GameObject[6];
    public GameObject[] grass = new GameObject[10];
    public GameObject[] stone = new GameObject[7];
    public GameObject[] prey = new GameObject[6];
    GameObject forest, preys;
    Camera mLevel_Camera;

    float minPosX = 20, maxPosX = 480,
          minPosZ = 20, maxPosZ = 80;

    float[] scale = new float[] { 3, 5, 7, 10, 13 };
    float[] flower_scale = new float[] { 30, 50, 70 };
    Vector3 new_pos;
    int random_index;
    float random_scale;
    float random_scale_f;
    GameObject newObj;
    Transform my_prey;

    // Use this for initialization
    void Start () {
        Init_tags();
        Create_forest();
        Create_prey();
        mLevel_Camera = GameObject.FindGameObjectWithTag("Level_camera").GetComponent<Camera>() as Camera;
    }

    void Create_prey()
    {
        random_index = Random.Range(0, prey.Length);
        Debug.Log(random_index);
        new_pos = new Vector3(236.7f, 5.0f, 15.08f);
        new_pos.x += 1003;
        new_pos.z += 1004;
        newObj = Instantiate(prey[random_index], new_pos, Quaternion.identity);
        float prey_scale = 500.0f;
        switch (random_index)
        {
            case 0: // Bee
                break;
            case 1: // Rabbit
                prey_scale = 50000;
                break;
            case 2: // Bat
                prey_scale = 50000;
                break;
            case 3: // monkey
                break;
            case 4: // mushroom
                break;
            case 5: // tiger
                prey_scale = 2500;
                break;
            default:
                break;
        }
        newObj.transform.localScale = new Vector3(prey_scale, prey_scale, prey_scale);
        newObj.transform.localRotation *= Quaternion.Euler(0, 5, 0);
        newObj.transform.parent = preys.transform;
        newObj.transform.localRotation *= Quaternion.Euler(0, 180, 0);
        my_prey = newObj.transform;
        BoxCollider prey_collider = newObj.AddComponent<BoxCollider>();
        prey_collider.transform.localScale = new Vector3(3, 3, 3);
    }

    void Init_tags()
    {
        forest = GameObject.FindWithTag("Forest");
        preys = GameObject.FindWithTag("Prey");
    }
    
    void Create_forest()
    {
        for (int i = 0; i < 350; i++)
        {
            if (i < 100) // trees
            {
                random_index = Random.Range(0, tree.Length);
                random_scale = scale[Random.Range(0, scale.Length)] * 0.5f;
                new_pos = Get_new_pos();
                newObj = Instantiate(tree[random_index], new_pos, Quaternion.identity);
                newObj.transform.localScale = new Vector3(random_scale, random_scale, random_scale);
                newObj.transform.parent = forest.transform;
            }


            // grass
            random_index = Random.Range(0, grass.Length);
            if (random_index == 1 || random_index == 9)
            {
                random_scale_f = Random.Range(0, 0.1f);
            }
            else
            {
                random_scale_f = Random.Range(0, 0.3f);
            }
            random_scale_f *= 0.5f;
            new_pos = Get_new_pos();
            newObj = Instantiate(grass[random_index], new_pos, Quaternion.identity);
            newObj.transform.localScale = new Vector3(random_scale_f, random_scale_f, random_scale_f);
            newObj.transform.parent = forest.transform;

            if (i < 130) // flowers
            {
                random_index = Random.Range(0, flower.Length);
                int random_scale_index = Random.Range(0, flower_scale.Length);
                random_scale = flower_scale[random_scale_index] * 0.5f;
                new_pos = Get_new_pos();
                newObj = Instantiate(flower[random_index], new_pos, Quaternion.identity);
                newObj.transform.localScale = new Vector3(random_scale, random_scale, random_scale);
                newObj.transform.parent = forest.transform;
            }

            if (i < 240)
            {
                // stone
                random_index = Random.Range(0, stone.Length);
                random_scale_f = Random.Range(1f, 2f) * 0.5f;
                new_pos = Get_new_pos();
                newObj = Instantiate(stone[random_index], new_pos, Quaternion.identity);
                newObj.transform.localScale = new Vector3(random_scale_f, random_scale_f, random_scale_f);
                newObj.transform.parent = forest.transform;
            }
        }
    }


    Vector3 Get_new_pos()
    {
        Vector3 pos = new Vector3(Random.Range(minPosX, maxPosX), 0, Random.Range(minPosZ, maxPosZ));
        while (200 < pos.x && pos.x < 300 && 200 < pos.z && pos.z < 300)
        { // if in the center range
            pos = new Vector3(Random.Range(minPosX, maxPosX), 0, Random.Range(minPosZ, maxPosZ));
        }
        pos.x += 1003;
        pos.z += 1004;
        return pos;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetMouseButtonDown(0)) // left mouse click
        {
            RaycastHit hit;
            Ray ray = mLevel_Camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if(objectHit == my_prey) // if hit the prey
                {
                    Debug.Log("to Main");
                    UnityEngine.SceneManagement.SceneManager.LoadScene("main");
                }
                
            }
        }
	}
}
