using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();
            string keyMatrix = "";
            string plainText = "";
            string newPlain = "";
            const int decrypt = -1;

            //Generate Key Matrix
            keyMatrix = GenerateKeyMatrix(key, keyMatrix);

            //Generate Plain Text
            int count = 0;
            while (count < cipherText.Length)
            {
                //same row
                if ((keyMatrix.IndexOf(cipherText[count]) / 5) == (keyMatrix.IndexOf(cipherText[count + 1]) / 5))
                    plainText += SameRowEncDec(cipherText, keyMatrix, count, decrypt);
                //same colomn
                else if ((keyMatrix.IndexOf(cipherText[count]) - keyMatrix.IndexOf(cipherText[count + 1])) % 5 == 0)
                    plainText += SameColumnEncDec(cipherText, keyMatrix, count, decrypt);
                //square
                else
                    plainText += SquareEncDec(cipherText, keyMatrix, count, decrypt);
                count += 2;
            }

            /// Rmove X at the end of the plain text
            if (plainText[plainText.Length - 1] == 'X')
                plainText = plainText.Remove(plainText.Length - 1);

            //Remove X if exists in plain text in odd position and its neigbours is equevilant
            for (int s = 0; s < plainText.Length; s++)
            {
                if ((plainText[s] == 'X') && (s % 2 != 0) && (plainText[s - 1] == plainText[s + 1]))
                    continue;
                newPlain += plainText[s];
            }
            return newPlain.ToLower();
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            string keyMatrix = "";
            string cipherText = "";
            const int encrypt = 1;

            //Generate Key Matrix
            keyMatrix = GenerateKeyMatrix(key, keyMatrix);

            //Generate Cipher Text
            plainText = plainText.ToUpper();
            int count = 0;
            while (count < plainText.Length)
            {
                //repeated number
                if (plainText[count] == plainText[count + 1])
                {
                    if (plainText[plainText.Length - 1] != 'X')
                        plainText += plainText[plainText.Length - 1];

                    for (int k = plainText.Length - 1; k >= count + 1; k--)
                        plainText = plainText.Remove(k, 1).Insert(k, plainText[k - 1].ToString());
                    plainText = plainText.Remove(count + 1, 1).Insert(count + 1, "X");
                }
                //for plain text length (odd/even)
                if (plainText.Length % 2 != 0)
                    plainText += 'X';
                //same row
                if ((keyMatrix.IndexOf(plainText[count]) / 5) == (keyMatrix.IndexOf(plainText[count + 1]) / 5))
                    cipherText += SameRowEncDec(plainText, keyMatrix, count, encrypt);
                //same colomn
                else if ((keyMatrix.IndexOf(plainText[count]) - keyMatrix.IndexOf(plainText[count + 1])) % 5 == 0)
                    cipherText += SameColumnEncDec(plainText, keyMatrix, count, encrypt);
                //square
                else
                    cipherText += SquareEncDec(plainText, keyMatrix, count, encrypt);
                count += 2;
            }

            return cipherText;
        }

        private string GenerateKeyMatrix(string key, string keyMatrix)
        {
            string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";

            key = key.ToUpper();
            keyMatrix += key[0];

            //till the end of key
            for (int i = 1; i < key.Length; i++)
            {
                if (!keyMatrix.Contains(key[i]) && (key[i] != 'I' | key[i] != 'J'))
                    keyMatrix += key[i];
                else if (!keyMatrix.Contains(key[i]) && (key[i] == 'I' | key[i] == 'J'))
                    keyMatrix += 'I';
                else
                    continue;
            }
            //continue with the rest of non rebeated alphabet
            for (int j = 0; j < alphabet.Length; j++)
            {
                if (!keyMatrix.Contains(alphabet[j]) && (alphabet[j] != 'I'))
                    keyMatrix += alphabet[j];
                else if (!keyMatrix.Contains(alphabet[j]) && (alphabet[j] == 'I'))
                    keyMatrix += 'I';
                else
                    continue;
            }

            return keyMatrix;
        }

        private string SameRowEncDec(string input, string keyMatrix, int index, int mood)
        {
            string result = "";
            int roundedRow = 0;
            //if encryption the round is happened in last column (its % = 0)
            //if decryption the round is happened in first column (its % = 1)
            if (mood == -1)
                roundedRow = 1;
            
            int lol = (keyMatrix.IndexOf(input[index]) + 1) % 5;
            //no letter is rounded
            if (((keyMatrix.IndexOf(input[index]) + 1) % 5 != roundedRow) && ((keyMatrix.IndexOf(input[index + 1]) + 1) % 5 != roundedRow))
            {
                result += keyMatrix[keyMatrix.IndexOf(input[index]) + 1 * mood];                //take right of first letter
                result += keyMatrix[keyMatrix.IndexOf(input[index + 1]) + 1 * mood];            //take right of second letter
            }
            else
            {
                //first letter is rounded
                if (((keyMatrix.IndexOf(input[index]) + 1) % 5 == roundedRow))
                {
                    result += keyMatrix[keyMatrix.IndexOf(input[index]) - 4 * mood];             //row mod
                    result += keyMatrix[keyMatrix.IndexOf(input[index + 1]) + 1 * mood];         //take right of second letter
                }
                //second letter is rounded
                if (((keyMatrix.IndexOf(input[index + 1]) + 1) % 5 == roundedRow))
                {
                    result += keyMatrix[keyMatrix.IndexOf(input[index]) + 1 * mood];             //take right of first letter
                    result += keyMatrix[keyMatrix.IndexOf(input[index + 1]) - 4 * mood];         //row mod
                }
            }

            return result;
        }

        private string SameColumnEncDec(string input, string keyMatrix, int index, int mood)
        {
            string result = "";
            int roundedColumn = 0;
            //if encryption the round is happened in last row (its / 5 = 4)
            //if decryption the round is happened in first row (its / 5 = 0)
            if (mood == 1)
                roundedColumn = 4;

            int lol = input.Length;
            int don = keyMatrix.IndexOf(input[index + 1]) + (5 * mood);
            char letter = keyMatrix[keyMatrix.IndexOf(input[index + 1])];
            //no letter need is rounded
            if (((keyMatrix.IndexOf(input[index])) / 5 != roundedColumn) && ((keyMatrix.IndexOf(input[index + 1])) / 5 != roundedColumn))
            {
                result += keyMatrix[keyMatrix.IndexOf(input[index]) + (5 * mood)];                 //take lower of first letter
                result += keyMatrix[keyMatrix.IndexOf(input[index + 1]) + (5 * mood)];             //take lower of second letter
            }
            else
            {
                //first letter is rounded
                if (((keyMatrix.IndexOf(input[index])) / 5) == roundedColumn)
                {
                    result += keyMatrix[keyMatrix.IndexOf(input[index]) - 20 * mood];            //column mod
                    result += keyMatrix[keyMatrix.IndexOf(input[index + 1]) + 5 * mood];         //take lower of second letter
                }
                //second letter is rounded
                else if (((keyMatrix.IndexOf(input[index + 1])) / 5) == roundedColumn)
                {
                    result += keyMatrix[keyMatrix.IndexOf(input[index]) + 5 * mood];             //take lower of first letter
                    result += keyMatrix[keyMatrix.IndexOf(input[index + 1]) - 20 * mood];        //column mod
                }
            }

            return result;
        }

        private string SquareEncDec(string input, string keyMatrix, int index, int mood)
        {
            string result = "";
            int displacement = 0;

            // no letter need round
            if (((keyMatrix.IndexOf(input[index]) + 1) % 5 != 0) && ((keyMatrix.IndexOf(input[index + 1]) + 1) % 5 != 0))
            {
                displacement = (keyMatrix.IndexOf(input[index]) + 1) % 5 - (keyMatrix.IndexOf(input[index + 1]) + 1) % 5;
                if (displacement < 0)
                    displacement *= -1;               //just for direction, displacement fixed from 1 to 3 here

                //first letter in left
                if ((keyMatrix.IndexOf(input[index]) + 1) % 5 < (keyMatrix.IndexOf(input[index + 1]) + 1) % 5)
                {
                    result += keyMatrix[keyMatrix.IndexOf(input[index]) + displacement];
                    result += keyMatrix[keyMatrix.IndexOf(input[index + 1]) - displacement];
                }
                //first letter in right
                else if ((keyMatrix.IndexOf(input[index]) + 1) % 5 > (keyMatrix.IndexOf(input[index + 1]) + 1) % 5)
                {
                    result += keyMatrix[keyMatrix.IndexOf(input[index]) - displacement];
                    result += keyMatrix[keyMatrix.IndexOf(input[index + 1]) + displacement];
                }
            }
            //one letter is rounded
            else
            {
                if ((keyMatrix.IndexOf(input[index]) + 1) % 5 == 0)
                    displacement = 5 - (keyMatrix.IndexOf(input[index + 1]) + 1) % 5;
                else if ((keyMatrix.IndexOf(input[index + 1]) + 1) % 5 == 0)
                    displacement = (keyMatrix.IndexOf(input[index]) + 1) % 5 - 5;

                if (displacement < 0)
                    displacement *= -1;               //just for direction, displacement fixed from 1 to 4 here

                //first letter will be rounded
                if (((keyMatrix.IndexOf(input[index]) + 1) % 5 != 0))
                {
                    result += keyMatrix[keyMatrix.IndexOf(input[index]) + displacement];
                    result += keyMatrix[keyMatrix.IndexOf(input[index + 1]) - displacement];
                }
                //second letter will be rounded
                if (((keyMatrix.IndexOf(input[index + 1]) + 1) % 5 != 0))
                {
                    result += keyMatrix[keyMatrix.IndexOf(input[index]) - displacement];
                    result += keyMatrix[keyMatrix.IndexOf(input[index + 1]) + displacement];
                }
            }

            return result;
        }

    }
}