 using UnityEngine;

using System.Collections.Generic;


public class EditWall : MonoBehaviour {

    List<GameObject> wallList = new List<GameObject> ();
    public List<GameObject> doorList = new List<GameObject>();
    public Texture texture1;
    public Texture texture2;
    public Texture texture3;
    private bool addElementSelected = false;
    public GameObject wallSelected;
    public GameObject moduleSelected;
    GameObject UIManager;
    void Start ()
    {
        UIManager = GameObject.Find("UIManager");
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
                    if (hit.collider.gameObject.GetComponent<WallSelection>().wallPlaced)
                    {
                        UIManager.GetComponent<UIManager>().texture = true;
                        UIManager.GetComponent<UIManager>().moduleGeneral = true;

                        for (int i = 0; i < wallList.Count; i++)
                        {
                            wallList[i].transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
                            wallList[i].transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
                        }
                        hit.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
                        hit.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.blue;
                        wallSelected = hit.transform.gameObject;
                        if (addElementSelected)
                        {
                            //AddElement();
                        }
                    }
                   
                }
                else if (hit.collider.gameObject.tag == "Door" || hit.collider.gameObject.tag == "Window")
                {

                        UIManager.GetComponent<UIManager>().texture = true;
                        UIManager.GetComponent<UIManager>().moduleGeneral = true;

                        for (int i = 0; i < wallList.Count; i++)
                        {
                            wallList[i].transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
                            wallList[i].transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
                        }
                        hit.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
                        hit.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.blue;
                        wallSelected = hit.transform.gameObject;
                        if (addElementSelected)
                        {
                            //AddElement();
                        }
                    
                }
                else
                {
                    UIManager.GetComponent<UIManager>().texture = false;
                    UIManager.GetComponent<UIManager>().moduleGeneral = false;
                    foreach (var item in wallList)
                    {
                        item.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
                        item.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
                    }

                    foreach (var item in doorList)
                    {
                        item.transform.GetComponent<Renderer>().material.color = Color.red;
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

    public void TextureLuxe()
    {
        if (GameObject.Find("Event").GetComponent<EditWall>().moduleSelected != null)
        {
            if (GameObject.Find("Event").GetComponent<EditWall>().moduleSelected.name.Contains("Window"))
            {
                GameObject.Find("Event").GetComponent<EditWall>().moduleSelected.gameObject.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().FenetreLuxe;
            }
            else if (GameObject.Find("Event").GetComponent<EditWall>().moduleSelected.name.Contains("Door"))
            {
                GameObject.Find("Event").GetComponent<EditWall>().moduleSelected.gameObject.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().PorteLuxe;
            }
            
        }
    }
    
    public void TextureLowcost()
    {
        if (GameObject.Find("Event").GetComponent<EditWall>().moduleSelected != null)
        {
            if (GameObject.Find("Event").GetComponent<EditWall>().moduleSelected.name.Contains("Window"))
            {
                GameObject.Find("Event").GetComponent<EditWall>().moduleSelected.gameObject.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().FenetreLowcost;
            }
            else if (GameObject.Find("Event").GetComponent<EditWall>().moduleSelected.name.Contains("Door"))
            {
                GameObject.Find("Event").GetComponent<EditWall>().moduleSelected.gameObject.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().PorteLowcost;
            }

        }
    }

    public void AddElementSelected()
    {
        if(wallSelected != null)
        {
            AddElement();
        }
    }

    public void AddElement()
    {
        foreach(var item in wallSelected.GetComponent<WallSelection>().listModule)
        {
            doorList.Remove(item.gameObject);
            GameObject.Find("Event").GetComponent<EditSlot>().moduleList.Remove(item.gameObject);
            Destroy(item.gameObject);
        }
        GameObject door = Instantiate(Resources.Load("SlotDoor", typeof(GameObject))) as GameObject;
        doorList.Add(door);
        wallSelected.GetComponent<WallSelection>().listModule.Add(door);
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
