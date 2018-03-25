using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Security
{
    public class HashProvider
    {
        private HashAlgorithm hashProvider;

        public enum AlgorithmType : int 
		{
			MD5, SHA, SHA256, SHA384, SHA512
		}

        public HashProvider(AlgorithmType algo)
        {
           switch (algo)
           {
               case AlgorithmType.MD5:
                   hashProvider = new MD5CryptoServiceProvider();
                   break;
               case AlgorithmType.SHA:
                   hashProvider = new SHA1CryptoServiceProvider();
                   break;
               case AlgorithmType.SHA256:
                   hashProvider = new SHA256Managed();
                   break;
               case AlgorithmType.SHA384:
                   hashProvider = new SHA384Managed();
                   break;
               default:
                   hashProvider = new SHA512Managed();
                   break;
           }
        }

        private string FormatOutput(byte[] hash)
        {
            StringBuilder output = new StringBuilder(2 + (hash.Length * 2));

            foreach (byte b in hash)
            {
                output.Append(b.ToString("x2"));
            }

            return output.ToString();
        }

        public string HashFile(string filename)
        {
            return HashStream(File.OpenRead(filename));
        }

        public string HashStream(Stream stream)
        {
            return FormatOutput(hashProvider.ComputeHash(stream));
        }
        public string HashString ( String strStringForHash )
        {
            byte[] src=Encoding.Unicode.GetBytes( strStringForHash );
            return FormatOutput( hashProvider.ComputeHash( src ) );
        }
     
        
    }
}
