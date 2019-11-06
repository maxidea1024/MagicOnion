using ChatApp.Shared.Hubs;
using ChatApp.Shared.MessagePackObjects;
using ChatApp.Shared.Services;
using Grpc.Core;
using MagicOnion.Client;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ChatComponent : MonoBehaviour, IChatHubReceiver
    {
        private IMatchService matchingClient;
        private IAgonesService matchingAgones;
        private Channel agonesChannel;
        private Channel channel;
        private IChatHub streamingClient;
        private IChatService client;
        private IAgonesService agonesClient;

        private bool isJoin;
        private bool isSelfDisConnected;
        private string matchId;

        public Text ChatText;

        public Button MatchButton;
        public Text MatchButtonText;
        public Text MatchIdText;

        public Button JoinOrLeaveButton;

        public Text JoinOrLeaveButtonText;

        public Button SendMessageButton;

        public InputField Input;

        public InputField ReportInput;

        public Button SendReportButton;

        public Button DisconnectButon;
        public Button ExceptionButton;
        public Button UnaryExceptionButton;
        public string ClientId = Guid.NewGuid().ToString();


        void Start()
        {
            //this.InitializeClient();
            //this.InitializeAgonesClient();
            this.InitializeMatchClient();
            this.InitializeUi();
        }


        async void OnDestroy()
        {
            // Clean up Hub and channel
            await this.streamingClient.DisposeAsync();
            await this.channel.ShutdownAsync();
        }

        private void InitializeMatchClient()
        {
            //var matchingAgonesChannel = new Channel("13.231.213.229", 7126, ChannelCredentials.Insecure);
            var matchingAgonesChannel = new Channel("localhost", 12346, ChannelCredentials.Insecure);
            matchingClient = MagicOnionClient.Create<IMatchService>(matchingAgonesChannel);
        }

        private void InitializeAgonesClient()
        {
            //var matchingAgonesChannel = new Channel("13.231.213.229", 7126, ChannelCredentials.Insecure);
            var matchingAgonesChannel = new Channel("localhost", 12346, ChannelCredentials.Insecure);
            matchingAgones = MagicOnionClient.Create<IAgonesService>(matchingAgonesChannel);
        }

        private void InitializeClient()
        {
            // Initialize the Hub
            //this.channel = new Channel("localhost", 12345, ChannelCredentials.Insecure);
            this.channel = new Channel("localhost", 12345, ChannelCredentials.Insecure);
            //this.channel = new Channel("13.231.213.229", 7126, ChannelCredentials.Insecure);

            // for SSL/TLS connection
            //var serverCred = new SslCredentials(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "server.crt")));
            //this.channel = new Channel("test.example.com", 12345, serverCred);
            this.streamingClient = StreamingHubClient.Connect<IChatHub, IChatHubReceiver>(this.channel, this);
            this.RegisterDisconnectEvent(streamingClient);
            this.client = MagicOnionClient.Create<IChatService>(this.channel);
            this.agonesClient = MagicOnionClient.Create<IAgonesService>(this.channel);
        }


        private void InitializeUi()
        {
            this.isJoin = false;

            this.SendMessageButton.interactable = false;
            this.ChatText.text = string.Empty;
            this.Input.text = string.Empty;
            this.Input.placeholder.GetComponent<Text>().text = "Please enter your name.";
            this.MatchButtonText.text = "Enter the match";
            this.MatchIdText.text = "MatchId";
            this.JoinOrLeaveButton.interactable = false;
            this.JoinOrLeaveButtonText.text = "Enter the room";
            this.ExceptionButton.interactable = false;
        }

        private void MatchedUi()
        {
            this.isJoin = false;

            this.MatchIdText.text = "matched!";
            this.JoinOrLeaveButton.interactable = true;
        }


        private async void RegisterDisconnectEvent(IChatHub streamingClient)
        {
            try
            {
                // you can wait disconnected event
                await streamingClient.WaitForDisconnect();
            }
            finally
            {
                // try-to-reconnect? logging event? close? etc...
                Debug.Log("disconnected server.");

                if (this.isSelfDisConnected)
                {
                    // there is no particular meaning
                    await Task.Delay(2000);

                    // reconnect
                    this.ReconnectServer();
                }
                else
                {
                    // agones shutdown
                }
            }
        }

        public async void DisconnectServer()
        {
            this.isSelfDisConnected = true;

            this.MatchButton.interactable = false;
            this.JoinOrLeaveButton.interactable = false;
            this.SendMessageButton.interactable = false;
            this.SendReportButton.interactable = false;
            this.DisconnectButon.interactable = false;
            this.ExceptionButton.interactable = false;
            this.UnaryExceptionButton.interactable = false;

            if (this.isJoin)
                this.JoinOrLeave();

            await this.streamingClient.DisposeAsync();
        }


        private void ReconnectServer()
        {
            this.streamingClient = StreamingHubClient.Connect<IChatHub, IChatHubReceiver>(this.channel, this);
            this.RegisterDisconnectEvent(streamingClient);
            Debug.Log("Reconnected server.");

            this.MatchButton.interactable = true;
            this.JoinOrLeaveButton.interactable = true;
            this.SendMessageButton.interactable = false;
            this.SendReportButton.interactable = true;
            this.DisconnectButon.interactable = true;
            this.ExceptionButton.interactable = true;
            this.UnaryExceptionButton.interactable = true;

            this.isSelfDisConnected = false;
        }

        private async Task<(string host, int port)> ConnectMatching()
        {
            Debug.Log("Connect");
            var matchResponse = await matchingClient.GetAsync(ClientId);
            this.matchId = matchResponse.MatchId;
            MatchButtonText.text = "matched!";
            MatchIdText.text = this.matchId;
            Debug.Log($"connectionInfo, cliendId: {ClientId}, matchId: {matchResponse.MatchId}, host: {matchResponse.Room.Host}, port: {matchResponse.Room.Port}");

            var host = matchResponse.Room.Host;
            var port = matchResponse.Room.Port;
            if (!System.Net.IPAddress.TryParse(host, out var _))
            {
                host = "127.0.0.1";
                port = 12345;

                Debug.Log($"override connectionInfo, matchId: {matchResponse.MatchId}, host: {host}, port: {port}");
            }
            return (host, port);
        }

        #region Client -> Server (Unary)
        public async void Connect()
        {
            Debug.Log("Connect");
            var (host, port) = await ConnectMatching();
            MatchedUi();

            //var res = await matchingAgones.GetGameServer();
            //Debug.Log($"connectionInfo, host: {res.Host}, port: {res.Port}");

            //// Initialize the Hub
            //this.channel = new Channel(res.Host, res.Port, ChannelCredentials.Insecure);
            this.channel = new Channel(host, port, ChannelCredentials.Insecure);

            //// for SSL/TLS connection
            ////var serverCred = new SslCredentials(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "server.crt")));
            ////this.channel = new Channel("test.example.com", 12345, serverCred);
            
            this.streamingClient = StreamingHubClient.Connect<IChatHub, IChatHubReceiver>(this.channel, this);
            this.RegisterDisconnectEvent(streamingClient);
            this.client = MagicOnionClient.Create<IChatService>(this.channel);
            this.agonesClient = MagicOnionClient.Create<IAgonesService>(this.channel);
        }
        #endregion

        #region Client -> Server (Streaming)
        public async void JoinOrLeave()
        {
            if (this.isJoin)
            {
                await this.streamingClient.LeaveAsync();
                await matchingClient.LeaveAsync(matchId, ClientId);
                this.InitializeUi();
            }
            else
            {
                // reconnect previous match
                var matchResponse = await matchingClient.JoinAsync(this.matchId, this.ClientId);
                MatchedUi();
                MatchIdText.text = this.matchId;
                Debug.Log($"connectionInfo, cliendId: {ClientId}, matchId: {matchResponse.MatchId}, host: {matchResponse.Room.Host}, port: {matchResponse.Room.Port}");

                var request = new JoinRequest { RoomName = "SampleRoom", UserName = this.Input.text };
                await this.streamingClient.JoinAsync(request);
                await this.agonesClient.Allocate();

                this.isJoin = true;
                this.SendMessageButton.interactable = true;
                this.JoinOrLeaveButtonText.text = "Leave the room";
                this.Input.text = string.Empty;
                this.Input.placeholder.GetComponent<Text>().text = "Please enter a comment.";
                this.ExceptionButton.interactable = true;
            }
        }


        public async void SendMessage()
        {
            if (!this.isJoin)
                return;

            await this.streamingClient.SendMessageAsync(this.Input.text);

            this.Input.text = string.Empty;
        }

        public async void GenerateException()
        {
            // hub
            if (!this.isJoin) return;
            await this.streamingClient.GenerateException("client exception(streaminghub)!");
        }

        public async void SampleMethod()
        {
            throw new System.NotImplementedException();
        }
        #endregion


        #region Server -> Client (Streaming)
        public void OnJoin(string name)
        {
            this.ChatText.text += $"\n<color=grey>{name} entered the room.</color>";
        }


        public void OnLeave(string name)
        {
            this.ChatText.text += $"\n<color=grey>{name} left the room.</color>";
        }

        public void OnSendMessage(MessageResponse message)
        {
            this.ChatText.text += $"\n{message.UserName}：{message.Message}";
        }
        #endregion


        #region Client -> Server (Unary)
        public async void SendReport()
        {
            await this.client.SendReportAsync(this.ReportInput.text);

            this.ReportInput.text = string.Empty;
        }

        public async void UnaryGenerateException()
        {
            // unary
            await this.client.GenerateException("client exception(unary)！");
        }
        #endregion
    }
}
