using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
//using RGiesecke.DllExport;

namespace AnomalyDLL
{
    public class AnomalyDetector
    {
        [DllImport("plugins\\SimpleAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getCorrelatedPairs_CPP(StringBuilder sb, string trainPath, int flag);
        [DllImport("plugins\\SimpleAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getCorrelatedPairsWithLines_CPP(IntPtr lineInfo, string trainPath, int flag);
        [DllImport("plugins\\SimpleAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateVectorWrapper();

        [DllImport("plugins\\SimpleAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vectorSize(IntPtr vec);

        [DllImport("plugins\\SimpleAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getByIndex(IntPtr vec, int index);
        [DllImport("plugins\\SimpleAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getAnomalies_CPP(IntPtr vecSI, string trainPath, string testPath);
        [DllImport("plugins\\SimpleAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vectorSizeSI(IntPtr vec);

        [DllImport("plugins\\SimpleAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getByIndexSIString(IntPtr vec, int index, StringBuilder sb);
        [DllImport("plugins\\SimpleAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getSizeStringByIndexSI(IntPtr vec, int index);
        [DllImport("plugins\\SimpleAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getByIndexSIInt(IntPtr vec, int index);


        //unsafe static void Main(string[] args)
        //{

        //utils functions:
        private static string getCorrelatedPairsWithMode(string trainCSVPath, int flag)
        {
            //flag=1 pearson mode, flag=0 simpleAnomalyDete mode
            StringBuilder sb = new StringBuilder(65542);
            getCorrelatedPairs_CPP(sb, trainCSVPath, flag);
            return sb.ToString();
        }

        private static Dictionary<string, Tuple<float, float>> getCorrelatedPairsWithLines(string trainCSVPath, int flag)
        {
            //flag=1 pearson mode, flag=0 simpleAnomalyDete mode
            string corPairs = getCorrelatedPairsWithMode(trainCSVPath, flag);
            string[] corPairsArr = corPairs.Split(' ');
            int numPairs = corPairs.Count(Char.IsWhiteSpace);
            IntPtr lineInfo = CreateVectorWrapper();
            getCorrelatedPairsWithLines_CPP(lineInfo, trainCSVPath, flag);
            //List<Tuple<float, float>> t = new List<Tuple<float, float>>();
            int size = vectorSize(lineInfo);

            Dictionary<string, Tuple<float, float>> dic = new Dictionary<string, Tuple<float, float>>();
            for (int i = 0; i < size; i++)
            {
                IntPtr f = getByIndex(lineInfo, i);
                float[] temp = new float[2];
                Marshal.Copy(f, temp, 0, 2);
                Tuple<float, float> p = new Tuple<float, float>(temp[0], temp[1]);
                dic.Add(corPairsArr[i], p);
            }
            return dic;
        }


        //[DllExport("getUseCaseEight", CallingConvention = CallingConvention.Cdecl)]
        public static Dictionary<string, Tuple<float, float>> getUseCaseEight(string trainCSVPath)
        {
            return getCorrelatedPairsWithLines(trainCSVPath, 1);
        }



        //Methods that should implement the API of Anomaly Detector, that was defined by the FlightModel:
        //[DllExport("getCorrelatedPairs", CallingConvention = CallingConvention.Cdecl)]
        public static string getCorrelatedPairs(string trainCSVPath)
        {
            /*
             * This method should return string of all correlated pairs according to the anomaly detector definition of correlation
             */
            return getCorrelatedPairsWithMode(trainCSVPath, 0);
        }

        //[DllExport("getAttributeWithADAnnotations", CallingConvention = CallingConvention.Cdecl)]
        public static Dictionary<string, OxyPlot.Wpf.Annotation> getAttributeWithADAnnotations(string trainCSVPath)
        {
            /*
             * This method should return a dictionary of attributes and a correlated annotation of it and its most correlated attribtue (correlation between paris is defined by the anomaly detector itself (can be without pearson))
             */
            Dictionary<string, OxyPlot.Wpf.Annotation> dWithAnno = new Dictionary<string, OxyPlot.Wpf.Annotation>();
            Dictionary<string, Tuple<float, float>> dWithFloats = getCorrelatedPairsWithLines(trainCSVPath, 0);
            int sizeDic = dWithFloats.Count;
            int i = 0;
            foreach (KeyValuePair<string, Tuple<float, float>> pair in dWithFloats)
            {
                OxyPlot.Wpf.LineAnnotation la = new OxyPlot.Wpf.LineAnnotation();
                la.Slope = pair.Value.Item1;
                la.Intercept = pair.Value.Item2;
                la.LineStyle = OxyPlot.LineStyle.Solid;
                dWithAnno.Add(pair.Key, la);
            }
            return dWithAnno;
        }
        //[DllExport("getAnomalies", CallingConvention = CallingConvention.Cdecl)]
        public static List<Tuple<string, int>> getAnomalies(string trainCSVPath, string testCSVPath)
        {
            /*
             * This method should return list of string and int of the anomalies that was detected by the anomaly detector.
             */
            IntPtr lineInfo = CreateVectorWrapper();
            getAnomalies_CPP(lineInfo, trainCSVPath, testCSVPath);
            List<Tuple<string, int>> anomalies = new List<Tuple<string, int>>();
            int size = vectorSizeSI(lineInfo);
            for (int i = 0; i < size; i++)
            {
                StringBuilder sb = new StringBuilder(65542);
                getByIndexSIString(lineInfo, i, sb);
                int y = getByIndexSIInt(lineInfo, i);
                anomalies.Add(new Tuple<string, int>(sb.ToString(), y));
            }
            return anomalies;
        }
        /*public static void tom()
        {
            string output = getCorrelatedPairs("D:\\Users\\tomma\\Studies\\SecondYearSemesterD\\Projects\\repos\\ConsoleFinalTest\\trainFile.csv");
            System.Diagnostics.Debug.WriteLine(output);
            Dictionary<string, Tuple<float, float>> dic = getUseCaseEight("D:\\Users\\tomma\\Studies\\SecondYearSemesterD\\Projects\\repos\\ConsoleFinalTest\\trainFile.csv");
            Dictionary<string, OxyPlot.Wpf.Annotation> dicWithAnno = getAttributeWithADAnnotations("D:\\Users\\tomma\\Studies\\SecondYearSemesterD\\Projects\\repos\\ConsoleFinalTest\\trainFile.csv");
            //Dictionary<string, OxyPlot.Wpf.Annotation> l = getCorrelatedPairsWithLines("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv");
            getAnomalies("D:\\Users\\tomma\\Studies\\SecondYearSemesterD\\Projects\\repos\\ConsoleFinalTest\\trainFile.csv", "D:\\Users\\tomma\\Studies\\SecondYearSemesterD\\Projects\\repos\\ConsoleFinalTest\\testFile.csv");
            List<Tuple<string, int>> ano = getAnomalies("D:\\Users\\tomma\\Studies\\SecondYearSemesterD\\Projects\\repos\\ConsoleFinalTest\\trainFile.csv", "D:\\Users\\tomma\\Studies\\SecondYearSemesterD\\Projects\\repos\\ConsoleFinalTest\\testFile.csv");
            return;
        }*/
    }
}
