using SecurityLibrary.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    class Helper
    {
        public static readonly String NUMBER_TO_lOWER_ALPHBET = "abcdefghijklmnopqrstuvwxyz";
        /*public static readonly Dictionary<Char, int> LOWER_ALPHABET_TO_NUMBER = new Dictionary<char, int>()
        {
            {'a', 0}, {'b', 0}, {'c', 0}, {'d', 0}, {'e', 0},
            {'f', 0}, {'g', 0}, {'h', 0}, {'i', 0}, {'j', 0},
            {'k', 0}, {'l', 0}, {'m', 0}, {'n', 0}, {'o', 0},
            {'p', 0}, {'q', 0}, {'r', 0}, {'s', 0}, {'t', 0},
            {'u', 0}, {'v', 0}, {'w', 0}, {'x', 0}, {'y', 0},
            {'z', 0},
        };*/

        public static long Mod(long number, long mod)
        {
            if (number >= 0)
            {
                return number % mod;
            }
            else
            {
                long temp = -number % mod;
                return -temp + mod;
            }
        }

        public static List<int> PadListWith(List<int> list, int size, int value)
        {
            while (list.Count < size)
            {
                list.Add(value);
            }

            return list;
        }

        public static void SwapInt(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }

        public static int MultiplicativeInverse(int n, int mod)
        {
            ExtendedEuclid extendedEuclidean = new ExtendedEuclid();
            int result = extendedEuclidean.GetMultiplicativeInverse((int)Mod(n, mod), mod);

            if (result == -1)
                throw new Exception("Can't find determinant inverse");

            return result;
        }

        public static List<int> ConvertStringToIntList(string text)
        {
            List<int> intList = new List<int>();
            text = text.ToLower();
            int character = -1;
            for (int i = 0; i < text.Length; i++)
            {
                character = text[i] - 'a';
                intList.Add(character);
            }

            return intList;
        }

        public static String ConvertIntListToString(List<int> list)
        {
            String s = "";
            int index = -1;
            for(int i = 0; i < list.Count; i++)
            {
                index = list[i];
                s += NUMBER_TO_lOWER_ALPHBET[index];
            }

            return s;
        }

        public static byte[][] ConvertStringToSquareByte(string str)
        {
            byte[] array1D = Encoding.ASCII.GetBytes(str);
            int size = (int)Math.Sqrt(str.Length);

            byte[][] squareByte = new byte[size][];
            for(int i = 0; i < size; i++)
            {
                squareByte[i] = new byte[size];
                for(int j = 0; j < size; j++)
                {
                    squareByte[i][j] = array1D[(i * size) + j];
                }
            }

            return squareByte;
        }
    }
}
