using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CloisonManager : MonoBehaviour
{

    GameObject cloisonVertical;
    GameObject cloisonHorizontal;
    GameObject _switch;
    public bool canBeActivate = true;
    public bool collisionDetected = false;
    public bool verticalActive = false;
    public bool horizontalActive = false;
    public GameObject target;
    public List<Collider> colliderList = new List<Collider>();
    void Start()
    {
        _switch = GameObject.Find("Switch");
        cloisonVertical = this.transform.parent.GetChild(2).gameObject;
        cloisonHorizontal = this.transform.parent.GetChild(3).gameObject;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (verticalActive)
        {
            cloisonVertical.SetActive(false);
        }
        else if (horizontalActive)
        {
            cloisonHorizontal.SetActive(false);
        }
        collisionDetected = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (this.target != null)
            {
                foreach (GameObject cloison in GameObject.Find("UIManager").GetComponent<UIManager>().listCloison)
                {
                    cloison.transform.parent.GetChild(2).GetComponent<Renderer>().material.color = Color.white;
                    cloison.transform.parent.GetChild(3).GetComponent<Renderer>().material.color = Color.white;
                }

                    target.GetComponent<Renderer>().material.color = Color.red;
                    GameObject.Find("UIManager").GetComponent<UIManager>().cloisonSelected = target;
                    GameObject.Find("UIManager").GetComponent<UIManager>().parametreCloison = true;
            }
            if (cloisonVertical.activeInHierarchy)
            {
                verticalActive = true;
                target = this.transform.parent.GetChild(2).gameObject;
                
            }

            if (cloisonHorizontal.activeInHierarchy)
            {
                horizontalActive = true;
                target = this.transform.parent.GetChild(3).gameObject;
                
            }

            
        }
    }

    void OnMouseEnter()
    {
        if (canBeActivate && !collisionDetected)
        {
            if (this.name.Contains("Vertical"))
            {
                if (!verticalActive)
                {
                    cloisonVertical.SetActive(true);
                }
                else
                {
                    Debug.Log("ENTER");
                    target = this.transform.parent.GetChild(2).gameObject;
                }
                //this.transform.parent.GetChild(2).gameObject.SetActive(true);
            }
            else if (this.name.Contains("Horizontal"))
            {
                if (!horizontalActive)
                {
                    cloisonHorizontal.SetActive(true);
                }
                else
                {
                    Debug.Log("ENTER");
                    target = this.transform.parent.GetChild(3).gameObject;
                }
            }
        }
    }

    void ClearAll()
    {
        foreach (GameObject cloison in GameObject.Find("UIManager").GetComponent<UIManager>().listCloison)
        {
            cloison.GetComponent<CloisonManager>().target = null;
        }
        foreach (GameObject cloison in GameObject.Find("UIManager").GetComponent<UIManager>().listCloison)
        {
            cloison.transform.parent.GetChild(2).GetComponent<Renderer>().material.color = Color.white;
            cloison.transform.parent.GetChild(3).GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void OnMouseExit()
    {
        if (canBeActivate)
        {
            foreach (GameObject cloison in GameObject.Find("UIManager").GetComponent<UIManager>().listCloison)
            {
                cloison.GetComponent<CloisonManager>().target = null;
            }
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
}