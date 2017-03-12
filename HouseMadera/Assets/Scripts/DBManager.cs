using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public List<GameObject> listCloison = new List<GameObject>();
    public List<GameObject> listModule = new List<GameObject>();
    int idDoor;
    int idWindow;
    int idCloison;

    void Start()
    {

    }

    public void FillInformations()
    {
        GetTypeModuleId();
        FillListCloison();
        FillListModule();
    }

    public void FillCloisonDatabase()
    {
        for (int i = 0; i < 99; i++)
        {
            string conn = "URI=file:C:\\HouseMaderaDB-sqlite\\HouseMaderaDB.db";
            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open();
            string sqlQuery = "INSERT INTO slotplace (Order, Module_Id, Slot_Id) VALUES ( " + i + ", 13, 10)";
            Debug.Log(sqlQuery);
            IDbCommand dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();
        }
    }

    public void GetTypeModuleId()
    {
        string conn = "URI=file:C:\\HouseMaderaDB-sqlite\\HouseMaderaDB.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        string sqlQuery = "SELECT Id, Nom FROM typemodule";
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            switch (reader.GetString(1))
            {
                case "Cloison":
                    idCloison = reader.GetInt32(0);
                    break;
                case "Fenetre":
                    idWindow = reader.GetInt32(0);
                    break;
                case "Porte":
                    idDoor = reader.GetInt32(0);
                    break;
            }
        }
        dbconn.Close();
    }

    public void CheckInformations()
    {
        CheckIfCloisonIsAlreadySaved();
        CheckIfModuleIsAlreadySaved();
    }

    void CheckIfCloisonIsAlreadySaved()
    {
        string conn = "URI=file:C:\\HouseMaderaDB-sqlite\\HouseMaderaDB.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        string sqlQuery = "SELECT Libelle FROM moduleplace WHERE Module_Id = " + idCloison;
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            foreach(GameObject targetCloison in listCloison)
            {
                string indexCloison = Regex.Match(targetCloison.name, @"\d+").Value;
            }
        }
        dbconn.Close();
    }

    void CheckIfModuleIsAlreadySaved()
    {

    }
    void FillListCloison()
    {
        GameObject house = GameObject.Find("House");
        foreach (Transform target in house.transform)
        {
            if (target.name.Contains("Cloison"))
            {
                if (target.GetChild(0).GetComponent<CloisonManager>().verticalActive || target.GetChild(0).GetComponent<CloisonManager>().horizontalActive)
                {
                    listCloison.Add(target.gameObject);
                }
            }
        }
    }

    void FillListModule()
    {
        GameObject house = GameObject.Find("House");
        foreach (Transform target in house.transform)
        {
            if (target.name.Contains("Wall"))
            {
                foreach (Transform targetSlot in target)
                {
                    if (targetSlot.name.Contains("Slot"))
                    {
                        if(targetSlot.transform.childCount > 0)
                        {
                            listModule.Add(targetSlot.GetChild(0).gameObject);
                        }
                    }
                }
            }
        }
    }

    void SaveCloisonToDatabase()
    {

    }

    

    void SaveModuleToDatabase()
    {

    }

   

    public void SendToDatabase()
    {
        string conn = "URI=file:C:\\HouseMaderaDB-sqlite\\HouseMaderaDB.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();

        GameObject house = GameObject.Find("House");
        foreach (Transform target in house.transform)
        {
            Cloisons newCloison = new Cloisons();
            if (target.name.Contains("Cloison"))
            {
                if (target.GetChild(0).GetComponent<CloisonManager>().verticalActive)
                {
                    newCloison.name = Regex.Match(target.name, @"\d+").Value;
                    newCloison.vertical = 1;
                    Debug.Log(Regex.Match(target.name, @"\d+").Value + " Vertical");
                }
                if (target.GetChild(0).GetComponent<CloisonManager>().horizontalActive)
                {
                    newCloison.name = Regex.Match(target.name, @"\d+").Value;
                    newCloison.horizontal = 1;
                    Debug.Log(Regex.Match(target.name, @"\d+").Value + " Horizontal");
                }
            }
            if (newCloison.name != "")
            {
                string command = "";//INSERT INTO ModulePlace (id_slot_place, id_module, libelle, H, V, produit_id) VALUES('L\'ID DU SLOT SELECTIONNE', 'L\'ID CORRESPONDANT AU MODULE DE TYPE CLOISON', "LIBELLE COMME TU VEUX (EX : cloison10)", "SI IL EST HORIZONTAL : 1 SINON 0", "LIBELLE COMME TU VEUX (EX : cloison10)", "SI IL EST VERTICAL : 1 SINON 0", L'ID DU PRODUIT SUR LEQUEL ON BOSSE);
                Debug.Log(command);
                IDbCommand dbcmd = dbconn.CreateCommand();
                dbcmd.CommandText = command;
                dbcmd.ExecuteNonQuery();
            }

        }
        //GameObject test = GameObject.Find("House");
        //foreach (Transform target in test.transform)
        //{
        //    if (target.name.Contains("Cloison"))
        //    {
        //        string command = "INSERT INTO Toto (tata)VALUES(" + Regex.Match(target.name, @"\d+").Value + ")"; 
        //        IDbCommand dbcmd = dbconn.CreateCommand();
        //        dbcmd.CommandText = command;
        //        dbcmd.ExecuteNonQuery();
        //    }
        //}
    }
}
