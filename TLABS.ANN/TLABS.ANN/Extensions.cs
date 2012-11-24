using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLABS.ANN
{
    public static class Extensions
    {
        public static void Normalize(this List<IOSet> IOSets)
        {
            double max = 0.0;
            for (int i = 0; i < IOSets.Count; i++)
            {
                for (int j = 0; j < IOSets[i].InputSet.Count; j++)
                {
                    max = Math.Max(Math.Abs(IOSets[i].InputSet[j]), max);
                }

                for (int j = 0; j < IOSets[i].OutputSet.Count; j++)
                {
                    max = Math.Max(Math.Abs(IOSets[i].OutputSet[j]), max);
                }              
            }
            if (max > 1)
            {
                double scaling_factor = 1.0 / max;
                for (int i = 0; i < IOSets.Count; i++)
                {
                    IOSets[i].InputSet.ForEach(delegate(double d) { d *= scaling_factor; });
                    IOSets[i].OutputSet.ForEach(delegate(double d) { d *= scaling_factor; });
                }
            }
        }
    }
}
