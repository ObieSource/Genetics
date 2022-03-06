using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject critterPrefab;
    public GameObject waterPrefab;
    public GameObject foodPrefab;

    // Start is called before the first frame update
    void Start()        
    {
        Instantiate(critterPrefab, new Vector3(3, 1.5f, 3), Quaternion.identity);
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
}
