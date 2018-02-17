using ReadWriteCsv;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class ContactsForm : DockContent, IDisp, ISingleRow
	{
		private const string SZ_HEADER_TEXT_NAME = "HeaderText";
		//private IContainer components;
		private Panel pnlContact;
		private DataGridView dgvContacts;
		private Button btnClear;
		private Button btnDelete;
		private Button btnAdd;
		private Class4 cmbAddType;
		private SGTextBox txtCallId;
		private SGTextBox txtName;
		private ComboBox cmbCallRxTone;
		private ComboBox cmbRingStyle;
		private ComboBox cmbType;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private Button btnDeleteSelect;
		private Button btnImport;
		private Button btnExport;
		private static readonly string[] SZ_HEADER_TEXT;

		public TreeNode Node
		{
			get;
			set;
		}

		protected override void Dispose(bool disposing)
		{
            /*
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}*/
			base.Dispose(disposing);
		}

		private void method_0()
		{
			this.pnlContact = new Panel();
			this.btnImport = new Button();
			this.btnExport = new Button();
			this.btnDeleteSelect = new Button();
			this.cmbCallRxTone = new ComboBox();
			this.cmbRingStyle = new ComboBox();
			this.cmbType = new ComboBox();
			this.btnClear = new Button();
			this.btnDelete = new Button();
			this.btnAdd = new Button();
			this.dgvContacts = new DataGridView();
			this.txtCallId = new SGTextBox();
			this.txtName = new SGTextBox();
			this.cmbAddType = new Class4();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			this.pnlContact.SuspendLayout();
			((ISupportInitialize)this.dgvContacts).BeginInit();
			base.SuspendLayout();
			this.pnlContact.AutoScroll = true;
			this.pnlContact.AutoSize = true;
			this.pnlContact.Controls.Add(this.btnImport);
			this.pnlContact.Controls.Add(this.btnExport);
			this.pnlContact.Controls.Add(this.btnDeleteSelect);
			this.pnlContact.Controls.Add(this.txtCallId);
			this.pnlContact.Controls.Add(this.txtName);
			this.pnlContact.Controls.Add(this.cmbCallRxTone);
			this.pnlContact.Controls.Add(this.cmbRingStyle);
			this.pnlContact.Controls.Add(this.cmbType);
			this.pnlContact.Controls.Add(this.cmbAddType);
			this.pnlContact.Controls.Add(this.btnClear);
			this.pnlContact.Controls.Add(this.btnDelete);
			this.pnlContact.Controls.Add(this.btnAdd);
			this.pnlContact.Controls.Add(this.dgvContacts);
			this.pnlContact.Dock = DockStyle.Fill;
			this.pnlContact.Location = new Point(0, 0);
			this.pnlContact.Name = "pnlContact";
			this.pnlContact.Size = new Size(729, 381);
			this.pnlContact.TabIndex = 0;
			this.btnImport.Location = new Point(606, 30);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new Size(75, 23);
			this.btnImport.TabIndex = 11;
			this.btnImport.Text = "Import";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += this.btnImport_Click;
			this.btnExport.Location = new Point(521, 30);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new Size(75, 23);
			this.btnExport.TabIndex = 11;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += this.btnExport_Click;
			this.btnDeleteSelect.Location = new Point(404, 30);
			this.btnDeleteSelect.Name = "btnDeleteSelect";
			this.btnDeleteSelect.Size = new Size(107, 23);
			this.btnDeleteSelect.TabIndex = 10;
			this.btnDeleteSelect.Text = "Delete Select";
			this.btnDeleteSelect.UseVisualStyleBackColor = true;
			this.btnDeleteSelect.Visible = false;
			this.btnDeleteSelect.Click += this.btnDeleteSelect_Click;
			this.cmbCallRxTone.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbCallRxTone.FlatStyle = FlatStyle.Flat;
			this.cmbCallRxTone.FormattingEnabled = true;
			this.cmbCallRxTone.Location = new Point(409, 2);
			this.cmbCallRxTone.Name = "cmbCallRxTone";
			this.cmbCallRxTone.Size = new Size(61, 20);
			this.cmbCallRxTone.TabIndex = 8;
			this.cmbCallRxTone.Visible = false;
			this.cmbCallRxTone.Leave += this.cmbCallRxTone_Leave;
			this.cmbRingStyle.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbRingStyle.FlatStyle = FlatStyle.Flat;
			this.cmbRingStyle.FormattingEnabled = true;
			this.cmbRingStyle.Location = new Point(327, 2);
			this.cmbRingStyle.Name = "cmbRingStyle";
			this.cmbRingStyle.Size = new Size(61, 20);
			this.cmbRingStyle.TabIndex = 7;
			this.cmbRingStyle.Visible = false;
			this.cmbRingStyle.Leave += this.cmbRingStyle_Leave;
			this.cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbType.FlatStyle = FlatStyle.Flat;
			this.cmbType.FormattingEnabled = true;
			this.cmbType.Location = new Point(81, 3);
			this.cmbType.Name = "cmbType";
			this.cmbType.Size = new Size(61, 20);
			this.cmbType.TabIndex = 4;
			this.cmbType.Visible = false;
			this.cmbType.Leave += this.cmbType_Leave;
			this.btnClear.Location = new Point(319, 30);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new Size(75, 23);
			this.btnClear.TabIndex = 3;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += this.btnClear_Click;
			this.btnDelete.Location = new Point(234, 30);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new Size(75, 23);
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += this.btnDelete_Click;
			this.btnAdd.Location = new Point(149, 30);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new Size(75, 23);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += this.btnAdd_Click;
			this.dgvContacts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvContacts.Location = new Point(22, 62);
			this.dgvContacts.Name = "dgvContacts";
			this.dgvContacts.ReadOnly = true;
			this.dgvContacts.RowHeadersWidth = 50;
			this.dgvContacts.RowTemplate.Height = 23;
            this.dgvContacts.Size = new Size(750, 303); //new Size(657, 303);
			this.dgvContacts.TabIndex = 9;
			this.dgvContacts.RowPostPaint += this.dgvContacts_RowPostPaint;
			this.dgvContacts.RowHeaderMouseDoubleClick += this.dgvContacts_RowHeaderMouseDoubleClick;
			this.dgvContacts.SelectionChanged += this.dgvContacts_SelectionChanged;

			this.txtCallId.InputString = null;
			this.txtCallId.Location = new Point(240, 3);
			this.txtCallId.MaxByteLength = 0;
			this.txtCallId.Name = "txtCallId";
			this.txtCallId.Size = new Size(61, 21);
			this.txtCallId.TabIndex = 6;
			this.txtCallId.Visible = false;
			this.txtCallId.Leave += this.txtCallId_Leave;
			this.txtName.InputString = null;
			this.txtName.Location = new Point(163, 2);
			this.txtName.MaxByteLength = 0;
			this.txtName.Name = "txtName";
			this.txtName.Size = new Size(61, 21);
			this.txtName.TabIndex = 5;
			this.txtName.Visible = false;
			this.txtName.Leave += this.txtName_Leave;
			this.cmbAddType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbAddType.FormattingEnabled = true;
			this.cmbAddType.Location = new Point(22, 31);
			this.cmbAddType.Name = "cmbAddType";
			this.cmbAddType.Size = new Size(112, 20);
			this.cmbAddType.TabIndex = 0;
			this.dataGridViewTextBoxColumn1.HeaderText = "Column1";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.HeaderText = "Column2";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.HeaderText = "Column3";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.HeaderText = "Column4";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.HeaderText = "Column5";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(729, 381);
			base.Controls.Add(this.pnlContact);
            this.Font = new Font("Arial", 9f, FontStyle.Regular);//Roger Clark , GraphicsUnit.Point, 134);
			base.Name = "ContactsForm";
			this.Text = "Contacts";
			base.Load += this.ContactsForm_Load;
			this.pnlContact.ResumeLayout(false);
			this.pnlContact.PerformLayout();
			((ISupportInitialize)this.dgvContacts).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public void SaveData()
		{
			this.dgvContacts.Focus();
		}

		public void DispData()
		{
			try
			{
				this.dgvContacts.Rows.Clear();
				for (int i = 0; i < ContactForm.data.Count; i++)
				{
					if (ContactForm.data.DataIsValid(i))
					{
						string callTypeS = ContactForm.data[i].CallTypeS;
						string callId = ContactForm.data[i].CallId;
						string name = ContactForm.data[i].Name;
						string callRxToneS = ContactForm.data[i].CallRxToneS;
						string ringStyleS = ContactForm.data[i].RingStyleS;
						int index = this.dgvContacts.Rows.Add((i + 1).ToString(), name, callId, callTypeS, ringStyleS, callRxToneS);
						this.dgvContacts.Rows[index].Tag = i;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void RefreshName()
		{
		}

		public void RefreshSingleRow(int index)
		{
			ContactForm.ContactOne contactOne = ContactForm.data[index];
			int index2 = 0;
			foreach (DataGridViewRow item in (IEnumerable)this.dgvContacts.Rows)
			{
				if (Convert.ToInt32(item.Tag) != index)
				{
					continue;
				}
				index2 = item.Index;
				break;
			}
			this.dgvContacts.Rows[index2].Cells[1].Value = contactOne.Name;
			this.dgvContacts.Rows[index2].Cells[2].Value = contactOne.CallId;
			this.dgvContacts.Rows[index2].Cells[3].Value = contactOne.CallTypeS;
			this.dgvContacts.Rows[index2].Cells[4].Value = contactOne.RingStyleS;
			this.dgvContacts.Rows[index2].Cells[5].Value = contactOne.CallRxToneS;
		}

		public ContactsForm()
		{
			//Class21.mKf3Qywz2M1Yy();
			//base._002Ector();
			this.method_0();
			base.Scale(Class15.smethod_6());
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(ContactsForm).Name;
			Class15.smethod_78("HeaderText", ContactsForm.SZ_HEADER_TEXT, name);
		}

		private void ContactsForm_Load(object sender, EventArgs e)
		{
			Class15.smethod_68(this);
			this.method_2();
			this.DispData();
			this.cmbAddType.SelectedIndex = 0;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			int selectedIndex = this.cmbAddType.SelectedIndex;
			int minIndex = ContactForm.data.GetMinIndex();
			MainForm mainForm = base.MdiParent as MainForm;
			string minCallID = ContactForm.data.GetMinCallID(selectedIndex, minIndex);
			string minName = ContactForm.data.GetMinName(this.Node);
			string callRxToneS = ContactForm.DefaultContact.CallRxToneS;
			string ringStyleS = ContactForm.DefaultContact.RingStyleS;
			string text = this.cmbAddType.Text;
			ContactForm.data.SetIndex(minIndex, 1);
			ContactForm.ContactOne value = new ContactForm.ContactOne(minIndex);
			value.Name = minName;
			value.CallId = minCallID;
			value.CallTypeS = text;
			value.RingStyleS = ringStyleS;
			value.CallRxToneS = callRxToneS;
			ContactForm.data[minIndex] = value;
			this.dgvContacts.Rows.Insert(minIndex, (minIndex + 1).ToString(), minName, minCallID, text, ringStyleS, callRxToneS);
			this.dgvContacts.Rows[minIndex].Tag = minIndex;
			this.method_1();
			int[] array = new int[3]
			{
				8,
				10,
				7
			};
			mainForm.InsertTreeViewNode(this.Node, minIndex, typeof(ContactForm), array[selectedIndex], ContactForm.data);
			mainForm.RefreshRelatedForm(base.GetType());
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (this.dgvContacts.CurrentRow != null && this.dgvContacts.CurrentRow.Tag != null)
			{
				int index = this.dgvContacts.CurrentRow.Index;
				int index2 = (int)this.dgvContacts.CurrentRow.Tag;
				if (index == 0)
				{
					MessageBox.Show(Class15.dicCommon["FirstNotDelete"]);
				}
				else
				{
					this.dgvContacts.Rows.Remove(this.dgvContacts.CurrentRow);
					ContactForm.data.ClearIndex(index2);
					this.method_1();
					MainForm mainForm = base.MdiParent as MainForm;
					mainForm.DeleteTreeViewNode(this.Node, index);
					mainForm.RefreshRelatedForm(base.GetType());
				}
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			int num = 0;
			MainForm mainForm = base.MdiParent as MainForm;
			while (this.dgvContacts.RowCount > 1)
			{
				num = (int)this.dgvContacts.Rows[1].Tag;
				this.dgvContacts.Rows.RemoveAt(1);
				this.Node.Nodes.RemoveAt(1);
				ContactForm.data.ClearIndex(num);
			}
			this.method_1();
			mainForm.RefreshRelatedForm(base.GetType());
		}

		private void btnDeleteSelect_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int count = this.dgvContacts.SelectedRows.Count;
			MainForm mainForm = base.MdiParent as MainForm;
			while (this.dgvContacts.SelectedRows.Count > 0)
			{
				num = this.dgvContacts.SelectedRows[0].Index;
				num2 = (int)this.dgvContacts.SelectedRows[0].Tag;
				if (num != 0)
				{
					this.dgvContacts.Rows.Remove(this.dgvContacts.SelectedRows[0]);
					ContactForm.data.ClearIndex(num2);
					mainForm.DeleteTreeViewNode(this.Node, num);
					num3++;
					if (num3 == count)
					{
						break;
					}
					continue;
				}
				MessageBox.Show(Class15.dicCommon["FirstNotDelete"]);
				break;
			}
			this.method_1();
			mainForm.RefreshRelatedForm(base.GetType());
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			int i = 0;
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.DefaultExt = "*.csv";
			saveFileDialog.AddExtension = true;
			saveFileDialog.Filter = "csv files|*.csv";
			saveFileDialog.OverwritePrompt = true;
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.FileName = "Contact_" + DateTime.Now.ToString("MMdd_HHmmss");
			if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != null)
			{
				using (CsvFileWriter csvFileWriter = new CsvFileWriter(new FileStream(saveFileDialog.FileName, FileMode.Create), Encoding.Default))
				{
					CsvRow csvRow = new CsvRow();
					csvRow.AddRange(ContactsForm.SZ_HEADER_TEXT);
					csvFileWriter.WriteRow(csvRow);
					for (i = 0; i < ContactForm.data.Count; i++)
					{
						if (ContactForm.data.DataIsValid(i))
						{
							csvRow.RemoveAll(ContactsForm.smethod_0);
							csvRow.Add(i.ToString());
							csvRow.Add(ContactForm.data[i].Name);
							csvRow.Add(ContactForm.data[i].CallId);
							csvRow.Add(ContactForm.data[i].CallTypeS);
							csvRow.Add(ContactForm.data[i].CallRxToneS);
							csvRow.Add(ContactForm.data[i].RingStyleS);
							csvFileWriter.WriteRow(csvRow);
						}
					}
				}
			}
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "csv files|*.csv";
			if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != null)
			{
				using (CsvFileReader csvFileReader = new CsvFileReader(openFileDialog.FileName, Encoding.Default))
				{
					CsvRow csvRow = new CsvRow();
					csvFileReader.ReadRow(csvRow);
					if (csvRow.Count == 6 && csvRow.SequenceEqual(ContactsForm.SZ_HEADER_TEXT))
					{
						for (num = 0; num < ContactForm.data.Count; num++)
						{
							ContactForm.data.SetName(num, "");
						}
						while (csvFileReader.ReadRow(csvRow))
						{
							if (csvRow.Count >= 6)
							{
								num = 0;
								CsvRow csvRow2 = csvRow;
								num = 1;
								num2 = Convert.ToInt32(((List<string>)csvRow2)[0]);
								if (num2 < ContactForm.data.Count)
								{
									ContactForm.data.SetName(num2, ((List<string>)csvRow)[num++]);
									ContactForm.data.SetCallID(num2, ((List<string>)csvRow)[num++]);
									ContactForm.data.SetCallType(num2, ((List<string>)csvRow)[num++]);
									ContactForm.data.SetCallRxTone(num2, ((List<string>)csvRow)[num++]);
									ContactForm.data.SetRingStyle(num2, ((List<string>)csvRow)[num++]);
								}
							}
						}
						this.DispData();
						MainForm mainForm = base.MdiParent as MainForm;
						mainForm.InitDigitContacts(this.Node);
						mainForm.VerifyRelatedForm(base.GetType());
						mainForm.RefreshRelatedForm(base.GetType());
					}
					else
					{
						MessageBox.Show(Class15.SZ_DATA_FORMAT_ERROR);
					}
				}
			}
		}

		private void method_1()
		{
			this.btnDelete.Enabled = !this.dgvContacts.SelectedRows.Contains(this.dgvContacts.Rows[0]);
			this.btnAdd.Enabled = (this.dgvContacts.RowCount < ContactForm.data.Count);
		}

		private void method_2()
		{
			int num = 0;
			int[] array = new int[6]
			{
				80,
				200,
				100,
				100,
				100,
				100
			};
			this.dgvContacts.ReadOnly = true;
			this.dgvContacts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			this.dgvContacts.AllowUserToAddRows = false;
			this.dgvContacts.AllowUserToDeleteRows = false;
			this.dgvContacts.AllowUserToResizeRows = false;
			this.dgvContacts.AllowUserToOrderColumns = false;
			this.dgvContacts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn = null;
			string[] sZ_HEADER_TEXT = ContactsForm.SZ_HEADER_TEXT;
			foreach (string headerText in sZ_HEADER_TEXT)
			{
				dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
				dataGridViewTextBoxColumn.Width = array[num++];
				dataGridViewTextBoxColumn.HeaderText = headerText;
				dataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				dataGridViewTextBoxColumn.ReadOnly = true;
				dataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvContacts.Columns.Add(dataGridViewTextBoxColumn);
			}
			Class15.smethod_40(this.cmbAddType, ContactForm.SZ_CALL_TYPE, new int[2]
			{
				0,
				1
			});
			Class15.smethod_37(this.cmbType, ContactForm.SZ_CALL_TYPE);
			Class15.smethod_37(this.cmbCallRxTone, ContactForm.SZ_CALL_RX_TONE);
			Class15.smethod_43(this.cmbRingStyle, 0, 10, 0, Class15.SZ_NONE);
			this.txtName.MaxLength = 16;
			this.txtCallId.MaxLength = 8;
		}

		private void method_3(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex != 5)
			{
				Control[] array = new Control[5]
				{
					this.txtName,
					this.txtCallId,
					this.cmbType,
					this.cmbRingStyle,
					this.cmbCallRxTone
				};
				Control control = array[e.ColumnIndex];
				if (this.dgvContacts.CurrentRow.Tag != null)
				{
					int index = (int)this.dgvContacts.CurrentRow.Tag;
					if (ContactForm.data.IsAllCall(index) && e.ColumnIndex == 2)
					{
						return;
					}
					if (e.RowIndex == 0 && e.ColumnIndex == 3)
					{
						return;
					}
					if ((e.ColumnIndex == 4 || e.ColumnIndex == 5) && Class15.smethod_4() != Class15.UserMode.Expert)
					{
						return;
					}
					Rectangle cellDisplayRectangle = this.dgvContacts.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
					Point location = cellDisplayRectangle.Location;
					location.Offset(this.dgvContacts.Location);
					location.Offset(this.pnlContact.Location);
					control.Location = location;
					control.Size = cellDisplayRectangle.Size;
					control.Text = ((DataGridView)sender).CurrentCell.Value.ToString();
					control.Visible = true;
					control.Focus();
					control.BringToFront();
				}
			}
		}

		private void dgvContacts_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			try
			{
				DataGridView dataGridView = sender as DataGridView;
				if (e.RowIndex >= dataGridView.FirstDisplayedScrollingRowIndex)
				{
					using (SolidBrush brush = new SolidBrush(dataGridView.RowHeadersDefaultCellStyle.ForeColor))
					{
						string s = (e.RowIndex + 1).ToString();
						e.Graphics.DrawString(s, e.InheritedRowStyle.Font, brush, (float)(e.RowBounds.Location.X + 15), (float)(e.RowBounds.Location.Y + 5));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void dgvContacts_SelectionChanged(object sender, EventArgs e)
		{
			this.method_1();
		}

		private void dgvContacts_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			MainForm mainForm = base.MdiParent as MainForm;
			if (mainForm != null)
			{
				DataGridView dataGridView = sender as DataGridView;
				int index = (int)dataGridView.Rows[e.RowIndex].Tag;
				mainForm.DispChildForm(typeof(ContactForm), index);
			}
		}

		private void cmbType_Leave(object sender, EventArgs e)
		{
			int index = this.dgvContacts.CurrentRow.Index;
			int index2 = (int)this.dgvContacts.CurrentRow.Tag;
			int selectedIndex = this.cmbType.SelectedIndex;
			string text = this.cmbType.Text;
			this.cmbType.Visible = false;
			if (!(this.cmbType.Text == ContactForm.data[index2].CallTypeS))
			{
				int[] array = new int[3]
				{
					8,
					10,
					7
				};
				if (text == ContactForm.SZ_CALL_TYPE[2])
				{
					if (ContactForm.data.HaveAll())
					{
						int num = ContactForm.data.AllCallIndex();
						int index3 = this.method_4(num);
						this.dgvContacts.Rows[index3].Cells[1].Value = this.dgvContacts.CurrentRow.Cells[1].Value;
						this.dgvContacts.Rows[index3].Cells[2].Value = this.dgvContacts.CurrentRow.Cells[2].Value;
						ContactForm.data.SetCallID(num, this.dgvContacts.CurrentRow.Cells[1].Value.ToString());
						ContactForm.data.SetCallType(num, this.dgvContacts.CurrentRow.Cells[2].Value.ToString());
						this.Node.Nodes[index3].ImageIndex = this.Node.Nodes[index].ImageIndex;
						this.Node.Nodes[index3].SelectedImageIndex = this.Node.Nodes[index].SelectedImageIndex;
					}
					string text2 = 16777215.ToString();
					ContactForm.data.SetCallID(index2, text2);
					this.dgvContacts.CurrentRow.Cells[1].Value = text2;
				}
				else if (text == ContactForm.SZ_CALL_TYPE[0])
				{
					string callId = ContactForm.data[index2].CallId;
					if (ContactForm.data.IsAllCall(index2) || ContactForm.data.CallIdExist(index2, 0, callId))
					{
						callId = ContactForm.data.GetMinCallID(0);
						this.dgvContacts.CurrentRow.Cells[1].Value = callId;
						ContactForm.data.SetCallID(index2, callId);
					}
				}
				else if (text == ContactForm.SZ_CALL_TYPE[1])
				{
					string callId2 = ContactForm.data[index2].CallId;
					if (ContactForm.data.IsAllCall(index2) || ContactForm.data.CallIdExist(index2, 1, callId2))
					{
						callId2 = ContactForm.data.GetMinCallID(1);
						this.dgvContacts.CurrentRow.Cells[1].Value = callId2;
						ContactForm.data.SetCallID(index2, callId2);
					}
				}
				this.dgvContacts.CurrentCell.Value = text;
				ContactForm.data.SetCallType(index2, text);
				this.Node.Nodes[index].ImageIndex = array[selectedIndex];
				this.Node.Nodes[index].SelectedImageIndex = array[selectedIndex];
				((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
			}
		}

		private int method_4(int int_0)
		{
			int num = 0;
			while (true)
			{
				if (num < this.dgvContacts.Rows.Count)
				{
					if ((int)this.dgvContacts.Rows[num].Tag != int_0)
					{
						num++;
						continue;
					}
					break;
				}
				return -1;
			}
			return num;
		}

		private void txtName_Leave(object sender, EventArgs e)
		{
			int index = (int)this.dgvContacts.CurrentRow.Tag;
			int index2 = this.dgvContacts.CurrentRow.Index;
			this.txtName.Visible = false;
			string text = this.txtName.Text;
			if (!(text == ContactForm.data[index].Name) && !ContactForm.data.NameExist(text))
			{
				this.dgvContacts.CurrentCell.Value = text;
				ContactForm.data.SetName(index, text);
				this.Node.Nodes[index2].Text = text;
				((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
			}
		}

		private void txtCallId_Leave(object sender, EventArgs e)
		{
			int index = (int)this.dgvContacts.CurrentRow.Tag;
			this.txtCallId.Visible = false;
			string text = this.txtCallId.Text.PadLeft(8, '0');
			if (!(text == ContactForm.data[index].CallId) && !ContactForm.data.CallIdExist(index, text) && ContactForm.data.CallIdValid(text))
			{
				this.dgvContacts.CurrentCell.Value = text;
				ContactForm.data.SetCallID(index, text);
				((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
			}
		}

		private void cmbRingStyle_Leave(object sender, EventArgs e)
		{
			int index = (int)this.dgvContacts.CurrentRow.Tag;
			this.cmbRingStyle.Visible = false;
			this.dgvContacts.CurrentCell.Value = this.cmbRingStyle.Text;
			ContactForm.data.SetRingStyle(index, this.cmbRingStyle.Text);
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
		}

		private void cmbCallRxTone_Leave(object sender, EventArgs e)
		{
			int index = (int)this.dgvContacts.CurrentRow.Tag;
			this.cmbCallRxTone.Visible = false;
			this.dgvContacts.CurrentCell.Value = this.cmbCallRxTone.Text;
			ContactForm.data.SetCallRxTone(index, this.cmbCallRxTone.Text);
			((MainForm)base.MdiParent).RefreshRelatedForm(base.GetType());
		}

		[CompilerGenerated]
		private static bool smethod_0(string string_0)
		{
			return true;
		}

		static ContactsForm()
		{
			Class21.mKf3Qywz2M1Yy();
			ContactsForm.SZ_HEADER_TEXT = new string[6]
			{
				"序号",
				"名称",
				"呼叫 ID",
				"类型",
				"音调",
				"提示音"
			};
		}
	}
}