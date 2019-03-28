using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChatMainServer{
    public static class UserController{
        

        public static void CreateUser(string username, string password){
            var foundUser = Configs.userCollection.Find(new BsonDocument(){
                {"Username", username}
            }).FirstOrDefault();
            if(foundUser != null){
                throw (new UserAuthenticationException("User Already Exists"));
            }else{
                User user = new User(username, password);
                BsonDocument bsonUser= user.ToBsonDocument();
                Configs.userCollection.InsertOne(bsonUser);
            }
            
        }


        public static bool UpdateUserInfo(User user){
            Configs.userCollection.FindOneAndUpdate<BsonDocument>( new BsonDocument(){
                {"_id", user.Id }
            }, user.ToBsonDocument() );
            return true;
        }
    }
}