using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Produit
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nom { get; set; }
    public int? Devis_Id { get; set; }
    public int Plan_Id { get; set; }
    public int Projet_Id { get; set; }
    public string MiseAJour { get; set; }
    public string Suppression { get; set; }
    public string Creation { get; set; }
    public int StatutProduit_Id { get; set; }
}
