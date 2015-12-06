using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class FireEffect : DamageEffect
    {

        public FireEffect(float duration) : base("Fire", 2, Color.Red, Color.Orange, duration) { }
    }
}
