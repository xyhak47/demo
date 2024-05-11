using System;
using UnityEngine;


/// <summary>
/// XXTEA 加密算法的摘要说明
/// </summary>
public class XXTEA
{
    public static Byte[] Encrypt(Byte[] Data, Byte[] Key)
    {
        if (Data.Length == 0)
        {
            return Data;
        }
        return ToByteArray(Encrypt(ToUInt32Array(Data, true), ToUInt32Array(Key, false)), false);
    }
    public static Byte[] Decrypt(Byte[] Data, Byte[] Key)
    {
        if (Data.Length == 0)
        {
            return Data;
        }
        return ToByteArray(Decrypt(ToUInt32Array(Data, false), ToUInt32Array(Key, false)), true);
    }
    public static UInt32[] Encrypt(UInt32[] v, UInt32[] k)
    {
        Int32 n = v.Length - 1;
        if (n < 1)
        {
            return v;
        }
        if (k.Length < 4)
        {
            UInt32[] Key = new UInt32[4];
            k.CopyTo(Key, 0);
            k = Key;
        }
        UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum = 0, e;
        Int32 p, q = 6 + 52 / (n + 1);
        while (q-- > 0)
        {
            sum = unchecked(sum + delta);
            e = sum >> 2 & 3;
            for (p = 0; p < n; p++)
            {
                y = v[p + 1];
                z = unchecked(v[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
            }
            y = v[0];
            z = unchecked(v[n] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
        }
        return v;
    }
    public static UInt32[] Decrypt(UInt32[] v, UInt32[] k)
    {
        Int32 n = v.Length - 1;
        if (n < 1)
        {
            return v;
        }
        if (k.Length < 4)
        {
            UInt32[] Key = new UInt32[4];
            k.CopyTo(Key, 0);
            k = Key;
        }
        UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum, e;
        Int32 p, q = 6 + 52 / (n + 1);
        sum = unchecked((UInt32)(q * delta));
        while (sum != 0)
        {
            e = sum >> 2 & 3;
            for (p = n; p > 0; p--)
            {
                z = v[p - 1];
                y = unchecked(v[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
            }
            z = v[n];
            y = unchecked(v[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
            sum = unchecked(sum - delta);
        }
        return v;
    }
    private static UInt32[] ToUInt32Array(Byte[] Data, Boolean IncludeLength)
    {
        Int32 n = (((Data.Length & 3) == 0) ? (Data.Length >> 2) : ((Data.Length >> 2) + 1));
        UInt32[] Result;
        if (IncludeLength)
        {
            Result = new UInt32[n + 1];
            Result[n] = (UInt32)Data.Length;
        }
        else
        {
            Result = new UInt32[n];
        }
        n = Data.Length;
        for (Int32 i = 0; i < n; i++)
        {
            Result[i >> 2] |= (UInt32)Data[i] << ((i & 3) << 3);
        }
        return Result;
    }
    private static Byte[] ToByteArray(UInt32[] Data, Boolean IncludeLength)
    {
        Int32 n;
        if (IncludeLength)
        {
            n = (Int32)Data[Data.Length - 1];
        }
        else
        {
            n = Data.Length << 2;
        }
        Byte[] Result = new Byte[n];
        for (Int32 i = 0; i < n; i++)
        {
            Result[i] = (Byte)(Data[i >> 2] >> ((i & 3) << 3));
        }
        return Result;
    }
}



public class XXTEAManager
{
    #region XXTEA 加密函数
    /// <summary>
    /// 加密字符串
    /// </summary>
    /// <param name="Str">传入的明文</param>
    /// <returns>返回密文</returns>
    public static string Encrypt(string Str)
    {
        System.Text.Encoding encoder = System.Text.Encoding.UTF8;
        Byte[] data = XXTEA.Encrypt(encoder.GetBytes(Str), encoder.GetBytes("KeyKeyKeyKey"));
        return System.Convert.ToBase64String(data);
    }
    #endregion

    #region XXTEA 解密函数
    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="Str">传入的加密字符串</param>
    /// <returns>返回明文</returns>
    public static string Decrypt(string Str)
    {
        System.Text.Encoding encoder = System.Text.Encoding.UTF8;
        string DeStr = string.Empty;
        try
        {
            DeStr = encoder.GetString(XXTEA.Decrypt(System.Convert.FromBase64String(Str), encoder.GetBytes("KeyKeyKeyKey")));
        }
        catch (Exception)
        {
            //return null;
        }
        return DeStr;
    }
    #endregion
}


