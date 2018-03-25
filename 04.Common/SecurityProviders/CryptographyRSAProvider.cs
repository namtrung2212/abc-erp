using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Windows.Forms;
using System.Collections;

namespace Security
{
    public class RSAProvider
    {

        public static bool CreateSigFile(string fileName, string privateKey, string hashValue, int bits, string outputSignfile)
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(bits);

            rsaProvider.FromXmlString(privateKey);
            byte[] output = rsaProvider.SignData(UTF8Encoding.UTF8.GetBytes(hashValue), SHA1Managed.Create());
            FileStream fsPrivate = new FileStream(outputSignfile + ".sig", FileMode.Create, FileAccess.Write);

            fsPrivate.Write(output, 0, output.Length);
            fsPrivate.Close();

            return true;
        }

        public static bool CreateSigFile(string fileName, string privateKey, string hashValue, int bits)
        {
            return CreateSigFile(fileName, privateKey, hashValue, bits, fileName);
        }

        public static bool VerifySigFile(string fileName, string publicKey, string orghashValue, int bits)
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(bits);
            FileStream signStream;

            try
            {
                signStream = new FileStream(fileName + ".sig", FileMode.Open, FileAccess.Read);
            }
            catch (System.Exception e)
            {
                 ABCHelper.ABCMessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            rsaProvider.FromXmlString(publicKey);

            byte[] signBytes = new byte[signStream.Length];
            signStream.Read(signBytes, 0, signBytes.Length);
            signStream.Close();

            return rsaProvider.VerifyData(UTF8Encoding.UTF8.GetBytes(orghashValue), SHA1Managed.Create(), signBytes);
            
        }

        public static void EncryptFile(string fileInput, string fileOutput, string publicKey, int bits)
        {
            try
            {
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(bits);
                FileStream inputStream = new FileStream(fileInput, FileMode.Open, FileAccess.Read);
                FileStream outputStream = new FileStream(fileOutput, FileMode.Create, FileAccess.Write);

                rsaProvider.FromXmlString(publicKey);

                int maxlength = rsaProvider.KeySize / 8 - 42;

                byte[] bytes = new byte[maxlength];

                int read = -1;
                while ((read = inputStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    byte[] tempBytes = new byte[read];
                    Buffer.BlockCopy(bytes, 0, tempBytes, 0, read);
                    byte[] encryptedBytes = rsaProvider.Encrypt(tempBytes, true);
                    outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                }

                inputStream.Close();
                outputStream.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static  byte[] Encrypt(byte[] textInput, string publicKey, int bits)
        {
            try
            {
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(bits);   
                rsaProvider.FromXmlString(publicKey);          

                byte[] encryptedBytes = rsaProvider.Encrypt(textInput, true);
                return encryptedBytes;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void DecryptFile(string fileInput, string fileOutput, string privateKey, int bits)
        {
            try
            {
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(bits);
                FileStream inputStream = new FileStream(fileInput, FileMode.Open, FileAccess.Read);
                FileStream outputStream = new FileStream(fileOutput, FileMode.Create, FileAccess.Write);

                rsaProvider.FromXmlString(privateKey);

                int maxlength = rsaProvider.KeySize / 8;

                byte[] bytes = new byte[maxlength];
                
                int read = -1;
                while ((read = inputStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    byte[] tempBytes = new byte[read];
                    Buffer.BlockCopy(bytes, 0, tempBytes, 0, read);
                    byte[] decryptedBytes = rsaProvider.Decrypt(tempBytes, true);
                    outputStream.Write(decryptedBytes, 0, decryptedBytes.Length);
                }

                inputStream.Close();
                outputStream.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static byte[] Decrypt(byte[] textInput, string privateKey, int bits)
        {
            try
            {
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(bits);
                rsaProvider.FromXmlString(privateKey);

                byte[] decryptedBytes = rsaProvider.Decrypt(textInput, true);
                return decryptedBytes;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static Hashtable CreateRSAKey(int bits)
        {
            Hashtable result = new Hashtable();
            RSACryptoServiceProvider rsaAlg = new RSACryptoServiceProvider(bits);

            result.Add("privateKey", rsaAlg.ToXmlString(true));
            result.Add("publicKey", rsaAlg.ToXmlString(false));

            return result;
        }
    }
}
