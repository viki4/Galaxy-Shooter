﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
  
    // Update is called once per frame
    void Update()
    {
        // translate laser up
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // if the laser position on y >=8  ,then destroy the object
        if(transform.position.y >= 8f)
        {
            // check if this laser obj has a parent. If yes
            // destroy the parent too.
            if(transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);  // gameObject is the object attached to this script
            }
           
        }

    }
}
