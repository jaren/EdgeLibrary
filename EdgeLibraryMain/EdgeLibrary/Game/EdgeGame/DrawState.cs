using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary
{
    //The type of drawing the game will use
    public enum DrawState
    {
        //Normal game drawing
        Normal,
        //Draws only collision bodies
        Debug,
        //Draws collision bodies and normal game drawing
        Hybrid
    }
}
