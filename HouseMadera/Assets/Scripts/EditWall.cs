using UnityEngine;

using System.Collections.Generic;


public class EditWall : MonoBehaviour {

    List<GameObject> wallList = new List<GameObject> ();
    public Texture texture1;
    public Texture texture2;
    public Texture texture3;
    private bool addElementSelected = false;
    public GameObject wallSelected;
    void Start ()
    {
        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("Wall"))
        {
            wallList.Add(fooObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.tag == "Wall")
                {
                   for (int i = 0; i< wallList.Count; i++)
                    {
                        wallList[i].transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
                        wallList[i].transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
                    }
                    hit.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
                    hit.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.blue;
                    wallSelected = hit.transform.gameObject;
                    if (addElementSelected)
                    {
                        AddElement();
                    }
                    
                }
            }
        }
    }

    public void ChangeTexture1()
    {
        wallSelected.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = texture1;
    }

    public void ChangeTexture2()
    {
        wallSelected.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = texture2;
    }

    public void ChangeTexture3()
    {
        wallSelected.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = texture3;
    }

    public void AddElementSelected()
    {
        addElementSelected = true;
    }
    public void AddElement()
    {
        GameObject door = Instantiate(Resources.Load("SlotDoor", typeof(GameObject))) as GameObject;
        float width = wallSelected.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale.z / 3;
        Debug.Log("WIDTH : " + width);
        door.transform.position = new Vector3(wallSelected.transform.position.x, wallSelected.transform.position.y, wallSelected.transform.position.z);
        door.transform.rotation = wallSelected.transform.GetChild(0).gameObject.transform.rotation;
        door.transform.parent = wallSelected.transform;
        door.tag = "Module";
        door.gameObject.GetComponent<Renderer>().material.color = Color.red;
        this.gameObject.GetComponent<EditSlot>().moduleList.Add(door);
        addElementSelected = false;
    }
}
