using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            int[] A = { 1, 0, baseN}, B = { 0, 1, number}, oldB;
            int q;

            while((B[2] != 0) && (B[2] != 1))
            {
                q = A[2]/B[2];

                oldB = (int[])B.Clone();
                B[0] = A[0] - (q * B[0]);
                B[1] = A[1] - (q * B[1]);
                B[2] = A[2] - (q * B[2]);

                A[0] = oldB[0];
                A[1] = oldB[1];
                A[2] = oldB[2];
            }

            if (B[2] == 0)
                return -1;
            else
                return Helper.Mod(B[1], baseN);
        }
    }
}
