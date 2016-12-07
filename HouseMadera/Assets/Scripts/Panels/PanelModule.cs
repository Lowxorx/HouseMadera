using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System;
using UnityEngine.UI;

public class PanelModule : MonoBehaviour {

    public GameObject slot;
    public Sprite fenêtre;
	void Start ()
    {
        SelectModules();
    }

    void SelectModules()
    {
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
