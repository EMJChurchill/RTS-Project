using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyntheticNodeScript : MonoBehaviour
{
    public GameObject[] Modles;
    public int richness_of_vain;
    public bool is_under_use;

    private void Start()
    {
        richness_of_vain = Random.Range(1000, 4000);
    }

    private void Update()
    {
        if(richness_of_vain <= 0)
        {
            Modles[0].SetActive(false);
            Modles[1].SetActive(false);
            Modles[2].SetActive(true);
        }

        if(is_under_use == true)
        {
            Modles[0].SetActive(false);
            Modles[1].SetActive(true);
            Modles[2].SetActive(false);
        }
    }
}
