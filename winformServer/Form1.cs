using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winformServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Received();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Received();
        }


        public void Received()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.UserName = "yy";
            factory.Password = "hello";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("hello", true, consumer);

                    //Console.WriteLine(" waiting for message.");
                    Write($" waiting for message.");
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        //Console.WriteLine("Received {0}", message);
                        Write($"Received {message}");
                    }
                }
            }
        }

        public void Write(string message)
        {
            textBox1.Text += $" \r\n  {message}";
        }
    }
}
