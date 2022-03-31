using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            string cipherText = null;
            plainText = plainText.ToUpper();
            int cipherIndex;
            
            for (int i = 0; i < plainText.Length; i++)
            {
                int charIndex = ((int)char.ToUpper(plainText[i])) - 65;
                cipherIndex =  (charIndex + key) % 26;
                cipherText += ((char)(cipherIndex + 65));
            }

            return cipherText;
        }

        public string Decrypt(string cipherText, int key)
        {
            string plainText = null;
            cipherText = cipherText.ToUpper();
            int plainIndex;

            for (int i = 0; i < cipherText.Length; i++) { 
                plainIndex = (((int)char.ToUpper(cipherText[i])) - 65) - key;
                if(plainIndex < 0)
                    plainIndex += 26;
                
                plainText += ((char)(plainIndex + 65)).ToString();
            }

            return plainText.ToLower();
        }

        public int Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();
            int cipherIndex = (int)(cipherText[0] - 65);
            int plainIndex = (int)(plainText[0] - 65);
            if(cipherText[0] < plainText[0])
            {
                return (cipherIndex + 26) - plainIndex;
            }
            return (cipherIndex - plainIndex);
        }
    }
}
