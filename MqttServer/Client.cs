using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttServer
{
    public class Client
    {
        private ManagedMqttClientOptions _options;
        private IManagedMqttClient _client;

        public Client()
        {
        }

        public void CreateManagedClientAsync()
        {
            var optionsBuilder = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883).Build();
            _options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(optionsBuilder).Build();

            _client = new MqttFactory().CreateManagedMqttClient();
            _ = _client.UseApplicationMessageReceivedHandler(async eventArgs =>
            {
                Console.WriteLine($"Received Message with QoS-Level: {eventArgs.ApplicationMessage.QualityOfServiceLevel}");
            });
            _ = _client.UseConnectedHandler(x => { });

        }

        public async Task StartAsync()
        {
            await _client.StartAsync(_options);
        }

        public async Task StopAsync()
        {
            await _client.StopAsync();
        }

        public async Task SubscribeToTopicAsync(string topic, params string[] subtopics)
        {
            var rawTopicString = topic;
            foreach (var item in subtopics)
            {
                rawTopicString += "/" + item;
            }
            await _client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(rawTopicString).Build());
        }


        public async Task SubscribeToTopicAsync(string topic = "main/")
        {
            await _client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
        }
    }
}
