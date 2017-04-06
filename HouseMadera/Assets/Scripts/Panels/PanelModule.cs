using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System;
using SimpleSQL;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class PanelModule : MonoBehaviour {

    public GameObject slot;
    public Sprite fenêtre;
    public GameObject dataBase;
    int currentId;
    List<Module> listModule = new List<Module>(); 
	void Start ()
    {
        SelectModules();
    }

    void SelectModules()
    {
        try
        {
            string conn = "URI=file:C:\\Users\\Jacky-Marley\\AppData\\LocalLow\\MaderaCorp\\HouseMadera\\HouseMaderaDB.db";
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open();
            string sqlQuery = "SELECT Id, Nom, icone  FROM typemoduleplacable";
            IDbCommand dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                currentId = reader.GetInt32(0);
                Debug.Log(currentId);
                GameObject module = new GameObject();
                module = Instantiate(Resources.Load("module", typeof(GameObject))) as GameObject;
                module.transform.SetParent(slot.transform);
                module.name = reader.GetString(1);
                module.transform.localScale = new Vector3(1, 1, 1);
                byte[] img = (byte[])reader["icone"];
                Texture2D tex = new Texture2D(64, 64);
                tex.LoadImage(img);
                module.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, 500, 500), new Vector2(0.5f, 0.5f));
                //module.transform.position = slot.transform.position;
                Debug.Log("id : "+reader.GetInt32(0) + " Slot : " + reader.GetString(1));
                module.GetComponent<Button>().onClick.AddListener(delegate 
                {
                    int types = module.name.ToString().Split('-').Length - 1;
                    //string sqlSlotPlaces = "SELECT Slot_Id FROM slotplace WHERE Module_Id = " + EventSystem.current.currentSelectedGameObject.name;
                    //Debug.Log(sqlSlotPlaces);
                    //IDbCommand dbcmdSlotPlaces = dbconn.CreateCommand();
                    //dbcmdSlotPlaces.CommandText = sqlSlotPlaces;
                    //IDataReader readerSlotPlaces = dbcmdSlotPlaces.ExecuteReader();
                    List<GameObject> objectToInstantiate = new List<GameObject>();
                    //while (readerSlotPlaces.Read())
                    //{
                    //    if (readerSlotPlaces.GetInt32(0).Equals(1))
                    //    {
                    //        GameObject window = new GameObject();
                    //        window.name = "window";
                    //        objectToInstantiate.Add(window);
                    //    }
                    //    else if (readerSlotPlaces.GetInt32(0).Equals(2))
                    //    {
                    //        GameObject door = new GameObject();
                    //        door.name = "door";
                    //        objectToInstantiate.Add(door);
                    //    }
                    //}
                    switch (types)
                    {
                        case 0:
                            GameObject door = new GameObject();
                            door.name = "door";
                            objectToInstantiate.Add(door);
                            break;
                        case 1:
                            GameObject window = new GameObject();
                            window.name = "window";
                            objectToInstantiate.Add(window);

                            GameObject window2 = new GameObject();
                            window2.name = "window";
                            objectToInstantiate.Add(window2);
                            break;
                        case 2:
                            GameObject window3 = new GameObject();
                            window3.name = "window";
                            objectToInstantiate.Add(window3);

                            GameObject door2 = new GameObject();
                            door2.name = "door";
                            objectToInstantiate.Add(door2);

                            GameObject window4 = new GameObject();
                            window4.name = "window";
                            objectToInstantiate.Add(window4);
                            break;
                    }
                    float leftSideValue = 0 - ((GameObject.Find("Event").GetComponent<EditWall>().wallSelected.GetComponent<BoxCollider>().size.z) / 2);
                    float separator = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.GetComponent<BoxCollider>().size.z / objectToInstantiate.Count();
                    float actualPosition = 0.0f;
                    foreach (var item in objectToInstantiate)
                    {
                        
                    }
                    InstantiateModule(objectToInstantiate, GameObject.Find("Event").GetComponent<EditWall>().wallSelected);
                    
                });
            }
            //dbconn.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        
    }

    public void InstantiateModule(List<GameObject> numberModule, GameObject parentModule)
    {
        Debug.Log(numberModule.Count);
        switch (numberModule.Count)
        {
            case 1:
                GameObject door = Instantiate(Resources.Load("SlotDoor", typeof(GameObject))) as GameObject;
                door.transform.position = parentModule.transform.GetChild(3).transform.position;
                door.transform.rotation = parentModule.transform.GetChild(0).transform.rotation;
                door.GetComponent<Renderer>().material.color = Color.red;
                door.transform.SetParent(parentModule.transform.GetChild(3));
                door.tag = "Door";
                break;

            case 2:
                GameObject window = Instantiate(Resources.Load("SlotWindow", typeof(GameObject))) as GameObject;
                window.transform.position = parentModule.transform.GetChild(2).transform.position;
                window.transform.rotation = parentModule.transform.GetChild(0).transform.rotation;
                window.GetComponent<Renderer>().material.color = Color.red;
                window.transform.SetParent(parentModule.transform.GetChild(2));
                window.tag = "Window";

                GameObject window2 = Instantiate(Resources.Load("SlotWindow", typeof(GameObject))) as GameObject;
                window2.transform.position = parentModule.transform.GetChild(4).transform.position;
                window2.transform.rotation = parentModule.transform.GetChild(0).transform.rotation;
                window2.GetComponent<Renderer>().material.color = Color.red;
                window2.transform.SetParent(parentModule.transform.GetChild(4));
                window2.tag = "Window";
                break;

            case 3:
                GameObject window3 = Instantiate(Resources.Load("SlotWindow", typeof(GameObject))) as GameObject;
                window3.transform.position = parentModule.transform.GetChild(2).transform.position;
                window3.transform.rotation = parentModule.transform.GetChild(0).transform.rotation;
                window3.GetComponent<Renderer>().material.color = Color.red;
                window3.transform.SetParent(parentModule.transform.GetChild(2));
                window3.tag = "Window";

                GameObject door2 = Instantiate(Resources.Load("SlotDoor", typeof(GameObject))) as GameObject;
                door2.transform.position = parentModule.transform.GetChild(3).transform.position;
                door2.transform.rotation = parentModule.transform.GetChild(0).transform.rotation;
                door2.GetComponent<Renderer>().material.color = Color.red;
                door2.transform.SetParent(parentModule.transform.GetChild(3));
                door2.tag = "Door";

                GameObject window4 = Instantiate(Resources.Load("SlotWindow", typeof(GameObject))) as GameObject;
                window4.transform.position = parentModule.transform.GetChild(4).transform.position;
                window4.transform.rotation = parentModule.transform.GetChild(0).transform.rotation;
                window4.GetComponent<Renderer>().material.color = Color.red;
                window4.transform.SetParent(parentModule.transform.GetChild(4));
                window4.tag = "Window";
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
