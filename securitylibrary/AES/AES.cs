﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        enum Direction
        {
            Left,
            Right
        }

        private static readonly byte[,] Sbox = new byte[16, 16] {
            {0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67,
            0x2B, 0xFE, 0xD7, 0xAB, 0x76}, {0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59,
            0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0}, {0xB7,
            0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1,
            0x71, 0xD8, 0x31, 0x15}, {0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05,
            0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75}, {0x09, 0x83,
            0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29,
            0xE3, 0x2F, 0x84}, {0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B,
            0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF}, {0xD0, 0xEF, 0xAA,
            0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C,
            0x9F, 0xA8}, {0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC,
            0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2}, {0xCD, 0x0C, 0x13, 0xEC,
            0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19,
            0x73}, {0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE,
            0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB}, {0xE0, 0x32, 0x3A, 0x0A, 0x49,
            0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79},
            {0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4,
            0xEA, 0x65, 0x7A, 0xAE, 0x08}, {0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6,
            0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A}, {0x70,
            0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9,
            0x86, 0xC1, 0x1D, 0x9E}, {0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E,
            0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF}, {0x8C, 0xA1,
            0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0,
            0x54, 0xBB, 0x16}};

        // Rijndael's inverse S-box as a 2-dimentional matrix
        private static readonly byte[,] InvSBox = new byte[16, 16] {
            {0x52, 0x9, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3,
             0x9e, 0x81, 0xf3, 0xd7, 0xfb}, {0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f,
             0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb},
            {0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0xb,
             0x42, 0xfa, 0xc3, 0x4e}, {0x8, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24,
             0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25}, {0x72, 0xf8,
             0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d,
             0x65, 0xb6, 0x92}, {0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda,
             0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84}, {0x90, 0xd8, 0xab,
             0x0, 0x8c, 0xbc, 0xd3, 0xa, 0xf7, 0xe4, 0x58, 0x5, 0xb8, 0xb3, 0x45,
             0x6}, {0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0xf, 0x2, 0xc1, 0xaf,
             0xbd, 0x3, 0x1, 0x13, 0x8a, 0x6b}, {0x3a, 0x91, 0x11, 0x41, 0x4f,
             0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73},
             {0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37,
             0xe8, 0x1c, 0x75, 0xdf, 0x6e}, {0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29,
             0xc5, 0x89, 0x6f, 0xb7, 0x62, 0xe, 0xaa, 0x18, 0xbe, 0x1b}, {0xfc,
             0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe,
             0x78, 0xcd, 0x5a, 0xf4}, {0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x7, 0xc7,
             0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f}, {0x60, 0x51,
             0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0xd, 0x2d, 0xe5, 0x7a, 0x9f, 0x93,
             0xc9, 0x9c, 0xef}, {0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0,
             0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61}, {0x17, 0x2b, 0x4,
             0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21,
             0xc, 0x7d}};

        private readonly byte[][] ROUND_CONSTANT = new byte[][]
        {
            new byte[] { 0x01, 0x00, 0x00, 0x00},
            new byte[] { 0x02, 0x00, 0x00, 0x00},
            new byte[] { 0x04, 0x00, 0x00, 0x00},
            new byte[] { 0x08, 0x00, 0x00, 0x00},
            new byte[] { 0x10, 0x00, 0x00, 0x00},
            new byte[] { 0x20, 0x00, 0x00, 0x00},
            new byte[] { 0x40, 0x00, 0x00, 0x00},
            new byte[] { 0x80, 0x00, 0x00, 0x00},
            new byte[] { 0x1B, 0x00, 0x00, 0x00},
            new byte[] { 0x36, 0x00, 0x00, 0x00},
        };

        private int[] MIX_COLUMNS_MATRIX =
        {
             2,   3,   1,   1 ,
             1,   2,   3,   1 ,
             1,   1,   2,   3 ,
             3,   1,   1,   2 
        };

        private int[] INV_MIX_COLUMNS_MATRIX =
        {
             14,  11,  13,  9 ,
             9,  14,  11,  13 ,
             13,  9,  14,  11 ,
             11,  13,  9,  14 
        };

        private void RotateWord(ref byte[] word, Direction direction)
        {
            int size = word.Length;
            if (direction == Direction.Left)
                for (int i = 0; i < size - 1; i++)
                    (word[i], word[i + 1]) = (word[i + 1], word[i]);

            else if(direction == Direction.Right)
                for (int i = size - 1; i > 0; i--)
                    (word[i], word[i - 1]) = (word[i - 1], word[i]);
        }

        public void LeftShiftByOne(ref BitArray bitArray) {
            for (int i = 1; i < bitArray.Count; i++)
                bitArray[i - 1] = bitArray[i];
            
            bitArray[bitArray.Count - 1] = false;
        }

        public static byte[] XOR(byte[] arr1, byte[] arr2)
        {
            byte[] result = new byte[arr1.Length];

            for (int i = 0; i < arr1.Length; ++i)
                result[i] = (byte)(arr1[i] ^ arr2[i]);

            return result;
        }

        public byte[] SubstituteBox(byte[] word)
        {
            for(int i = 0; i < 4; i++)
            {
                string hexaStr = word[i].ToString();
                int decimalNum = int.Parse(hexaStr);
                hexaStr = decimalNum.ToString("X");
                int index1 = 0;
                int index2 = index2 = Convert.ToInt32(hexaStr[0].ToString(), 16);
                if (hexaStr.Length > 1)
                {
                    index1 = Convert.ToInt32(hexaStr[0].ToString(), 16);
                    index2 = Convert.ToInt32(hexaStr[1].ToString(), 16);
                }
                word[i] = Sbox[index1,index2];
            }
            return word;
        }

        public static byte[] ConvertStringToByteArray(string hex)
        {
            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public void MixColumns(byte[] plain)
        {
            BitArray[] plainBits = new BitArray[plain.Length];
            BitArray xorValue = new BitArray(new byte[] {0x1B});

            for (int i = 0; i < plain.Length; i++) 
                plainBits[i] = new BitArray(new byte[] { plain[i] });

            for(int i = 0; i < 16; i++)
            {
                for(int j = 1; j < MIX_COLUMNS_MATRIX[i]; j++)
                {
                    bool isOne = plainBits[i].Get(7);
                    LeftShiftByOne(ref plainBits[i]);
                    if (isOne)
                        plainBits[i].Xor(xorValue);
                }
            }

            int counter = 0;
            for(int i = 0; i < 16; i++)
            {
                counter = 0;
                for(int x = 0; x < 4; x++)
                {
                    BitArray[] vector = new BitArray[4];
                    for (int j = 1; j < MIX_COLUMNS_MATRIX[i]; j++)
                    {
                        bool isOne = plainBits[i].Get(7);
                        LeftShiftByOne(ref plainBits[i]);
                        if (isOne)
                            plainBits[i].Xor(xorValue);
                    }
                    counter++;
                }
            }
        }

        public byte[,] GenerateKeyForAllRounds(byte[] mainKey)
        {
            byte[,] rounds = new byte[11,16];
            byte[,] previousWords = new byte[4, 4];
            byte[,] CurrentWords = new byte[4, 4];
            int counter = 0;
            for(int keyCount = 0; keyCount < 16; keyCount++)
                rounds[0, keyCount] = mainKey[keyCount];

            for(int roundCount = 1; roundCount < 11; roundCount++)
            {
                counter = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        previousWords[i, j] = rounds[roundCount-1,counter];
                        counter++;
                    }
                }

                byte[] previousWord0 = new byte[4];
                for (int x = 0; x < 4; x++)
                    previousWord0[x] = previousWords[0, x];

                byte[] roundConstant = ROUND_CONSTANT[roundCount-1];
                byte[] currentword0 = new byte[4];
                currentword0 = XOR(previousWord0, roundConstant);

                byte[] previousWord3 = new byte[4];
                for (int x = 0; x < 4; x++)
                    previousWord3[x] = previousWords[3, x];
                
                RotateWord(ref previousWord3, Direction.Right);
                previousWord3 = SubstituteBox(previousWord3);
                currentword0 = XOR(currentword0,previousWord3);
                
                for (int x = 0; x < 4; x++)
                    CurrentWords[0, x] = currentword0[x];

                byte[] previous = new byte[4];
                byte[] current = new byte[4];
                byte[] word = new byte[4];
                for (int wordNum = 1; wordNum < 4; wordNum++)
                {
                    for(int x = 0; x < 4; x++) { 
                        previous[x] = previousWords[wordNum, x];
                        current[x] = CurrentWords[wordNum-1, x];
                    }
                    word = XOR(previous, current);
                    for(int x = 0; x < 4; x++)
                        CurrentWords[wordNum, x] = word[x];
                }

                int keyIndex = 0;
                for (int wordNum = 0; wordNum < 4; wordNum++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        mainKey[keyIndex] = CurrentWords[wordNum, x];
                        keyIndex++;
                    }
                }

                for (int keyCount = 0; keyCount < 16; keyCount++)
                    rounds[roundCount, keyCount] = mainKey[keyCount];
            }

            return rounds;
        }

        public override string Decrypt(string cipherText, string key)
        {
            key = key.Substring(2, key.Length - 2); //remove 0x
            byte[] keyByteArray = ConvertStringToByteArray(key);
            cipherText = cipherText.Substring(2, cipherText.Length - 2); //remove 0x
            byte[] cipherTextArray = ConvertStringToByteArray(cipherText);
            byte[,] rounds = GenerateKeyForAllRounds(keyByteArray);
            MixColumns(cipherTextArray);
            return null;
        }

        public override string Encrypt(string plainText, string key)
        {
            byte[][] keyMatrix = Helper.ConvertStringToSquareByte(key); 
            byte[][] plainTextMatrix = Helper.ConvertStringToSquareByte(plainText);
            return null;

        }
    }
}
