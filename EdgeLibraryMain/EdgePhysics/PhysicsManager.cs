using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgePhysics
{
    public class PhysicsManager
    {
        public bool CheckAndResolve(Body A, Body B)
        {
            if (Check(A, B))
            {
                Resolve(A, B);
                return true;
            }
            return false;
        }

        public bool Check(Body A, Body B)
        {
            return true;
        }

        public void Resolve(Body A, Body B)
        {
        }
    }

}
