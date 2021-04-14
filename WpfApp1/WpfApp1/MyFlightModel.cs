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
using System.Text;

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
        // Object to hold the DLL instance with the relevant methods.
        string dllFullPath;
        private volatile List<string> anomalyReportList = new List<string> ();
        //private volatile Dictionary<string, > anomalyReportList = new List<string>();
        private volatile List<Tuple<string, int>> TESTForTomsDLL = new List<Tuple<string, int>> { new Tuple<string, int>("A,B", 5), new Tuple<string, int>("C,D", 180), new Tuple<string, int>("G,F", 183) };
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
        private volatile string investigatedAnomaly;
        //private volatile List<string> anomalyReportList = new List<string>();
        //private MyTelnetClient tc_reader;
        private string[] get_msgs = new string[6] { "get /instrumentation/altimeter/indicated-altitude-ft", "get /velocities/airspeed-kt[0]", "get /orientation/heading-deg", "get /orientation/roll-deg", "get /orientation/pitch-deg", "get /orientation/side-slip-deg" };
        private volatile float altmeter = 0, airspeed = 0, registeredHeading_degrees = 0;
        private volatile float pitch = 0, roll = 0, yaw = 0;
        private volatile float aileron = 0, throttle0 = 0, rudder = 0, elevator = 0;
        private short atributes_index = 0;
        private volatile bool start_to_read = false, first = false, init = false;
        /*private volatile bool atributes_are_ready = false;*/
        private volatile List<string> xmlNameList;
        /*private volatile List<ListBoxItem> listBoxxmlNameList;*/
        public volatile List<DataPoint>[] atributes = new List<DataPoint>[42];
        public volatile Dictionary<String, List<DataPoint>> attribute = new Dictionary<string, List<DataPoint>>();
        public volatile Dictionary<String, String> attribute_correlated = new Dictionary<string, String>();
        public volatile Dictionary<string, AnomalyInfo> Anomaly_2_AnomalyInfo = new Dictionary<string, AnomalyInfo>();
        private volatile List<DataPoint> anomalyReportRegressionList = new List<DataPoint>();

        volatile string plotTitle = "";
        volatile List<DataPoint> plotPoints = new List<DataPoint>();

        volatile string plotTitle_correlated = "";
        volatile List<DataPoint> plotPoints_correlated = new List<DataPoint>();
        volatile List<DataPoint> regressionPoints_last_30 = new List<DataPoint>();
        volatile List<DataPoint> regressionPoints = new List<DataPoint>();

        //key - attribute(like ailrone) , value - tuple(first slope, second intersect)
        Dictionary<string, Tuple<float, float>> regression_dict = new Dictionary<string, Tuple<float, float>>();
        /*Dictionary<string, Tuple<float, float, List<DataPoint>>> regression_and_points_dict = new Dictionary<string, Tuple<float, float, List<DataPoint>>>();*/
        //Two_Correlated_attribute
        Dictionary<string, Two_Correlated_attribute> atributes_2_Two_Correlated_attribute_dict = new Dictionary<string, Two_Correlated_attribute>();

        volatile Dictionary<string, OxyPlot.Wpf.Annotation> ReturnValue2;
        private OxyPlot.Wpf.Annotation investigated_Annotation;

        public float SlopeLineAnnotation
        {
            get
            {
                return slopeLineAnnotation;
            }
            set
            {
                slopeLineAnnotation = value;
                NotifyPropertyChanged("SlopeLineAnnotation");
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
                NotifyPropertyChanged("InterceptLineAnnotation");
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

        public List<DataPoint> AnomalyReportRegressionList
        {
            get
            {
                return anomalyReportRegressionList;
            }
            set
            {
                anomalyReportRegressionList = value;
                NotifyPropertyChanged("AnomalyReportRegressionList");
            }
        }
        //regressionPoints_last_30
        public List<DataPoint> RegressionPoints_last_30
        {
            get
            {
                return regressionPoints_last_30;
            }
            set
            {
                regressionPoints_last_30 = value;
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
        public OxyPlot.Wpf.Annotation Investigated_Annotation
        {
            get
            {

                return investigated_Annotation;
            }
            set
            {
                investigated_Annotation = value;
                NotifyPropertyChanged("Investigated_Annotation");
            }
        }
        public string InvestigatedAnomaly
        {
            get { return investigatedAnomaly; }
            set
            {
                investigatedAnomaly = value;
                if (Anomaly_2_AnomalyInfo.ContainsKey(InvestigatedAnomaly))
                {
                    CurrentLine = Anomaly_2_AnomalyInfo[InvestigatedAnomaly].AnomalyLine;
                    AnomalyReportRegressionList = Anomaly_2_AnomalyInfo[InvestigatedAnomaly].Points;
                    Investigated_Annotation = Anomaly_2_AnomalyInfo[InvestigatedAnomaly].Anno;
                    NotifyPropertyChanged("InvestigatedAnomaly");
                }
                
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
        public string DllFullPath
        {
            get { return dllFullPath; }
            set
            {
                dllFullPath = value;
            }
        }
        public MyFlightModel()
        {
            
            tc = new MyTelnetClient();
            //tc_reader = new MyTelnetClient();
            PlaySpeed = "1";
            CurrentLine = -1;
            NumOfLines = 1;
            ProgressDirection = 1;
            csvPath = "";
            //listBoxxmlNameList = new List<ListBoxItem>();
            xmlNameList = new List<string>();
            attribute.Add(no_attribute, new List<DataPoint>());
            //Current_attribute = current_attribute;

            //Current_attribute
            //attribute 
            //atributes[0] = new ChartValues<float>();
            //atributes[0].Add(0);
            var dllFile = new System.IO.FileInfo(@"plugins\SimpleAnomalyDLL.dll");
            DllFullPath = dllFile.FullName;
            NotifyPropertyChanged("Current_attribute");

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
                /*Dictionary<string, Tuple<float, float>> deletme = new Dictionary<string, Tuple<float, float>>();
                deletme.Add("aileron,elevator", new Tuple<float, float>(0.5F, 0.3F));
                deletme.Add("throttle,latitude-deg", new Tuple<float, float>(0.5f, 1.23F));*/
                var dllFile = new System.IO.FileInfo(@"plugins\SimpleAnomalyDLL.dll");
                System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(dllFile.FullName);
                Object MyDLLInstance = (Object)myDllAssembly.CreateInstance("AnomalyDLL.AnomalyDetector");
                object[] argstopass = new object[] { (object)@"csvs\trainFile.csv" };
                Dictionary<string, Tuple<float, float>> ReturnValue = (Dictionary<string, Tuple<float, float>>)MyDLLInstance
                .GetType() //Get the type of MyDLLForm
                .GetMethod("getUseCaseEight") //Gets a System.Reflection.MethodInfo 
                                              //object representing Some
                .Invoke(MyDLLInstance, argstopass);
                Current_attribute = current_attribute;
                
                initDictionary(ReturnValue);
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
        public int connect(string ip, int port)
        {
            /*
             * This method purpose is to connect to the filghtGear using TelnetClient field's connect method
             */
            stop = false; 
            int result = 0;
            if (port == 5400)// it is possible to connect only with port 5400, this port is used by the fg to accept lines from regflight.csv
                result = tc.connect(ip, port);
            return result;
        }

        public void disconnect()
        {
            /*
             * This method purpose is to disconnnect from the flightGear using TelnetClient's disconnet method
             */
            stop = true;
            tc.disconnect();
        }

        //Properties that are in use of the view:
        public string Time
        {
            //Is in use of the time displaying control (view)
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
            //Is in use of the Slider 
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

        private void getAndSaveFG_attribute()
        {
            /*
             * This method purpose is to update attributes according to the currentLine (=int variable that equals to the number of the last line that was sent to the fg)
             * 
             */
            NotifyPropertyChanged("Atributes_atIndex");


            // List of strings binded from the XAML directly to the attributes in the model
            string[] bindedPropertiesAttributes = { "altimeter_indicated-altitude-ft", "airspeed-kt", "heading-deg", "roll-deg", "pitch-deg", "side-slip-deg" };
            while (currentLine < numOfLines && !stop)
            {
                if (Play && start_to_read)
                {
                   // foreach attribute in bindedPropertiesAttributes update the View Model that a change was made
                   //attribute is a dictionary that mapped an attributes to all of its values in the csv file that was loaded
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
            }


        }
        private void display_atribute_update()
        {
            /*
             * This methods is incharge of updating the variables that containes the points of selected attribute and its most correlated attribute
             * This method will be running on seperate thread. It is running as long the flight is running. The main loop will run as long the flight is on with sleep of 500 milisec between two followed iterations.
             *  
             */
            int local_current_line = 0, range = 0;
            string local_Current_attribute, local_Correlated_attribute;
            DataPoint[] temporalCv, temporalCv_cor, temporalRg;
            while (currentLine < 1)
            {
                Thread.Sleep(200);
            }
            while (currentLine < numOfLines && !stop)
            {
                mutex.WaitOne();//The main thread is also updates the plotPoints list, therefore mutex is in use.
                local_current_line = CurrentLine;
                local_Current_attribute = Current_attribute;
                local_Correlated_attribute = attribute_correlated[Current_attribute];
                
                if (Play && start_to_read)
                {
                    if (display_lines_temp < local_current_line)
                    {
                        //as the flight progresses forward, it is not needed to build a new list of data point in each iteration, threrefore the list will be updated according to the difference between the last current line
                        //that was sent to the fg before the iteration was started, and the last line that was sent to fg and the list of plotPoints is updated according to.
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
                        //In case the flight was moved backward then the plotPoint list and the plotPoint_correlated are built from the start
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
                    temporalRg = new DataPoint[30];
                    if(local_current_line<30)
                        regressionPoints.CopyTo(0, temporalRg, 0, local_current_line);
                    else
                        regressionPoints.CopyTo(local_current_line - 30, temporalRg, 0, 30);
                    //display_lines_temp += range;
                    regressionPoints_last_30 = new List<DataPoint> (temporalRg);
                    NotifyPropertyChanged("RegressionPoints_last_30");
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
               /* pair.Value.Correlated_points.Add(new DataPoint(0, pair.Value.Intercept));
                pair.Value.Correlated_points.Add(new DataPoint(numOfLines, (pair.Value.Slope * numOfLines)+ pair.Value.Intercept));*/
            }
            Current_attribute = current_attribute;
        }
        public static void AddAttributeLineToCSV(string pathRead, string pathWrite)
        {
            // Name of all attributes to add to the csv first line
            const string attribute = "aileron	elevator	rudder	flaps	slats	speedbrake	throttle	throttle1	engine-pump	engine-pump1	electric-pump	electric-pump1	external-power	APU-generator	latitude-deg	longitude-deg	altitude-ft	roll-deg	pitch-deg	heading-deg	side-slip-deg	airspeed-kt	glideslope	vertical-speed-fps	airspeed-indicator_indicated-speed-kt	altimeter_indicated-altitude-ft	altimeter_pressure-alt-ft	attitude-indicator_indicated-pitch-deg	attitude-indicator_indicated-roll-deg	attitude-indicator_internal-pitch-deg	attitude-indicator_internal-roll-deg	encoder_indicated-altitude-ft	encoder_pressure-alt-ft	gps_indicated-altitude-ft	gps_indicated-ground-speed-kt	gps_indicated-vertical-speed	indicated-heading-deg	magnetic-compass_indicated-heading-deg	slip-skid-ball_indicated-slip-skid	turn-indicator_indicated-turn-rate	vertical-speed-indicator_indicated-speed-fpm	engine_rpm";
            IList<string> attributeList = attribute.Split("\t");
            IList<string> nonReplicaAttributeList = new List<string>();

            foreach (string str in attributeList)
            {
                if (nonReplicaAttributeList.Contains(str))
                {
                    nonReplicaAttributeList.Add(str + "1");
                }
                else
                {
                    nonReplicaAttributeList.Add(str);
                }
            }
            StringBuilder sb = new StringBuilder();
            // creating the line to write in the first line of the CSV file
            int index = 1;
            foreach (string str in nonReplicaAttributeList)
            {
                sb.Append(str + ",");
                index++;
            }
            // Holds the attributes seperated by ','
            string strAttributeList = sb.ToString() + "\n";
            // THe file without the Attributes
            StreamReader sr = new StreamReader(pathRead);
            // Creating new CSVFile with headers
            StreamWriter sw = new StreamWriter(pathWrite);
            // Writing the first line in the CSV (All the attributes name)
            sw.Write(strAttributeList);

            // filling the destination CSV file with the attributes headers
            string line = sr.ReadLine();
            while (line != null)
            {
                sw.WriteLine(line);
                line = sr.ReadLine();
            }
            sr.Close();
            sw.Close();
        }
        private void fill_AnomalyRerpotList()
        {
            string min, sec;
            //
            //creat testfile with headers
            AddAttributeLineToCSV(CsvPath, @"csvs\testFile.csv");
            //

            System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(DllFullPath);
            Object MyDLLInstance = (Object)myDllAssembly.CreateInstance("AnomalyDLL.AnomalyDetector");
            object[] argstopass1 = new object[] { (object)@"csvs\trainFile.csv", @"csvs\testFile.csv" };
            List<Tuple<string, int>> ReturnValue = (List<Tuple<string, int>>)MyDLLInstance
            .GetType() //Get the type of MyDLLForm
            .GetMethod("getAnomalies") //Gets a System.Reflection.MethodInfo 
                                       //object representing Some
            .Invoke(MyDLLInstance, argstopass1);

           
            object[] argstopass2 = new object[] { (object)@"csvs\trainFile.csv" };
            ReturnValue2 = (Dictionary<string, OxyPlot.Wpf.Annotation>)MyDLLInstance
            .GetType() //Get the type of MyDLLForm
            .GetMethod("getAttributeWithADAnnotations") //Gets a System.Reflection.MethodInfo 
                                                        //object representing Some
            .Invoke(MyDLLInstance, argstopass2);

            string str, first,second;
            List<DataPoint> temp;
            foreach (Tuple<string, int> pair in ReturnValue)
            {

                min = ((pair.Item2 / 10) / 60).ToString();
                sec = ((pair.Item2 / 10) % 60).ToString();
                if (Convert.ToInt32(sec) < 10)
                    sec = "0" + sec;
                str = pair.Item1 + " " + min + ":" + sec + "\nAnomaly Line: " + pair.Item2.ToString();
                anomalyReportList.Add(str);
                temp = new List<DataPoint>();
                first = pair.Item1.Split(",")[0];
                second = pair.Item1.Split(",")[1];
                for (int point_index = 0; point_index < numOfLines; ++point_index)
                {
                    temp.Add(new DataPoint(attribute[first][point_index].Y, attribute[second][point_index].Y));
                }
                Anomaly_2_AnomalyInfo.Add(str, new AnomalyInfo(pair.Item2, ReturnValue2[pair.Item1], temp));

            }
            NotifyPropertyChanged("AnomalyReportList");
        }

        public void start()
        {
            /*
             * When the uploadFile button is selected the model will try to connect the flightGear, after they are connected this method is invoked.
             * This method will invoke 3 threads - In general: thread number one is incharge of sending lines from the csv to the flightgear,
             * theard number two is in charge of updating the properties of the flight's attributes, each one of them will be update to the most updated value according to the last line that was sent to FG.
             * thread number three is in charge of updating the list of dataPoints of the selected attribute (by the user to be displayed in the graph) and its most correlated attribute.
             */
            
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
                init = true;
                float ps;
                while (CurrentLine <= numOfLines - 1 && !stop)
                {
                    try
                    {
                        ps = float.Parse(PlaySpeed);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    if ((CurrentLine >= -1)  && Play && (ps > 0))
                    {

                        if ((ProgressDirection == 1) || (CurrentLine > 0))
                        {
                            CurrentLine += ProgressDirection;
                        }
                        tc.write(result[CurrentLine]);
                        start_to_read = true;
                        Thread.Sleep(Convert.ToInt32(100 * (1 / ps)));
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }).Start();
            new Thread(getAndSaveFG_attribute).Start();
            new Thread(display_atribute_update).Start();
            while (!init)
            {

            }
            fill_AnomalyRerpotList();
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

//$$$