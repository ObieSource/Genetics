using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Genetics g = new Genetics();

    // Start is called before the first frame update
    void Start()        
    {
        g.setup();
    }

    // Update is called once per frame
    void Update()
    {
        g.draw();
    }
}
