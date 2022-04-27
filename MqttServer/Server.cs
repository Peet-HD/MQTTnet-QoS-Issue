using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace MqttServer;

public class Server
{
    private IMqttServer _server;

    public Server()
    {
    }


    #region MQTT

    public void ConnectAsync(int Port = 1883)
    {
        _server = new MqttFactory()
            .CreateMqttServer();

        var options = new MqttServerOptionsBuilder()
            .WithConnectionBacklog(100)
            .WithDefaultEndpointPort(Port)
            .WithConnectionValidator(connection =>
            {
                connection.ReasonCode = MqttConnectReasonCode.Success;
            })
            .Build();

        _server.StartAsync(options);

    }


    public void PublishMessage(string message, string Topic = "test", MQTTnet.Protocol.MqttQualityOfServiceLevel qos = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
    {
        var msg = new MqttApplicationMessageBuilder()
                        .WithPayload(message)
                        .WithQualityOfServiceLevel(qos)
                        .WithTopic(Topic)
                        .Build();
        _server.PublishAsync(msg);
    }

    #endregion
}
