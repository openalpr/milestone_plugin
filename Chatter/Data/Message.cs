using System;
using System.Runtime.Serialization;

namespace OpenALPRQueueConsumer.Chatter
{
    [DataContract]
    [Serializable]
    public class Message
    {
        [DataMember]
        public Info MessageInfo { private set; get; }

        public Message(Info info)
        {
            MessageInfo = info;
        }
    }

    [DataContract]
    [Serializable]
    public class Info
    {
        [DataMember]
        public string Message1 { set; get; }

        [DataMember]
        public string Message2 { set; get; }

        [DataMember]
        public string Message3 { set; get; }

        [DataMember]
        public string Message4 { set; get; }

        [DataMember]
        public int Integer { set; get; }

        [DataMember]
        public MessageId MsgId { set; get; } 

        [DataMember]
        public bool Bool { set; get; }

        public Info()
        {
            Message1 = string.Empty;
            Message2 = string.Empty;
            Message3 = string.Empty;
            Message4 = string.Empty;
        }

        public override string ToString()
        {
            return MsgId.ToString();
        }
    }

    public enum MessageId : int
    {
        Init,
        //ExportProgress,
        //ExportNotFound,
        //Message,
        //Status,
        ConnectedToMilestoneServer,
        //StartExport,

        //InitToServer,
        //NewExport,
        //ExportUpdated,
        //ExportDeleted,
        //ExportPaused,
        ConnectedToMilestoneToServer,
        //LicenseFilePath,
        //Search,
        //SearchResult,

        //IsApplicationLicensed
    }
}
