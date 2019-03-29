using System;
using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatMainServer{
    public class User{
        private ObjectId _id;
        private string username, password;
        private bool isLoggedIn = false;
        public User(string username, string password){
            this._id = ObjectId.GenerateNewId();
            this.username = username;
            this.password = password;
            this.isLoggedIn = false;
            this.setMessageReceiver();
        }

        public ObjectId Id{
            get { return this._id;}
            set { this._id = value;}
        }

        public string Username{
            get { return this.username;}
            set { this.username = value;}
        }

        public string Password{
            get { return this.password;}
            set { this.password = value;}
        }

        public bool IsLoggedIn{
            get { return this.isLoggedIn;}
            set { this.isLoggedIn = value;}
        }

        public void Display(){
            Console.WriteLine("username : {0}, password : {1}", this.Username, this.Password);
        }

        public void setMessageReceiver(){
            using(IConnection connection = Configs.rabbitConnectionFactory.CreateConnection()){
                using(IModel channel = connection.CreateModel()){
                channel.QueueDeclare(
                    queue : Configs.RabbitMQChatKey,
                    durable : false,
                    autoDelete : false,
                    exclusive : false,
                    arguments : null
                );

                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea)=>{
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Received Message by {0} from {1} : {2}", this.username, "MyChatRoom", message);
                };

                Console.WriteLine("User : "+this.Username.ToString() + " connecting to the rabbitmq server");

                channel.BasicConsume(
                    queue : Configs.RabbitMQChatKey,
                    autoAck : true,
                    consumer : consumer
                );


            }//channel scope ends here
            }
            
        }
    }
}