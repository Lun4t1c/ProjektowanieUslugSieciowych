using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CChat.Models;
using CChat.Enums;

namespace CChatClientGUI.ViewModels
{
    internal class ShellViewModel : Conductor<object>
    {
        #region Properties
        public string UserName { get; set; }
        public Thread ClientThread { get; set; }

        private string _message = "";
        private BindableCollection<MessageModel> _currentMessages = new BindableCollection<MessageModel>();

        public string Message
        {
            get { return _message; }
            set { _message = value; NotifyOfPropertyChange(() => Message); }
        }        

        public BindableCollection<MessageModel> CurrentMessages
        {
            get { return _currentMessages; }
            set { _currentMessages = value; NotifyOfPropertyChange(() => CurrentMessages); }
        }

        #endregion


        #region Constructor
        public ShellViewModel()
        {
            CurrentMessages.Add(new MessageModel() 
            {
                User = new UserModel() { Name = "TEST", Type = UserType.MODERATOR },
                Content = "TEST MESSAGE"
            });
        }
        #endregion


        #region Methods
        private void JoinChat(string name)
        {
            if (string.IsNullOrEmpty(name)) return;

            Globals.Client.Initialize(name, CatchMessage);
           
            Globals.LoggedUser = new UserModel() 
            { 
                Name = name,
                Type = UserType.YOU
            };

            ClientThread = new Thread(() =>
            {
                Globals.Client.Start();
            });
            ClientThread.Start();
        }

        private void SendMessage()
        {
            Globals.Client.SendMessage(Message);
            Message = "";
        }

        private void CatchMessage(string message)
        {
            CurrentMessages.Add(DissectMessage(message));
        }

        private MessageModel DissectMessage(string message)
        {
            MessageModel result;
            string[] dissectedMessage = message.Split(CChat.CChatClient.MESSAGE_SEPARATOR);

            result = new MessageModel()
            {
                User = new UserModel() { Name = dissectedMessage[0], Type = UserType.OTHER_USER },
                Content = dissectedMessage[2]
            };

            if (result.User.Name == Globals.LoggedUser.Name)
                result.User.Type = UserType.YOU;
            else
            {
                switch (dissectedMessage[1])
                {
                    case "system":
                        result.User.Type = UserType.SYSTEM;
                        break;

                    case "moderator":
                        result.User.Type = UserType.MODERATOR;
                        break;

                    case "user":
                        result.User.Type = UserType.OTHER_USER;
                        break;
                }
            }

            return result;
        }

        private string _constructMessageForServer()
        {
            string result = "";

            result = 
                $"{Globals.LoggedUser.Name}{CChat.CChatClient.MESSAGE_SEPARATOR}" +
                $"{Globals.LoggedUser.Type.ToString().ToLower()}{CChat.CChatClient.MESSAGE_SEPARATOR}" +
                $"{Message}{CChat.CChatClient.MESSAGE_SEPARATOR}" +
                $"{CChat.CChatClient.MESSAGE_SIGNOFF}"
                ;

            return result;
        }
        #endregion


        #region Button clicks
        public void JoinChatButton()
        {
            JoinChat(UserName);
        }

        public void SendMessageButton()
        {
            SendMessage();
        }
        #endregion
    }
}
