using System;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace Security
{
   public class RijndaelProvider
    {

        private RijndaelManaged rijndael =
                 new RijndaelManaged();

        private UTF8Encoding m_utf8 = new UTF8Encoding();
        private byte[] key;
        private byte[] iv;

        private PaddingMode paddingMode;
        public System.Security.Cryptography.PaddingMode PaddingMode
        {
            get { return paddingMode; }
            set { paddingMode = value; rijndael.Padding = paddingMode; }
        }
        private CipherMode cipherMode;
        public System.Security.Cryptography.CipherMode CipherMode
        {
            get { return cipherMode; }
            set { cipherMode = value; rijndael.Mode = cipherMode; }
        }

        public RijndaelProvider ( byte[] key , byte[] iv )
        {
            this.key = key;
            this.iv = iv;
            this.rijndael.Padding = PaddingMode.PKCS7;
            this.rijndael.BlockSize = 128;
            this.rijndael.KeySize = 256;
        }

        public byte[] Encrypt(byte[] input)
        {
            try
            {
                return Transform(input,
                   rijndael.CreateEncryptor(key, iv));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public byte[] Decrypt(byte[] input)
        {
            try
            {
                return Transform(input,
                    rijndael.CreateDecryptor(key, iv));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Encrypt(string text)
        {
            try
            {
                byte[] input = m_utf8.GetBytes(text);
                byte[] output = Transform(input,
                                rijndael.CreateEncryptor(key, iv));
                return Convert.ToBase64String(output);
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public string Decrypt(string text)
        {
            try
            {
                byte[] input = Convert.FromBase64String(text);
                byte[] output = Transform(input,
                                rijndael.CreateDecryptor(key, iv));
                return m_utf8.GetString(output);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private byte[] Transform(byte[] input,
                       ICryptoTransform CryptoTransform)
        {

            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream,
                         CryptoTransform, CryptoStreamMode.Write);

            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();

            memStream.Position = 0;
            byte[] result = memStream.ToArray();

            memStream.Close();
            cryptStream.Close();

            return result;
        }
    }
}