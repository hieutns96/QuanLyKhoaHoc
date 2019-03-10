using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Web;

public static class Encryptor
{
    //update by Khiet

    //Random Key
    static string chuoinn = "1234567890qazwsxedcrfvtgbyhnujmik,ol.p;/[']";
    public static string GenRandomKey(this string nd)
    {
        Random rd = new Random((int)DateTime.Now.Ticks);
        string kq = "";
        for (int i = 0; i < 5; i++)
            kq += chuoinn[rd.Next(0, chuoinn.Length)];
        return kq;
    }

    //MD5
    public static byte[] MD5Hash(string inputString)
    {
        MD5 algorithm = new MD5CryptoServiceProvider();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }
    //Hash String 
    public static string GetHashString(byte[] getHash)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in getHash)
            sb.Append(b.ToString("X2"));
        return sb.ToString();
    }
    public static byte[] ToBytes(string hex)
    {
        var shb = SoapHexBinary.Parse(hex);
        return shb.Value;
    }
}
