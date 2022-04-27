using MqttServer;

#region Setup
Server server = new Server();
Client client = new Client();

var topic = "test";
int increment = 0;
server.ConnectAsync();

client.CreateManagedClientAsync();
client.StartAsync();

client.SubscribeToTopicAsync(topic);

System.Timers.Timer timer = new System.Timers.Timer();
timer.Elapsed += Timer_Elapsed;
timer.Interval = 1000;
timer.Start();
string message = "";
#endregion

while (Console.Read() != 'q') ;

void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
{
    Interlocked.Increment(ref increment);
    switch (increment % 3)
    {
        case 0:
            message = "AtMostOnce";
            server.PublishMessage(message, topic, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce);
            break;
        case 1:
            message = "AtLeastOnce";
            server.PublishMessage(message, topic, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);
            break;
        case 2:
            message = "ExactlyOnce";
            server.PublishMessage(message, topic, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
            break;
        default:
            throw new NotImplementedException();
    }
    Console.WriteLine($"Send with QoS-Level: {message}");
}