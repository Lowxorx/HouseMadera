using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    void Start()
    {

    }

    void SaveCloisonToDatabase()
    {

    }

    void CheckIfCloisonIsAlreadySaved()
    {

    }

    void SaveModuleToDatabase()
    {

    }

    void CheckIfModuleIsAlreadySaved()
    {

    }

    public void SendToDatabase()
    {
        string conn = "URI=file:C:\\HouseMaderaDB-sqlite\\Test.db";
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
