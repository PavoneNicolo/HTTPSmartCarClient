using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class RandomGenerator
    {
        Random ran;
        public RandomGenerator()
        {
            ran = new Random();
        }
        public decimal getRan(int min, int max)
        {
            int minUsable = min * 100;
            int maxUsable = max * 100;
            return new decimal((double)ran.Next(minUsable, maxUsable) / 100);
        }
    }
}
