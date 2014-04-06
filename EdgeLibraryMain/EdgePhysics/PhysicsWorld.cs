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
    public class PhysicsWorld
    {
        private class BodyPair
        {
            public Body A;
            public Body B;

            public BodyPair(Body a, Body b)
            {
                A = a;
                B = b;
            }

            public bool Equals(BodyPair pair)
            {
                return ((A==pair.A && B==pair.B) || (A==pair.B && B==pair.A));
            }
        }

        private List<BodyPair> Pairs;
        private PhysicsManager Manager;

        public List<Body> Bodies;

        public PhysicsWorld()
        {
            Pairs = new List<BodyPair>();
            Bodies = new List<Body>();
            Manager = new PhysicsManager();
        }

        public void Update(GameTime gameTime)
        {
            Pairs.Clear();
            GeneratePossiblePairs();
            foreach (BodyPair pair in Pairs)
            {
                Manager.CheckAndResolve(pair.A, pair.B);
            }
        }

        //Checks only for bounding boxes
        private void GeneratePossiblePairs()
        {
            for (int i = 0; i < Bodies.Count; i++)
            {
                for (int g = 0; g < Bodies.Count; g++)
                {
                    Body A = Bodies[i];
                    Body B = Bodies[g];

                    //Skip check with self
                    if (A == B)
                    {
                        continue;
                    }

                    //Checks for the same layers
                    if ((A.Layers & B.Layers) == 0)
                    {
                        continue;
                    }

                    //Checks if the bounding boxes collide
                    if (A.GetBox().CheckColliding(B.GetBox()))
                    {
                        BodyPair pair = new BodyPair(A, B);

                        //Checks for duplicates
                        bool duplicate = false;
                        foreach (BodyPair testPair in Pairs)
                        {
                            if (testPair.Equals(pair))
                            {
                                duplicate = true;
                                break;
                            }
                        }
                        if (!duplicate)
                        {
                            Pairs.Add(pair);
                        }
                    }
                }
            }
        }
    }
}
