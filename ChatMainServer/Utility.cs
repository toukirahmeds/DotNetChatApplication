using System;
using System.Security.Cryptography;

namespace ChatMainServer{
    public static class Utility{
        public static string GetHash256String(string myString){
            if(String.IsNullOrEmpty(myString)) return String.Empty;
            using( SHA256 sha = SHA256.Create() ){
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(myString);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public static bool CheckPasswordMatch(string givenString, string encodedString){
            string givenStringEncoded = Utility.GetHash256String(givenString);
            Console.WriteLine(givenString);
            Console.WriteLine( givenStringEncoded );
            Console.WriteLine( encodedString);
            Console.WriteLine( givenStringEncoded.CompareTo(encodedString) );
            return false;
        }

        public static void PrintList(this string[] strList){
            foreach(string str in strList) Console.WriteLine(str);
        }

        public static void PrintList(this User[] userList){
            foreach(User user in userList) Console.WriteLine("Id : {0}, Username : {1}", user.Id, user.Username);
        }
    }
}