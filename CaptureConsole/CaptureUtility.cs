using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Capture
{
    public class CaptureUtility : ICaptureUtility
    {
        private string _FilePath;
        private string _OutputPath;
        private int _Width;
        private int _Height;
        private int _StartDelay;
        private List<int> _OutputIntervalsInMilisecond = new List<int>();
        private static readonly object _object = new object();
        private bool _IsBrowserReady = false;

        public CaptureUtility(int width, int height, string filePath, string outputPath, List<int> intervalList, int startDelay = 0)
        {
            _FilePath = filePath;
            _OutputPath = outputPath;
            _StartDelay = startDelay;

            _Width = width;
            _Height = height;

            _OutputIntervalsInMilisecond = intervalList;
        }

        public void CaptureAndSave()
        {
            foreach (int interval in _OutputIntervalsInMilisecond)
            {
                Thread thread = new Thread(delegate ()
                {
                    using (WebBrowser vBrowser = new WebBrowser())
                    {
                        vBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.OnDocumentCompleted);
                        vBrowser.ScriptErrorsSuppressed = true;
                        vBrowser.Width = _Width;
                        vBrowser.Height = _Height;
                        vBrowser.ScrollBarsEnabled = false;
                        vBrowser.Navigate(_FilePath);

                        while (!_IsBrowserReady)
                        {
                            Application.DoEvents();
                        }

                        Stopwatch sw = new Stopwatch();
                        sw.Start();

                        while (true)
                        {
                            if (sw.ElapsedMilliseconds >= interval)
                            {
                                lock (_object)
                                {
                                    using (Graphics graphics = vBrowser.CreateGraphics())
                                    using (Bitmap bitmap = new Bitmap(_Width, _Height, graphics))
                                    {
                                        Rectangle bounds = new Rectangle(0, 0, _Width, _Height);
                                        vBrowser.DrawToBitmap(bitmap, bounds);
                                        this.SaveScreenshot(bitmap, _OutputIntervalsInMilisecond.IndexOf(interval));
                                        vBrowser.Dispose();
                                        break;
                                    }
                                }
                            }

                            Application.DoEvents();
                        }
                    }
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
        }

        private void OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _IsBrowserReady = true;
        }

        private void SaveScreenshot(Bitmap bitmap, int sequence)
        {
            string screenshotFileName = Path.GetFullPath(Path.Combine(_OutputPath, string.Format("{0}.png", sequence)));
            bitmap.Save(screenshotFileName, ImageFormat.Png);
        }
    }
}
