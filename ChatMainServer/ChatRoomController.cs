using System;
using System.IO;
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
        public static string space = "\t\t\t";
        public static string doubleSpace = "\t\t\t\t\t\t\t\t\t\t";
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

        
        public static void PrintChatMessage(User user, string message){
            Console.WriteLine(space + "============================================");
            Console.WriteLine(space + "SENDER : {0}", user.Username);
            Console.WriteLine(space + "MESSAGE TEXT : {0}", message);
            Console.WriteLine(space + "=============================================");
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
                    PrintChatMessage(user, message);
                    foreach(ChatUser cu in cr.ConnectedUsers){
                        // Console.WriteLine(cu.ToBsonDocument());
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

                    channel.Close();
                    
                    cr.AddChatMessage( user.Id, message );
                    UpdateChatRoom(cr);

                }
            }else{
                throw (new ChatRoomException("User do not belong to this chat room."));
            }
            
        }

        public static string getMessageString(string customSpace, string identifier, string username, string messageTime, string messageType, string message){
            string messageString = customSpace + "===========================================\n";
            messageString += customSpace + identifier + " : " + username + "\n";
            messageString += customSpace + "Message Time : "+ messageTime.ToString() + "\n";
            messageString += customSpace + "Message Type : " + messageType + "\n";
            messageString += customSpace + "Message Text : " + message + "\n"; 
            messageString += customSpace + "===========================================\n";

            return messageString;
        }


        public static void DownloadChatHistory(this User user){
            string chatHistory = space+" ***********CHAT HISTORY FOR : "+user.Username.ToString()+"*****************\n";
            string identifier = "";
            string customSpace = "";
            var filter = Builders<BsonDocument>.Filter.ElemMatch(
                "ConnectedUsers", Builders<BsonDocument>.Filter.Eq("Username", user.Username)
            );

            var userChatRoomList = Configs.chatRoomCollection.Find(filter).ToList();
            Console.WriteLine();
            Console.WriteLine(userChatRoomList);
            userChatRoomList.ForEach((elem)=>{
                // Console.WriteLine(elem["ChatHistory"]);
                chatHistory += "----------- CHAT ROOM : "+ elem["Name"] + "\n\n";
                elem["ChatHistory"].AsBsonArray.ToList().ForEach((chatElem)=>{
                    Console.WriteLine(chatElem);
                    if( chatElem["UserId"].ToString().CompareTo( user.Id.ToString() ) == 0 ){
                        identifier = "SENDER";
                        customSpace = space;
                    }else{
                        identifier = "RECEIVER";
                        customSpace = doubleSpace;
                    }
                    chatHistory += getMessageString( customSpace, identifier, user.Username.ToString(), chatElem["MessageTime"].ToString(), "TEXT", chatElem["Message"].ToString());
                });
                chatHistory += "\n\n\n\n";
            });
            chatHistory += space+" **********END OF CHAT HISTORY****************\n";

            Console.WriteLine(chatHistory);
            File.WriteAllText("history.txt", chatHistory);
        }
    }
    
}