using LiveCharts;//chart
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using OxyPlot;
using System.Xml.Linq;
using OxyPlot.Series;
using System.Linq;
using System.Windows.Controls;

namespace WpfApp1
{
    public class Two_Correlated_attribute
    {
        float slope;
        float intercept;
        List<DataPoint> correlated_points;
        public Two_Correlated_attribute(float slp, float inter, List<DataPoint> cor_points)
        {
            slope = slp;
            intercept = inter;
            correlated_points = cor_points;
        }
        public Two_Correlated_attribute(float slp, float inter)
        {
            slope = slp;
            intercept = inter;
            correlated_points = new List<DataPoint>();
        }
        public Two_Correlated_attribute()
        {
            correlated_points = new List<DataPoint>();
        }
        public float Slope
        {
            get
            {
                return slope;
            }
            set
            {
                slope = value;
            }
        }
        public float Intercept
        {
            get
            {
                return intercept;
            }
            set
            {
                intercept = value;
            }
        }
        public List<DataPoint> Correlated_points
        {
            get
            {
                return correlated_points;
            }
            set
            {
                correlated_points = value;
            }
        }
    }
    class MyFlightModel : IFlightModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static Mutex mutex = new Mutex();
        private MyTelnetClient tc;
        private volatile Boolean stop, play = false;
        private volatile string csvPath, xmlPath;
        private volatile int currentLine;
        private volatile int numOfLines;
        private volatile string playSpeed;
        private volatile int progressDirection;
        /*private volatile int Num_of_Atributes = 42;*/
        private volatile int display_lines_temp = 0;
        private volatile string current_attribute = "aileron", no_attribute = "-";
        private volatile float slopeLineAnnotation = 0, interceptLineAnnotation = 0;
        private volatile List<string> anomalyReportList = new List<string> {"Hello", "World"};
        private volatile string investigatedAnomaly;
        //private volatile List<string> anomalyReportList = new List<string>();
        private MyTelnetClient tc_reader;
        private string[] get_msgs = new string[6] { "get /instrumentation/altimeter/indicated-altitude-ft", "get /velocities/airspeed-kt[0]", "get /orientation/heading-deg", "get /orientation/roll-deg", "get /orientation/pitch-deg", "get /orientation/side-slip-deg" };
        private volatile float altmeter = 0, airspeed = 0, registeredHeading_degrees = 0;
        private volatile float pitch = 0, roll = 0, yaw = 0;
        private volatile float aileron = 0, throttle0 = 0, rudder = 0, elevator = 0;
        private short atributes_index = 0;
        private volatile bool start_to_read = false;
        /*private volatile bool atributes_are_ready = false;*/
        private volatile List<string> xmlNameList;
        /*private volatile List<ListBoxItem> listBoxxmlNameList;*/
        public volatile List<DataPoint>[] atributes = new List<DataPoint>[42];
        public volatile Dictionary<String, List<DataPoint>> attribute = new Dictionary<string, List<DataPoint>>();
        public volatile Dictionary<String, String> attribute_correlated = new Dictionary<string, String>();

        volatile string plotTitle = "";
        volatile List<DataPoint> plotPoints = new List<DataPoint>();

        volatile string plotTitle_correlated = "";
        volatile List<DataPoint> plotPoints_correlated = new List<DataPoint>();
        volatile List<DataPoint> regressionPoints = new List<DataPoint>();

        //key - attribute(like ailrone) , value - tuple(first slope, second intersect)
        Dictionary<string, Tuple<float, float>> regression_dict = new Dictionary<string, Tuple<float, float>>();
        /*Dictionary<string, Tuple<float, float, List<DataPoint>>> regression_and_points_dict = new Dictionary<string, Tuple<float, float, List<DataPoint>>>();*/
        //Two_Correlated_attribute
        Dictionary<string, Two_Correlated_attribute> atributes_2_Two_Correlated_attribute_dict = new Dictionary<string, Two_Correlated_attribute>();

