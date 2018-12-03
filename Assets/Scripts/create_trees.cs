using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class create_trees : MonoBehaviour {

    public GameObject[] tree = new GameObject[16];
    public GameObject[] flower = new GameObject[6];
    public GameObject[] grass = new GameObject[10];
    public GameObject[] stone = new GameObject[7];

    float minPosX = 20, maxPosX = 480, 
          minPosZ = 20, maxPosZ = 480;

    int[] scale = new int[] {3, 5, 7, 10, 13};
    int[] flower_scale = new int[] {30, 50, 70 };
    Vector3 new_pos;
    int random_index, random_scale;
    float random_scale_f;
    GameObject newObj;

    // terrain is 500*500 for x and z
    // Use this for initialization
    void Start () {
        for (int i = 0; i < 1800; i++)
        {
            if (i < 500) // trees
            {
                random_index = Random.Range(0, tree.Length);
                random_scale = scale[Random.Range(0, scale.Length)];
                new_pos = new Vector3(Random.Range(minPosX, maxPosX), 0, Random.Range(minPosZ, maxPosZ));
                newObj = Instantiate(tree[random_index], new_pos, Quaternion.identity);
                newObj.transform.localScale = new Vector3(random_scale, random_scale, random_scale);
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
            new_pos = new Vector3(Random.Range(minPosX, maxPosX), 0, Random.Range(minPosZ, maxPosZ));
            newObj = Instantiate(grass[random_index], new_pos, Quaternion.identity);
            newObj.transform.localScale = new Vector3(random_scale_f, random_scale_f, random_scale_f);

            if (i < 800) // flowers
            {
                random_index = Random.Range(0, flower.Length);
                int random_scale_index = Random.Range(0, flower_scale.Length);
                random_scale = flower_scale[random_scale_index];
                new_pos = new Vector3(Random.Range(minPosX, maxPosX), random_scale_index, Random.Range(minPosZ, maxPosZ));
                newObj = Instantiate(flower[random_index], new_pos, Quaternion.identity);
                newObj.transform.localScale = new Vector3(random_scale, random_scale, random_scale);
            }

            if (i < 1200)
            {
                // stone
                random_index = Random.Range(0, stone.Length);
                random_scale_f = Random.Range(1f, 2f);
                new_pos = new Vector3(Random.Range(minPosX, maxPosX), 0, Random.Range(minPosZ, maxPosZ));
                newObj = Instantiate(stone[random_index], new_pos, Quaternion.identity);
                newObj.transform.localScale = new Vector3(random_scale_f, random_scale_f, random_scale_f);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
