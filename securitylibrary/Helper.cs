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

        public static int Mod(int number, int mod)
        {
            if(number >= 0)
            {
                return number % mod;
            }
            else
            {
                int temp = -number % mod;
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

        public static int NumberInverse(int n, int mod)
        {
            int i = 1;
            while (Helper.Mod(i * n, mod) != 1 && i != 26)
            {
                i++;
            }

            if (i == 26)
                throw new Exception("Can't find determinant inverse");

            return i;
        }

        public static List<int> ConvertStringToIntList(String text)
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
    }
}
