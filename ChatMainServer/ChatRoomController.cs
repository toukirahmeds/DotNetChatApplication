using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using System.Linq;
using System.Collections;
using System.Text;
using RabbitMQ.Client;

namespace ChatMainServer{
    
    public static class ChatRoomController{
        public static ChatRoom CreateChatRoom(string name){
            var foundChatRoom = Configs.chatRoomCollection.Find(new BsonDocument(){
                { "Name", name }
            }).FirstOrDefault();

            if( foundChatRoom != null) throw (new ChatRoomException("Chat room with the given name already exists."));
            else{
                ChatRoom cr = new ChatRoom(name);
                Configs.chatRoomCollection.InsertOne(cr.ToBsonDocument());
                return cr;
            }
            
        }

        public static void UpdateChatRoom(this ChatRoom cr){
            Configs.chatRoomCollection.FindOneAndUpdate<BsonDocument>(
                new BsonDocument(){ { "_id", cr.Id } },
                cr.ToBsonDocument()
            );

        }


        public static void AddChatRoomUser(this ChatRoom cr, User user){
            
            if(cr.HasUser(user.Username)){
                throw (new ChatRoomException("This user already exists in the chat room."));
            }else{
                cr.AddUser(user);
                UpdateChatRoom(cr);
            }
            
        }



        public static ChatRoom GetChatRoomUsingName(string name){
            var mq = Configs.chatRoomCollection.Find(
                new BsonDocument(){
                    {"Name", name}
                }
            ).FirstOrDefault();
            ArrayList connectedUsers = new ArrayList();
            ArrayList chatHistory = new ArrayList();
            foreach(dynamic user in mq.GetValue("ConnectedUsers").AsBsonArray){
                connectedUsers.Add( new ChatUser( new ObjectId(user["_id"].ToString()), user["Username"].ToString()) );
            }
            
            foreach(dynamic chat in mq.GetValue("ChatHistory").AsBsonArray){
                chatHistory.Add( new ChatMessage( new ObjectId(chat["UserId"].ToString()), chat["Message"].ToString() ){ MessageTime =  (DateTime)chat["MessageTime"]} );
            }
            

            return new ChatRoom(mq.GetValue("Name").ToString()){
                Id = new ObjectId( mq.GetValue("_id").ToString()  ),
                ConnectedUsers = connectedUsers,
                ChatHistory = chatHistory
            };
            
        }

        


        public static void SendMessage(this User user, ChatRoom cr, string message){
            // if(cr.HasUser(user.Username)){
            //     cr.AddChatMessage( user.Id, message );
            //     UpdateChatRoom(cr);
            // }else{
            //     throw (new ChatRoomException("User do not belong to this chat room."));
            // }
            using(IConnection connection = Configs.rabbitConnectionFactory.CreateConnection()){
                using(IModel channel = Configs.rabbitConnection.CreateModel()){
                channel.QueueDeclare(
                    queue : Configs.RabbitMQChatKey,
                    autoDelete: false,
                    exclusive : false,
                    durable : false,
                    arguments : null
                );

                string msg = "Message from user : "+user.Username.ToString() + ", chatRoom : " + cr.Name.ToString();
                var body = Encoding.UTF8.GetBytes(message);
                Console.WriteLine("Sending Message");
                channel.BasicPublish(
                    exchange : "",
                    routingKey : Configs.RabbitMQChatKey,
                    mandatory : true,
                    basicProperties : null,
                    body : body
                );
            }
            }
            
            
        }
    }
    
}