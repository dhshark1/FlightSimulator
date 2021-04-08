//
// Created by tom on 09/01/2021.
//

#include "timeseries.h"
#include "HybridAnomalyDetector.h"
#include <fstream>
#define EXPORT extern "C" __declspec(dllexport)




EXPORT void getCorrelatedPairs_CPP(char args[], const char* trainPath, int csflag) {
	//ofstream out("asdasdasd.txt");
	
	SimpleAnomalyDetector ad;
	if (csflag) {
		ad.flag = csflag;
		ad.setCorrelationThreshold(0);
	}
	//TimeSeries ts("C:\\Users\\16475\\source\\repos\\ConsoleFinalTest\\trainFile.csv");
	TimeSeries ts(trainPath);
	ad.learnNormal(ts);
	std::vector<correlatedFeatures> cf = ad.getNormalModel();
	int j;
	int i = 0;
	for (int k = 0; k < cf.size(); k++) {
		string f1 = cf.at(k).feature1;
		string f2 = cf.at(k).feature2;
		for (j = 0;j < f1.size();++j) {
			args[i + j] = f1[j];
		}
		i += j;
		args[i++] = ',';
		for (j = 0;j < f2.size();++j) {
			args[i + j] = f2[j];
		}
		i += j;
		args[i++] = ' ';
	}
	args[i] = 0;
}

class VectorWrapper {
	std::vector<pair<float, float>> vec;
	std::vector<pair<string, int>> vecSI;
	
public:
	float curPair[2];
	char p[100];
	VectorWrapper() {
	}
	int VecSize() {
		return vec.size();
	}
	int VecSizeSI() {
		return vecSI.size();
	}
	void push_back(float f1, float f2) {
		vec.push_back({ f1, f2 });
	}
	void push_back(string s1, int i1) {
		//char* p = new char[s1.size() + 1];
		//char p[100];
		//strcpy(p, s1.c_str());
		vecSI.push_back({ s1,i1 });
	}
	std::pair<float,float> getByIndex(int x) {
		return vec[x];
	}
	std::pair<string, int> getByIndexSI(int x) {
		return vecSI[x];
	}
};
EXPORT void* CreateVectorWrapper() {
	return (void*) new VectorWrapper;
}

EXPORT int vectorSize(VectorWrapper * v) {
	return v->VecSize();
}

EXPORT int vectorSizeSI(VectorWrapper* v) {
	return v->VecSizeSI();
}

EXPORT void* getByIndex(VectorWrapper* v, int index) {
	v->curPair[0] = v->getByIndex(index).first;
	v->curPair[1] = v->getByIndex(index).second;
	return (void*) v->curPair;
}

EXPORT void getByIndexSIString(VectorWrapper* v, int index, char args[]) {
	//strcpy(v->p, v->getByIndexSI(index).first.c_str());
	//return (void*) v->p;
	int i = 0;
	for (;i < v->getByIndexSI(index).first.size();++i) {
		args[i] = v->getByIndexSI(index).first[i];
	}
	args[i] = 0;
}

EXPORT int getSizeStringByIndexSI(VectorWrapper* v, int index) {
	/*char* c = v->getByIndexSI(index).first;
	int i = 1;
	while (*c != 0) {
		++i;++c;
	}*/
	return v->getByIndexSI(index).first.size();
}

EXPORT int getByIndexSIInt(VectorWrapper* v, int index) {
	return v->getByIndexSI(index).second;
}

EXPORT void getCorrelatedPairsWithLines_CPP(VectorWrapper* lineInfo, const char* trainPath, int csflag) {
	SimpleAnomalyDetector ad;
	if (csflag) {
		ad.flag = csflag;
		ad.setCorrelationThreshold(0);
	}
	TimeSeries ts(trainPath);
	ad.learnNormal(ts);
	std::vector<correlatedFeatures> cf = ad.getNormalModel();
	for (int i=0; i < cf.size();i++) {
		lineInfo->push_back(cf[i].lin_reg.a, cf[i].lin_reg.b);
	}
}

EXPORT void getAnomalies_CPP(VectorWrapper* lineInfo, const char* trainPath, const char* testPath) {
	SimpleAnomalyDetector ad;
	TimeSeries ts(trainPath);
	ad.learnNormal(ts);
	TimeSeries ts2(testPath);
	vector<AnomalyReport> arr = ad.detect(ts2);
	for (auto& i : arr) {
		string a = i.description.substr(0, i.description.find('-'));
		a += "," + i.description.substr(i.description.find('-')+1, (i.description.size()- i.description.find('-')));
		lineInfo->push_back(a, i.timeStep);
	}
}