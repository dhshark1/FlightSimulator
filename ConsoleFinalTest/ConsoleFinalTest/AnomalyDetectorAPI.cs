using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace ConsoleFinalTest
{
    
    public static class AnomalyDetectorAPI
    {
        [DllImport("FINALCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getCorrelatedPairs_CPP(StringBuilder sb, string trainPath, int flag);
        [DllImport("FINALCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getCorrelatedPairsWithLines_CPP(IntPtr lineInfo, string trainPath, int flag);
        [DllImport("FINALCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateVectorWrapper();

        [DllImport("FINALCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vectorSize(IntPtr vec);

        [DllImport("FINALCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getByIndex(IntPtr vec, int index);
        [DllImport("FINALCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getAnomalies_CPP(IntPtr vecSI, string trainPath, string testPath);
        [DllImport("FINALCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vectorSizeSI(IntPtr vec);

        [DllImport("FINALCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getByIndexSIString(IntPtr vec, int index, StringBuilder sb);
        [DllImport("FINALCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getSizeStringByIndexSI(IntPtr vec, int index);
        [DllImport("FINALCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getByIndexSIInt(IntPtr vec, int index);


        //unsafe static void Main(string[] args)
        //{

        /*public static Dictionary<string,Tuple<float,float>> getCorPairsAndLines(List<string> att, string trainCSVPath)
        {
            Dictionary<string, Tuple<float, float>> result = new Dictionary<string, Tuple<float, float>>();
            string corPairs = getCorrelatedPairs(trainCSVPath,1);

            return result;
        }*/
        public static string getCorrelatedPairs(string trainCSVPath, int flag)
        {
            //flag=1 pearson mode, flag=0 simpleanomaly mode
            //trainCsvPath format's example: C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv
            //
            StringBuilder sb = new StringBuilder();
            getCorrelatedPairs_CPP(sb, trainCSVPath, flag);
            return sb.ToString();
            //System.Diagnostics.Debug.WriteLine(sb.ToString());
        }
        public static Dictionary<string, OxyPlot.Wpf.Annotation> getCorrelatedPairsWithLines(string trainCSVPath, int flag)
        {
            //flag=1 pearson mode, flag=0 simpleanomaly mode
            string corPairs = getCorrelatedPairs(trainCSVPath,flag);
            int numPairs = corPairs.Count(Char.IsWhiteSpace);
            IntPtr lineInfo = CreateVectorWrapper();
            getCorrelatedPairsWithLines_CPP(lineInfo, trainCSVPath, flag);
            List<Tuple<float, float>> t = new List<Tuple<float, float>>();
            Dictionary<string, OxyPlot.Wpf.Annotation> my_dic = new Dictionary<string, OxyPlot.Wpf.Annotation>();
            int size = vectorSize(lineInfo);
            for (int i = 0; i < size; i++)
            {
                IntPtr f = getByIndex(lineInfo, i);
                float[] temp = new float[2];
                Marshal.Copy(f, temp, 0, 2);
                Tuple<float, float> p = new Tuple<float, float>(temp[0], temp[1]);
                t.Add(p);
            }
            string[] corPairsArr = corPairs.Split(' ');
            for(int i =0; i<numPairs;++i)
            {
                string[] pair = corPairsArr[i].Split(',');
                OxyPlot.Wpf.LineAnnotation la = new OxyPlot.Wpf.LineAnnotation();
                la.Slope = t[i].Item1;
                la.Intercept = t[i].Item2;
                la.LineStyle = OxyPlot.LineStyle.Solid;
                my_dic.Add(pair[0], la);
                my_dic.Add(pair[1], la);
            }
            
            //TBD - depends on which wpf form we need to create with the given data above
            return my_dic;
        }

        public static void getAnomalies(string trainCSVPath, string testCSVPath)
        {
            IntPtr lineInfo = CreateVectorWrapper();
            getAnomalies_CPP(lineInfo, trainCSVPath, testCSVPath);
            List<Tuple<string, int>> anomalies = new List<Tuple<string, int>>();
            int size = vectorSizeSI(lineInfo);
            for (int i = 0; i < size; i++)
            {
                StringBuilder sb = new StringBuilder();
                getByIndexSIString(lineInfo, i, sb);
                //int sizeS = getSizeStringByIndexSI(lineInfo, i);
                int y = getByIndexSIInt(lineInfo, i);
                //char[] cArr = new char[sizeS+1];


                //Marshal.Copy(s, cArr, 0, sizeS);
                //string tom = Marshal.PtrToStringAuto(s, sizeS);
                //string dic = Marshal.PtrToStringUni(s);
                Tuple<string, int> p = new Tuple<string, int>(sb.ToString(), y);
                anomalies.Add(p);
            }
            return;
        }
        public static void Main()
        {
            //string output = getCorrelatedPairs("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv");
            //System.Diagnostics.Debug.WriteLine(output);
            Dictionary<string, OxyPlot.Wpf.Annotation> l =getCorrelatedPairsWithLines("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv",1);
            getAnomalies("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv", "C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\testFile.csv");
        }
    }
}
