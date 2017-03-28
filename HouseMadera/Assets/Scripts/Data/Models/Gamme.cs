using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gamme
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nom { get; set; }
    public int Isolant_Id { get; set; }
    public int Finition_Id { get; set; }

    public static implicit operator Gamme(List<Gamme> v)
    {
        throw new NotImplementedException();
    }
}
