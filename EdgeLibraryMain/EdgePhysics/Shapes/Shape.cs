using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EdgePhysics
{
    /* License:
    Copyright (c) 2013 Randy Gaul http://RandyGaul.net

    This software is provided 'as-is', without any express or implied
    warranty. In no event will the authors be held liable for any damages
    arising from the use of this software.

    Permission is granted to anyone to use this software for any purpose,
    including commercial applications, and to alter it and redistribute it
    freely, subject to the following restrictions:
      1. The origin of this software must not be misrepresented; you must not
         claim that you wrote the original software. If you use this software
         in a product, an acknowledgment in the product documentation would be
         appreciated but is not required.
      2. Altered source versions must be plainly marked as such, and must not be
         misrepresented as being the original software.
      3. This notice may not be removed or altered from any source distribution.
     */

    public enum ShapeType
    {
        Circle, Polygon, Count, Null
    }

    public class Shape
    {
        //For circle
        public float Radius;

        //For polygon
        public Mat2 Matrix;

        public virtual void Init() { } //Possibly remove
        public virtual Shape Clone() { return null; }
        public virtual float GetMass(float density) { return 0; }
        public virtual float GetInertia(float density) { return 0; }
        public virtual void SetRotation(float degrees) { }
        public virtual ShapeType GetShapeType() { return ShapeType.Null; }
    }
}
