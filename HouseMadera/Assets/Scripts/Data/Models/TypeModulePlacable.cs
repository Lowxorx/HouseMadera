using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeModulePlacable
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string nom { get; set; }
    public string icone { get; set; }
}
