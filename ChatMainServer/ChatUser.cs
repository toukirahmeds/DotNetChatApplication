using System;
using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatMainServer{
    public class ChatUser{
        private ObjectId userId;
        private string username;

        public static string space = "\t\t\t\t\t\t\t";

        public ChatUser(User user){
            this.userId = user.Id;
            this.username = user.Username;
        }

        public ChatUser(ObjectId userId, string username){
            this.userId = userId;
            this.username = username;
        }


        public string Username{
            get { return this.username;}
            set { this.username = value;}
        }

        public ObjectId UserId{
            get { return this.userId;}
            set { this.userId = value;}
        }

        public static void printChatMessage(ChatUser cu, string message){
                Console.WriteLine(space + "============================================");
                Console.WriteLine(space + "RECEIVER : {0}", cu.Username);
                Console.WriteLine(space + "MESSAGE TEXT : {0}", message);
                Console.WriteLine(space + "=============================================");
        }
        public void setMessageReceiver(){
            ConnectionFactory f = new ConnectionFactory(){HostName = "localhost"};
            using(IConnection con = f.CreateConnection())
            using(IModel channel = con.CreateModel()){

                channel.QueueDeclare(
                    queue : "hello",
                    exclusive : false,
                    autoDelete : false,
                    arguments : null
                );

                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea)=>{
                    var body = ea.Body;
                    string message = Encoding.UTF8.GetString(body);
                    printChatMessage(this, message);
                    channel.Close();
                };

                channel.BasicConsume(
                    queue : "hello",
                    autoAck : true,
                    consumer : consumer
                );

                Console.ReadKey();
            }
        }
    }
}