﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peteffect : MonoBehaviour
{

    public float dmgBoost = 9999999f;
    public GameObject petInfo;

    // Start is called before the first frame update
    void Start()
    {

        petInfo.SetActive(false);



    }


    void OnMouseOver()
    {

        petInfo.SetActive(true);
     
    }


    void OnMouseExit()
    {

        petInfo.SetActive(false);

    }


    // Update is called once per frame
    void Update()
    {

        GameObject.FindGameObjectWithTag("Gun").GetComponent<gunshoots>().dmg = dmgBoost;
        
    }
}
