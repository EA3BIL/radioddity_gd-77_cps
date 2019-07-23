﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMR
{
	public class OpenGD77CommsTransferData
	{
			public enum CommsDataMode { DataModeNone = 0, DataModeReadFlash = 1, DataModeReadEEPROM = 2, DataModeWriteFlash = 3, DataModeWriteEEPROM = 4 };
			public enum CommsAction { NONE, BACKUP_EEPROM, BACKUP_FLASH, RESTORE_EEPROM, RESTORE_FLASH, READ_CODEPLUG, WRITE_CODEPLUG }


			public CommsDataMode mode;
			public CommsAction action;
			public int startDataAddressInTheRadio = 0;
			public int transferLength = 0;

			public int localDataBufferStartPosition = 0;

			public int data_sector = 0;
			public byte[] dataBuff;

			public OpenGD77CommsTransferData(CommsAction theAction)
			{
				action = theAction;
			}
	}
}