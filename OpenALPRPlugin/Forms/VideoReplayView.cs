// Copyright OpenALPR Technology, Inc. 2018

using System;
using System.Windows.Forms;
using VideoOS.Platform;

namespace OpenALPRPlugin.Forms
{
    public partial class VideoReplayView : Form
    {
        public VideoReplayView()
        {
            InitializeComponent();
            ClientControl.Instance.RegisterUIControlForAutoTheming(this);
        }

        public void Add(Tuple<Item, DateTime, DateTime>[] tuples)
        {
            for (int i = 0; i < tuples.Length; i++)
            {
                VideoReplayViewItemUserControl videoReplayViewItemUserControl = new VideoReplayViewItemUserControl();
                videoReplayViewItemUserControl.Init(tuples[i].Item1, tuples[i].Item2, tuples[i].Item3);
                flowLayoutPanel1.Controls.Add(videoReplayViewItemUserControl);
            }

            if (tuples.Length > 1)
                Height = Height + Height * 3 / 4;
        }

        public void CloseMe()
        {
            for(int i = flowLayoutPanel1.Controls.Count - 1; i >= 0 ; i-- )
            {
                Control c = flowLayoutPanel1.Controls[i];
                flowLayoutPanel1.Controls.RemoveAt(i);
                if (c != null)
                {
                    (c as VideoReplayViewItemUserControl).Close();
                    c.Dispose();
                }
            }

            Close();
        }

    }
}
