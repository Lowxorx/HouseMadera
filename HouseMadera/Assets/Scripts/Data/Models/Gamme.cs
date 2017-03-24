using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamme
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nom { get; set; }
    public int Isolant_Id { get; set; }
    public int Finition_Id { get; set; }
}
