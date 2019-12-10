using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Capture
{
    public class CaptureTimerSection : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            List<Time> myConfigObject = new List<Time>();

            foreach (XmlNode childNode in section.ChildNodes)
            {
                foreach (XmlAttribute attrib in childNode.Attributes)
                {
                    myConfigObject.Add(new Time()
                    {
                        At = int.Parse(attrib.Value)
                    });
                }
            }
            return myConfigObject;
        }
    }

    public class Time
    {
        public int At { get; set; }
    }
}