        public float SlopeLineAnnotation
        {
            get
            {
                return slopeLineAnnotation;
            }
            set
            {
                slopeLineAnnotation = value;
                NotifyPropertyChanged("interceptLineAnnotation");
            }
        }
        public float InterceptLineAnnotation
        {
            get
            {
                return interceptLineAnnotation;
            }
            set
            {
                interceptLineAnnotation = value;
                NotifyPropertyChanged("interceptLineAnnotation");
            }
        }
        private void buildNameListFromXML()
        {
            List<string> temp;
            XElement xe = XElement.Load(XmlPath);
            temp = (xe.Descendants("output").Descendants("name").Select(name => (string)name)).ToList();
            foreach(string name in temp)
            {
                if (attribute.ContainsKey(name))
                {
                    xmlNameList.Add(name + "1");
                    attribute.Add(name + "1", new List<DataPoint>());
                }
                else
                {
                    xmlNameList.Add(name);
                    attribute.Add(name, new List<DataPoint>());
                }
            }
        }
      
        public string Current_attribute
        {
            get
            {
                return current_attribute;
            }
            set
            {
                mutex.WaitOne();
                current_attribute = value;
                display_lines_temp = 0;
                plotPoints = new List<DataPoint>();
                plotPoints_correlated = new List<DataPoint>();
                mutex.ReleaseMutex();
                if (attribute_correlated.ContainsKey(current_attribute))
                    PlotTitle_correlated = attribute_correlated[current_attribute];
                //slopeLineAnnotation = 
                NotifyPropertyChanged("Current_attribute");
                NotifyPropertyChanged("PlotTitle_correlated");
                if (atributes_2_Two_Correlated_attribute_dict.ContainsKey(current_attribute))
                {
                    SlopeLineAnnotation = atributes_2_Two_Correlated_attribute_dict[current_attribute].Slope;
                    InterceptLineAnnotation = atributes_2_Two_Correlated_attribute_dict[current_attribute].Intercept;
                    RegressionPoints = atributes_2_Two_Correlated_attribute_dict[current_attribute].Correlated_points;
                    NotifyPropertyChanged("SlopeLineAnnotation");
                    NotifyPropertyChanged("InterceptLineAnnotation");
                }
            }
        }
        public string PlotTitle
        {
            get
            {
                return plotTitle;

            }
            set
            {
                plotTitle = value;
                NotifyPropertyChanged("PlotTitle");
            }
        }
        public IList<DataPoint> PlotPoints {
            get
            {
                return plotPoints;
               
            }
        }
        public string PlotTitle_correlated
        {
            get
            {
                return plotTitle_correlated;

            }
            set
            {
                plotTitle_correlated = value;
                NotifyPropertyChanged("PlotTitle_correlated");
            }
        }
        public IList<DataPoint> PlotPoints_correlated
        {
            get
            {
                return plotPoints_correlated;

            }
        }
        //
        public float Altmeter
        {
            get
            {
                return altmeter;

            }
            set
            {
                altmeter = value;
                NotifyPropertyChanged("Altmeter");
            }
        }
        public float Airspeed
        {
            get
            {
                return airspeed;

            }
            set
            {
                airspeed = value;
                NotifyPropertyChanged("Airspeed");
            }
        }
        public float Registered_heading_degrees
        {
            get
            {
                return registeredHeading_degrees;
            }
            set
            {
                registeredHeading_degrees = value;
                NotifyPropertyChanged("Registered_heading_degrees");
            }
        }
        public float Pitch
        {
            get
            {
                return pitch;

            }
            set
            {
                pitch = value;
                NotifyPropertyChanged("Pitch");
            }
        }
        public float Roll
        {
            get
            {
                return roll;

            }
            set
            {
                roll = value;
                NotifyPropertyChanged("Roll");
            }
        }
        public float Yaw
        {
            get
            {
                return yaw;

            }
            set
            {
                yaw = value;
                NotifyPropertyChanged("Yaw");
            }
        }
        public float Aileron
        {
            get
            {
                return aileron;

            }
            set
            {
                aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }
        public float Throttle0
        {
            get
            {
                return throttle0;

            }
            set
            {
                throttle0 = value;
                NotifyPropertyChanged("Throttle0");
            }
        }
        public float Rudder
        {
            get
            {
                return rudder;

            }
            set
            {
                rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }
        public float Elevator
        {
            get
            {
                return elevator;

            }
            set
            {
                elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }
        public short Atributes_index
        {
            get
            {
                return atributes_index;
            }
            set
            {

                atributes_index = value;
                NotifyPropertyChanged("Atributes_index");
                NotifyPropertyChanged("Atributes_atIndex");
            }
        }

        public int ProgressDirection
        {
            get
            {
                return progressDirection;
            }
            set
            {
                progressDirection = value;
                NotifyPropertyChanged("ProgressDirection");
            }
        }
        public int NumOfLines
        {
            get
            {
                return numOfLines;
            }
            set
            {
                numOfLines = value;
                NotifyPropertyChanged("NumOfLines");
            }
        }
        public string InvestigatedAnomaly
        {
            get { return investigatedAnomaly; }
            set
            {
                investigatedAnomaly = value;
                NotifyPropertyChanged("InvestigatedAnomaly");
            }
        }
        public List<string> AnomalyReportList
        {
            get { return anomalyReportList; }
            set
            {
                anomalyReportList = value;
                NotifyPropertyChanged("AnomalyReportList");
            }
        }


        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public string PlaySpeed
        {
            get
            {
                return playSpeed;
            }
            set
            {
                playSpeed = value;
                NotifyPropertyChanged("PlaySpeed");
            }
        }
        public int CurrentLine
        {
            get
            {
                if (currentLine >= numOfLines)
                    return numOfLines - 1;
                return currentLine;
            }
            set
            {
                if (value >= numOfLines)
                {
                    currentLine = numOfLines - 1;
                }
                else
                {
                    //mutex.WaitOne();
                    currentLine = value;
                    //mutex.ReleaseMutex();

                }
                NotifyPropertyChanged("Time");
                NotifyPropertyChanged("CurrentLine");
                NotifyPropertyChanged("LineRatio");
            }
        }
        public MyFlightModel()
        {
            tc = new MyTelnetClient();
            tc_reader = new MyTelnetClient();
            PlaySpeed = "1";
            CurrentLine = -1;
            NumOfLines = 1;
            ProgressDirection = 1;
            csvPath = "";
            //listBoxxmlNameList = new List<ListBoxItem>();
            xmlNameList = new List<string>();
            attribute.Add(no_attribute, new List<DataPoint>());
            //Current_attribute = current_attribute;
            NotifyPropertyChanged("Current_attribute");
            //Current_attribute
            //attribute 
            //atributes[0] = new ChartValues<float>();
            //atributes[0].Add(0);
        }
        public Boolean Play
        {
            get
            {
                return play;
            }
            set
            {
                play = value;
                NotifyPropertyChanged("Play");
            }
        }

        private void initDictionary(Dictionary<string, Tuple<float, float>> src_dict)
        {
            string pair_delim = ",", input_delim = " ";
            string[] splited_pair;
            foreach (KeyValuePair<string, Tuple<float, float>> entry in src_dict)
            {
                splited_pair = entry.Key.Split(pair_delim);
                if (splited_pair.Length == 2)
                {
                    attribute_correlated.Add(splited_pair[0], splited_pair[1]);
                    //regression_dict.Add(splited_pair[0], entry.Value);
                    //regression_and_points_dict.Add(splited_pair[0], new Tuple<float, float, List<DataPoint>>(entry.Value.Item1, entry.Value.Item2, new List<DataPoint>()));
                    //init atributes_2_Two_Correlated_attribute_dict 
                    atributes_2_Two_Correlated_attribute_dict.Add(splited_pair[0], new Two_Correlated_attribute(entry.Value.Item1, entry.Value.Item2));
                }
            }
            Current_attribute = attribute_correlated.First().Key;
        }
        public string CsvPath
        {
            get
            {
                return csvPath;
            }
            set
            {
                csvPath = value;
                /*atributes_are_ready = false;*/
                //call tom and dani!!!!!!!!!!!!!!!!!!!!!!!!!!
                Dictionary<string, Tuple<float, float>> deletme = new Dictionary<string, Tuple<float, float>>();
                deletme.Add("aileron,elevator", new Tuple<float, float>(0.5F, 0.3F));
                deletme.Add("throttle,latitude-deg", new Tuple<float, float>(0.5f, 1.23F));
                Current_attribute = current_attribute;
                //delet me
                initDictionary(deletme);
                NotifyPropertyChanged("CsvPath");
            }
        }

        public List<string> XmlNameList
        {
            get
            {
                return xmlNameList;
            }
            set
            {
                xmlNameList = value;
                NotifyPropertyChanged("XmlNameList");
            }
        }
        /* private void creat_csvWithHeader()
         {
             string file_name = "csvWithHeader.csv";
             string[] lines = { "First line", "Second line", "Third line" };
             string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
             using (StreamWriter outputFile = new StreamWriter(Path.Combine(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()), file_name))) 
             {
                 foreach (string line in lines)
                     outputFile.WriteLine(line);
             };
         }*/
        public List<DataPoint> RegressionPoints
        {
            get
            {
                return regressionPoints;
            }
            set
            {
                regressionPoints = atributes_2_Two_Correlated_attribute_dict[Current_attribute].Correlated_points;
                NotifyPropertyChanged("RegressionPoints");
            }
        }
        public string XmlPath
        {
            get
            {
                return xmlPath;
            }
            set
            {
                xmlPath = value;
                buildNameListFromXML();
                /*initDictionary("aileron,elevator rudder,flaps slats,speedbrake throttle,latitude-deg");*/
                NotifyPropertyChanged("XmlNameList");
                NotifyPropertyChanged("XmlPath");
            }
        }
        public void connect(string ip, int port)
        {
            stop = false;
            if (port == 5400)
                tc.connect(ip, port);
            if (port == 5402)
                tc_reader.connect(ip, port);
        }

        public void disconnect()
        {
            stop = true;
            tc.disconnect();
        }
        public string Time
        {
            get
            {
                int x = (CurrentLine / 10) % 60;
                int y = (CurrentLine / 10) / 60;
                return (y.ToString() + ":" + x.ToString());
            }
            set
            {
            }
        }
        public float LineRatio
        {
            get
            {
                return ((float)CurrentLine) / NumOfLines;
            }
            set
            {
                CurrentLine = Convert.ToInt32(value * ((float)NumOfLines));
                NotifyPropertyChanged("CurrentLine");
                NotifyPropertyChanged("LineRatio");
            }
        }
        /*private void notifyHelper()
        {
            NotifyPropertyChanged("Atributes_atIndex");
        }*/
        //
        private void getAndSaveFG_attribute()
        {
            /*string input, input_digits_and_dot = "";
            int first, second, i = 0;
            float converted_input;*/
            NotifyPropertyChanged("Atributes_atIndex");


            //new Thread(display_atribute_update).Start();
            // List of strings binded from the XAML directly to the attributes in the model
            string[] bindedPropertiesAttributes = { "altimeter_indicated-altitude-ft", "airspeed-kt", "heading-deg", "roll-deg", "pitch-deg", "side-slip-deg" };
            while (currentLine < numOfLines && !stop)
            {
                if (Play && start_to_read)
                {
                   // foreach attribute in bindedPropertiesAttributes update the View Model that a change was made
                   for(int j = 0; j < 6; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                Altmeter = Convert.ToSingle(attribute[bindedPropertiesAttributes[0]][currentLine].Y);
                                break;
                            case 1:
                                Airspeed = Convert.ToSingle(attribute[bindedPropertiesAttributes[1]][currentLine].Y);
                                break;
                            case 2:
                                Registered_heading_degrees = Convert.ToSingle(attribute[bindedPropertiesAttributes[2]][currentLine].Y);
                                break;
                            case 3:
                                Roll = Convert.ToSingle(attribute[bindedPropertiesAttributes[3]][currentLine].Y);
                                break;
                            case 4:
                                Pitch = Convert.ToSingle(attribute[bindedPropertiesAttributes[4]][currentLine].Y);
                                break;
                            case 5:
                                Yaw = Convert.ToSingle(attribute[bindedPropertiesAttributes[5]][currentLine].Y);
                                break;

                        }
                    }

                    Aileron = Convert.ToSingle(attribute["aileron"][CurrentLine].Y);
                    Rudder = Convert.ToSingle(attribute["rudder"][CurrentLine].Y);
                    Throttle0 = Convert.ToSingle(attribute["throttle"][CurrentLine].Y);
                    Elevator = Convert.ToSingle(attribute["elevator"][CurrentLine].Y);
                    Thread.Sleep(1000);
                }
            }
        }
        public void line_to_atributes_arr(String line, int index)
        {
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            String[] Fields = CSVParser.Split(line);

            int size = xmlNameList.Count();
            for (int i = 0; i < size; i++)
            {
                attribute[xmlNameList[i]].Add(new DataPoint(index, float.Parse(Fields[i])));
               // atributes[i].Add(new DataPoint(index, float.Parse(Fields[i])));
            }
            // atributes_are_ready = true;


        }
        //VERSION WITHOU IENUMERATOR
        private void display_atribute_update()
        {
            int local_current_line = 0, range = 0;
            string local_Current_attribute, local_Correlated_attribute;
            DataPoint[] temporalCv, temporalCv_cor;
            while (currentLine < 1)
            {
                Thread.Sleep(200);
            }
            while (currentLine < numOfLines && !stop)
            {
                mutex.WaitOne();
                local_current_line = CurrentLine;
                local_Current_attribute = Current_attribute;
                local_Correlated_attribute = attribute_correlated[Current_attribute];
                
                if (Play && start_to_read)
                {
                    if (display_lines_temp < local_current_line)
                    {
                        range = local_current_line - display_lines_temp;
                        temporalCv = new DataPoint[range];

                        attribute[local_Current_attribute].CopyTo(display_lines_temp, temporalCv, 0, range);
                        
                        plotPoints.AddRange(temporalCv);
                        NotifyPropertyChanged("PlotPoints");

                        if (!local_Correlated_attribute.Equals(no_attribute))
                        {
                           // range = attribute[attribute_correlated[Current_attribute]].Co
                            temporalCv_cor = new DataPoint[range];

                            attribute[local_Correlated_attribute].CopyTo(display_lines_temp, temporalCv, 0, range);
                            //display_lines_temp += range;
                            plotPoints_correlated.AddRange(temporalCv);
                        }
                        else
                        {
                            plotPoints_correlated = new List<DataPoint>();
                        }
                        display_lines_temp += range;
                        NotifyPropertyChanged("PlotPoints_correlated");
                    }
                    else if (display_lines_temp > local_current_line)
                    {
                        display_lines_temp = 0;
                        plotPoints_correlated = new List<DataPoint>();
                        plotPoints = new List<DataPoint>();
                        for (; display_lines_temp < local_current_line; display_lines_temp++)
                        {
                            plotPoints.Add(attribute[local_Current_attribute][display_lines_temp]);
                            if (!local_Current_attribute.Equals(no_attribute))
                                plotPoints_correlated.Add(attribute[local_Correlated_attribute][display_lines_temp]);
                        }
                        NotifyPropertyChanged("PlotPoints");
                        NotifyPropertyChanged("PlotPoints_correlated");
                    } 
                }
                mutex.ReleaseMutex();
                Thread.Sleep(500);
            }
        }
       private void fill_atributes_2_Two_Correlated_attribute_dict_List()
        {
            //attribute
            foreach(KeyValuePair<string, Two_Correlated_attribute> pair in atributes_2_Two_Correlated_attribute_dict){
                for(int point_index = 0; point_index < numOfLines; ++point_index)
                {
                    pair.Value.Correlated_points.Add(new DataPoint(attribute[pair.Key][point_index].Y, attribute[attribute_correlated[pair.Key]][point_index].Y ));
                }
            }
            Current_attribute = current_attribute;
        }
        public void start()
        {
            new Thread(delegate ()
            {

                var list = new List<string>();
                using (var reader = new StreamReader(csvPath))
                {
                    //Reading the file into a list then reading it to an array

                    string tmpLine;
                    while ((tmpLine = reader.ReadLine()) != null)
                    {
                        list.Add(tmpLine);
                    }
                    numOfLines = list.Count;
                }
                string[] result = list.ToArray();
                for (int k = 0; k < numOfLines; k++)
                {
                    line_to_atributes_arr(result[k], k);
                }
                fill_atributes_2_Two_Correlated_attribute_dict_List();
                while (CurrentLine <= numOfLines - 1 && !stop)
                {
                    if ((CurrentLine >= -1) && (PlaySpeed != "") && Play && (float.Parse(PlaySpeed) > 0))
                    {
                        if ((ProgressDirection == 1) || (CurrentLine > 0))
                        {
                            CurrentLine += ProgressDirection;
                        }
                        tc.write(result[CurrentLine]);
                        start_to_read = true;
                        Thread.Sleep(Convert.ToInt32(100 * (1 / float.Parse(PlaySpeed))));
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }).Start();
            new Thread(getAndSaveFG_attribute).Start();
            new Thread(display_atribute_update).Start();
        }

        //anomalyis

        private void setCorrelatedAttributes(string correlatedInfoStr)
        {
            string[] correlated_couples = correlatedInfoStr.Split(' ');
            foreach (string correlated_couple in correlated_couples)
            {
               
            }
        }

    }
}

//TOM AND RON AND MAIKY AND DANY 26.3 18:41

    //maiky g 7/4 15:42