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

public class PanelModule : MonoBehaviour {

    public GameObject slot;
    public Sprite fenêtre;
    public SimpleSQLManager dbManager;
    List<modules> listModule = new List<modules>(); 
	void Start ()
    {
        SelectModules();
    }

    void SelectModules()
    {
        modules dbModule = new modules();
        listModule = new List<modules>(from modules in dbManager.Table<modules>() select modules);
        try
        {
            string conn = "URI=file:" + Application.dataPath + "/HouseMaderaDB.db";
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open();
            string sqlQuery = "SELECT Id, Nom  FROM modules";
            IDbCommand dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                GameObject module = new GameObject();
                module = Instantiate(Resources.Load("module", typeof(GameObject))) as GameObject;
                module.transform.SetParent(slot.transform);
                module.name = reader.GetString(1);
                module.transform.localScale = new Vector3(1, 1, 1);
                module.GetComponent<Image>().sprite = fenêtre;
                //module.transform.position = slot.transform.position;
                Debug.Log("id : "+reader.GetInt32(0) + " Slot : " + reader.GetString(1));
                module.GetComponent<Button>().onClick.AddListener(delegate 
                {
                    string sqlSlotPlaces = "SELECT * FROM slotplaces WHERE Id = " + reader.GetInt32(0).ToString();
                    IDbCommand dbcmdSlotPlaces = dbconn.CreateCommand();
                    dbcmdSlotPlaces.CommandText = sqlSlotPlaces;
                    IDataReader readerSlotPlaces = dbcmdSlotPlaces.ExecuteReader();
                    while (reader.Read())
                    {
                        
                    }
                    //GameObject door = Instantiate(Resources.Load("SlotDoor", typeof(GameObject))) as GameObject;
                });
            }
            dbconn.Close();
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
