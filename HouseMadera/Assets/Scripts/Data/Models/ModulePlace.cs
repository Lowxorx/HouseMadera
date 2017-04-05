using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePlace
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Module_Id { get; set; }
    public int SlotPlace_Id { get; set; }
    public string Libelle { get; set; }
    public int Horizontal { get; set; }
    public int Vertical { get; set; }
    public string MiseAJour { get; set; }
    public string Suppression { get; set; }
    public string Creation { get; set; }
    public int Produit_Id { get; set; }
}
