using Caliburn.Micro;
using CChatClientGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                User = new UserModel() { Name = "TEST" },
                Content = "TEST MESSAGE"
            });
        }
        #endregion


        #region Methods
        private void JoinChat(string name)
        {
            if (string.IsNullOrEmpty(name)) return;

            Globals.Client.Initialize(name, CatchMessage);
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
            CurrentMessages.Add(new MessageModel()
            {
                User = new UserModel() { Name = "USER" },
                Content = message
            });
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
