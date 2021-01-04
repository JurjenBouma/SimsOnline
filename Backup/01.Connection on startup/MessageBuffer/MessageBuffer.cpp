// MessageBuffer.cpp : Defines the exported functions for the DLL application.
//
#include "stdafx.h"
#include "MessageBuffer.h"
using namespace std;

int m_bufferSize;
LPVOID m_bufferAddress;
HANDLE m_hProcess;

void _stdcall Initialize(int bufferSize)
{
	m_bufferSize = bufferSize;
	m_bufferAddress = 0;
	m_hProcess = 0;
}

bool _stdcall Connect(int pID, unsigned long long bufferID)
{
	m_hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, pID);
	MEMORY_BASIC_INFORMATION MemInfo;
	LPVOID address = 0;

	unsigned long long bufferIDCode = ReverseBytes(bufferID);
	char readBuffer[512]{0};
	while (VirtualQueryEx(m_hProcess, address, &MemInfo, sizeof(MemInfo)))
	{
		DWORD page_unusable = PAGE_READWRITE | PAGE_WRITECOPY | PAGE_EXECUTE_READWRITE | PAGE_EXECUTE_WRITECOPY;
		if (MemInfo.State & MEM_COMMIT && MemInfo.Protect & page_unusable)
		{
			for (int i = 0; i, MemInfo.RegionSize; i+=512)
			{
				ReadProcessMemory(m_hProcess, (LPVOID)((SIZE_T)address+i), &readBuffer, 512, 0);//read to readbuffer.

				LPVOID tempAddress = 0;
				tempAddress = SearchBuffer(readBuffer, bufferIDCode, 512, (LPVOID)((SIZE_T)address +i));//returns 0 if no match
				if (tempAddress != 0)
				{
					m_bufferAddress = tempAddress;//address is found
					return true;
				}
			}
		}
		address = (LPVOID)((unsigned long)MemInfo.BaseAddress + MemInfo.RegionSize);
	}
	return false;
}

void _stdcall ReadMessage(char receiveBuffer[], unsigned long long receiveID, bool keepMessage)
{
	if (m_bufferAddress != 0)
	{
		char tempBuffer[10000];
		unsigned long long iD = 0;
		unsigned long long receiveIDCode = (receiveID);
		ReadProcessMemory(m_hProcess, m_bufferAddress, &iD, sizeof(iD), 0);
		if (iD == receiveIDCode)//new message
		{
			ReadProcessMemory(m_hProcess, m_bufferAddress, &tempBuffer, m_bufferSize, 0);
			if (!keepMessage)
			{
				unsigned long long emptyBufferID = 0;
				WriteProcessMemory(m_hProcess, m_bufferAddress, &emptyBufferID, sizeof(emptyBufferID), 0);
			}
			for (int i = 0; i < m_bufferSize; i++)
			{
				receiveBuffer[i] = tempBuffer[i];
			}
		}
	}
}

bool _stdcall WriteMessage(char sendBuffer[])
{
	if (m_bufferAddress != 0)
	{
		char tempBuffer[10000];
		for (int i = 0; i < m_bufferSize; i++)
		{
			tempBuffer[i] = sendBuffer[i];//copy buffer to prevent rubish data.
		}
		return (WriteProcessMemory(m_hProcess, m_bufferAddress, &tempBuffer, m_bufferSize,0) !=0);
	}
	return false;
}

unsigned long long ReverseBytes(unsigned long long value)
{
	unsigned long long bytes[8];
	bytes[0] = (value & 0xFF00000000000000) >> 56;
	bytes[1] = (value & 0x00FF000000000000) >> 40;
	bytes[2] = (value & 0x0000FF0000000000) >> 24;
	bytes[3] = (value & 0x000000FF00000000) >> 8;
	bytes[4] = (value & 0x00000000FF000000) << 8;
	bytes[5] = (value & 0x0000000000FF0000) << 24;
	bytes[6] = (value & 0x000000000000FF00) << 40;
	bytes[7] = (value & 0x00000000000000FF) << 56;
	unsigned long long returnValue = 0;
	for (int i = 0; i < 8; i++)
	{
		returnValue = returnValue | bytes[i];
	}
	return returnValue;
}
LPVOID SearchBuffer(char buffer[], unsigned long long bufferID, SIZE_T bufferSize,LPVOID baseAddress)
{
	LPVOID address = 0;
	unsigned long long iD =0;

	for (SIZE_T i = 0; i <bufferSize -7; i++)//prevent overflow
	{
		memcpy(&iD, &buffer[i], sizeof(iD));//copy bufferpart in iD

		if (iD == bufferID)//If iD matches bufferID
		{
			address = (LPVOID)((SIZE_T)baseAddress + i);
			return address;
		}
	}
	return address;
}