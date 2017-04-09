using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Data;
using Mono.Data.Sqlite;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class UIManager : MonoBehaviour {

    public GameObject panelTexture;
    public string[] args;
    public GameObject panelModule;
    public GameObject texturePosition;
    public GameObject modulePosition;
    public GameObject cloisonPosition;
    public GameObject optionCloison;
    public Text commercialName;
    public Text projectName;
    public List<GameObject> listCloison;
    public List<GameObject> listWall;
    public Text text;
    Vector3 textureInitialPosition;
    Vector3 modulePositionInitial;
    Vector3 optionCloisonPosition;
    Vector3 optionCloisonPositionInitial;
    public GameObject cloisonSelected;
    bool show = false;
    bool hide = true;
    public bool texture = false;
    public bool moduleGeneral = false;
    public bool parametreCloison = false;
	// Use this for initialization
	void Start ()
    {


        textureInitialPosition = panelTexture.transform.position;
        modulePositionInitial = panelModule.transform.position;
        optionCloisonPositionInitial = optionCloison.transform.position;
        try
        {
            string conn = "URI=file:" + Application.dataPath + "/HouseMaderaDB.db";
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open();
        }
        catch
        {
            Debug.LogError("Fail database");
        }
        

        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("Wall"))
        {
            listWall.Add(fooObj);
        }

        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("Cloison"))
        {
            listCloison.Add(fooObj);
        }

        InsideOutside();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Cursor.visible = true;
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag != "Wall" && !EventSystem.current.IsPointerOverGameObject())
                {
                    GameObject.Find("Event").GetComponent<EditWall>().wallSelected = null;
                    show = false;
                    hide = true;
                }
            }
        }

        if (texture)
        {
            parametreCloison = false;
            if (panelTexture.transform.position.x < texturePosition.transform.position.x)
            {
                panelTexture.transform.Translate(Vector3.right * Time.deltaTime * 300);
            }
        }
        else
        {
            if (panelTexture.transform.position.x > textureInitialPosition.x)
            {
                panelTexture.transform.Translate(Vector3.left * Time.deltaTime * 300);
            }
        }

        if (moduleGeneral)
        {
            parametreCloison = false;
            if (panelModule.transform.position.x > modulePosition.transform.position.x)
            {
                panelModule.transform.Translate(Vector3.left * Time.deltaTime * 300);
            }
        }
        else
        {
            if (panelModule.transform.position.x < modulePositionInitial.x)
            {
                panelModule.transform.Translate(Vector3.right * Time.deltaTime * 300);
            }
        }

        if (parametreCloison)
        {
            if(optionCloison.transform.position.y > cloisonPosition.transform.position.y && cloisonSelected != null)
            {
                optionCloison.transform.Translate(Vector3.down * Time.deltaTime * 300);
            }
        }
        else
        {
            if (optionCloison.transform.position.y < optionCloisonPositionInitial.y)
            {
                optionCloison.transform.Translate(Vector3.up * Time.deltaTime * 300);
            }
        }
    }

    public void InsideOutside()
    {
        if (GameObject.Find("Switch").GetComponent<Switch>().isOn)
        {
            foreach(GameObject item in listCloison)
            {
                item.GetComponent<CloisonManager>().canBeActivate = true;
            }

            foreach (GameObject item in listWall)
            {
                item.GetComponent<WallSelection>().canBeActivate = false;
            }
        }
        else
        {
            foreach (GameObject item in listCloison)
            {
                item.GetComponent<CloisonManager>().canBeActivate = false;
            }

            foreach (GameObject item in listWall)
            {
                item.GetComponent<WallSelection>().canBeActivate = true;
            }
        }
    }

    public void CancelCloison()
    {
        if (cloisonSelected.name.Contains("Vertical"))
        {
            cloisonSelected.SetActive(false);
            cloisonSelected.transform.parent.GetChild(0).GetComponent<CloisonManager>().verticalActive = false;
            cloisonSelected.transform.parent.GetChild(1).GetComponent<CloisonManager>().verticalActive = false;
        }

        if (cloisonSelected.name.Contains("Horizontal"))
        {
            cloisonSelected.SetActive(false);
            cloisonSelected.transform.parent.GetChild(1).GetComponent<CloisonManager>().horizontalActive = false;
            cloisonSelected.transform.parent.GetChild(0).GetComponent<CloisonManager>().horizontalActive = false;
        }
    }

    public void AddCloisonDoor()
    {
        if (cloisonSelected)
        {
            GameObject arch = Instantiate(Resources.Load("Arch", typeof(GameObject))) as GameObject;
            arch.transform.parent = cloisonSelected.transform.parent;
            arch.transform.position = cloisonSelected.transform.position;
            if (cloisonSelected.name.Contains("Vertical"))
            {
                arch.transform.rotation = Quaternion.Euler(0, 90, 0);
                arch.transform.GetComponent<Transform>().localScale = new Vector3(cloisonSelected.transform.GetComponent<Transform>().localScale.z, cloisonSelected.transform.GetComponent<Transform>().localScale.y, cloisonSelected.transform.GetComponent<Transform>().localScale.x);
                cloisonSelected.transform.parent.GetChild(0).GetComponent<CloisonManager>().horizontalActive = false;
                cloisonSelected.transform.parent.GetChild(1).GetComponent<CloisonManager>().horizontalActive = false;

                cloisonSelected.transform.parent.GetChild(0).GetComponent<CloisonManager>().verticalArch = true;
                cloisonSelected.transform.parent.GetChild(1).GetComponent<CloisonManager>().verticalArch = true;
            }
            else
            {
                arch.transform.rotation = Quaternion.Euler(0, 0, 0);
                arch.transform.GetComponent<Transform>().localScale = cloisonSelected.transform.GetComponent<Transform>().localScale;
                cloisonSelected.transform.parent.GetChild(0).GetComponent<CloisonManager>().verticalActive = false;
                cloisonSelected.transform.parent.GetChild(1).GetComponent<CloisonManager>().verticalActive = false;

                cloisonSelected.transform.parent.GetChild(0).GetComponent<CloisonManager>().horizontalArch = true;
                cloisonSelected.transform.parent.GetChild(1).GetComponent<CloisonManager>().horizontalArch = true;
            }


            cloisonSelected.transform.parent.GetChild(2).GetComponent<Renderer>().material.color = Color.white;
            cloisonSelected.transform.parent.GetChild(3).GetComponent<Renderer>().material.color = Color.white;
            cloisonSelected.transform.parent.GetChild(2).gameObject.SetActive(false);
            cloisonSelected.transform.parent.GetChild(3).gameObject.SetActive(false);
            cloisonSelected = null;
        }       
    }

    public void SaveHouse()
    {
        GameObject house = GameObject.Find("House");
        foreach (Transform target in house.transform)
        {
            if (target.name.Contains("Cloison"))
            {
                if (target.GetChild(0).GetComponent<CloisonManager>().verticalActive)
                {
                    Debug.Log(Regex.Match(target.name, @"\d+").Value + " Vertical"); 
                }
                if (target.GetChild(0).GetComponent<CloisonManager>().horizontalActive)
                {
                    Debug.Log(Regex.Match(target.name, @"\d+").Value + " Horizontal");
                }
            }
        }
    }

    public void ShowPanels()
    {
        hide = false;
        show = true;
    }
}
