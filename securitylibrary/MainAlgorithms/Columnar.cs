using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        List<List<int>> seq = new List<List<int>>();
        public void createSeq(int[] a, int n)
        {
            List<int> myList = new List<int>();
            for (int i = 0; i < n; i++)
            {
                myList.Add(a[i]);
            }
            seq.Add(myList);
        }
        public void permutation(int[] a, int size, int n)
        {
            if (size == 1)
                createSeq(a, n);

            for (int i = 0; i < size; i++)
            {
                permutation(a, size - 1, n);
                if (size % 2 == 1)
                {
                    int temp = a[0];
                    a[0] = a[size - 1];
                    a[size - 1] = temp;
                }
                else
                {
                    int temp = a[i];
                    a[i] = a[size - 1];
                    a[size - 1] = temp;
                }
            }
        }
        public List<int> Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            //List<List<int>> seqOfKeys = new List<List<int>>();
            List<int> key = new List<int>();
            for (int i = 4; i < 8; i++)
            {
                int[] a = Enumerable.Range(1, i).ToArray<int>();
                permutation(a, a.Length, a.Length);
            }

            foreach (List<int> Key in seq)
            {
                string cipher = Encrypt(plainText, Key);
                cipherText = cipherText.ToLower();
                if (cipherText == cipher)
                {
                    key = Key;
                    break;
                }  
            }
            return key;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            //throw new NotImplementedException();
            int colSize = key.AsQueryable().Max();
            int minNumber = key.AsQueryable().Min();
            int i = 0;
            string plainText = "";
            double x = (double)cipherText.Length / colSize;
            double rowSize = Math.Ceiling(x);
            char[,] matrix = new char[(int)rowSize, colSize];
            for (int column = key.IndexOf(minNumber); column < colSize; column = key.IndexOf(++minNumber))
            {
                if (column == -1)
                    break;
                for (int row = 0; row < rowSize; row++)
                {
                    matrix[row, column] = cipherText[i++];
                    if (i.Equals(cipherText.Length))
                        break;
                }
            }
            for (int row = 0; row < rowSize; row++)
            {
                for (int Column = 0; Column < colSize; Column++)
                {
                    if (matrix[row, Column] == '\0')
                        continue;
                    plainText += matrix[row, Column];
                }
            }
            return plainText.ToUpper();
        }

        public string Encrypt(string plainText, List<int> key)
        {
            //throw new NotImplementedException();
            int colSize = key.AsQueryable().Max();
            int minNum = key.AsQueryable().Min();
            int i = 0;
            string output = "";
            double x = (double)plainText.Length / colSize;
            double rowSize = Math.Ceiling(x);
            char[,] matrix = new char[(int)rowSize, colSize];

            for (int row = 0; row < rowSize; row++)
            {
                for (int Column = 0; Column < colSize; Column++)
                {
                    matrix[row, Column] = plainText[i++];
                    if (i.Equals(plainText.Length))
                    {
                        break;
                    }
                }

            }
            for (int column = key.IndexOf(minNum); column < colSize ; column = key.IndexOf(++minNum))
            {
                if (column == -1)
                    break;
                for (int row = 0; row < rowSize; row++)
                {
                    if(matrix[row, column] == '\0')
                    {
                        continue;
                    }
                    output += matrix[row, column];
                }
                
            }

            return output;
        }
    }
}
