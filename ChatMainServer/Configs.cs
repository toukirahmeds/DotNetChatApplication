using System;
using MongoDB.Driver;
using MongoDB.Bson;
using RabbitMQ.Client;

namespace ChatMainServer{
     public static class Configs{
        public static MongoClient dbClient;
        public static IMongoDatabase db;
        public static IMongoCollection<BsonDocument> userCollection;
        public static IMongoCollection<BsonDocument> chatRoomCollection;
        public static ConnectionFactory rabbitConnectionFactory;
        public static string RabbitMQChatKey = "ChatKey";

        public static IConnection rabbitConnection;


        public static void MongoConnect(){
            Configs.dbClient = new MongoClient("mongodb://localhost:27017");
            Configs.db = Configs.dbClient.GetDatabase("ChatApp");
            Configs.userCollection = Configs.db.GetCollection<BsonDocument>("users");
            Configs.chatRoomCollection = Configs.db.GetCollection<BsonDocument>("chatRooms");
            Console.WriteLine("Connected to MongoDB Server.");
        }

        public static void RabbitMQConnect(){
            Configs.rabbitConnectionFactory = new ConnectionFactory(){ HostName = "localhost" };
            Configs.rabbitConnection = Configs.rabbitConnectionFactory.CreateConnection();
            Console.WriteLine("Connected to RabbitMQ Server.");
        }

        public static void SetConfigs(){
            MongoConnect();
            RabbitMQConnect();
        }
    }
}