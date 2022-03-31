using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string key = null;
            cipherText = cipherText.ToUpper();
            plainText = plainText.ToUpper();

            for (int i = 0; i < plainText.Length; i++)
            {
                if (plainText[i] > cipherText[i])
                    key += (char)(((cipherText[i] + 26) - plainText[i]) + 65);
                else
                    key += (char)((cipherText[i] - plainText[i]) + 65);
            }
            key = key.ToLower();
            plainText = plainText.ToLower();

            int repeatIndex = 0;
            for (int i = 0; i < key.Length - 1; i++)
            {
                if (key[i].Equals(plainText[0]) && key[i+1].Equals(plainText[1]))
                {
                    repeatIndex = i;
                    break;
                }
            }

            string finalKey = key.Substring(0, repeatIndex);
            return finalKey;
        }

        public string Decrypt(string cipherText, string key)
        {
            string plainText = null;
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            int length = cipherText.Length - key.Length;

            for (int i = 0; i < cipherText.Length; i++)
            {
                if (key[i] > cipherText[i])
                {
                    plainText += (char)(((cipherText[i] + 26) - key[i]) + 65);
                    key += plainText[i];
                }
                else
                {
                    plainText += (char)((cipherText[i] - key[i]) + 65);
                    key += plainText[i];
                }
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
            for (int j = 0; j < length; j++)
                key += plainText[j];

            for (int i = 0; i < plainText.Length; i++)
                cipherText += (char)((((plainText[i] + key[i]) % 65) % 26) + 65);

            return cipherText;
        }
    }
}
