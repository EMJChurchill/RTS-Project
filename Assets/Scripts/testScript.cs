using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    public GameObject node;

    private void Update()
    {
        node = GameObject.FindGameObjectWithTag("SyntheticNode");
    }
}
