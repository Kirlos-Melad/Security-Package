using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            int i=0;
            for (int key = 2; key < plainText.Length - 1; key++)
            {
                string cipher = Encrypt(plainText, key);
                if (cipherText==cipher)
                {
                    i= key;
                    break;
                }
            }
            return i;
        }

        public string Decrypt(string cipherText, int key)
        {
            //throw new NotImplementedException();
            double y= (double)cipherText.Length / key;
            double colSize = Math.Ceiling(y);
            string plainText = "";
            int j = 0;
            char[,] matrix2 = new char[key, (int)colSize];
            for (int row = 0; row < key; row++)
            {
                for (int column = 0; column < colSize; column++)
                {
                    matrix2[row, column] = cipherText[j++];
                    if (j.Equals(cipherText.Length))
                        break;
                }
            }
            for (int column = 0; column < colSize; column++)
            {
                for (int row = 0; row < key; row++)
                    plainText += matrix2[row, column];
            }
            return plainText;
        }
        public string Encrypt(string plainText, int key)
        {
            //throw new NotImplementedException();
            String cipherText = "";
            int i = 0;
            double x = (double)plainText.Length / key;
            double colSize = Math.Ceiling(x);
            char[,] matrix = new char[key,(int)colSize];
            for(int column = 0; column < colSize; column++)
            {
                for (int R = 0; R < key; R++)
                {
                    matrix[R, column] = plainText[i++];
                    if (i.Equals(plainText.Length))
                        break;
                }
            }
            for(int row = 0; row < key; row++)
            {
                for (int column = 0; column < colSize; column++)
                {
                    if (matrix[row, column] == '\0')
                        continue;
                    cipherText += matrix[row, column];
                }
            }
            return cipherText.ToUpper();
        }
    }
}
