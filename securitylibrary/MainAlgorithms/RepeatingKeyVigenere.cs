using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string key = null;
            cipherText = cipherText.ToUpper();
            plainText = plainText.ToUpper();

            for (int i = 0; i < plainText.Length; i++)
            {
                if(plainText[i] > cipherText[i])
                    key += (char)(((cipherText[i] + 26) - plainText[i]) + 65);
                else
                    key += (char)((cipherText[i] - plainText[i]) + 65);
            }
            key = key.ToLower();

            int repeatIndex = 0;
            for (int i = 2; i < key.Length - 1; i++)
            {
                if (key[0].Equals(key[i]) && key[1].Equals(key[i + 1])) { 
                    repeatIndex = i;
                    break;
                }
            }

            string finalKey = key.Substring(0 , repeatIndex);
            //0 1 2 3 4 5 6 7    8  9  10 11 12 13 14 15
            //h o u g h t o n  - h  o  u  g  h  t  o  n  -  h o u g h t o n  -  h o u g h t o
            return finalKey;
        }

        public string Decrypt(string cipherText, string key)
        {
            string plainText = null;
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            int length = cipherText.Length - key.Length;
            for (int j = 0; j < length; j++)
                key += key[j % key.Length];
            

            for (int i = 0; i < cipherText.Length; i++)
            {
                if (key[i] > cipherText[i])
                    plainText += (char)(((cipherText[i] + 26) - key[i]) + 65);
                else
                    plainText += (char)((cipherText[i] - key[i]) + 65);
            }

            plainText = plainText.ToLower();
            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            string cipherText = null;
            plainText = plainText.ToUpper();
            key = key.ToUpper();
            int length = plainText.Length - key.Length;
            for (int i = 0; i < length; i++)
                key += key[i % key.Length];

            for (int i = 0; i < plainText.Length; i++)
                cipherText += (char)((((plainText[i] + key[i]) % 65) % 26) + 65);
            
            return cipherText;
        }
    }
}