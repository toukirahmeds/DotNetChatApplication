using System;

using System.Text;
using System.Linq;
using System.Collections;
using System.Security.Cryptography;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace ChatMainServer
{

    public static class Utility{
        public static string GetHash256String(string myString){
            if(String.IsNullOrEmpty(myString)) return String.Empty;
            using( SHA256 sha = SHA256.Create() ){
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(myString);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }

    public class User{
        private string _id, username, password;
        private bool isLoggedIn = false;
        public User(string username, string password){
            this.username = username;
            this.password = Utility.GetHash256String(password);
            this.isLoggedIn = false;
        }

        public string Username{
            get { return this.username;}
            set { this.username = value;}
        }

        public string Password{
            get { return this.password;}
            set { this.password = Utility.GetHash256String(value);}
        }

        public bool IsLoggedIn{
            get { return this.isLoggedIn;}
            set { this.isLoggedIn = value;}
        }

        public void Display(){
            Console.WriteLine("username : {0}, password : {1}", this.Username, this.Password);
        }
    }

    public class ChatRoom{
        private ArrayList connectedUserIds;

        public ChatRoom(){
            this.connectedUserIds = new ArrayList();
        }

        public bool AddUser(string userId){
            this.connectedUserIds.Add(userId);
            return true;
        }


        public ArrayList ConnectedUsers{
            get { return this.connectedUserIds;}
        }
    }

    

    class Program
    {
        static MongoClient dbClient;
        static IMongoDatabase db;
        static IMongoCollection<BsonDocument> userCollection;
        static ConnectionFactory rabbitConnectionFactory;

        static void MongoConnect(){
            Program.dbClient = new MongoClient("mongodb://localhost:27017");
            Program.db = Program.dbClient.GetDatabase("ChatApp");
            Program.userCollection = Program.db.GetCollection<BsonDocument>("user");
            Console.WriteLine("Connected to MongoDB Server.");
        }

        static void RabbitMQConnect(){
            Program.rabbitConnectionFactory = new ConnectionFactory(){ HostName = "localhost" };
            Console.WriteLine("Connected to RabbitMQ Server.");
        }

        

        public static bool CreateUser(string username, string password){
            User user = new User(username, password);
            BsonDocument bsonUser= user.ToBsonDocument();
            Program.userCollection.InsertOne(bsonUser);
            Console.WriteLine("Successfully Create a new user: {0}", bsonUser);
            return true;
        }


        public static bool CheckPasswordMatch(string givenString, string encodedString){
            string givenStringEncoded = Utility.GetHash256String(givenString);
            Console.WriteLine( givenStringEncoded );
            Console.WriteLine( encodedString);
            Console.WriteLine( givenStringEncoded.CompareTo(encodedString) );
            return false;
        }

        public static bool SignIn(string username, string password){
            
            return false;
        }

        public static void SignOut(string username){

        }




        static void Main(string[] args)
        {
            Program.MongoConnect();
            Program.RabbitMQConnect();
            Program.CreateUser("toukir","123456");
        }
    }
}
