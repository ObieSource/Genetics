using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject critterPrefab;
    public GameObject waterPrefab;
    public GameObject foodPrefab;
    public float minX, maxX, minZ, maxZ;
    public int foodInitAmount;
    public int waterInitAmount;
    public int initialPopulationSize;

    // Start is called before the first frame update
    void Start()        
    {
        //Instantiate(critterPrefab, new Vector3(3, 1.5f, 3), Quaternion.identity);
        for (int i = 0; i < initialPopulationSize; i++)
        {
            Instantiate(critterPrefab, RandomPosition(1.5f), Quaternion.identity);
        }

        for (int i = 0; i < waterInitAmount; i++)
        {
            Instantiate(waterPrefab, RandomPosition(1.5f), Quaternion.identity);
        }

        for (int i = 0; i < foodInitAmount; i++)
        {
            Instantiate(foodPrefab, RandomPosition(1.5f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Kill(GameObject critter)
    {
        Vector3 pos = critter.transform.position;
        Destroy(critter);
        Instantiate(foodPrefab, pos, Quaternion.identity);
    }

    private Vector3 RandomPosition(float y)
    {
        return new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
    }
}
