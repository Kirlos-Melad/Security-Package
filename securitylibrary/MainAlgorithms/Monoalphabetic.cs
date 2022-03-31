using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            char counter = '0';
            char[] key = new char[26];
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            for (int i = 0; i < 26; i++)
            {
                for(int j = 0; j < plainText.Length; j++)
                {
                    if(plainText[j] == (char)(i + 97))
                    {
                        key[i] += cipherText[j];
                        break;
                    }
                }
                if (key[i].Equals('\0'))
                {
                    key[i] = counter;
                    counter++;
                }
            }

            return new string(key);
        }

        public string Decrypt(string cipherText, string key)
        {
            string plainText = null;
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            int index = 0;

            for (int i = 0; i < cipherText.Length; i++)
            {
                for(int j = 0; j < key.Length; j++)
                {
                   if(key[j] == cipherText[i])
                   {
                        index = j;
                        break;
                   }
                }
                plainText += (char)(index + 65);
            }

            return plainText.ToLower();
        }

        public string Encrypt(string plainText, string key)
        {
            string cipherText = null;
            plainText = plainText.ToUpper();
            key = key.ToUpper();

            for (int i = 0; i < plainText.Length; i++)
                cipherText += key[plainText[i] % 65];
            
            return cipherText;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            int[] freq = new int[26];
            string freqInfo = "etaoinsrhldcumfpgwybvkxjqz";
            cipher = cipher.ToLower();
            char[] plainText = new char[cipher.Length];

            for (int i = 0; i < 26; i++)
                for (int j = 0; j < cipher.Length; j++)
                    if (cipher[j] == (char)(i + 97))
                        freq[i]++;

            int x = 0;
            int max;
            int index;
            for (int i = 0; i < 26; i++)
            {
                max = freq.Max();
                index = Array.IndexOf(freq, max);
                char oldChar = (char)(index + 97);
                char newChar = freqInfo[x];
                for (int j = 0; j < cipher.Length; j++)
                {
                    if (cipher[j] == oldChar)
                        plainText[j] = newChar;
                }
                //cipher = cipher.Replace(oldChar, newChar);
                x++;
                freq[index] = 0;
            }

            string finalPlainText = new string(plainText);
            return finalPlainText;
        }
    }
}
