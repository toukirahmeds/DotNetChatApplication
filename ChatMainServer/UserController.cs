using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ChatMainServer{
    public static class UserController{
        

        public static User CreateUser(string username, string password){
            var foundUser = Configs.userCollection.Find(new BsonDocument(){
                {"Username", username}
            }).FirstOrDefault();
            if(foundUser != null){
                throw (new UserAuthenticationException("User Already Exists"));
            }else{
                User user = new User(username, password);
                BsonDocument bsonUser= user.ToBsonDocument();
                Configs.userCollection.InsertOne(bsonUser);
                return user;
            }
            
        }


        public static bool UpdateUserInfo(User user){
            Configs.userCollection.FindOneAndUpdate<BsonDocument>( new BsonDocument(){
                {"_id", user.Id }
            }, user.ToBsonDocument() );
            return true;
        }

        public static User GetUserUsingUsername(string username){
            var mq = Configs.userCollection.Find(new BsonDocument(){
                {"Username", username}
            }).FirstOrDefault();
            Console.WriteLine(BsonSerializer.Deserialize<User>(mq));
            return BsonSerializer.Deserialize<User>(mq);
        }
    }
}