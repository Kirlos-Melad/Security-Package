using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
    {

        public static int GetModules(BigInteger befor_mod, int after_mod)
        {
            BigInteger result1 = befor_mod / after_mod;
            BigInteger modules = befor_mod - (result1 * after_mod);
            return (int)modules;
        }
        public static BigInteger GetPower(int b, int exponent, int q)
        {
            List<int> ele = new List<int> { };
            BigInteger result1 = 1;
            BigInteger result2 = 1;
            int num;
            int count = 1;
            if (exponent <= 10)
            {
                for (int i = 0; i < exponent; i++)
                    result1 = result1 * b;
            }
            else
            {
                num = exponent - 10;
                while (num >= 10)
                {
                    count++;
                    num = num - 10;
                }
                for (int i = 0; i < count; i++)
                {
                    BigInteger result3 = 1;
                    for (int j = 0; j < 10; j++)

                        result3 = result3 * b;
                    ele.Add(GetModules(result3, q));
                }
                if (num > 0)
                {
                    for (int i = 0; i < num; i++)
                        result2 = result2 * b;

                    ele.Add(GetModules(result2, q));
                }
            }
            for (int i = 0; i < ele.Count; i++)
                result1 = result1 * ele[i];

            return result1;

        }

        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            BigInteger power1, power2;
            int public_key1;
            int public_key2;
            int private_key1;
            int private_key2;
            List<int> keys = new List<int> { };
            //generate Public key
            power1 = GetPower(alpha, xa, q);
            power2 = GetPower(alpha, xb, q);

            public_key1 = GetModules(power1, q);
            public_key2 = GetModules(power2, q);

            //generate private key
            power1 = GetPower(public_key2, xa, q);
            power2 = GetPower(public_key1, xb, q);

            private_key1 = GetModules(power1, q);
            private_key2 = GetModules(power2, q);
            keys.Add(private_key1);
            keys.Add(private_key2);
            return keys;
        }
    }
}
