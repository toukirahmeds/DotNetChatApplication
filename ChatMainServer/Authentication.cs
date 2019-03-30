using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace ChatMainServer{
    public static class Authentication{
        public static string space = "\t\t\t\t\t\t";

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


        public static User SignIn(string username, string password){
            var user = Configs.userCollection.Find(new BsonDocument(){
                {"Username" , username}
            }).FirstOrDefault();
            if(user != null){
                User foundUser = BsonSerializer.Deserialize<User>(user);
                if( Utility.CheckHashMatch( password, foundUser.Password ) ){
                    foundUser.IsLoggedIn = true;
                    UserController.UpdateUserInfo(foundUser);
                    Console.WriteLine("User Signing In Successful");
                    return foundUser;
                }else{
                    Console.WriteLine("Wrong Username or password");
                }
            }

            return null;
        }

        public static void SignOut(this User user){
            user.IsLoggedIn = false;
            Configs.userCollection.FindOneAndUpdate(
                new BsonDocument(){ { "Username", user.Username } },
                user.ToBsonDocument()
            );
            Console.WriteLine("SUCCESSFULLY SIGNED OUT");
        }

        public static void OnlineUserList(){
            var onlineUserListBson = Configs.userCollection.Find("{ IsLoggedIn : true }").Project("{ _id : 1, Username : 1 }").ToList();
            Console.WriteLine(space + "=================LIST OF ONLINE USERS ======================= ");
            if(onlineUserListBson.Count == 0){
                Console.WriteLine(space + "==================End OF ONLINE USER LIST============================");
            }else{
                int serial = 1;
                foreach(BsonDocument onlineUserBson in onlineUserListBson){
                    User onlineUser = BsonSerializer.Deserialize<User>(onlineUserBson);
                    Console.WriteLine(space + "{0}) USERNAME : {1}", serial, onlineUser.Username);
                    serial++;
                }
            }
            
            Console.WriteLine(space + "==================End OF ONLINE USER LIST============================");
        }
    }
}