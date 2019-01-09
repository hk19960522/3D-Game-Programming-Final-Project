using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class create_trees : MonoBehaviour {

    public GameObject[] tree = new GameObject[16];
    public GameObject[] flower = new GameObject[6];
    public GameObject[] grass = new GameObject[10];
    public GameObject[] stone = new GameObject[7];
    public BoxCollider box_collider;
    public GameObject a_ball_for_marking;
    GameObject forest, level; // for tag
    
    float minPosX = 20, maxPosX = 480, 
          minPosZ = 20, maxPosZ = 480;

    float[] scale = new float[] {3, 5, 7, 10, 13};
    float[] flower_scale = new float[] {30, 50, 70 };
    Vector3 new_pos;
    int random_index;
    float random_scale;
    float random_scale_f;
    GameObject newObj, newBall;
    BoxCollider newCollider;
   
    // terrain is 500*500 for x and z
    // Use this for initialization
    void Start () {
        Init_tags();
        Create_forest();
        Create_level_key();
    }

    void Init_tags()
    {
        forest = GameObject.FindWithTag("Forest");
        level = GameObject.FindWithTag("Level");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Create_forest()
    {
        for (int i = 0; i < 3600; i++)
        {
            if (i < 1500) // trees
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

            if (i < 1600) // flowers
            {
                random_index = Random.Range(0, flower.Length);
                int random_scale_index = Random.Range(0, flower_scale.Length);
                random_scale = flower_scale[random_scale_index] * 0.5f;
                new_pos = Get_new_pos();
                newObj = Instantiate(flower[random_index], new_pos, Quaternion.identity);
                newObj.transform.localScale = new Vector3(random_scale, random_scale, random_scale);
                newObj.transform.parent = forest.transform;
            }

            if (i < 2400)
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
        
    void Create_level_key()
    {
        // enter the key then change the scene
        for (int i = 0; i < 40; i++)
        {
            Vector3 new_pos = Get_new_pos();
            new_pos.y = 0;
            newCollider = Instantiate(box_collider, new_pos, Quaternion.identity);
            newCollider.transform.parent = level.transform;
            newCollider.gameObject.tag = "levels";

            newBall = Instantiate(a_ball_for_marking, new_pos, Quaternion.identity);
            newBall.transform.parent = newCollider.transform;
        }
    }

    Vector3 Get_new_pos()
    {
        Vector3 pos = new Vector3(Random.Range(minPosX, maxPosX), 0, Random.Range(minPosZ, maxPosZ));
        while (200 < pos.x && pos.x < 300 && 200 < pos.z && pos.z < 300)
        { // if in the center range
            pos = new Vector3(Random.Range(minPosX, maxPosX), 0, Random.Range(minPosZ, maxPosZ));
        }
        pos.x -= 250;
        pos.z -= 250;
        return pos;
    }
}
