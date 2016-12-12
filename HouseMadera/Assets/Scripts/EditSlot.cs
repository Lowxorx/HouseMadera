using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditSlot : MonoBehaviour {

    public List<GameObject> moduleList;
	void Start () {
	
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
                    foreach (Transform target in hit.collider.transform.parent.transform.parent)
                    {
                        if (target.childCount > 0)
                        {
                            target.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                    GameObject.Find("Event").GetComponent<EditWall>().moduleSelected = hit.collider.gameObject;
                    //GameObject door = hit.transform.gameObject;
                    //door = Instantiate(Resources.Load("Door", typeof(GameObject))) as GameObject;
                    //door.transform.position = hit.transform.position;
                    //door.transform.rotation = hit.transform.rotation;
                    //Destroy(hit.transform.gameObject);
                }
                else if(hit.collider.gameObject.tag == "Window")
                {
                    foreach (Transform target in hit.collider.transform.parent.transform.parent.transform.parent)
                    {
                        if (target.childCount > 0 && target.gameObject.name.Contains("Wall"))
                        {
                            foreach(Transform target2 in target.transform)
                            {
                                if (target2.name.Contains("Slot") && target2.childCount > 0)
                                {
                                    target2.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
                                }
                            }
                        }
                    }
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                    GameObject.Find("Event").GetComponent<EditWall>().moduleSelected = hit.collider.gameObject;
                    //GameObject door = hit.transform.gameObject;
                    //door = Instantiate(Resources.Load("Door", typeof(GameObject))) as GameObject;
                    //door.transform.position = hit.transform.position;
                    //door.transform.rotation = hit.transform.rotation;
                    //Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
