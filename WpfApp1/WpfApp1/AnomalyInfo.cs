using System;
using System.Collections.Generic;
using System.Text;
using OxyPlot;
using OxyPlot.Series;

namespace WpfApp1
{
    class AnomalyInfo
    {
        //private string correlatedFeatures;
        private int anomalyLine;
        private OxyPlot.Wpf.Annotation anno;
        List<DataPoint> points;
        public AnomalyInfo(int line, OxyPlot.Wpf.Annotation a, List<DataPoint> p)
        {
            //correlatedFeatures = info;
            anomalyLine = line;
            anno = a;
            points = p;
        }
        public AnomalyInfo(int line, List<DataPoint> p)
        {
            anomalyLine = line;
            points = p;
        }
        public AnomalyInfo(int line)
        {
            anomalyLine = line;
            points = new List<DataPoint>();
        }
        public int AnomalyLine
        {
            set
            {
                anomalyLine = value;
            }
            get
            {
                return anomalyLine;
            }
        }
        public OxyPlot.Wpf.Annotation Anno
        {
            set
            {
                anno = value;
            }
            get
            {
                return anno;
            }
        }
        public List<DataPoint> Points
        {
            set
            {
                points = value;
            }
            get
            {
                return points;
            }
        }

    }
}
