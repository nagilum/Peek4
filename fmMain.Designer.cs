using System.Security.AccessControl;

namespace Peek {
	partial class fmMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmMain));
			this.pbPreview = new System.Windows.Forms.PictureBox();
			this.cmOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miWindowBackgroundColor = new System.Windows.Forms.ToolStripMenuItem();
			this.miWBCStandard = new System.Windows.Forms.ToolStripMenuItem();
			this.miWBCBlack = new System.Windows.Forms.ToolStripMenuItem();
			this.miWBCWhite = new System.Windows.Forms.ToolStripMenuItem();
			this.pnImages = new System.Windows.Forms.Panel();
			this.ttMain = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
			this.cmOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// pbPreview
			// 
			this.pbPreview.ContextMenuStrip = this.cmOptions;
			this.pbPreview.Location = new System.Drawing.Point(649, 341);
			this.pbPreview.Name = "pbPreview";
			this.pbPreview.Size = new System.Drawing.Size(112, 47);
			this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbPreview.TabIndex = 0;
			this.pbPreview.TabStop = false;
			// 
			// cmOptions
			// 
			this.cmOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miWindowBackgroundColor});
			this.cmOptions.Name = "cmOptions";
			this.cmOptions.Size = new System.Drawing.Size(218, 26);
			// 
			// miWindowBackgroundColor
			// 
			this.miWindowBackgroundColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miWBCStandard,
            this.miWBCBlack,
            this.miWBCWhite});
			this.miWindowBackgroundColor.Name = "miWindowBackgroundColor";
			this.miWindowBackgroundColor.Size = new System.Drawing.Size(217, 22);
			this.miWindowBackgroundColor.Text = "Window Background Color";
			// 
			// miWBCStandard
			// 
			this.miWBCStandard.Name = "miWBCStandard";
			this.miWBCStandard.Size = new System.Drawing.Size(121, 22);
			this.miWBCStandard.Text = "Standard";
			this.miWBCStandard.Click += new System.EventHandler(this.miWBC_Click);
			// 
			// miWBCBlack
			// 
			this.miWBCBlack.Name = "miWBCBlack";
			this.miWBCBlack.Size = new System.Drawing.Size(121, 22);
			this.miWBCBlack.Text = "Black";
			this.miWBCBlack.Click += new System.EventHandler(this.miWBC_Click);
			// 
			// miWBCWhite
			// 
			this.miWBCWhite.Name = "miWBCWhite";
			this.miWBCWhite.Size = new System.Drawing.Size(121, 22);
			this.miWBCWhite.Text = "White";
			this.miWBCWhite.Click += new System.EventHandler(this.miWBC_Click);
			// 
			// pnImages
			// 
			this.pnImages.AutoScroll = true;
			this.pnImages.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnImages.Location = new System.Drawing.Point(0, 0);
			this.pnImages.Name = "pnImages";
			this.pnImages.Size = new System.Drawing.Size(147, 785);
			this.pnImages.TabIndex = 1;
			this.pnImages.Visible = false;
			// 
			// fmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1418, 785);
			this.ContextMenuStrip = this.cmOptions;
			this.Controls.Add(this.pnImages);
			this.Controls.Add(this.pbPreview);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "fmMain";
			this.Text = "Peek";
			this.Load += new System.EventHandler(this.fmMain_Load);
			this.Shown += new System.EventHandler(this.fmMain_Shown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fmMain_MouseMove);
			this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.fmMain_MouseWheel);
			this.Resize += new System.EventHandler(this.fmMain_Resize);
			((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
			this.cmOptions.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pbPreview;
		private System.Windows.Forms.ContextMenuStrip cmOptions;
		private System.Windows.Forms.ToolStripMenuItem miWindowBackgroundColor;
		private System.Windows.Forms.ToolStripMenuItem miWBCStandard;
		private System.Windows.Forms.ToolStripMenuItem miWBCBlack;
		private System.Windows.Forms.ToolStripMenuItem miWBCWhite;
		private System.Windows.Forms.Panel pnImages;
		private System.Windows.Forms.ToolTip ttMain;
	}
}