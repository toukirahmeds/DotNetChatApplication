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
                connectedUsers.Add( new ChatUser( new ObjectId(user["UserId"].ToString()), user["Username"].ToString()) );
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
            if(cr.HasUser(user.Username)){
                ConnectionFactory f = new ConnectionFactory(){HostName = "localhost"};
                using(IConnection con = f.CreateConnection())
                using(IModel channel = con.CreateModel()){
                    channel.QueueDeclare(
                        queue : "hello",
                        autoDelete: false,
                        exclusive : false,
                        durable : false,
                        arguments : null
                    );

                    var body = Encoding.UTF8.GetBytes(message);
                    foreach(ChatUser cu in cr.ConnectedUsers){
                        if(user.Id.ToString().CompareTo(cu.UserId.ToString()) != 0){
                            channel.BasicPublish(
                                exchange : "",
                                routingKey : "hello",
                                mandatory : true,
                                basicProperties : null,
                                body : body
                            );
                            cu.setMessageReceiver();
                        }
                            
                    }
                    
                    cr.AddChatMessage( user.Id, message );
                    UpdateChatRoom(cr);

                }
            }else{
                throw (new ChatRoomException("User do not belong to this chat room."));
            }
            
            
            
        }
    }
    
}