using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ChatMainServer{
    public static class UserController{
        

        


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
            User foundUser = new User( mq["Username"].ToString(), mq["Password"].ToString() );
            foundUser.Id = new ObjectId(mq["_id"].ToString());
            return foundUser;
        }

        
    }
}