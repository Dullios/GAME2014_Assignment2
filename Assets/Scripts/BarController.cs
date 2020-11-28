using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BarController : MonoBehaviour
{
    public Transform bar;
    public Transform entity;
    public int currentValue;
    public int maxValue;

    // Start is called before the first frame update
    void Start()
    {
        currentValue = 100;
        maxValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(entity != null)
        {
            transform.position = entity.position + new Vector3(0, 0.8f, 0);
        }
    }

    public void SetValue(int new_value)
    {
        currentValue = new_value;
        bar.localScale = new Vector3((float)((double)currentValue / (double)maxValue), 1.0f, 1.0f);

        // Clamp the X scale to 0 minimum
        if(bar.localScale.x < 0)
            bar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
    }
}
