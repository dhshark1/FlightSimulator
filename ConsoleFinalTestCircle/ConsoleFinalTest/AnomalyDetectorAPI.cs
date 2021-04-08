using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using OxyPlot;
using OxyPlot.Series;

namespace ConsoleFinalTest
{
    
    public static class AnomalyDetectorAPI
    {
        [DllImport("ANOTHERCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getCorrelatedPairs_CPP(StringBuilder sb, string trainPath);
        [DllImport("ANOTHERCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getCorrelatedPairsWithPointAndRadius_CPP(IntPtr lineInfo, string trainPath);
        [DllImport("ANOTHERCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateVectorWrapper();

        [DllImport("ANOTHERCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vectorSize(IntPtr vec);

        [DllImport("ANOTHERCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getByIndex(IntPtr vec, int index);
        [DllImport("ANOTHERCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getAnomalies_CPP(IntPtr vecSI, string trainPath, string testPath);
        [DllImport("ANOTHERCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vectorSizeSI(IntPtr vec);

        [DllImport("ANOTHERCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getByIndexSIString(IntPtr vec, int index, StringBuilder sb);
        [DllImport("ANOTHERCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getSizeStringByIndexSI(IntPtr vec, int index);
        [DllImport("ANOTHERCHANCEDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getByIndexSIInt(IntPtr vec, int index);


        //unsafe static void Main(string[] args)
        //{
        public static string getCorrelatedPairs(string trainCSVPath)
        {
            //trainCsvPath format's example: C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv
            //
            StringBuilder sb = new StringBuilder();
            getCorrelatedPairs_CPP(sb, trainCSVPath);
            return sb.ToString();
            //System.Diagnostics.Debug.WriteLine(sb.ToString());
        }
        public static Dictionary<string,OxyPlot.Wpf.Annotation> getCorrelatedPairsWithAnnotation(string trainCSVPath)
        {
            string corPairs = getCorrelatedPairs(trainCSVPath);
            int numPairs = corPairs.Count(Char.IsWhiteSpace);
            IntPtr lineInfo = CreateVectorWrapper();
            getCorrelatedPairsWithPointAndRadius_CPP(lineInfo, trainCSVPath);
            List<Tuple<Tuple<float, float>, float>> t = new List<Tuple<Tuple<float, float>,float>>();
            int size = vectorSize(lineInfo);
            for (int i = 0; i < size; i++)
            {
                IntPtr f = getByIndex(lineInfo, i);
                float[] temp = new float[3];
                Marshal.Copy(f, temp, 0, 3);
                Tuple<float, float> p = new Tuple<float, float>(temp[0], temp[1]);
                t.Add(new Tuple<Tuple<float,float>,float>(p, temp[2]));
            }
            //TBD - depends on which wpf form we need to create with the given data above
            return;
        }

        public static List<Tuple<Tuple<string, string>,int>> getAnomalies(string trainCSVPath, string testCSVPath)
        {
            IntPtr lineInfo = CreateVectorWrapper();
            getAnomalies_CPP(lineInfo, trainCSVPath, testCSVPath);
            List<Tuple<Tuple<string, string>, int>> anomalies = new List<Tuple<Tuple<string, string>,int>>();
            int size = vectorSizeSI(lineInfo);
            for (int i = 0; i < size; i++)
            {
                StringBuilder sb = new StringBuilder();
                getByIndexSIString(lineInfo, i, sb);
                string[] features = sb.ToString().Split(',');
                int y = getByIndexSIInt(lineInfo, i);
                Tuple<string, string> p = new Tuple<string, string>(features[0], features[1]);
                anomalies.Add(new Tuple<Tuple<string, string>, int>(p, y));
            }
            return anomalies;
        }
        public static void Main()
        {
            string output = getCorrelatedPairs("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv");
            System.Diagnostics.Debug.WriteLine(output);
            getCorrelatedPairsWithAnnotation("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv");
            getAnomalies("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv", "C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\testFile.csv");
        }
    }
}
