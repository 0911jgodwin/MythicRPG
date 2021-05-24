using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Targeting : MonoBehaviour
{
    public List<GameObject> targets;
    public GameObject currentTarget;
    public Transform player;
    public SelectionIndicator selectionIndicator;
    public Combat playerCombat;
    public Camera mainCamera;
    public LayerMask IgnoreMask;
    public Slider targetSlider;
    Health targetHealth;

    public void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SelectNextTarget();
        }
        if(Input.GetMouseButtonDown(0))
        {
            RaycastSelect();
        }
        if (currentTarget != null)
        {
            targetSlider.gameObject.SetActive(true);
            targetHealth = currentTarget.GetComponent<Health>();
            targetSlider.value = (targetHealth.currentHealth / targetHealth.maxHealth) * 100;
        }
        else 
        {
            targetSlider.gameObject.SetActive(false);
        }
    }

    private void RaycastSelect()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 1000f, ~IgnoreMask))
        {
            GameObject hitObject = hitInfo.transform.gameObject;
            if (hitObject.CompareTag("Enemy"))
            {
                SelectTarget(hitObject);
                UpdateListByRange();
            }
            else
            {
                ClearTarget();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            targets.Add(other.gameObject);
            UpdateListByRange();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            RemoveTargetFromList(other.gameObject);
        }
    }

    public void RemoveTargetFromList(GameObject removeTarget)
    {
        if (GameObject.ReferenceEquals(removeTarget, currentTarget))
        {
            SelectNextTarget();
        }

        if (targets.Contains(removeTarget))
        {
            targets.Remove(removeTarget);
            UpdateListByRange();
        }

        if (GameObject.ReferenceEquals(removeTarget, currentTarget))
        {
            ClearTarget();
        }
    }

    private void SelectTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
        selectionIndicator.UpdateTarget(currentTarget);
        playerCombat.UpdateTarget(currentTarget);
    }

    private void ClearTarget()
    {
        currentTarget = null;
        selectionIndicator.UpdateTarget(null);
        playerCombat.UpdateTarget(null);
    }

    private void SelectNextTarget()
    {
        int targetIndex = targets.IndexOf(currentTarget);
        if (targetIndex != -1)
        {
            if(targetIndex + 1 < targets.Count())
            {
                SelectTarget(targets[targetIndex + 1]);
            }
            else
            {
                SelectTarget(targets[0]);
            }
            
        }
        else if(targets.Count > 0)
        {
            UpdateListByRange();
            SelectTarget(targets[0]);
        }
        else
        {
            ClearTarget();
        }
        
    }

    private void UpdateListByRange()
    {
        //Doing a bubble sort kind of thing to order targets by range, sort resets to 0 each time it moves a target so near targets bubble to top.
        for (int i = 0; i < targets.Count - 1; i++)
        {
            float sqrMag1 = (targets[i].transform.position - player.position).sqrMagnitude;
            float sqrMag2 = (targets[i+1].transform.position - player.position).sqrMagnitude;

            if (sqrMag1 > sqrMag2)
            {
                GameObject temporary = targets[i];
                targets[i] = targets[i + 1];
                targets[i + 1] = temporary;
                i = 0;
            }
        }
    }
}
