using System;
using System.Security.Cryptography;
using System.Text;

namespace SuperUtility
{
    public static class SuperUtilityClass
    {

        public static string GetHash256String(string myString = ""){
            if(String.IsNullOrEmpty(myString)) return String.Empty;
            SHA256 sh = SHA256.Create();
            byte[] shaBytes = sh.ComputeHash( Encoding.UTF8.GetBytes(myString) );
            string hashedString = "";
            foreach(byte shaByte in shaBytes){
                hashedString += shaByte.ToString("x2");
            }


            return hashedString.ToString();
        }

        public static bool CheckHashMatch(string givenString, string encodedString){
            string givenStringEncoded = SuperUtilityClass.GetHash256String(givenString);
            return givenStringEncoded.CompareTo(encodedString) == 0;
        }

        public static void PrintList(this string[] strList){
            foreach(string str in strList) Console.WriteLine(str);
        }

    
    }
}
