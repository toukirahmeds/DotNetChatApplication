using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;

namespace ChatMainServer{
    
    public static class ChatRoomController{
        public static ChatRoom CreateChatRoom(string name){
            var foundChatRoom = Configs.chatRoomCollection.Find(new BsonDocument(){
                { "Name", name }
            }).FirstOrDefault();

            if( foundChatRoom != null) throw (new ChatRoomException("Chat room with the given name already exists."));
            else{
                ChatRoom cr = new ChatRoom(name);
                Configs.chatRoomCollection.InsertOne( new BsonDocument(){
                    { "Name", cr.Name },
                    { "ConnectedUsers", new BsonArray() },
                    { "ChatHistory", new BsonArray() }
                } );
                Console.WriteLine("New Chat Room Created");
                return cr;
            }
            
        }

        public static void UpdateChatRoom(ChatRoom cr){
            Configs.chatRoomCollection.FindOneAndUpdate<BsonDocument>(
                new BsonDocument(){ { "_id", cr.Id } },
                cr.ToBsonDocument()
            );

        }


        public static ChatRoom AddUser(ChatRoom cr, User user){
            var foundChatRoom = Configs.chatRoomCollection.Find(
                new BsonDocument(){
                    { "_id", cr.Id }
                }
            ).FirstOrDefault();
            ChatRoom newChatRoom = BsonSerializer.Deserialize<ChatRoom>(foundChatRoom);
            newChatRoom.AddUser( new ChatUser( user ) );
            UpdateChatRoom(newChatRoom);
            return newChatRoom;
        }

        public static void SendMessage(ObjectId crId, User user, string message){

        }
    }
    
}