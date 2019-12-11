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

namespace Capture.Util
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
                            vBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;
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

        //TODO: Need to check it for all browser instances
        private void OnBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs args)
        {
            if (args.Frame.IsMain)
            {
                args.Browser.MainFrame.ExecuteJavaScriptAsync("document.body.style.overflow = 'hidden'");
                for (int i = 0; i < args.Browser.GetFrameCount(); i++)
                {
                    var frame = args.Browser.GetFrame(i);
                    if (frame != null)
                    {
                        frame.ExecuteJavaScriptAsync("document.body.style.overflow = 'hidden'");
                    }
                }
            }
            _IsBrowserReady = true;
        }

        private void SaveScreenshot(Bitmap bitmap, int sequence)
        {
            string screenshotFileName = Path.GetFullPath(Path.Combine(_OutputPath, string.Format("{0}.png", sequence)));
            bitmap.Save(screenshotFileName, ImageFormat.Png);
        }
    }
}
