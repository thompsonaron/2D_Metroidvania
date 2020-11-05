using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Character : MonoBehaviour
    {
        protected Collider2D col;
        protected Rigidbody2D rb;



        // Start is called before the first frame update
        void Start()
        {
            Initializiation();
        }

        protected virtual void Initializiation()
        {
            col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        
    }
}
