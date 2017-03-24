using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nom { get; set; }
    public float Hauteur { get; set; }
    public float Largeur { get; set; }
    public int TypeSlot_Id { get; set; }
}
