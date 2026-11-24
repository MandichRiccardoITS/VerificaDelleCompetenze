using System;
using System.Diagnostics;
using System.Text;

class Program
{
    static string CaesarEncrypt(string msg, int offset)
    {
        char[] c = msg.ToCharArray(); //Trasformiamo una stringa in un array di caratteri

        for (int j = 0; j < c.Length; j++) //Con questo ciclo accediamo a ogni singolo valore dell'array
        {
            for (int i = 0; i < offset; i++) //Ciclo for per controllare che non si vada oltre la Z
            {
                if (c[j] < char.MaxValue) //Se il valore non è Z 
                {
                    c[j]++; //continuiamo
                }
                else //Altrimenti
                {
                    c[j] = char.MinValue; //Torniamo ad A
                }
            }
        }
        return new string(c); //Ritorna come valore la nuova stringa encriptata
    }

    /** @params msg: stringa di SOLE lettere maiuscole
    * @params offset: valore intero positivo (chiave del cifrario)
     * 
    */
    static string CaesarDecrypt(string msg, int offset)
    {
        char[] c = msg.ToCharArray(); //Trasformiamo una stringa in un array di caratteri

        for (int j = 0; j < c.Length; j++) //Con questo ciclo accediamo a ogni singolo valore dell'array
        {
            for (int i = 0; i < offset; i++) //Ciclo for per controllare che non si vada oltre la Z
            {
                if (c[j] > 'A') //Se il valore non è Z 
                {
                    c[j]--; //continuiamo
                }
                else //Altrimenti
                {
                    c[j] = 'Z'; //Torniamo ad A
                }
            }
        }
        return new string(c); //Ritorna come valore la nuova stringa encriptata
    }
    static string VigenereEncrypt(string msg, string pwd)
    {
        char[] ret = new char[msg.Length];
        char[] pwd2 = new char[msg.Length];

        int i = 0;

        for (int j = 0; j < msg.Length; j++)
        {
            pwd2[j] = pwd[i];

            if (i < pwd.Length - 1)
            {
                i++;
            } else
            {
                i = 0;
            }
        }

        for (i = 0; i < msg.Length; i++) 
        {
            int offset = pwd2[i] - 'A' + 1;
            ret[i] = CaesarEncrypt(msg[i].ToString(), offset).ToCharArray()[0];
            //Console.WriteLine($"{msg[i]} viene criptato con {pwd2[i]} ({offset}) e diventa {ret[i]}");
        }

        return new string(ret); 
    }

    static string VigenereDecrypt(string msg, string pwd)
    {
        char[] ret = new char[msg.Length];
        char[] pwd2 = new char[msg.Length];

        int i = 0;

        for (int j = 0; j < msg.Length; j++)
        {
            pwd2[j] = pwd[i];

            if (i < pwd.Length - 1)
            {
                i++;
            }
            else
            {
                i = 0;
            }
        }

        for (i = 0; i < msg.Length; i++)
        {
            int offset = pwd2[i] - 'A' + 1;
            ret[i] = CaesarDecrypt(msg[i].ToString(), offset).ToCharArray()[0];
            //Console.WriteLine($"{msg[i]} viene criptato con {pwd2[i]} ({offset}) e diventa {ret[i]}");
        }

        return new string(ret);
    }
    static string Base64Encode(string msg)
    {
        return Base64Encode(msg, false);
    }
    static string Base64Encode(string msg, bool debug)
    {
        Byte[] byteMsg = System.Text.Encoding.UTF8.GetBytes(msg);
        if(debug) Console.WriteLine(string.Join("--", byteMsg));
        string bitMsg = "";
        foreach(Byte b in byteMsg)
        {
            bitMsg += Convert.ToString(b, 2).PadLeft(8, '0');
        }
        while(bitMsg.Length % 6 != 0)
        {
            bitMsg += "0";
        }
        string encoded = "";
        string table = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        for (int i = 0;i< bitMsg.Length; i += 6)
        {
            string subText = bitMsg.Substring(i, 6);
            encoded += table[Convert.ToInt32(subText, 2)];
            if(debug) Console.WriteLine($"{subText} -> {Convert.ToInt32(subText, 2)} -> {table[Convert.ToInt32(subText, 2)]}");
        }
        return encoded;
    }
    static string Base64Decode(string msg)
    {
        return Base64Decode(msg, false);
    }
    static string Base64Decode(string msg, bool debug)
    {
        string table = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        string bitMsg = "";
        foreach (char s in msg)
        {
            if(!table.Contains(s))
            {
                throw new Exception("Carattere non valido in Base64");
            }
            string subString = Convert.ToString(table.IndexOf(s), 2).PadLeft(6, '0');
            if(debug) Console.WriteLine($"{s} -> {table.IndexOf(s)} -> {subString}");
            bitMsg += subString;
        }
        while (bitMsg.Length % 8 != 0)
        {
            bitMsg = bitMsg.Substring(0, bitMsg.Length - 1);
        }
        Byte[] byteMsg = new Byte[bitMsg.Length / 8];
        int j = 0;
        for (int i = 0; i < bitMsg.Length; i += 8)
        {
            string subText = bitMsg.Substring(i, 8);
            byteMsg[j++] = Convert.ToByte(subText, 2);
        }
        if(debug) Console.WriteLine("Byte array:");
        foreach (Byte b in byteMsg)
        {
            if(debug) Console.WriteLine(b);
        }
        return System.Text.Encoding.UTF8.GetString(byteMsg);
    }
    static void Main()
    {
        string msg = Input.GetString("Inserisci il messaggio: ");
        foreach(string m in msg.Split(" "))
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < char.MaxValue; i++)
            {
                Console.WriteLine($"{m} with {i} -> {CaesarDecrypt(m, i)}");
                sb.Append(i + "=>" + CaesarDecrypt(m, i));
            }
            File.WriteAllText(m, sb.ToString());
        }
    }
}
