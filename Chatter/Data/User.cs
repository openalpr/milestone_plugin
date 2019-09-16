using System.ComponentModel;
using System.Runtime.Serialization;

namespace OpenALPRQueueConsumer.Chatter
{
    [DataContract]
    public class User : INotifyPropertyChanged
    {
        public static string AutoExporterServiceName = "AutoExporterSvc";
        public static string SystemTrayIconName = "SystemTrayIcon";
        public const string ManagmentClientPluginName = "ManagmentClientPlugin";

        public event PropertyChangedEventHandler PropertyChanged;

        private string name;

        public User(string name)
        {
            this.name = name;
        }
        
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        protected void OnPropertyChanged(string propertyValue)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyValue));
        }

        public override string ToString()
        {
            return name ;
        }
    }
}
