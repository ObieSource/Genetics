using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject critterPrefab;

    // Start is called before the first frame update
    void Start()        
    {
        Instantiate(critterPrefab, new Vector3(3, 1.5f, 3), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
