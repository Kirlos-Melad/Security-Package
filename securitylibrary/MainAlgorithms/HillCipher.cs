using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
        public string Analyse(string plainText, string cipherText)
        {
            List<int> plainTextINT = Helper.ConvertStringToIntList(plainText);
            List<int> cipherTexttINT = Helper.ConvertStringToIntList(cipherText);

            List<int> key = Analyse(plainTextINT, cipherTexttINT);

            return Helper.ConvertIntListToString(key);
        }


        public string Decrypt(string cipherText, string key)
        {
            List<int> keyINT = Helper.ConvertStringToIntList(key);
            List<int> cipherTexttINT = Helper.ConvertStringToIntList(cipherText);

            List<int> plainText = Decrypt(cipherTexttINT, keyINT);

            return Helper.ConvertIntListToString(plainText);
        }



        public string Encrypt(string plainText, string key)
        {
            List<int> keyINT = Helper.ConvertStringToIntList(key);
            List<int> plainTexttINT = Helper.ConvertStringToIntList(plainText);

            List<int> cipherText = Encrypt(plainTexttINT, keyINT);

            return Helper.ConvertIntListToString(cipherText);
        }



        public string Analyse3By3Key(string plainText, string cipherText)
        {
            List<int> plainTextINT = Helper.ConvertStringToIntList(plainText);
            List<int> cipherTexttINT = Helper.ConvertStringToIntList(cipherText);

            List<int> key = Analyse3By3Key(plainTextINT, cipherTexttINT);

            return Helper.ConvertIntListToString(key);
        }


        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            // this is 2x2 key analysis
            int KEY_SIZE = 2;
            int column = KEY_SIZE, row = (int)Math.Ceiling(((double)cipherText.Count / KEY_SIZE));

            cipherText = Helper.PadListWith(cipherText, row * column, 0);
            plainText = Helper.PadListWith(plainText, row * column, 0);

            // Brute force all combinations
            for (int i = 0; i < row - 1; i++)
            {
                for (int j = i + 1; j < row; j++)
                {
                    // For each iteration
                    Matrix cipherTextMatrix = new Matrix(new int[KEY_SIZE, KEY_SIZE]);
                    Matrix plainTextMatrix = new Matrix(new int[KEY_SIZE, KEY_SIZE]);

                    // Fill the matrices in ROW MODE
                    for (int c = 0; c < KEY_SIZE; c++)
                    {
                        // Fill cipher text matrix in ROW MODE
                        cipherTextMatrix[0, c] = cipherText[i * column + c];
                        cipherTextMatrix[1, c] = cipherText[j * column + c];

                        // Fill plain text matrix in ROW MODE
                        plainTextMatrix[0, c] = plainText[i * column + c];
                        plainTextMatrix[1, c] = plainText[j * column + c];
                    }

                    try
                    {
                        Matrix keyMatrix = plainTextMatrix.Inverse().Multiply(cipherTextMatrix);
                        return keyMatrix.ToList(Mode.Column);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            throw new InvalidAnlysisException();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            // Create Key Matrix
            Matrix keyMatrix = Matrix.CreateMatrix(key, Mode.Column);

            // Create Cipher Text Matrix
            int row = keyMatrix.columnSize, column = (int)Math.Ceiling(((double)cipherText.Count / keyMatrix.rowSize));
            cipherText = Helper.PadListWith(cipherText, row * column, 0);
            Matrix cipherTextMatrix = Matrix.CreateMatrix(cipherText, row, column, Mode.Row);

            // Find The Plain Text
            Matrix plainTextMatrix = keyMatrix.Inverse().Multiply(cipherTextMatrix);

            return plainTextMatrix.ToList(Mode.Row);
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            // Create Key Matrix
            Matrix keyMatrix = Matrix.CreateMatrix(key, Mode.Column);

            // Create Plain Text Matrix
            int row = keyMatrix.columnSize, column = (int)Math.Ceiling(((double)plainText.Count / keyMatrix.rowSize));
            plainText = Helper.PadListWith(plainText, row * column, 0);
            Matrix plainTextMatrix = Matrix.CreateMatrix(plainText, row, column, Mode.Row);
            
            // Find The Cipher Text
            Matrix cipherTextMatrix = keyMatrix.Multiply(plainTextMatrix);

            // Return the Cipher Text as a List
            return cipherTextMatrix.ToList(Mode.Row);
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            // this is 2x2 key analysis
            int KEY_SIZE = 3;
            int column = KEY_SIZE, row = (int)Math.Ceiling(((double)cipherText.Count / KEY_SIZE));

            cipherText = Helper.PadListWith(cipherText, row * column, 0);
            plainText = Helper.PadListWith(plainText, row * column, 0);

            // Brute force all combinations
            for (int i = 0; i < row - 2; i++)
            {
                for (int j = i + 1; j < row - 1; j++)
                {
                    for(int k = j + 1; k < row; k++)
                    {
                        // For each iteration
                        Matrix cipherTextMatrix = new Matrix(new int[KEY_SIZE, KEY_SIZE]);
                        Matrix plainTextMatrix = new Matrix(new int[KEY_SIZE, KEY_SIZE]);

                        // Fill the matrices in ROW MODE
                        for(int c = 0; c < KEY_SIZE; c++)
                        {
                            // Fill cipher text matrix in ROW MODE
                            cipherTextMatrix[0, c] = cipherText[i * column + c];
                            cipherTextMatrix[1, c] = cipherText[j * column + c];
                            cipherTextMatrix[2, c] = cipherText[k * column + c];

                            // Fill plain text matrix in ROW MODE
                            plainTextMatrix[0, c] = plainText[i * column + c];
                            plainTextMatrix[1, c] = plainText[j * column + c];
                            plainTextMatrix[2, c] = plainText[k * column + c];
                        }

                        try
                        {
                            Matrix keyMatrix = plainTextMatrix.Inverse().Multiply(cipherTextMatrix);
                            return keyMatrix.ToList(Mode.Column);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }

            throw new InvalidAnlysisException();
        }

    }
}
