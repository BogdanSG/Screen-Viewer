using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ScreenViewer {
    public class EncryptionString {

        public static string Encrypt(string ishText, string pass = "24DC266E28186BD7453B7C671442B",
       string sol = "E58BC9CCD6164B788917A322369AD", string cryptographicAlgorithm = "SHA1",
       int passIter = 2, string initVec = "b8doAgHhtOz2hZe#",
       int keySize = 256) {

            if (string.IsNullOrEmpty(ishText))
                return "";

            byte[] initVecB = Encoding.UTF8.GetBytes(initVec);

            byte[] solB = Encoding.UTF8.GetBytes(sol);

            byte[] ishTextB = Encoding.UTF8.GetBytes(ishText);


            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);

            byte[] keyBytes = derivPass.GetBytes(keySize / 8);

            RijndaelManaged symmK = new RijndaelManaged();

            symmK.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmK.CreateEncryptor(keyBytes, initVecB)) {

                using (MemoryStream memStream = new MemoryStream()) {

                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write)) {

                        cryptoStream.Write(ishTextB, 0, ishTextB.Length);

                        cryptoStream.FlushFinalBlock();

                        cipherTextBytes = memStream.ToArray();

                        memStream.Close();

                        cryptoStream.Close();

                    }//using

                }//using

            }//using

            symmK.Clear();

            return Convert.ToBase64String(cipherTextBytes);

        }//Encrypt

        public static string Decrypt(string ciphText, string pass = "24DC266E28186BD7453B7C671442B",
               string sol = "E58BC9CCD6164B788917A322369AD", string cryptographicAlgorithm = "SHA1",
               int passIter = 2, string initVec = "b8doAgHhtOz2hZe#",
               int keySize = 256) {

            if (string.IsNullOrEmpty(ciphText))
                return "";

            byte[] initVecB = Encoding.UTF8.GetBytes(initVec);

            byte[] solB = Encoding.UTF8.GetBytes(sol);

            byte[] cipherTextBytes = Convert.FromBase64String(ciphText);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);

            byte[] keyBytes = derivPass.GetBytes(keySize / 8);

            RijndaelManaged symmK = new RijndaelManaged();

            symmK.Mode = CipherMode.CBC;

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int byteCount = 0;

            using (ICryptoTransform decryptor = symmK.CreateDecryptor(keyBytes, initVecB)) {

                using (MemoryStream mSt = new MemoryStream(cipherTextBytes)) {

                    using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read)) {

                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                        mSt.Close();

                        cryptoStream.Close();

                    }//using

                }//using

            }//using

            symmK.Clear();

            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);

        }//Decrypt

    }//EncryptionString

}//ScreenViewer
