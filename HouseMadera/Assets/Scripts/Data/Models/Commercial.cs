using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Commercial
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string MiseAJour { get; set; }
    public string Suppression { get; set; }
    public string Creation { get; set; }
}
