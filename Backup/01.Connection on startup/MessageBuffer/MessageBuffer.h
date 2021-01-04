#pragma once
#include <Windows.h>
#include <sstream>

unsigned long long ReverseBytes(unsigned long long value);
LPVOID SearchBuffer(char buffer[], unsigned long long ID, SIZE_T bufferSize, LPVOID baseAddress);

extern "C"
{
	void __declspec(dllexport) _stdcall Initialize(int bufferSize);
	bool __declspec(dllexport) _stdcall Connect(int pID, unsigned long long bufferID);
	void __declspec(dllexport) _stdcall ReadMessage(char receiveBuffer[], unsigned long long receiveCode, bool keepMessage);
	bool __declspec(dllexport) _stdcall WriteMessage(char sendBuffer[]);
};