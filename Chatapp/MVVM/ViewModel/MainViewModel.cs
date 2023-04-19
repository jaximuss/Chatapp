using ChatClient.MVVM.core;
using ChatClient.MVVM.Model;
using ChatClient.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }

        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        public string Message { get; set; }

        public string Username { get; set; }

        private Server _server;
        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _server.connectedEvent += UserConnected;
            _server.messageRecievedEvent += MessageRecieved;
            _server.userDisconnectedEvent += UserDisconnected;

            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));



        }

        private void UserDisconnected()
        {
            var uid = _server.packetReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        private void MessageRecieved()
        {
            var message  = _server.packetReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }

        private void UserConnected()
        {
            UserModel user = new UserModel
            {
                UserName = _server.packetReader.ReadMessage(),
                UID = _server.packetReader.ReadMessage()
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
