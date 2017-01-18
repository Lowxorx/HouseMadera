using UnityEngine;
using System.Collections;

public class CloisonManager : MonoBehaviour
{

    GameObject cloisonVertical;
    GameObject cloisonHorizontal;
    bool verticalActive = false;
    bool horizontalActive = false;
    void Start()
    {
        cloisonVertical = this.transform.parent.GetChild(2).gameObject;
        cloisonHorizontal = this.transform.parent.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (cloisonVertical.active)
            {
                verticalActive = true;
            }

            if (cloisonHorizontal.active)
            {
                horizontalActive = true;
            }
        }
    }

    void OnMouseEnter()
    {
        if (this.name.Contains("Vertical"))
        {
            if (!verticalActive)
            {
                cloisonVertical.SetActive(true);
            }
            //this.transform.parent.GetChild(2).gameObject.SetActive(true);
        }
        else if (this.name.Contains("Horizontal"))
        {
            if (!horizontalActive)
            {
                cloisonHorizontal.SetActive(true);
            }
            //this.transform.parent.GetChild(3).gameObject.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (this.name.Contains("Vertical"))
        {
            if (!verticalActive)
            {
                cloisonVertical.SetActive(false);
            }
            //this.transform.parent.GetChild(2).gameObject.SetActive(true);
        }
        else if (this.name.Contains("Horizontal"))
        {
            if (!horizontalActive)
            {
                cloisonHorizontal.SetActive(false);
            }
            //this.transform.parent.GetChild(3).gameObject.SetActive(true);
        }
    }
}