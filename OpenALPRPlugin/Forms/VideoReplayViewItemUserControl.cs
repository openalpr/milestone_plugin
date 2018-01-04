using OpenALPRPlugin.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;

namespace OpenALPRPlugin.Forms
{
    public partial class VideoReplayViewItemUserControl : ViewItemUserControl
    {
        #region Component private class variables

        private bool _stop = false;
        private Item selectedCamera;
        private DateTime start;
        private DateTime end;

        #endregion

        #region Component constructors + dispose

        public VideoReplayViewItemUserControl()
        {
            InitializeComponent();

            ClientControl.Instance.RegisterUIControlForAutoTheming(panelMain);
        }

        public void Init(Item selectedCamera, DateTime start, DateTime end)
        {
            SetUpApplicationEventListeners();
            this.selectedCamera = selectedCamera;
            if (selectedCamera != null)
                labelName.Text = selectedCamera.Name;

            this.start = start;
            this.end = end;
        }

        private void SetUpApplicationEventListeners()
        {
            var imageClear = new Bitmap(320, 240);
            var g = Graphics.FromImage(imageClear);
            g.FillRectangle(Brushes.Black, 0, 0, 320, 240);
            g.Dispose();
            SetImage(imageClear);
        }

        #endregion

        private void OnReplay(object sender, EventArgs e)
        {
            if (selectedCamera != null)
            {
                _stop = true;
                //var thread = new Thread( new //new ThreadStart(ShowReplay));
                var thread = new Thread(() => ShowReplay(selectedCamera, start, end));
                thread.Start();
            }
        }

        private void ShowReplay(Item camera, DateTime startDate, DateTime endDate)
        {
            var imageClear = new Bitmap(320, 240);
            var g = Graphics.FromImage(imageClear);
            g.FillRectangle(Brushes.Black, 0, 0, 320, 240);
            g.Dispose();
            SetImage( imageClear );

            var source = new JPEGVideoSource(camera);
            source.Init();
            List<object> resultList = source.Get(startDate, new TimeSpan(endDate.Ticks), 150);

            //TODO remove ---TEST source.Close();

            _stop = true;
            if (resultList != null)
            {
                this.UIThread(() => label2.Text = $"Number of frames: {resultList.Count.ToString()}");
                if (resultList.Count > 0)
                    _stop = false;
            }

            while (!_stop)
            {
                int avgInterval = 3000 / resultList.Count;
                foreach (JPEGData jpeg in resultList)
                {
                    var ms = new MemoryStream(jpeg.Bytes);
                    var image = new Bitmap(ms);
                    SetImage(image);
                    ms.Close();
                    Thread.Sleep(avgInterval);
                    if (_stop)
                        break;
                }
                if (!_stop)
                    Thread.Sleep(1500);
            }
            source.Close();     //TODO
        }


        //private delegate void SetImageDelegate(Bitmap bitmap);
        private void SetImage(Bitmap bitmap)
        {
            this.UIThread(() =>
            {
                if (pictureBox2.Visible && pictureBox2.Height > 0)
                {
                    double ratio = Convert.ToDouble(pictureBox2.Width) / pictureBox2.Height;
                    double ratioNew = Convert.ToDouble(bitmap.Width) / bitmap.Height;

                    double w = pictureBox2.Width;
                    double h = pictureBox2.Height;
                    if (ratio > ratioNew)
                        w = h * ratioNew;
                    else
                        h = w / ratioNew;

                    pictureBox2.Image = new Bitmap(bitmap, new Size(Convert.ToInt32(w), Convert.ToInt32(h)));
                }
            });
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            _stop = true;
        }

        public override void Close()
        {
            _stop = true;
            Thread.Sleep(300);
        }

    }
}
