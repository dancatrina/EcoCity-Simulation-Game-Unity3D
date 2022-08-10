using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBuildingInfo : MonoBehaviour
{
    public BuildingType building;
    public string denumireCladire;
    public float pret;
    public string descriere;

    public float taxaCladire;
    public float consumElectricitate;
    public float venitCladire;

    public BuildingType Building { get => building; set => building = value; }
    public float Pret { get => pret; set => pret = value; }
    public string Descriere { get => descriere; set => descriere = value; }

    public void Start()
    {

    }
}
