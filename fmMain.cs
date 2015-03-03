using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Peek {
	public partial class fmMain : Form {
		/// <summary>
		/// Full path of current folder.
		/// </summary>
		private readonly string filePath;

		/// <summary>
		/// Active file.
		/// </summary>
		private string fileName;

		/// <summary>
		/// Height of the loaded image.
		/// </summary>
		private int imageHeight;

		/// <summary>
		/// Width of the loaded image.
		/// </summary>
		private int imageWidth;

		/// <summary>
		/// List of all files in folder.
		/// </summary>
		private List<string> files = new List<string>(); 

		/// <summary>
		/// Init this sucka!
		/// </summary>
		/// <param name="args">List of arguments trailing the executable.</param>
		public fmMain(IEnumerable<string> args) {
			InitializeComponent();

			var temp = args.Aggregate("", (current, arg) => current + (arg + " "));

			if (File.Exists(temp)) {
				this.fileName = Path.GetFileName(temp);
				this.filePath = Path.GetDirectoryName(temp);
			}
			else if (Directory.Exists(temp)) {
				this.filePath = temp;
			}
			else {
				this.filePath = Path.GetDirectoryName(Application.ExecutablePath);
			}

			this.populateListFromFolder();

			if (string.IsNullOrWhiteSpace(this.fileName) &&
			    this.files.Count > 0)
				this.fileName = Path.GetFileName(this.files.First());

			this.loadOptions();
		}

		/// <summary>
		/// Load up the app.
		/// </summary>
		private void fmMain_Load(object sender, EventArgs e) {
			if (string.IsNullOrWhiteSpace(this.fileName)) {
				MessageBox.Show(
					"Unable to load file or folder to display. Aborting!",
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);

				this.Close();
			}
		}

		/// <summary>
		/// Trigger slide-in/out of file-list.
		/// </summary>
		private void fmMain_MouseMove(object sender, MouseEventArgs e) {
			this.pnImages.Visible = (e.X < 150);
		}

		/// <summary>
		/// Trigger next/prev image.
		/// </summary>
		private void fmMain_MouseWheel(object sender, MouseEventArgs e) {
			var useNextControl = false;

			if (e.Delta > 0) {
				for (var i = this.pnImages.Controls.Count - 1; i > -1; i--) {
					var pb = this.pnImages.Controls[i] as PictureBox;

					if (pb == null)
						continue;

					var pbt = pb.Tag as PictureBoxTag;

					if (pbt == null)
						continue;

					if (useNextControl) {
						thumbnail_Click(this.pnImages.Controls[i], null);
						break;
					}

					if (pbt.IsActive)
						useNextControl = true;
				}

				if (useNextControl ||
					this.pnImages.Controls.Count <= 0)
					return;

				var tobj = this.pnImages.Controls[this.pnImages.Controls.Count - 1];
				thumbnail_Click(tobj, null);
			}
			else {
				foreach (var obj in this.pnImages.Controls) {
					var pb = obj as PictureBox;

					if (pb == null)
						continue;

					var pbt = pb.Tag as PictureBoxTag;

					if (pbt == null)
						continue;

					if (useNextControl) {
						thumbnail_Click(obj, null);
						break;
					}

					if (pbt.IsActive)
						useNextControl = true;
				}

				if (useNextControl ||
					this.pnImages.Controls.Count <= 0)
					return;

				var tobj = this.pnImages.Controls[0];
				thumbnail_Click(tobj, null);
			}
		}

		/// <summary>
		/// Reposition and size the current image.
		/// </summary>
		private void fmMain_Resize(object sender, EventArgs e) {
			// Image
			Point location;
			Size size;

			this.resizeImage(out location, out size);

			this.pbPreview.Location = location;
			this.pbPreview.Size = size;
		}

		/// <summary>
		/// Perform after the window is done loading and shown.
		/// </summary>
		private void fmMain_Shown(object sender, EventArgs e) {
			this.loadImage();
			this.updateWindowTitle();
			this.loadImages();
		}

		/// <summary>
		/// Change background color and save to settings.
		/// </summary>
		private void miWBC_Click(object sender, EventArgs e) {
			var menuItem = sender as ToolStripMenuItem;

			if (menuItem == null)
				return;

			this.miWBCStandard.Checked = (menuItem.Name == "miWBCStandard");
			this.miWBCWhite.Checked = (menuItem.Name == "miWBCWhite");
			this.miWBCBlack.Checked = (menuItem.Name == "miWBCBlack");

			var color = SystemColors.Control;

			switch (menuItem.Name) {
				case "miWBCWhite":
					color = Color.White;
					break;

				case "miWBCBlack":
					color = Color.Black;
					break;
			}

			this.BackColor = color;

			Properties.Settings.Default.WindowBackgroundColor = color.ToString();
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// Click-event for thumbnail image.
		/// </summary>
		private void thumbnail_Click(object sender, EventArgs e) {
			var thumbnail = sender as PictureBox;

			if (thumbnail == null)
				return;

			var tag = thumbnail.Tag as PictureBoxTag;

			if (tag == null)
				return;

			foreach (var pb in this.pnImages.Controls)
				(pb as PictureBox).SetInActive();

			thumbnail.SetActive();

			this.fileName = tag.FileName;
			this.loadImage();
		}

		/// <summary>
		/// Load current active filename.
		/// </summary>
		private void loadImage() {
			try {
				var image = Image.FromFile(Path.Combine(this.filePath, this.fileName));

				this.imageHeight = image.Height;
				this.imageWidth = image.Width;

				Point location;
				Size size;

				this.resizeImage(out location, out size);

				this.pbPreview.Location = location;
				this.pbPreview.Size = size;
				this.pbPreview.Image = image;
			}
			catch {}
		}

		/// <summary>
		/// Load thumbnails of all images in folder for browser.
		/// </summary>
		private void loadImages() {
			var top = 5;
			const int ch = 60;
			const int cw = 120;

			foreach (var file in this.files) {
				try {
					var image = Image.FromFile(file);
					var w = image.Width;
					var h = image.Height;

					if (h > ch ||
						w > cw) {
						var ow = (100M/w)*(w - cw);
						var oh = (100M/h)*(h - ch);
						decimal p;

						if (ow > oh) {
							p = ((100M/w)*cw)/100M;

							w = cw;
							h = (int) (h*p);
						}
						else {
							p = ((100M/h)*ch)/100M;

							h = ch;
							w = (int) (w*p);
						}
					}

					var x = (cw - w)/2;
					var y = (ch - h)/2;

					var pbt = new PictureBoxTag {
						IsActive = (Path.GetFileName(file) == this.fileName),
						FileName = Path.GetFileName(file)
					};

					var thumbnail = new PictureBox {
						Image = image.GetThumbnailImage(w, h, null, IntPtr.Zero),
						Location = new Point(x + 5, top + y),
						Size = new Size(w, h),
						SizeMode = PictureBoxSizeMode.StretchImage,
						Tag = pbt
					};

					thumbnail.Click += thumbnail_Click;

					this.ttMain.SetToolTip(thumbnail, Path.GetFileName(file) + " (" + image.Width + "x" + image.Height + ")");

					this.pnImages.Controls.Add(thumbnail);

					top += (ch + 5);
				}
				catch { }

				Application.DoEvents();
			}
		}

		/// <summary>
		/// Load options from settings-file.
		/// </summary>
		private void loadOptions() {
			switch (Properties.Settings.Default.WindowBackgroundColor) {
				case "Black":
					miWBCBlack.Checked = true;
					break;

				case "White":
					miWBCWhite.Checked = true;
					break;

				default:
					miWBCStandard.Checked = true;
					break;
			}
		}

		/// <summary>
		/// Populate the files array with all filename from the current folder.
		/// </summary>
		private void populateListFromFolder() {
			var types = new[] {
				"jpg",
				"jpeg",
				"gif",
				"png",
				"bmp",
				"tif",
				"tiff",
				"ico"
			};

			try {
				foreach (var file in Directory.GetFiles(this.filePath, "*.*", SearchOption.TopDirectoryOnly).Where(file => types.Contains(file.Substring(file.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase) + 1).ToLower())))
					this.files.Add(file);
			}
			catch { }

			this.files = this.files.OrderBy(f => f).ToList();
		}

		/// <summary>
		/// Calulate new position and size of the preview imagebox.
		/// </summary>
		private void resizeImage(out Point location, out Size size) {
			var h = this.imageHeight;
			var w = this.imageWidth;
			var ch = this.ClientSize.Height;
			var cw = this.ClientSize.Width;

			if (h > ch ||
			    w > cw) {
				var ow = (100M / w) * (w - cw);
				var oh = (100M / h) * (h - ch);
				decimal p;

				if (ow > oh) {
					p = ((100M / w) * cw) / 100M;

					w = cw;
					h = (int)(h * p);
				}
				else {
					p = ((100M / h) * ch) / 100M;

					h = ch;
					w = (int)(w * p);
				}
			}

			var x = (this.ClientSize.Width - w)/2;
			var y = (this.ClientSize.Height - h)/2;

			location = new Point(x, y);
			size = new Size(w, h);
		}

		/// <summary>
		/// Update the window title with current file and path.
		/// </summary>
		private void updateWindowTitle() {
			this.Text = "Peek - " + this.fileName.Trim() + " (" + this.imageWidth + "x" + this.imageHeight + ") [" + this.filePath + "]";
		}
	}

	/// <summary>
	/// Extender class for objects.
	/// </summary>
	public static class ObjectExtenders {
		/// <summary>
		/// Check if a picturebox if the active box.
		/// </summary>
		public static bool IsActive(this PictureBox pb) {
			var tag = pb.Tag as PictureBoxTag;
			return tag != null && tag.IsActive;
		}

		/// <summary>
		/// Set this pb as active.
		/// </summary>
		/// <param name="pb"></param>
		public static void SetActive(this PictureBox pb) {
			var tag = pb.Tag as PictureBoxTag;

			if (tag == null)
				return;

			tag.IsActive = true;
			pb.Tag = tag;
		}

		/// <summary>
		/// Set this pb as inactive.
		/// </summary>
		/// <param name="pb"></param>
		public static void SetInActive(this PictureBox pb) {
			var tag = pb.Tag as PictureBoxTag;

			if (tag == null)
				return;

			tag.IsActive = false;
			pb.Tag = tag;
		}

		/// <summary>
		/// Get the current file for this pb.
		/// </summary>
		public static string GetFileName(this PictureBox pb) {
			var tag = pb.Tag as PictureBoxTag;
			return tag == null ? null : tag.FileName;
		}

		/// <summary>
		/// Set the current file for this pb.
		/// </summary>
		public static void SetFileName(this PictureBox pb, string fileName) {
			var tag = pb.Tag as PictureBoxTag;

			if (tag == null)
				return;

			tag.FileName = fileName;
			pb.Tag = tag;
		}
	}

	/// <summary>
	/// Tag-class for pb.
	/// </summary>
	public class PictureBoxTag {
		/// <summary>
		/// File loaded.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Whether or not this is the active pb.
		/// </summary>
		public bool IsActive { get; set; }
	}
}