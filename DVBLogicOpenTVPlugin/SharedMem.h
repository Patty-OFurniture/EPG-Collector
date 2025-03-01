////////////////////////////////////////////////////////////////////////////////// 
//                                                                              //
//      Copyright (C) 2005-2016 nzsjb                                           //
//                                                                              //
//  This Program is free software; you can redistribute it and/or modify        //
//  it under the terms of the GNU General Public License as published by        //
//  the Free Software Foundation; either version 2, or (at your option)         //
//  any later version.                                                          //
//                                                                              //
//  This Program is distributed in the hope that it will be useful,             //
//  but WITHOUT ANY WARRANTY; without even the implied warranty of              //
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                //
//  GNU General Public License for more details.                                //
//                                                                              //
//  You should have received a copy of the GNU General Public License           //
//  along with GNU Make; see the file COPYING.  If not, write to                //
//  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.       //
//  http://www.gnu.org/copyleft/gpl.html                                        //
//                                                                              //  
//////////////////////////////////////////////////////////////////////////////////

#pragma once

#include <windows.h>

typedef struct sharedDataMap
{
	int currentPointer;
	int clearCount;
	int pids[32];
	BYTE data[1];
} SHARED_DATA;

int createSharedMemory(int processID, int identity, HANDLE logHandle);
void closeSharedMemory(HANDLE logHandle);
SHARED_DATA* getSharedMemory(HANDLE logHandle);
BOOL isOpen(HANDLE logHandle);
void logString(HANDLE logHandle, char *sz);
int sharedDataSize(HANDLE logHandle);
int reservedDataSize(HANDLE logHandle);
