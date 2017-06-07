using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditSlot : MonoBehaviour {

    public List<GameObject> moduleList;
    GameObject UIManager;

    void Start ()
    {
        UIManager = GameObject.Find("UIManager");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Door")
                {
                    UIManager.GetComponent<UIManager>().texture = true;
                    ClearModule();
                    foreach (Transform target in hit.collider.transform.parent.transform.parent)
                    {
                        if (target.childCount > 0)
                        {
                            target.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.white;
                        }
                    }
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                    GameObject.Find("Event").GetComponent<EditWall>().moduleSelected = hit.collider.gameObject;
                    GameObject.Find("Event").GetComponent<EditWall>().wallSelected = null;


                }
                else if(hit.collider.gameObject.tag == "Window")
                {
                    UIManager.GetComponent<UIManager>().texture = true;
                    ClearModule();
                    foreach (Transform target in hit.collider.transform.parent.transform.parent.transform.parent)
                    {
                        if (target.childCount > 0 && target.gameObject.name.Contains("Wall"))
                        {
                            foreach(Transform target2 in target.transform)
                            {
                                if (target2.name.Contains("Slot") && target2.childCount > 0)
                                {
                                    target2.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.white;
                                }
                            }
                        }
                    }
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                    GameObject.Find("Event").GetComponent<EditWall>().moduleSelected = hit.collider.gameObject;
                    GameObject.Find("Event").GetComponent<EditWall>().wallSelected = null;

                }
            }
        }
    }

    void ClearModule()
    {
        GameObject house = GameObject.Find("House");
        foreach(Transform child in house.transform)
        {
            if (child.name.Contains("Wall"))
            {
                child.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
                child.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
                foreach (Transform mod in child)
                {
                    if (mod.name.Contains("clone"))
                    {
                        mod.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
        }
    }
}
