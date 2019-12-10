using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Capture
{
    public class CaptureUtilityCef : ICaptureUtility
    {
        private string _FilePath;
        private string _OutputPath;
        private int _Width;
        private int _Height;
        private int _StartDelay;
        private List<int> _OutputIntervalsInMilisecond = new List<int>();
        private static readonly object _object = new object();
        private bool _IsBrowserReady = false;

        public CaptureUtilityCef(int width, int height, string filePath, string outputPath, List<int> intervalList, int startDelay = 0)
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
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);

            foreach (int interval in _OutputIntervalsInMilisecond)
            {
                Thread thread = new Thread(delegate ()
                {
                    try
                    {
                        using (ChromiumWebBrowser vBrowser = new ChromiumWebBrowser(_FilePath))
                        {
                            vBrowser.FrameLoadEnd += OnDocumentCompleted;
                            vBrowser.Size = new Size(_Width, _Height);

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
                                        Bitmap bitmap = vBrowser.ScreenshotAsync().Result;
                                        this.SaveScreenshot(bitmap, _OutputIntervalsInMilisecond.IndexOf(interval));
                                        vBrowser.Dispose();
                                        break;
                                    }
                                }

                                Application.DoEvents();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                    }
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            Cef.Shutdown();
        }

        private void OnDocumentCompleted(object sender, FrameLoadEndEventArgs e)
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
