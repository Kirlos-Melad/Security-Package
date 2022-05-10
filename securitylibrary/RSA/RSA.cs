using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int Encrypt(int p, int q, int M, int e)
        {
            long n = p * q;
            return (int)Helper.Mod(Power(M, e, n), n);
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            long n = p * q;
            BigInteger fn = BigInteger.Multiply(p - 1, q - 1);
            long eInverse = Helper.MultiplicativeInverse(e, (int)fn);
            long d = Helper.Mod(eInverse, (long)fn);

            return (int)Helper.Mod(Power(C, d, n), n);
        }

        private long Power(long a, long b, long n)
        {
            a = Helper.Mod(a, n);
            long Amul = Helper.Mod(a * a, n);
            if (b == 0) return 1;
            if (b == 1) return a;
            if (a == 0) return 0;
            if (b % 2 == 0)
            {
                //n is mod <3
                long powA = Helper.Mod(Power(Amul, b / 2, n), n);
                return powA;
            }
            else if (b % 2 == 1)
            {
                long powA = Helper.Mod(Power(Amul, b / 2, n), n);
                powA = Helper.Mod(powA * a, n);
                return powA;
            }
            return 0;
        }

    }
}
