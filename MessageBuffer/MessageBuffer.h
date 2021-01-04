#pragma once
#include <Windows.h>
#include <sstream>

unsigned long long ReverseBytes(unsigned long long value);
LPVOID SearchBuffer(char buffer[], unsigned long long ID, SIZE_T bufferSize, LPVOID baseAddress);

extern "C"
{
	__declspec(dllexport) void _stdcall Initialize(int bufferSize);
	__declspec(dllexport) bool _stdcall Connect(int pID, unsigned long long bufferID);
	__declspec(dllexport) void _stdcall ReadMessage(char receiveBuffer[], unsigned long long receiveCode, bool keepMessage);
	__declspec(dllexport) bool _stdcall WriteMessage(char sendBuffer[]);
	__declspec(dllexport) int _stdcall GetAddress();
	__declspec(dllexport) int _stdcall GetPageNum();
};