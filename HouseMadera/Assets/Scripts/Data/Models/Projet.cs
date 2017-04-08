using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projet
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Reference { get; set; }
    public string UpdateDate { get; set; }
    public string CreateDate { get; set; }
    public int Client_Id { get; set; }
    public int Commercial_Id { get; set; }
    public string MiseAJour { get; set; }
    public string Suppression { get; set; }
    public string Creation { get; set; }
}
