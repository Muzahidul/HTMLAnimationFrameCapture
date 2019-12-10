using Freezer.Core;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Capture
{
    public partial class CaptureForm : Form
    {
        private string _FilePath = @"D:\Practice\screenshot\test\330321_swp_pdf_output_test_(327901___will_be_deleted)_160x600.html";
        private int _Width;
        private int _Height;
        private List<int> outputIntervalsInMilisecond = new List<int>();

        public CaptureForm()
        {
            InitializeComponent();
            _Width = 160;
            _Height = 600;

            outputIntervalsInMilisecond.Add(500);
            outputIntervalsInMilisecond.Add(3500);
            outputIntervalsInMilisecond.Add(3000);
            outputIntervalsInMilisecond.Add(3000);
            outputIntervalsInMilisecond.Add(3000);

            //this.browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.OnDocumentCompleted);
        }


        private void btnCaptureIE_Click(object sender, EventArgs e)
        {
            CaptureUsingIEAsync();
        }

        private void CaptureUsingIEAsync()
        {
            browser.Navigate(_FilePath);
            foreach (int interval in outputIntervalsInMilisecond)
            {
                Thread.Sleep(interval);
                Application.DoEvents();

                using (Graphics graphics = this.CreateGraphics())
                using (Bitmap bitmap = new Bitmap(_Width, _Height, graphics))
                {
                    Rectangle bounds = new Rectangle(0, 0, _Width, _Height);
                    this.browser.DrawToBitmap(bitmap, bounds);
                    this.SaveScreenshot(bitmap, "IE");
                }
                Application.DoEvents();
            }
        }

        private void btnCaptureVirtualIE_Click(object sender, EventArgs e)
        {
            //CaptureUsingVirtualIE();

            CaptureUtility captureUtility = new CaptureUtility();
            captureUtility.CaptureAndSave();
        }

        static readonly object _object = new object();
        private void CaptureUsingVirtualIE()
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

            //WebBrowser vBrowser = new WebBrowser();
            //vBrowser.Width = 160;
            //vBrowser.Height = 600;
            //vBrowser.ScrollBarsEnabled = false;
            //vBrowser.Navigate(_FilePath);
            ////Application.DoEvents();

            //foreach (int interval in outputIntervalsInMilisecond)
            //{
            //    //Application.DoEvents();
            //    Thread.Sleep(interval);
            //    //Application.DoEvents();

            //    using (Graphics graphics = this.CreateGraphics())
            //    using (Bitmap bitmap = new Bitmap(_Width, _Height, graphics))
            //    {
            //        Rectangle bounds = new Rectangle(0, 0, _Width, _Height);
            //        vBrowser.DrawToBitmap(bitmap, bounds);
            //        this.SaveScreenshot(bitmap, "V_IE");
            //        Application.DoEvents();
            //    }
            //}
        }

        private void btnCaptureGecko_Click(object sender, EventArgs e)
        {
            //var prevInterval = 0;
            //foreach (int interval in outputIntervalsInMilisecond)
            //{
            //    prevInterval = interval + prevInterval;
            //    var screenshotJob = ScreenshotJobBuilder.Create(_FilePath)
            //      .SetBrowserSize(_Width, _Height)
            //      .SetCaptureZone(CaptureZone.VisibleScreen)
            //      .SetTrigger(new WindowLoadTrigger())
            //      .SetTimeout(prevInterval);

            //    System.Drawing.Bitmap bitmap = (Bitmap)screenshotJob.Freeze();
            //    this.SaveScreenshot(bitmap, "Gecko");
            //}


            string html = File.ReadAllText(_FilePath);

            string patternInterval = @"var[\s]+_DellshareFallbackImage(?:\s+)?=(?:\s+)?'([^']+)'(?:\s+)?;";
            System.Text.RegularExpressions.Match matchInterval = System.Text.RegularExpressions.Regex.Match(html, patternInterval);
            string imageId = matchInterval.Success ? matchInterval.Groups[1].Value : string.Empty;

            string patternArea = @"var[\s]+_DellShareFrameInputArea(?:\s+)?=(?:\s+)?'([^']+)'(?:\s+)?;";
            System.Text.RegularExpressions.Match matchArea = System.Text.RegularExpressions.Regex.Match(html, patternArea);
            string inputArea = matchArea.Success ? matchArea.Groups[1].Value : string.Empty;


            FirefoxOptions options = new FirefoxOptions();
            options.BrowserExecutableLocation = ("C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe");
            options.SetPreference("security.sandbox.content.level", 5);
            OpenQA.Selenium.IWebDriver driver = new FirefoxDriver(options);
            //using (var FireFoxPage = new FirefoxDriver(options))
            driver.Navigate().GoToUrl(string.Format("file:///{0}", _FilePath.Replace(@"\\", "/")));
            var remElement = driver.FindElement(OpenQA.Selenium.By.Id(inputArea));
            Point location = remElement.Location;

            foreach (int interval in outputIntervalsInMilisecond)
            {
                Thread.Sleep(interval);

                var screenshot = (driver as FirefoxDriver).GetScreenshot();

                using (MemoryStream stream = new MemoryStream(screenshot.AsByteArray))
                {
                    using (Bitmap bitmap = new Bitmap(stream))
                    {
                        RectangleF part = new RectangleF(location.X, location.Y, remElement.Size.Width, remElement.Size.Height);

                        using (Bitmap bn = bitmap.Clone(part, bitmap.PixelFormat))
                        {
                            this.SaveScreenshot(bn, "Selenium");
                        }
                    }
                }
            }

            driver.Dispose();
        }

        private void btnCaptureOther_Click(object sender, EventArgs e)
        {
            browser.Navigate(_FilePath);
            foreach (int interval in outputIntervalsInMilisecond)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(interval);
                Application.DoEvents();

                Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
                int titleHeight = screenRectangle.Top - this.Top;

                int left = ActiveForm.Location.X + browser.Parent.Location.X + 7;//SystemInformation.Border3DSize.Width;
                int top = ActiveForm.Location.Y + browser.Parent.Location.Y + titleHeight;

                Bitmap bitmap = new Bitmap(_Width, _Height, PixelFormat.Format32bppArgb);
                Rectangle captureRectangle = new Rectangle(left, top, _Width, _Height);//Screen.AllScreens[0].Bounds;
                Graphics captureGraphics = Graphics.FromImage(bitmap);
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
                this.SaveScreenshot(bitmap, "Other");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            browser.Navigate(_FilePath);
        }


        private void OnClickGenerateScreenshot(object sender, EventArgs e)
        {
            string webAddressString = _FilePath;

            Uri webAddress;
            if (Uri.TryCreate(webAddressString, UriKind.Absolute, out webAddress))
            {
                this.browser.Navigate(webAddress);
            }
            else
            {
                MessageBox.Show("Please enter a valid URI.", "WebBrowser Screenshot Forms Sample", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (this.browser.ReadyState == WebBrowserReadyState.Complete && e.Url == this.browser.Url)
            {
                CaptureUsingIEAsync();
            }
        }

        private void SaveScreenshot(Bitmap bitmap, string method)
        {
            string screenshotFileName = Path.GetFullPath(string.Format("screenshot_{0}_{1}.png", DateTime.Now.Ticks, method));
            bitmap.Save(screenshotFileName, ImageFormat.Png);
            imgPreview.Image = (Bitmap)bitmap.Clone();
            //MessageBox.Show("Screenshot saved to '" + screenshotFileName + "'.", "WebBrowser Screenshot Forms Sample", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            browser.Navigate(_FilePath);
        }
    }
}
