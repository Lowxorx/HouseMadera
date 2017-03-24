using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeModule
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nom { get; set; }
}
