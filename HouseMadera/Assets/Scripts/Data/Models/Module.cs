using UnityEngine;
using System.Collections;

public class Module
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public float Hauteur { get; set; }
    public float Largeur { get; set; }
    public int Gamme_Id { get; set; }
    public int TypeModule_Id { get; set; }

    public Module()
    {
        Id = 0;
        Nom = "";
        Hauteur = 0.0f;
        Largeur = 0.0f;
        Gamme_Id = 0;
        TypeModule_Id = 0;
    }
}
