using Capture;
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
            int startDelay = 0;
            int captureWidth = 160;
            int captureHeight = 600;
            int.TryParse(ConfigurationManager.AppSettings["startDelay"], out startDelay);
            int.TryParse(ConfigurationManager.AppSettings["captureWidth"], out captureWidth);
            int.TryParse(ConfigurationManager.AppSettings["captureHeight"], out captureHeight);

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

            //CaptureUtility captureUtility = new CaptureUtility(captureWidth, captureHeight, filePath, outputPath, outputIntervalsInMilisecond, startDelay);
            captureUtility.CaptureAndSave();
        }
    }
}
