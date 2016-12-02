namespace Emgu_4._0
{
	partial class Calibration
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Calibration));
			Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
			Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
			Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
			Telerik.WinControls.UI.RadListDataItem radListDataItem4 = new Telerik.WinControls.UI.RadListDataItem();
			this.imageBox1 = new Emgu.CV.UI.ImageBox();
			this.upButton = new System.Windows.Forms.Button();
			this.downButton = new System.Windows.Forms.Button();
			this.rightButton = new System.Windows.Forms.Button();
			this.leftButton = new System.Windows.Forms.Button();
			this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
			this.button1 = new System.Windows.Forms.Button();
			this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
			((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
			this.SuspendLayout();
			// 
			// imageBox1
			// 
			this.imageBox1.Location = new System.Drawing.Point(115, 67);
			this.imageBox1.Name = "imageBox1";
			this.imageBox1.Size = new System.Drawing.Size(640, 480);
			this.imageBox1.TabIndex = 2;
			this.imageBox1.TabStop = false;
			// 
			// upButton
			// 
			this.upButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.upButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.upButton.Image = ((System.Drawing.Image)(resources.GetObject("upButton.Image")));
			this.upButton.Location = new System.Drawing.Point(403, 2);
			this.upButton.Name = "upButton";
			this.upButton.Size = new System.Drawing.Size(65, 59);
			this.upButton.TabIndex = 3;
			this.upButton.UseVisualStyleBackColor = true;
			this.upButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
			this.upButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);
			// 
			// downButton
			// 
			this.downButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.downButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.downButton.Image = ((System.Drawing.Image)(resources.GetObject("downButton.Image")));
			this.downButton.Location = new System.Drawing.Point(403, 553);
			this.downButton.Name = "downButton";
			this.downButton.Size = new System.Drawing.Size(65, 59);
			this.downButton.TabIndex = 4;
			this.downButton.UseVisualStyleBackColor = true;
			this.downButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
			this.downButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);
			// 
			// rightButton
			// 
			this.rightButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.rightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rightButton.Image = ((System.Drawing.Image)(resources.GetObject("rightButton.Image")));
			this.rightButton.Location = new System.Drawing.Point(761, 278);
			this.rightButton.Name = "rightButton";
			this.rightButton.Size = new System.Drawing.Size(65, 59);
			this.rightButton.TabIndex = 5;
			this.rightButton.UseVisualStyleBackColor = true;
			this.rightButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
			this.rightButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);
			// 
			// leftButton
			// 
			this.leftButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.leftButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.leftButton.Image = ((System.Drawing.Image)(resources.GetObject("leftButton.Image")));
			this.leftButton.Location = new System.Drawing.Point(44, 278);
			this.leftButton.Name = "leftButton";
			this.leftButton.Size = new System.Drawing.Size(65, 59);
			this.leftButton.TabIndex = 6;
			this.leftButton.UseVisualStyleBackColor = true;
			this.leftButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
			this.leftButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);
			// 
			// serialPort1
			// 
			this.serialPort1.PortName = "COM3";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(744, 25);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(125, 36);
			this.button1.TabIndex = 7;
			this.button1.Text = "Save Calibration";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// radDropDownList1
			// 
			radListDataItem1.Text = "Right";
			radListDataItem2.Text = "Left";
			radListDataItem3.Text = "Top";
			radListDataItem4.Text = "Bottom";
			this.radDropDownList1.Items.Add(radListDataItem1);
			this.radDropDownList1.Items.Add(radListDataItem2);
			this.radDropDownList1.Items.Add(radListDataItem3);
			this.radDropDownList1.Items.Add(radListDataItem4);
			this.radDropDownList1.Location = new System.Drawing.Point(744, 2);
			this.radDropDownList1.Name = "radDropDownList1";
			this.radDropDownList1.Size = new System.Drawing.Size(125, 20);
			this.radDropDownList1.TabIndex = 8;
			// 
			// Calibration
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(870, 615);
			this.Controls.Add(this.radDropDownList1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.leftButton);
			this.Controls.Add(this.rightButton);
			this.Controls.Add(this.downButton);
			this.Controls.Add(this.upButton);
			this.Controls.Add(this.imageBox1);
			this.Name = "Calibration";
			this.Text = "Calibration";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Calibration_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Emgu.CV.UI.ImageBox imageBox1;
		private System.Windows.Forms.Button upButton;
		private System.Windows.Forms.Button downButton;
		private System.Windows.Forms.Button rightButton;
		private System.Windows.Forms.Button leftButton;
		private System.IO.Ports.SerialPort serialPort1;
		private System.Windows.Forms.Button button1;
		private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
	}
}