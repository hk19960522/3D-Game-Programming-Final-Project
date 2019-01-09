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

    float minPosX = 20, maxPosX = 480,
          minPosZ = 20, maxPosZ = 80;

    int[] scale = new int[] { 3, 5, 7, 10, 13 };
    int[] flower_scale = new int[] { 30, 50, 70 };
    Vector3 new_pos;
    int random_index, random_scale;
    float random_scale_f;
    GameObject newObj;
    Transform my_prey;

    // Use this for initialization
    void Start () {
        Init_tags();
        Create_forest();
        Create_prey();
	}

    void Create_prey()
    {
        random_index = Random.Range(0, prey.Length);
        new_pos = new Vector3(236.7f, 5.0f, 15.08f);
        if (random_index == 0 || random_index == 3 || random_index == 4 || random_index == 5)
        {
            new_pos.y += 2;
        }
        else if(random_index == 2)
        {
            new_pos.y += 1;
        }
        newObj = Instantiate(prey[random_index], new_pos, Quaternion.identity);
        newObj.transform.localScale = new Vector3(2, 2, 2);
        newObj.transform.localRotation *= Quaternion.Euler(0, 5, 0);
        newObj.transform.parent = preys.transform;
        if(random_index == 1 || random_index == 3 || random_index == 4 || random_index == 5)
        {
            // rotate 180 degree
            newObj.transform.localRotation *= Quaternion.Euler(0, 180, 0);
        }
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
                random_scale = scale[Random.Range(0, scale.Length)];
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
            new_pos = Get_new_pos();
            newObj = Instantiate(grass[random_index], new_pos, Quaternion.identity);
            newObj.transform.localScale = new Vector3(random_scale_f, random_scale_f, random_scale_f);
            newObj.transform.parent = forest.transform;

            if (i < 130) // flowers
            {
                random_index = Random.Range(0, flower.Length);
                int random_scale_index = Random.Range(0, flower_scale.Length);
                random_scale = flower_scale[random_scale_index];
                new_pos = Get_new_pos();
                newObj = Instantiate(flower[random_index], new_pos, Quaternion.identity);
                newObj.transform.localScale = new Vector3(random_scale, random_scale, random_scale);
                newObj.transform.parent = forest.transform;
            }

            if (i < 240)
            {
                // stone
                random_index = Random.Range(0, stone.Length);
                random_scale_f = Random.Range(1f, 2f);
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
        return pos;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetMouseButtonDown(0)) // left mouse click
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
