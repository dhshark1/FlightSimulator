/*#include "SimpleAnomalyDetector.h"
#include <stdlib.h>
#include <iostream>
#include <fstream>*/
#include "pch.h"
#include "MainTrain.h"



//using namespace std;

/*extern "C" {
	__declspec(dllexport) int mainTrain() {
		ofstream out("asdasdasd.txt");
		SimpleAnomalyDetector ad;
		TimeSeries ts("trainFile.csv");
		ad.learnNormal((const TimeSeries&)ts);
		TimeSeries ts2("testFile.csv");
		vector<AnomalyReport> arr = ad.detect(ts2);
		string anomalies;
		string str;
		for (auto& i : arr) {
			string a = i.description.substr(0, i.description.find('-'));
			a += "," + i.description.substr(2, i.description.find('-'));
			str = a + ", " + to_string(i.timeStep);
			out.write(str.c_str(), sizeof(str));
		}
		out.close();
		return 0;
	}
}
*/