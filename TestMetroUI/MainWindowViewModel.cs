using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Mvvm;

namespace TestMetroUI
{
    public class Message : BindableBase
    {
        public Message(string author, string text, DateTime date)
        {
            Author = author;
            MessageText = text;
            Date = date;
        }

        public string Author
        {
            get { return GetProperty(() => Author); }
            private set { SetProperty(() => Author, value); }
        }
        public string MessageText
        {
            get { return GetProperty(() => MessageText); }
            private set { SetProperty(() => MessageText, value); }
        }

        public DateTime Date
        {
            get { return GetProperty(() => Date); }
            private set { SetProperty(() => Date, value); }
        }

    }

    public class ChatChannelViewModel : ViewModelBase
    {
        public ChatChannelViewModel()
        {
            Messages = new ObservableCollection<Message>();
            OnlineUsers = new ObservableCollection<string>();

            var rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < rnd.Next(3, 8); i++)
            {
                var author = Guid.NewGuid().ToString().Substring(0, 5);
                var text = Guid.NewGuid().ToString();
                var date = DateTime.Now;
                Messages.Add(new Message(author, text, date));
            }

            for (int i = 0; i < rnd.Next(3,8); i++)
            {
                OnlineUsers.Add(Guid.NewGuid().ToString().Substring(0, 5));
            }

        }

        public ChatChannelViewModel(string label) : this()
        {
            Label = label;
        }

        public string Label
        {
            get { return GetProperty(() => Label); }
            set { SetProperty(() => Label, value); }
        }

        public ObservableCollection<Message> Messages
        {
            get { return GetProperty(() => Messages); }
            set { SetProperty(() => Messages, value); }
        }

        public ObservableCollection<string> OnlineUsers
        {
            get { return GetProperty(() => OnlineUsers); }
            set { SetProperty(() => OnlineUsers, value); }
        }

        public string UserInput
        {
            get { return GetProperty(() => UserInput); }
            set { SetProperty(() => UserInput, value); }
        }






    }

    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            ChatChannels = new ObservableCollection<ChatChannelViewModel>();
            ChatChannels.Add(new ChatChannelViewModel("chat1"));
            ChatChannels.Add(new ChatChannelViewModel("chat2"));
            ChatChannels.Add(new ChatChannelViewModel("chat3"));
        }

        public ObservableCollection<ChatChannelViewModel> ChatChannels
        {
            get { return GetProperty(() => ChatChannels); }
            set { SetProperty(() => ChatChannels, value);}
        }


        public ChatChannelViewModel SelectedChatChannel
        {
            get { return GetProperty(() => SelectedChatChannel); }
            set { SetProperty(() => SelectedChatChannel, value); }
        }

        private ICommand _keyBoardSendMessageCommand;
        public ICommand KeyBoardSendMessageCommand
        {
            get { return _keyBoardSendMessageCommand ?? (_keyBoardSendMessageCommand = new DelegateCommand<ChatChannelViewModel>(KeyBoardSendMessage)); }
        }

        private void KeyBoardSendMessage(ChatChannelViewModel chatChannelViewModel)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl)) && Keyboard.IsKeyDown(Key.Enter))
                SendMessage(chatChannelViewModel);
        }


        private ICommand _sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get { return _sendMessageCommand ?? (_sendMessageCommand = new DelegateCommand<ChatChannelViewModel>(SendMessage)); }
        }

        private static void SendMessage(ChatChannelViewModel chatChannelViewModel)
        {
            var msg = chatChannelViewModel.UserInput.Trim();
            if (!string.IsNullOrEmpty(msg))
            {
                var author = Guid.NewGuid().ToString().Substring(0, 5);
                chatChannelViewModel.Messages.Add(new Message("You", msg, DateTime.Now));
                chatChannelViewModel.UserInput = string.Empty;
            }
        }
    }
}