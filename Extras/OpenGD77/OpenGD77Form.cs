﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace DMR
{
	public partial class OpenGD77Form : Form
	{
		private SerialPort _port = null;

		private BackgroundWorker worker;

		public enum CommsDataMode { DataModeNone = 0, DataModeReadFlash = 1, DataModeReadEEPROM = 2, DataModeWriteFlash = 3, DataModeWriteEEPROM = 4 };
		public enum CommsAction { NONE, BACKUP_EEPROM, BACKUP_FLASH,RESTORE_EEPROM, RESTORE_FLASH ,READ_CODEPLUG, WRITE_CODEPLUG }

		private SaveFileDialog _saveFileDialog = new SaveFileDialog();
		private OpenFileDialog _openFileDialog = new OpenFileDialog();


		public OpenGD77Form()
		{
			InitializeComponent();
		}

		bool flashWritePrepareSector(int address, ref byte[] sendbuffer, ref byte[] readbuffer,OpenGD77CommsTransferData dataObj)
		{
			dataObj.data_sector = address / 4096;

			sendbuffer[0] = (byte)'W';
			sendbuffer[1] = 1;
			sendbuffer[2] = (byte)((dataObj.data_sector >> 16) & 0xFF);
			sendbuffer[3] = (byte)((dataObj.data_sector >> 8) & 0xFF);
			sendbuffer[4] = (byte)((dataObj.data_sector >> 0) & 0xFF);
			_port.Write(sendbuffer, 0, 5);
			while (_port.BytesToRead == 0)
			{
				Thread.Sleep(0);
			}
			_port.Read(readbuffer, 0, 64);

			return ((readbuffer[0] == sendbuffer[0]) && (readbuffer[1] == sendbuffer[1]));
		}

		bool flashSendData(int address, int len, ref byte[] sendbuffer, ref byte[] readbuffer)
		{
			sendbuffer[0] = (byte)'W';
			sendbuffer[1] = 2;
			sendbuffer[2] = (byte)((address >> 24) & 0xFF);
			sendbuffer[3] = (byte)((address >> 16) & 0xFF);
			sendbuffer[4] = (byte)((address >> 8) & 0xFF);
			sendbuffer[5] = (byte)((address >> 0) & 0xFF);
			sendbuffer[6] = (byte)((len >> 8) & 0xFF);
			sendbuffer[7] = (byte)((len >> 0) & 0xFF);
			_port.Write(sendbuffer, 0, len + 8);
			while (_port.BytesToRead == 0)
			{
				Thread.Sleep(0);
			}
			_port.Read(readbuffer, 0, 64);

			return ((readbuffer[0] == sendbuffer[0]) && (readbuffer[1] == sendbuffer[1]));
		}

		bool flashWriteSector(ref byte[] sendbuffer, ref byte[] readbuffer,OpenGD77CommsTransferData dataObj)
		{
			dataObj.data_sector = -1;

			sendbuffer[0] = (byte)'W';
			sendbuffer[1] = 3;
			_port.Write(sendbuffer, 0, 2);
			while (_port.BytesToRead == 0)
			{
				Thread.Sleep(0);
			}
			_port.Read(readbuffer, 0, 64);

			return ((readbuffer[0] == sendbuffer[0]) && (readbuffer[1] == sendbuffer[1]));
		}

		private void close_data_mode()
		{
			//data_mode = OpenGD77CommsTransferData.CommsDataMode.DataModeNone;
		}

		private void ReadFlashOrEEPROM(OpenGD77CommsTransferData dataObj)
		{
			int old_progress = 0;
			byte[] sendbuffer = new byte[512];
			byte[] readbuffer = new byte[512];
			byte[] com_Buf = new byte[256];

			int currentDataAddressInTheRadio = dataObj.startDataAddressInTheRadio;
			int currentDataAddressInLocalBuffer = dataObj.localDataBufferStartPosition;

			int size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;

			while (size > 0)
			{
				if (size > 32)
				{
					size = 32;
				}

				sendbuffer[0] = (byte)'R';
				sendbuffer[1] = (byte)dataObj.mode;
				sendbuffer[2] = (byte)((currentDataAddressInTheRadio >> 24) & 0xFF);
				sendbuffer[3] = (byte)((currentDataAddressInTheRadio >> 16) & 0xFF);
				sendbuffer[4] = (byte)((currentDataAddressInTheRadio >> 8) & 0xFF);
				sendbuffer[5] = (byte)((currentDataAddressInTheRadio >> 0) & 0xFF);
				sendbuffer[6] = (byte)((size >> 8) & 0xFF);
				sendbuffer[7] = (byte)((size >> 0) & 0xFF);
				_port.Write(sendbuffer, 0, 8);
				while (_port.BytesToRead == 0)
				{
					Thread.Sleep(0);
				}
				_port.Read(readbuffer, 0, 64);

				if (readbuffer[0] == 'R')
				{
					int len = (readbuffer[1] << 8) + (readbuffer[2] << 0);
					for (int i = 0; i < len; i++)
					{
						dataObj.dataBuff[currentDataAddressInLocalBuffer++] = readbuffer[i + 3];
					}

					int progress = (currentDataAddressInTheRadio - dataObj.startDataAddressInTheRadio) * 100 / dataObj.transferLength;
					if (old_progress != progress)
					{
						updateProgess(progress);
						old_progress = progress;
					}

					currentDataAddressInTheRadio = currentDataAddressInTheRadio + len;
				}
				else
				{
					Console.WriteLine(String.Format("read stopped (error at {0:X8})", currentDataAddressInTheRadio));
					close_data_mode();

				}
				size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			}
			close_data_mode();
		}

		private void WriteFlash(OpenGD77CommsTransferData dataObj)
		{
			int old_progress = 0;
			byte[] sendbuffer = new byte[512];
			byte[] readbuffer = new byte[512];
			byte[] com_Buf = new byte[256];
			int currentDataAddressInTheRadio = dataObj.startDataAddressInTheRadio;

			int currentDataAddressInLocalBuffer = dataObj.localDataBufferStartPosition;
			dataObj.data_sector = -1;// Always needs to be initialised to -1 so the first flashWritePrepareSector is called

			int size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			while (size > 0)
			{
				if (size > 32)
				{
					size = 32;
				}

				if (dataObj.data_sector == -1)
				{
					if (!flashWritePrepareSector(currentDataAddressInTheRadio, ref sendbuffer, ref readbuffer,dataObj))
					{
						close_data_mode();
						break;
					};
				}

				if (dataObj.mode != 0)
				{
					int len = 0;
					for (int i = 0; i < size; i++)
					{
						sendbuffer[i + 8] = dataObj.dataBuff[currentDataAddressInLocalBuffer++];
						len++;

						if (dataObj.data_sector != ((currentDataAddressInTheRadio + len) / 4096))
						{
							break;
						}
					}
					if (flashSendData(currentDataAddressInTheRadio, len, ref sendbuffer, ref readbuffer))
					{
						int progress = (currentDataAddressInTheRadio - dataObj.startDataAddressInTheRadio) * 100 / dataObj.transferLength;
						if (old_progress != progress)
						{
							updateProgess(progress);
							old_progress = progress;
						}

						currentDataAddressInTheRadio = currentDataAddressInTheRadio + len;

						if (dataObj.data_sector != (currentDataAddressInTheRadio / 4096))
						{
							if (!flashWriteSector(ref sendbuffer, ref readbuffer,dataObj))
							{
								close_data_mode();
								break;
							};
						}
					}
					else
					{
						close_data_mode();
						break;
					}
				}
				size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			}

			if (dataObj.data_sector != -1)
			{
				if (!flashWriteSector(ref sendbuffer, ref readbuffer,dataObj))
				{
					Console.WriteLine(String.Format("Error. Write stopped (write sector error at {0:X8})", currentDataAddressInTheRadio));
				};
			}

			close_data_mode();
		}

		private void WriteEEPROM(OpenGD77CommsTransferData dataObj)
		{
			int old_progress = 0;
			byte[] sendbuffer = new byte[512];
			byte[] readbuffer = new byte[512];
			byte[] com_Buf = new byte[256];

			int currentDataAddressInTheRadio = dataObj.startDataAddressInTheRadio;
			int currentDataAddressInLocalBuffer = dataObj.localDataBufferStartPosition;

			int size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			while (size > 0)
			{
				if (size > 32)
				{
					size = 32;
				}

				if (dataObj.data_sector == -1)
				{
					dataObj.data_sector = currentDataAddressInTheRadio / 128;
				}

				int len = 0;
				for (int i = 0; i < size; i++)
				{
					sendbuffer[i + 8] = (byte)dataObj.dataBuff[currentDataAddressInLocalBuffer++];
					len++;

					if (dataObj.data_sector != ((currentDataAddressInTheRadio + len) / 128))
					{
						dataObj.data_sector = -1;
						break;
					}
				}

				sendbuffer[0] = (byte)'W';
				sendbuffer[1] = 4;
				sendbuffer[2] = (byte)((currentDataAddressInTheRadio >> 24) & 0xFF);
				sendbuffer[3] = (byte)((currentDataAddressInTheRadio >> 16) & 0xFF);
				sendbuffer[4] = (byte)((currentDataAddressInTheRadio >> 8) & 0xFF);
				sendbuffer[5] = (byte)((currentDataAddressInTheRadio >> 0) & 0xFF);
				sendbuffer[6] = (byte)((len >> 8) & 0xFF);
				sendbuffer[7] = (byte)((len >> 0) & 0xFF);
				_port.Write(sendbuffer, 0, len + 8);
				while (_port.BytesToRead == 0)
				{
					Thread.Sleep(0);
				}
				_port.Read(readbuffer, 0, 64);

				if ((readbuffer[0] == sendbuffer[0]) && (readbuffer[1] == sendbuffer[1]))
				{
					int progress = (currentDataAddressInTheRadio - dataObj.startDataAddressInTheRadio) * 100 / dataObj.transferLength;
					if (old_progress != progress)
					{
						updateProgess(progress);
						old_progress = progress;
					}

					currentDataAddressInTheRadio = currentDataAddressInTheRadio + len;
				}
				else
				{
					Console.WriteLine(String.Format("Error. Write stopped (write sector error at {0:X8})", currentDataAddressInTheRadio));
					close_data_mode();
				}
				size = (dataObj.startDataAddressInTheRadio + dataObj.transferLength) - currentDataAddressInTheRadio;
			}
			close_data_mode();
		}

		void updateProgess(int progressPercentage)
		{
			if (progressBar1.InvokeRequired)
				progressBar1.Invoke(new MethodInvoker(delegate()
				{
					progressBar1.Value = progressPercentage;
				}));
			else
			{
				progressBar1.Value = progressPercentage;
			}
		}

		void displayMessage(string message)
		{
			if (txtMessage.InvokeRequired)
				txtMessage.Invoke(new MethodInvoker(delegate()
				{
					txtMessage.Text = message;
				}));
			else
			{
				txtMessage.Text = message;
			}
		}


		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			OpenGD77CommsTransferData dataObj = e.Result as OpenGD77CommsTransferData;

			if (dataObj.action != OpenGD77CommsTransferData.CommsAction.NONE)
			{
				switch (dataObj.action)
				{
					case OpenGD77CommsTransferData.CommsAction.BACKUP_EEPROM:
						_saveFileDialog.Filter = "EEPROM files (*.bin)|*.bin";
						_saveFileDialog.FilterIndex = 1;
						if (_saveFileDialog.ShowDialog() == DialogResult.OK)
						{
							File.WriteAllBytes(_saveFileDialog.FileName, dataObj.dataBuff);
						}
						enableDisableAllButtons(true);
						dataObj.action = OpenGD77CommsTransferData.CommsAction.NONE;
						break;
					case OpenGD77CommsTransferData.CommsAction.BACKUP_FLASH:
						_saveFileDialog.Filter = "Flash files (*.bin)|*.bin";
						_saveFileDialog.FilterIndex = 1;
						if (_saveFileDialog.ShowDialog() == DialogResult.OK)
						{
							File.WriteAllBytes(_saveFileDialog.FileName, dataObj.dataBuff);
						}
						enableDisableAllButtons(true);
						dataObj.action = OpenGD77CommsTransferData.CommsAction.NONE;
						break;
					case OpenGD77CommsTransferData.CommsAction.RESTORE_EEPROM:
					case OpenGD77CommsTransferData.CommsAction.RESTORE_FLASH:
						MessageBox.Show("Restore complete");
						enableDisableAllButtons(true);
						dataObj.action = OpenGD77CommsTransferData.CommsAction.NONE;
						break;
					case OpenGD77CommsTransferData.CommsAction.READ_CODEPLUG:
						MessageBox.Show("Read Codeplug complete");
						MainForm.ByteToData(dataObj.dataBuff);
						/*
						enableDisableAllButtons(true);
						dataObj.action = OpenGD77CommsTransferData.CommsAction.NONE;

						_saveFileDialog.Filter = "Codeplug files (*.bin)|*.bin";
						_saveFileDialog.FilterIndex = 1;
						if (_saveFileDialog.ShowDialog() == DialogResult.OK)
						{
							File.WriteAllBytes(_saveFileDialog.FileName, dataObj.dataBuff);
						}
						 */
						break;
					case OpenGD77CommsTransferData.CommsAction.WRITE_CODEPLUG:
						break;					
				}
			}
			progressBar1.Value = 0;
		}

		void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			OpenGD77CommsTransferData dataObj = e.Argument as OpenGD77CommsTransferData;
			const int CODEPLUG_FLASH_PART_END	= 0x1EE60;
			const int CODEPLUG_FLASH_PART_START = 0xB000;
			try
			{
				switch (dataObj.action)
				{
					case OpenGD77CommsTransferData.CommsAction.BACKUP_FLASH:
						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadFlash;
						dataObj.dataBuff = new Byte[1024 * 1024];
						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 1024 * 1024;
						displayMessage("Reading Flash");
						ReadFlashOrEEPROM(dataObj);
						displayMessage("");
						break;
					case OpenGD77CommsTransferData.CommsAction.BACKUP_EEPROM:
						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadEEPROM;
						dataObj.dataBuff = new Byte[64 * 1024];

						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 64*1024;
						displayMessage("Reading EEPROM");
						ReadFlashOrEEPROM(dataObj);
						displayMessage("");
						break;

					case OpenGD77CommsTransferData.CommsAction.RESTORE_FLASH:
						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteFlash;
						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 1024 * 1024;
						displayMessage("Restoring Flash");
						WriteFlash(dataObj);
						displayMessage("Restore complete");
						break;
					case OpenGD77CommsTransferData.CommsAction.RESTORE_EEPROM:
						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteEEPROM;
						dataObj.localDataBufferStartPosition = 0;
						dataObj.startDataAddressInTheRadio = 0;
						dataObj.transferLength = 64 * 1024;
						displayMessage("Restoring EEPROM");
						WriteEEPROM(dataObj);
						displayMessage("Restore complete");
						break;
					case OpenGD77CommsTransferData.CommsAction.READ_CODEPLUG:
						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadEEPROM;
						dataObj.localDataBufferStartPosition = 0x00E0;
						dataObj.startDataAddressInTheRadio = dataObj.localDataBufferStartPosition;
						dataObj.transferLength =  0x6000 - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Reading EEPROM 0x{0:X6} to  0x{1:X6}", dataObj.localDataBufferStartPosition, (dataObj.localDataBufferStartPosition + dataObj.transferLength)));
						ReadFlashOrEEPROM(dataObj);

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadEEPROM;
						dataObj.localDataBufferStartPosition = 0x7500;
						dataObj.startDataAddressInTheRadio = dataObj.localDataBufferStartPosition;
						dataObj.transferLength = 0xB000 - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Reading EEPROM 0x{0:X6} to  0x{1:X6}", dataObj.localDataBufferStartPosition, (dataObj.localDataBufferStartPosition + dataObj.transferLength)));
						ReadFlashOrEEPROM(dataObj);


						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeReadFlash;
						dataObj.localDataBufferStartPosition = CODEPLUG_FLASH_PART_START;
						dataObj.startDataAddressInTheRadio = 0x7b000;
						dataObj.transferLength = CODEPLUG_FLASH_PART_END - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Reading Flash 0x{0:X6} to  0x{1:X6}", dataObj.localDataBufferStartPosition, dataObj.localDataBufferStartPosition + dataObj.transferLength));
						ReadFlashOrEEPROM(dataObj);
						displayMessage("Codeplug read complete");
						break;
					case OpenGD77CommsTransferData.CommsAction.WRITE_CODEPLUG:
						dataObj.dataBuff = MainForm.DataToByte();
						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteEEPROM;
						dataObj.localDataBufferStartPosition = 0x00E0;
						dataObj.startDataAddressInTheRadio = dataObj.localDataBufferStartPosition;
						dataObj.transferLength =  0x6000 - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Writing EEPROM 0x{0:X6} to  0x{1:X6}", dataObj.localDataBufferStartPosition, dataObj.localDataBufferStartPosition + dataObj.transferLength));
						WriteEEPROM(dataObj);

						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteEEPROM;
						dataObj.localDataBufferStartPosition = 0x7500;
						dataObj.startDataAddressInTheRadio = dataObj.localDataBufferStartPosition;
						dataObj.transferLength = 0xB000 - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Writing EEPROM 0x{0:X6} to  0x{1:X6}", dataObj.localDataBufferStartPosition, dataObj.localDataBufferStartPosition + dataObj.transferLength));
						WriteEEPROM(dataObj);


						dataObj.mode = OpenGD77CommsTransferData.CommsDataMode.DataModeWriteFlash;
						dataObj.localDataBufferStartPosition = CODEPLUG_FLASH_PART_START;
						dataObj.startDataAddressInTheRadio = 0x7b000;
						dataObj.transferLength = CODEPLUG_FLASH_PART_END - dataObj.localDataBufferStartPosition;
						displayMessage(String.Format("Writing Flash 0x{0:X6} to  0x{1:X6}", dataObj.localDataBufferStartPosition, dataObj.localDataBufferStartPosition + dataObj.transferLength));
						WriteFlash(dataObj);
						displayMessage("Codeplug write complete");
						break;

				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			e.Result = dataObj;
		}

		void perFormCommsTask(OpenGD77CommsTransferData dataObj)
		{
			try
			{
				worker = new BackgroundWorker();
				worker.DoWork += new DoWorkEventHandler(worker_DoWork);
				worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
				worker.RunWorkerAsync(dataObj);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnBackupEEPROM_Click(object sender, EventArgs e)
		{
			if (_port==null)
			{
				MessageBox.Show("Please select a comm port");
				return;
			}
			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.BACKUP_EEPROM);
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);
		}

		private void btnBackupFlash_Click(object sender, EventArgs e)
		{
			if (_port==null)
			{
				MessageBox.Show("Please select a comm port");
				return;
			}

			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.BACKUP_FLASH);
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);
		}

		bool arrayCompare(byte[] buf1, byte[] buf2)
		{
			int len = Math.Min(buf1.Length, buf2.Length);

			for (int i=0; i<len; i++)
			{
				if (buf1[i]!=buf2[i])
				{
					return false;
				}
			}
			return true;
		}

		private void btnRestoreEEPROM_Click(object sender, EventArgs e)
		{
			if (_port == null)
			{
				MessageBox.Show("Please select a comm port");
				return;
			}
			if (DialogResult.Yes == MessageBox.Show("Are you sure you want to restore the EEPROM from a previously saved file?", "Warning", MessageBoxButtons.YesNo))
			{
				if (DialogResult.OK == _openFileDialog.ShowDialog())
				{
					OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.RESTORE_EEPROM);
					dataObj.dataBuff = File.ReadAllBytes(_openFileDialog.FileName);
					if (dataObj.dataBuff.Length == (64 * 1024))
					{
						byte []signature = {0x00 ,0x00 ,0x00 ,0x01 ,0x56 ,0x33 ,0x2E ,0x30 ,0x31};
						if (arrayCompare(dataObj.dataBuff, signature))
						{
							MessageBox.Show("Please set your radio into FM mode\nDo not press any buttons on the radio while the EEPROM is being restored");
							enableDisableAllButtons(false);
							perFormCommsTask(dataObj);
						}
						else
						{
							MessageBox.Show("The file does not start with the correct signature bytes", "Error");
						}
					}
					else
					{
						MessageBox.Show("The file is not the correct size.", "Error");
					}
				}
			}
		}

		private void btnRestoreFlash_Click(object sender, EventArgs e)
		{
			if (_port == null)
			{
				MessageBox.Show("Please select a comm port");
				return;
			}
			if (DialogResult.Yes == MessageBox.Show("Are you sure you want to restore the Flash memory from a previously saved file?", "Warning", MessageBoxButtons.YesNo))
			{
				if (DialogResult.OK == _openFileDialog.ShowDialog())
				{
					OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.RESTORE_FLASH);
					dataObj.dataBuff = File.ReadAllBytes(_openFileDialog.FileName);
					
					if (dataObj.dataBuff.Length == (1024 * 1024))
					{
						byte[] signature = { 0x54, 0x59, 0x54, 0x3A, 0x4D, 0x44, 0x2D, 0x37, 0x36, 0x30 };
						if (arrayCompare(dataObj.dataBuff, signature))
						{
							MessageBox.Show("Please set your radio into FM mode\nDo not press any buttons on the radio while the Flash memory is being restored");
							enableDisableAllButtons(false);
							perFormCommsTask(dataObj);
						}
						else
						{
							MessageBox.Show("The file does not start with the correct signature bytes", "Error");
						}
					}
					else
					{
						MessageBox.Show("The file is not the correct size.", "Error");
					}
				}
			}
		}

		private void enableDisableAllButtons(bool show)
		{
			btnBackupEEPROM.Enabled = show;
			btnBackupFlash.Enabled = show;
			btnRestoreEEPROM.Enabled = show;
			btnRestoreFlash.Enabled = show;
			btnReadCodeplug.Enabled = show;
			btnWriteCodeplug.Enabled = show;
		}

		private void OpenGD77Form_Load(object sender, EventArgs e)
		{
			String gd77CommPort;

			gd77CommPort = SetupDiWrap.ComPortNameFromFriendlyNamePrefix("OpenGD77");
			if (gd77CommPort == null)
			{
				MessageBox.Show("Please connect the GD-77 running OpenGD77 firmware, and try again.", "OpenGD77 radio not detected.");
				this.Close();
			}
			else
			{
				try
				{
					_port = new SerialPort(gd77CommPort, 115200, Parity.None, 8, StopBits.One);
					_port.ReadTimeout = 1000;
					_port.Open();
				}
				catch (Exception)
				{
					_port = null;
					MessageBox.Show("Failed to open comm port", "Error");
				}
			}
		}


		private void OpenGD77Form_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (_port != null)
			{
				try
				{
					_port.Close();
					_port = null;
				}
				catch (Exception)
				{
					// don't care if we get an error while closing the port, we can handle the error if they can't open it the next time they want to upload or download
				}
			}
		}

		private void btnReadCodeplug_Click(object sender, EventArgs e)
		{
			if (_port == null)
			{
				MessageBox.Show("Please select a comm port");
				return;
			}

			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.READ_CODEPLUG);
			dataObj.dataBuff = MainForm.DataToByte();// overwrite the existing data, so that we can use the header etc, which the CPS checks for when we later call Byte2Data
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);
		}

		private void btnWriteCodeplug_Click(object sender, EventArgs e)
		{
			if (_port == null)
			{
				MessageBox.Show("Please select a comm port");
				return;
			}

			OpenGD77CommsTransferData dataObj = new OpenGD77CommsTransferData(OpenGD77CommsTransferData.CommsAction.WRITE_CODEPLUG);
			enableDisableAllButtons(false);
			perFormCommsTask(dataObj);

		}


	}


}