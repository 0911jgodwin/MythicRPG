using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    public bool hasTarget = false;
    Transform targetBase;
    GameObject circleIndicator;

    private void Start()
    {
        circleIndicator = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if(hasTarget)
        {
            this.transform.position = targetBase.position;
            this.transform.rotation = targetBase.rotation;
            this.transform.localScale = targetBase.localScale;
        }
        else
        {

        }
    }

    public void UpdateTarget(GameObject currentTarget)
    {
        if (currentTarget != null)
        {
            targetBase = currentTarget.transform.Find("Base");
            hasTarget = true;
        }
        else
        {
            hasTarget = false;
        }
        circleIndicator.SetActive(hasTarget);
    }
}
