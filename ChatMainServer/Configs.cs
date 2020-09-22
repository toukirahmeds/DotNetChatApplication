using System;
using MongoDB.Driver;
using MongoDB.Bson;
using RabbitMQ.Client;

using Amazon.S3;
using Amazon;
using Amazon.S3.Model;

namespace ChatMainServer{
     public static class Configs{
        public static MongoClient dbClient;
        public static IMongoDatabase db;
        public static IMongoCollection<BsonDocument> userCollection;
        public static IMongoCollection<BsonDocument> chatRoomCollection;
        public static ConnectionFactory rabbitConnectionFactory;
        public static string RabbitMQChatKey = "ChatKey";

        public static IConnection rabbitConnection;

        public static string S3AccessKey = "[S3_ACCESS_KEY]";
        public static string S3SecretKey = "[S3_SECRET_KEY]";
        public static AmazonS3Config S3Config;
        public static AmazonS3Client S3Client;

        public static string S3BucketName = "stp-selise";

        public static string FileDownloadPath = "./DOWNLOADS/";

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

        public static void S3Connect(){
            S3Config = new AmazonS3Config();
            S3Client = new AmazonS3Client(
                Configs.S3AccessKey,
                Configs.S3SecretKey,
                RegionEndpoint.USEast1
            );
            Console.WriteLine("Connected to S3 Client");
        }

        public static void SetConfigs(){
            MongoConnect();
            RabbitMQConnect();
            S3Connect();
        }
    }
}
