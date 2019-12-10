using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Capture
{
    public class CaptureUtility
    {
        private string _FilePath = @"D:\Practice\screenshot\test\330321_swp_pdf_output_test_(327901___will_be_deleted)_160x600.html";
        private int _Width;
        private int _Height;
        private List<int> outputIntervalsInMilisecond = new List<int>();
        private static readonly object _object = new object();

        public CaptureUtility()
        {
            _Width = 160;
            _Height = 600;

            outputIntervalsInMilisecond.Add(500);
            outputIntervalsInMilisecond.Add(2500);
            outputIntervalsInMilisecond.Add(3000);
            outputIntervalsInMilisecond.Add(2000);
            outputIntervalsInMilisecond.Add(3000);
            outputIntervalsInMilisecond.Add(2000);
        }

        public void CaptureAndSave()
        {
            int totalInterval = 50;
            foreach (int interval in outputIntervalsInMilisecond)
            {
                totalInterval = totalInterval + interval;
                Thread thread = new Thread(delegate ()
                {
                    using (WebBrowser vBrowser = new WebBrowser())
                    {
                        vBrowser.Width = 160;
                        vBrowser.Height = 600;
                        vBrowser.ScrollBarsEnabled = false;
                        vBrowser.Navigate(_FilePath);

                        Stopwatch sw = new Stopwatch();
                        sw.Start();

                        while (true)
                        {
                            if (sw.ElapsedMilliseconds >= totalInterval)
                            {
                                lock (_object)
                                {
                                    using (Graphics graphics = vBrowser.CreateGraphics())
                                    using (Bitmap bitmap = new Bitmap(_Width, _Height, graphics))
                                    {
                                        Rectangle bounds = new Rectangle(0, 0, _Width, _Height);
                                        vBrowser.DrawToBitmap(bitmap, bounds);
                                        this.SaveScreenshot(bitmap, "V_IE");
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

        private void SaveScreenshot(Bitmap bitmap, string method)
        {
            string screenshotFileName = Path.GetFullPath(string.Format("screenshot_{0}_{1}.png", DateTime.Now.Ticks, method));
            bitmap.Save(screenshotFileName, ImageFormat.Png);
        }
    }
}
