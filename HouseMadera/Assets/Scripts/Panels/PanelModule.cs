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
    List<modules> listModule = new List<modules>(); 
	void Start ()
    {
        SelectModules();
    }

    void SelectModules()
    {
        try
        {
            string conn = "URI=file:C:\\HouseMaderaDB-qslite\\HouseMaderaDB.db";
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
                        Debug.Log(item.name);
                    }
                    switch (objectToInstantiate.Count)
                    {
                        case 1:
                            for (int i = 0; i< objectToInstantiate.Count; i++)
                            {
                                if (objectToInstantiate[i].name.Equals("window"))
                                {
                                    objectToInstantiate[i] = Instantiate(Resources.Load("SlotWindow", typeof(GameObject))) as GameObject;
                                    objectToInstantiate[i].transform.position = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(3).transform.position;
                                    objectToInstantiate[i].transform.rotation = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(0).gameObject.transform.rotation;
                                    objectToInstantiate[i].tag = "Window";
                                }
                                else
                                {
                                    objectToInstantiate[i] = Instantiate(Resources.Load("SlotDoor", typeof(GameObject))) as GameObject;
                                    objectToInstantiate[i].transform.position = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(3).transform.position;
                                    objectToInstantiate[i].transform.rotation = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(0).gameObject.transform.rotation;
                                    objectToInstantiate[i].tag = "Door";
                                }
                                objectToInstantiate[i].gameObject.transform.SetParent(GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(2));
                                objectToInstantiate[i].gameObject.GetComponent<Renderer>().material.color = Color.red;
                            }
                            break;

                        case 2:
                            for (int i = 0; i < objectToInstantiate.Count; i++)
                            {
                                if (objectToInstantiate[i].name.Equals("window"))
                                {
                                    int index = i;
                                    if(index != 0) { index++; }
                                    objectToInstantiate[i] = Instantiate(Resources.Load("SlotWindow", typeof(GameObject))) as GameObject;
                                    objectToInstantiate[i].transform.position = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(2 + index).transform.position;
                                    objectToInstantiate[i].transform.rotation = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(0).gameObject.transform.rotation;
                                    objectToInstantiate[i].tag = "Window";
                                }
                                else
                                {
                                    int index = i;
                                    if (index != 0) { index++; }
                                    objectToInstantiate[i] = Instantiate(Resources.Load("SlotDoor", typeof(GameObject))) as GameObject;
                                    objectToInstantiate[i].transform.position = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(2 + index).transform.position;
                                    objectToInstantiate[i].transform.rotation = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(0).gameObject.transform.rotation;
                                    objectToInstantiate[i].tag = "Door";
                                }
                                
                                objectToInstantiate[i].gameObject.GetComponent<Renderer>().material.color = Color.red;
                                if(i == 0) { objectToInstantiate[i].gameObject.transform.SetParent(GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(2)); }
                                if (i == 1) { objectToInstantiate[i].gameObject.transform.SetParent(GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(3)); }
                            }
                            break;

                        case 3:
                            for (int i = 0; i < objectToInstantiate.Count; i++)
                            {
                                Debug.Log("Iteration :" + i);
                                if (objectToInstantiate[i].name.Equals("window"))
                                {
                                    objectToInstantiate[i] = Instantiate(Resources.Load("SlotWindow", typeof(GameObject))) as GameObject;
                                    objectToInstantiate[i].transform.position = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(2 + i).transform.position;
                                    objectToInstantiate[i].transform.rotation = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(0).gameObject.transform.rotation;
                                    objectToInstantiate[i].tag = "Window";
                                }
                                else
                                {
                                    objectToInstantiate[i] = Instantiate(Resources.Load("SlotDoor", typeof(GameObject))) as GameObject;
                                    objectToInstantiate[i].transform.position = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(2 + i).transform.position;
                                    objectToInstantiate[i].transform.rotation = GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(0).gameObject.transform.rotation;
                                    objectToInstantiate[i].tag = "Door";
                                }
                                if (i == 0) { objectToInstantiate[i].gameObject.transform.SetParent(GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(2)); }
                                if (i == 1) { objectToInstantiate[i].gameObject.transform.SetParent(GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(3)); }
                                if (i == 2) { objectToInstantiate[i].gameObject.transform.SetParent(GameObject.Find("Event").GetComponent<EditWall>().wallSelected.transform.GetChild(4)); }
                                objectToInstantiate[i].gameObject.GetComponent<Renderer>().material.color = Color.red;
                            }
                            break;
                    }
                });
            }
            //dbconn.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
