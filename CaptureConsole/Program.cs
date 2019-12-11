using Capture;
using Capture.Util;
using System.Collections.Generic;
using System.Configuration;

namespace Capture
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = ConfigurationManager.AppSettings["inputFile"];
            string outputPath = ConfigurationManager.AppSettings["outputLocation"];
            string agent = ConfigurationManager.AppSettings["agent"];
            int.TryParse(ConfigurationManager.AppSettings["startDelay"], out int startDelay);
            int.TryParse(ConfigurationManager.AppSettings["captureWidth"], out int captureWidth);
            int.TryParse(ConfigurationManager.AppSettings["captureHeight"], out int captureHeight);

            List<int> outputIntervalsInMilisecond = new List<int>();

            List<Time> times = ConfigurationManager.GetSection("captureTimers") as List<Time>;
            times.ForEach(x => outputIntervalsInMilisecond.Add(x.At));

            ICaptureUtility captureUtility = null;

            if (agent.ToLower() == "ie")
            {
                captureUtility = new CaptureUtility(captureWidth, captureHeight, filePath, outputPath, outputIntervalsInMilisecond, startDelay);
            }
            else
            {
                captureUtility = new CaptureUtilityCef(captureWidth, captureHeight, filePath, outputPath, outputIntervalsInMilisecond, startDelay);
            }

            captureUtility.CaptureAndSave();
        }
    }
}
