using System;
using System.Collections.Generic;

namespace LARPSystem
{
    public class Trait
    {
        public float traitValue; 

        public Trait(Traits trait, float value = 0.5f)
        {
            traitValue = value;
        }
    }
}