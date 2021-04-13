using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AnomalyDLL
{
    public class AnomalyDetector
    {
        [DllImport("plugins\\HybridAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getCorrelatedPairs_CPP(StringBuilder sb, string trainPath);
        [DllImport("plugins\\HybridAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getCorrelatedPairsWithPointAndRadius_CPP(IntPtr lineInfo, string trainPath);
        [DllImport("plugins\\HybridAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateVectorWrapper();

        [DllImport("plugins\\HybridAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vectorSize(IntPtr vec);

        [DllImport("plugins\\HybridAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getByIndex(IntPtr vec, int index);
        [DllImport("plugins\\HybridAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void getAnomalies_CPP(IntPtr vecSI, string trainPath, string testPath);
        [DllImport("plugins\\HybridAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vectorSizeSI(IntPtr vec);

        [DllImport("plugins\\HybridAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getByIndexSIString(IntPtr vec, int index, StringBuilder sb);
        [DllImport("plugins\\HybridAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getSizeStringByIndexSI(IntPtr vec, int index);
        [DllImport("plugins\\HybridAnomalyDLLCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getByIndexSIInt(IntPtr vec, int index);


        public static string getCorrelatedPairs(string trainCSVPath)
        {
            StringBuilder sb = new StringBuilder(65542);
            getCorrelatedPairs_CPP(sb, trainCSVPath);
            return sb.ToString();
        }
        public static Dictionary<string, OxyPlot.Wpf.Annotation> getAttributeWithADAnnotations(string trainCSVPath)
        {
            string corPairs = getCorrelatedPairs(trainCSVPath);
            string[] corPairsArr = corPairs.Split(' ');

            int numPairs = corPairs.Count(Char.IsWhiteSpace);
            IntPtr lineInfo = CreateVectorWrapper();
            getCorrelatedPairsWithPointAndRadius_CPP(lineInfo, trainCSVPath);
            List<Tuple<Tuple<float, float>, float>> t = new List<Tuple<Tuple<float, float>, float>>();
            int size = vectorSize(lineInfo);
            for (int i = 0; i < size; i++)
            {
                IntPtr f = getByIndex(lineInfo, i);
                float[] temp = new float[3];
                Marshal.Copy(f, temp, 0, 3);
                Tuple<float, float> p = new Tuple<float, float>(temp[0], temp[1]);
                t.Add(new Tuple<Tuple<float, float>, float>(p, temp[2]));
            }
            Dictionary<string, OxyPlot.Wpf.Annotation> dWithAnno = new Dictionary<string, OxyPlot.Wpf.Annotation>();
            int j = 0;
            foreach(Tuple<Tuple<float, float>, float> i in t)
            {
                OxyPlot.Wpf.EllipseAnnotation pa = new OxyPlot.Wpf.EllipseAnnotation();
                pa.X = i.Item1.Item1;
                pa.Y = i.Item1.Item2;
                pa.Height = i.Item2*2;
                pa.Width = i.Item2 * 2;
                pa.Fill = System.Windows.Media.Colors.Transparent;
                pa.StrokeThickness = 2;
                dWithAnno.Add(corPairsArr[j], pa);
                ++j;
            }


            return dWithAnno;
        }

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


        /*public static void Main()
        {
            string output = getCorrelatedPairs("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv");
            System.Diagnostics.Debug.WriteLine(output);
            getCorrelatedPairsWithAnnotation("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv");
            getAnomalies("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv", "C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\testFile.csv");
        }*/
    }
}
