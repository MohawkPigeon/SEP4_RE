using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.HardwareSimulator
{
    public class RandomNumber
    {
        private static RandomNumber instance;

        private static readonly object lockRoot = new object();


        private RandomNumber() { }

        public static RandomNumber GetInstance()
        {
            if (instance == null)
            {
                instance = new RandomNumber();
            }

            return instance;
        }

        public float RNG()
        {
            lock (lockRoot)
            {
                Random rng = new Random();
                float randomFloat = (float) rng.NextDouble();
                return randomFloat;
            }
        }

    }
}
