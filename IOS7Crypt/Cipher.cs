using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOS7Crypt
{
    class Cipher
    {
        private Random random;

        public static int[] xlat = {
            0x64, 0x73, 0x66, 0x64, 0x3b, 0x6b, 0x66, 0x6f,
            0x41, 0x2c, 0x2e, 0x69, 0x79, 0x65, 0x77, 0x72,
            0x6b, 0x6c, 0x64, 0x4a, 0x4b, 0x44, 0x48, 0x53,
            0x55, 0x42, 0x73, 0x67, 0x76, 0x63, 0x61, 0x36,
            0x39, 0x38, 0x33, 0x34, 0x6e, 0x63, 0x78, 0x76,
            0x39, 0x38, 0x37, 0x33, 0x32, 0x35, 0x34, 0x6b,
            0x3b, 0x66, 0x67, 0x38, 0x37
        };

        public static String encrypt(String password) {
            int seed = new Random().Next(0, 16);

            int[] key = new int[password.Length];
            for (int i = 0; i < password.Length; i++) {
                key[i] = xlat[(seed + i) % xlat.Length];
            }

            int[] plaintext = new int[password.Length];
            for (int i = 0; i < password.Length; i++) {
                plaintext[i] = (int) password[i];
            }

            int[] ciphertext = new int[password.Length];

            for (int i = 0; i < password.Length; i++) {
                ciphertext[i] = plaintext[i] ^ key[i];
            }

            String[] hexpairs0 = new String[password.Length];
            for (int i = 0; i < ciphertext.Length; i++) {
                hexpairs0[i] = String.Format("{0:x2}", ciphertext[i]);
            }

            String hexpairs = String.Join("", hexpairs0);

            String hash = String.Format("{0:d2}{1}", seed, hexpairs);

            return hash;
        }

        public static String decrypt(String hash) {
            if (hash.Length < 4) {
                return "";
            }

            try
            {
                int seed = int.Parse(hash.Substring(0, 2));

                String hexpairs = hash.Substring(2);
                String[] hexpairs0 = new String[hexpairs.Length / 2];

                for (int i = 0; i < hexpairs.Length / 2; i++)
                {
                    hexpairs0[i] = hexpairs.Substring(i * 2, 2);
                }

                int[] ciphertext = new int[hexpairs0.Length];
                for (int i = 0; i < hexpairs0.Length; i++)
                {
                    ciphertext[i] = Int32.Parse(hexpairs0[i], System.Globalization.NumberStyles.HexNumber);
                }

                int[] key = new int[ciphertext.Length];
                for (int i = 0; i < ciphertext.Length; i++)
                {
                    key[i] = xlat[(seed + i) % xlat.Length];
                }

                int[] plaintext = new int[ciphertext.Length];
                for (int i = 0; i < ciphertext.Length; i++)
                {
                    plaintext[i] = ciphertext[i] ^ key[i];
                }

                byte[] plaintext0 = new byte[plaintext.Length];
                for (int i = 0; i < plaintext.Length; i++)
                {
                    plaintext0[i] = (byte)plaintext[i];
                }

                return System.Text.Encoding.UTF8.GetString(plaintext0, 0, plaintext0.Length);
            }
            catch (Exception e)
            {
                var e0 = e;
                return "";
            }
        }
    }
}