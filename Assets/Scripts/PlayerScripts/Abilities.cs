using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Abilities : Character
    {
        protected Character character;

        protected override void Initializiation()
        {
            base.Initializiation();

            character = GetComponent<Character>();
        }
    }
}