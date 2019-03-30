using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace ChatMainServer{
    public static class Authentication{

        public static User SignUp(string username, string password){
            var foundUser = Configs.userCollection.Find(new BsonDocument(){
                {"Username", username}
            }).FirstOrDefault();
            if(foundUser != null){
                throw (new UserAuthenticationException("User Already Exists"));
            }else{
                User user = new User(username, Utility.GetHash256String(password) );
                BsonDocument bsonUser= user.ToBsonDocument();
                Configs.userCollection.InsertOne(bsonUser);
                return user;
            }
            
        }


        public static bool SignIn(string username, string password){
            var user = Configs.userCollection.Find(new BsonDocument(){
                {"Username" , username}
            }).FirstOrDefault();
            if(user != null){
                User foundUser = BsonSerializer.Deserialize<User>(user);
                if( Utility.CheckHashMatch( password, foundUser.Password ) ){
                    foundUser.IsLoggedIn = true;
                    UserController.UpdateUserInfo(foundUser);
                    return true;
                }
            }

            return false;
        }

        public static void SignOut(string username){
            var user = Configs.userCollection.Find(new BsonDocument(){
                { "Username", username }
            });

            if(user != null){
                
            }
        }

        public static void OnlineUserList(){
            var onlineUserListBson = Configs.userCollection.Find("{ IsLoggedIn : true }").Project("{ _id : 1, Username : 1 }").ToList();
            Console.WriteLine("LIST OF ONLINE USERS : ");
            foreach(BsonDocument onlineUserBson in onlineUserListBson){
                User onlineUser = BsonSerializer.Deserialize<User>(onlineUserBson);
                Console.WriteLine("USERNAME : {0}",onlineUser.Username);
            }
            Console.WriteLine("End OF ONLINE USER LIST");
        }
    }
}