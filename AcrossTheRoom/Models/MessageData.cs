using System;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;

namespace AcrossTheRoom.Models
{
    public sealed class MessageData
    {
        private const string MESSAGEDATA_FILE_NAME = "MessageData.json";

        private static readonly MessageData instance = new MessageData();

        private MessageData() { }

        public static MessageData Instance
        {
            get
            {
                return instance;
            }
        }

        public void Save()
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = store.CreateFile(MESSAGEDATA_FILE_NAME))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Message>));
                    serializer.WriteObject(stream, _messages);
                    System.Diagnostics.Debug.WriteLine("Data saved.");
                }
            }
        }

        public void Load()
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(MESSAGEDATA_FILE_NAME))
                {
                    using (IsolatedStorageFileStream stream = store.OpenFile(MESSAGEDATA_FILE_NAME, System.IO.FileMode.Open))
                    {
                        System.Diagnostics.Debug.WriteLine("Loading data...");
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Message>));
                        _messages = (ObservableCollection<Message>)serializer.ReadObject(stream);
                    }
                }
            }
        }


        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get
            {
                if (_messages == null) _messages = new ObservableCollection<Message>();
                return _messages;
            }

            private set { }
        }

    }
}
