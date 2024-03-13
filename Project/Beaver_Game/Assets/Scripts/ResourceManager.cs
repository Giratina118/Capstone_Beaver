using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public TMP_Text[] resourceCountTexts;
    public int[] resourceCountInts = new int[3] {0, 0, 0};


    public void StorageResourceCountChange(int resource1Variation, int resource2Variation, int resource3Variation)
    {
        if (resourceCountInts[0] + resource1Variation >= 0 && resourceCountInts[1] + resource2Variation >= 0 && resourceCountInts[2] + resource3Variation >= 0)
        {
            resourceCountInts[0] += resource1Variation;
            resourceCountInts[1] += resource2Variation;
            resourceCountInts[2] += resource3Variation;

            for (int i = 0; i < 3; i++)
                resourceCountTexts[i].text = resourceCountInts[i].ToString();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
