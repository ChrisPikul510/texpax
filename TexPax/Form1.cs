using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Drawing.Imaging;


namespace TexPax {
	public partial class TexPax : Form {
		public enum CHAN {
			AVERAGE, RED, GREEN, BLUE, ALPHA
		}

		public enum InpMode {
			NONE, WHITE, BLACK
		}

		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();

		//Input channels
		public CHAN metIn = CHAN.AVERAGE, roufIn = CHAN.AVERAGE, specIn = CHAN.AVERAGE,
			aoIn = CHAN.AVERAGE;

		//Output channels
		public CHAN metOut = CHAN.RED, roufOut = CHAN.GREEN, specOut = CHAN.ALPHA, aoOut = CHAN.BLUE;

		//Alpha routes
		public Boolean useAlpha = true, useDisp = false;

		//Texture paths
		public string texDif, texAlpha, texMet, texRouf, texSpec, texAO, texNorm, texDisp;

		//Input Modes
		public InpMode modeDif, modeAlpha, modeMet, modeRouf, modeSpec = InpMode.WHITE, modeAO, modeNorm, modeDisp = InpMode.WHITE;

		//Inversion switches
		public Boolean invDif, invAlpha, invMet, invRouf, invSpec, invAO, invNorm, invDisp;

		//Output
		public string outputPath;

		public TexPax() {
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			this.FormBorderStyle = FormBorderStyle.None;

			this.button3_Click(null, null);
			/*
            setImages();
            setModeImages();
            */

			this.comboBox1.SelectedIndex = 0;
		}

		private Bitmap getImage(CHAN chan) {
			switch(chan) {
				case CHAN.RED:
					return global::TexPax.Properties.Resources.chan_red;
				case CHAN.GREEN:
					return global::TexPax.Properties.Resources.chan_green;
				case CHAN.BLUE:
					return global::TexPax.Properties.Resources.chan_blue;
				case CHAN.ALPHA:
					return global::TexPax.Properties.Resources.chan_alpha;
				case CHAN.AVERAGE:
				default:
					return global::TexPax.Properties.Resources.chan_average;
			}
		}

		private void setImages() {
			this.pictureBox11.BackgroundImage = getImage(metIn);
			this.pictureBox12.BackgroundImage = getImage(roufIn);
			this.pictureBox13.BackgroundImage = getImage(specIn);
			this.pictureBox14.BackgroundImage = getImage(specIn);

			this.pictureBox15.BackgroundImage = getImage(metOut);
			this.pictureBox16.BackgroundImage = getImage(roufOut);
			this.pictureBox17.BackgroundImage = getImage(specOut);
			this.pictureBox18.BackgroundImage = getImage(aoOut);
		}

		private Image getModeImage(InpMode mode) {
			if(mode == InpMode.NONE)
				return global::TexPax.Properties.Resources.ico_cross;
			else if(mode == InpMode.WHITE)
				return global::TexPax.Properties.Resources.ico_white;
			else
				return global::TexPax.Properties.Resources.ico_black;
		}

		private void setModeImages() {
			this.panel15.BackgroundImage = getModeImage(modeDif);
			this.panel19.BackgroundImage = getModeImage(modeAlpha);
			this.panel23.BackgroundImage = getModeImage(modeMet);
			this.panel27.BackgroundImage = getModeImage(modeRouf);
			this.panel31.BackgroundImage = getModeImage(modeSpec);
			this.panel35.BackgroundImage = getModeImage(modeAO);
			this.panel39.BackgroundImage = getModeImage(modeNorm);
			this.panel43.BackgroundImage = getModeImage(modeDisp);
		}

		private Image getInvImage(Boolean inv) {
			if(inv)
				return global::TexPax.Properties.Resources.ico_invert;
			return global::TexPax.Properties.Resources.ico_invert_disabled;
		}

		private void setInvImages() {
			this.panel17.BackgroundImage = getInvImage(invDif);
			this.panel21.BackgroundImage = getInvImage(invAlpha);
			this.panel25.BackgroundImage = getInvImage(invMet);
			this.panel29.BackgroundImage = getInvImage(invRouf);
			this.panel33.BackgroundImage = getInvImage(invSpec);
			this.panel37.BackgroundImage = getInvImage(invAO);
			this.panel41.BackgroundImage = getInvImage(invNorm);
			this.panel45.BackgroundImage = getInvImage(invDisp);
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			if(e.Button == MouseButtons.Left) {
				this.Capture = false;
				Message msg = Message.Create(this.Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
				this.WndProc(ref msg);
			}
		}


		protected override bool ProcessDialogKey(Keys keyData) {
			if(Form.ModifierKeys == Keys.None && keyData == Keys.Escape) {
				this.Close();
				return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		private void button1_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void panel1_MouseMove(object sender, MouseEventArgs e) {
			if(e.Button == MouseButtons.Left) {
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private void button3_Click(object sender, EventArgs e) {
			//RESET
			modeDif = InpMode.NONE;
			modeAlpha = InpMode.WHITE;
			modeMet = InpMode.BLACK;
			modeRouf = InpMode.NONE;
			modeSpec = InpMode.WHITE;
			modeAO = InpMode.WHITE;
			modeNorm = InpMode.NONE;
			modeDisp = InpMode.WHITE;

			invDif = false;
			invAlpha = false;
			invMet = false;
			invRouf = false;
			invSpec = false;
			invAO = false;
			invNorm = false;
			invDisp = false;

			metIn = CHAN.AVERAGE;
			roufIn = CHAN.AVERAGE;
			specIn = CHAN.AVERAGE;
			aoIn = CHAN.AVERAGE;

			metOut = CHAN.RED;
			roufOut = CHAN.GREEN;
			specOut = CHAN.ALPHA;
			aoOut = CHAN.BLUE;

			useAlpha = true;
			useDisp = false;

			setImages();
			setModeImages();
			setInvImages();

			if(this.useAlpha)
				this.pictureBox9.BackgroundImage = global::TexPax.Properties.Resources.route_up;
			else
				this.pictureBox9.BackgroundImage = global::TexPax.Properties.Resources.route_right;

			if(this.useDisp)
				this.pictureBox10.BackgroundImage = global::TexPax.Properties.Resources.route_up;
			else
				this.pictureBox10.BackgroundImage = global::TexPax.Properties.Resources.route_right;

			this.textBox1.Text = "_DIFFUSE";
			this.textBox2.Text = "_COMP";
			this.textBox3.Text = "_NORM";
			this.textBox4.Text = "_DISP";

			this.texDif = null;
			this.texAlpha = null;
			this.texMet = null;
			this.texRouf = null;
			this.texSpec = null;
			this.texAO = null;
			this.texNorm = null;
			this.texDisp = null;

			this.panel3.BackgroundImage = global::TexPax.Properties.Resources.img_icon;
			this.panel11.BackgroundImage = global::TexPax.Properties.Resources.img_icon;
			this.panel5.BackgroundImage = global::TexPax.Properties.Resources.img_icon;
			this.panel6.BackgroundImage = global::TexPax.Properties.Resources.img_icon;
			this.panel7.BackgroundImage = global::TexPax.Properties.Resources.img_icon;
			this.panel8.BackgroundImage = global::TexPax.Properties.Resources.img_icon;
			this.panel13.BackgroundImage = global::TexPax.Properties.Resources.img_icon;
			this.panel10.BackgroundImage = global::TexPax.Properties.Resources.img_icon;
		}

		private void panel11_DragEnter(object sender, DragEventArgs e) {
			//Drag enter Alpha
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
				this.panel11.BackColor = Color.FromArgb(255, 64, 64, 64);
			}
		}

		private void panel11_DragLeave(object sender, EventArgs e) {
			//Clear Alpha
			this.panel11.BackColor = Color.FromArgb(255, 18, 18, 18);
		}

		private void panel11_DragDrop(object sender, DragEventArgs e) {
			//Drop on Alpha
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0) {
				string file = files[0];
				this.panel11.BackgroundImage = Image.FromFile(file);
				this.texAlpha = file;
				this.panel11.BackColor = Color.FromArgb(255, 18, 18, 18);

				this.textBox6.Text = this.panel11.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel11.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel5_DragEnter(object sender, DragEventArgs e) {
			//Drag enter Metallic
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
				this.panel5.BackColor = Color.FromArgb(255, 64, 64, 64);
			}
		}

		private void panel5_DragLeave(object sender, EventArgs e) {
			//Clear Metallic
			this.panel5.BackColor = Color.FromArgb(255, 18, 18, 18);
		}

		private void panel5_DragDrop(object sender, DragEventArgs e) {
			//Drop on Metallic
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0) {
				string file = files[0];
				this.panel5.BackgroundImage = Image.FromFile(file);
				this.texMet = file;
				this.panel5.BackColor = Color.FromArgb(255, 18, 18, 18);

				this.textBox6.Text = this.panel5.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel5.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel6_DragEnter(object sender, DragEventArgs e) {
			//Drag enter Roughness
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
				this.panel6.BackColor = Color.FromArgb(255, 64, 64, 64);
			}
		}

		private void panel6_DragLeave(object sender, EventArgs e) {
			//Clear Roughness
			this.panel6.BackColor = Color.FromArgb(255, 18, 18, 18);
		}

		private void panel6_DragDrop(object sender, DragEventArgs e) {
			//Drop on Roughness
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0) {
				string file = files[0];
				this.panel6.BackgroundImage = Image.FromFile(file);
				this.texRouf = file;
				this.panel6.BackColor = Color.FromArgb(255, 18, 18, 18);

				this.textBox6.Text = this.panel6.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel6.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel7_DragEnter(object sender, DragEventArgs e) {
			//Drag enter Specular
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
				this.panel7.BackColor = Color.FromArgb(255, 64, 64, 64);
			}
		}

		private void panel7_DragLeave(object sender, EventArgs e) {
			//Clear Specular
			this.panel7.BackColor = Color.FromArgb(255, 18, 18, 18);
		}

		private void panel7_DragDrop(object sender, DragEventArgs e) {
			//Drop on Specular
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0) {
				string file = files[0];
				this.panel7.BackgroundImage = Image.FromFile(file);
				this.texSpec = file;
				this.panel7.BackColor = Color.FromArgb(255, 18, 18, 18);

				this.textBox6.Text = this.panel7.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel7.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel8_DragEnter(object sender, DragEventArgs e) {
			//Drag enter AO
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
				this.panel8.BackColor = Color.FromArgb(255, 64, 64, 64);
			}
		}

		private void panel8_DragLeave(object sender, EventArgs e) {
			//Clear AO
			this.panel8.BackColor = Color.FromArgb(255, 18, 18, 18);
		}

		private void panel8_DragDrop(object sender, DragEventArgs e) {
			//Drop on AO
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0) {
				string file = files[0];
				this.panel8.BackgroundImage = Image.FromFile(file);
				this.texAO = file;
				this.panel8.BackColor = Color.FromArgb(255, 18, 18, 18);

				this.textBox6.Text = this.panel8.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel8.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel17_Click(object sender, EventArgs e) {
			invDif = !invDif;
			setInvImages();
		}

		private void panel21_Click(object sender, EventArgs e) {
			invAlpha = !invAlpha;
			setInvImages();
		}

		private void panel25_Click(object sender, EventArgs e) {
			invMet = !invMet;
			setInvImages();
		}

		private void panel29_Click(object sender, EventArgs e) {
			invRouf = !invRouf;
			setInvImages();
		}

		private void panel33_Click(object sender, EventArgs e) {
			invSpec = !invSpec;
			setInvImages();
		}

		private void panel37_Click(object sender, EventArgs e) {
			invAO = !invAO;
			setInvImages();
		}

		private void panel41_Click(object sender, EventArgs e) {
			invNorm = !invNorm;
			setInvImages();
		}

		private void panel45_Click(object sender, EventArgs e) {
			invDisp = !invDisp;
			setInvImages();
		}

		private void panel3_Click(object sender, EventArgs e) {
			//Open diffuse
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Image Textures|*.png,*.jpg,*.bmp,*.tiff,*.jpeg";
			dlg.Title = "Select a Diffuse Texture";
			dlg.Multiselect = false;

			if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				string file = dlg.FileName;
				this.texDif = file;
				this.panel3.BackgroundImage = Image.FromFile(file);

				this.textBox6.Text = this.panel3.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel3.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel19_Click(object sender, EventArgs e) {
			//Mode Alpha
			if(modeAlpha == InpMode.NONE)
				modeAlpha = InpMode.WHITE;
			else if(modeAlpha == InpMode.WHITE)
				modeAlpha = InpMode.BLACK;
			else
				modeAlpha = InpMode.NONE;
			setModeImages();
		}

		private void panel23_Click(object sender, EventArgs e) {
			//Mode Metallic
			if(modeMet == InpMode.NONE)
				modeMet = InpMode.WHITE;
			else if(modeMet == InpMode.WHITE)
				modeMet = InpMode.BLACK;
			else
				modeMet = InpMode.NONE;
			setModeImages();
		}

		private void panel27_Click(object sender, EventArgs e) {
			//Mode Rouf
			if(modeRouf == InpMode.NONE)
				modeRouf = InpMode.WHITE;
			else if(modeRouf == InpMode.WHITE)
				modeRouf = InpMode.BLACK;
			else
				modeRouf = InpMode.NONE;
			setModeImages();
		}

		private void panel31_Click(object sender, EventArgs e) {
			//Mode Spec
			if(modeSpec == InpMode.NONE)
				modeSpec = InpMode.WHITE;
			else if(modeSpec == InpMode.WHITE)
				modeSpec = InpMode.BLACK;
			else
				modeSpec = InpMode.NONE;
			setModeImages();
		}

		private void panel35_Click(object sender, EventArgs e) {
			//Mode AO
			if(modeAO == InpMode.NONE)
				modeAO = InpMode.WHITE;
			else if(modeAO == InpMode.WHITE)
				modeAO = InpMode.BLACK;
			else
				modeAO = InpMode.NONE;
			setModeImages();
		}

		private void panel11_Click(object sender, EventArgs e) {
			//Open alpha
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Image Textures|*.png,*.jpg,*.bmp,*.tiff,*.jpeg";
			dlg.Title = "Select a Alpha Mask";
			dlg.Multiselect = false;

			if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				string file = dlg.FileName;
				this.texAlpha = file;
				this.panel11.BackgroundImage = Image.FromFile(file);

				this.textBox6.Text = this.panel11.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel11.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel5_Click(object sender, EventArgs e) {
			//Open Metallic
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Image Textures|*.png,*.jpg,*.bmp,*.tiff,*.jpeg";
			dlg.Title = "Select a Metallic Texture";
			dlg.Multiselect = false;

			if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				string file = dlg.FileName;
				this.texMet = file;
				this.panel5.BackgroundImage = Image.FromFile(file);

				this.textBox6.Text = this.panel5.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel5.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel6_Click(object sender, EventArgs e) {
			//Open rouf
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Image Textures|*.png,*.jpg,*.bmp,*.tiff,*.jpeg";
			dlg.Title = "Select a Roughness Texture";
			dlg.Multiselect = false;

			if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				string file = dlg.FileName;
				this.texRouf = file;
				this.panel6.BackgroundImage = Image.FromFile(file);

				this.textBox6.Text = this.panel6.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel6.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel7_Click(object sender, EventArgs e) {
			//Open spec
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Image Textures|*.png,*.jpg,*.bmp,*.tiff,*.jpeg";
			dlg.Title = "Select a Specular Texture";
			dlg.Multiselect = false;

			if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				string file = dlg.FileName;
				this.texSpec = file;
				this.panel7.BackgroundImage = Image.FromFile(file);

				this.textBox6.Text = this.panel7.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel7.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel8_Click(object sender, EventArgs e) {
			//Open ao
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Image Textures|*.png,*.jpg,*.bmp,*.tiff,*.jpeg";
			dlg.Title = "Select a Ambient Occlusion Texture";
			dlg.Multiselect = false;

			if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				string file = dlg.FileName;
				this.texAO = file;
				this.panel8.BackgroundImage = Image.FromFile(file);

				this.textBox6.Text = this.panel8.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel8.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel13_Click(object sender, EventArgs e) {
			//Open norm
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Image Textures|*.png,*.jpg,*.bmp,*.tiff,*.jpeg";
			dlg.Title = "Select a Normals Texture";
			dlg.Multiselect = false;

			if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				string file = dlg.FileName;
				this.texNorm = file;
				this.panel13.BackgroundImage = Image.FromFile(file);

				this.textBox6.Text = this.panel13.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel13.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel10_Click(object sender, EventArgs e) {
			//Open disp
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Image Textures|*.png,*.jpg,*.bmp,*.tiff,*.jpeg";
			dlg.Title = "Select a Displacement Texture";
			dlg.Multiselect = false;

			if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				string file = dlg.FileName;
				this.texDisp = file;
				this.panel10.BackgroundImage = Image.FromFile(file);

				this.textBox6.Text = this.panel10.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel10.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel39_Click(object sender, EventArgs e) {
			//Mode Norm
			if(modeNorm == InpMode.NONE)
				modeNorm = InpMode.WHITE;
			else if(modeNorm == InpMode.WHITE)
				modeNorm = InpMode.BLACK;
			else
				modeNorm = InpMode.NONE;
			setModeImages();
		}

		private void panel43_Click(object sender, EventArgs e) {
			//Mode Disp
			if(modeDisp == InpMode.NONE)
				modeDisp = InpMode.WHITE;
			else if(modeDisp == InpMode.WHITE)
				modeDisp = InpMode.BLACK;
			else
				modeDisp = InpMode.NONE;
			setModeImages();
		}

		private void panel13_DragEnter(object sender, DragEventArgs e) {
			//Drag enter Normals
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
				this.panel13.BackColor = Color.FromArgb(255, 64, 64, 64);
			}
		}

		private void panel13_DragLeave(object sender, EventArgs e) {
			//Clear Normals
			this.panel13.BackColor = Color.FromArgb(255, 18, 18, 18);
		}

		private void panel13_DragDrop(object sender, DragEventArgs e) {
			//Drop on Normals
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0) {
				string file = files[0];
				this.panel13.BackgroundImage = Image.FromFile(file);
				this.texNorm = file;
				this.panel13.BackColor = Color.FromArgb(255, 18, 18, 18);

				this.textBox6.Text = this.panel13.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel13.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel10_DragEnter(object sender, DragEventArgs e) {
			//Drag enter Displacement
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
				this.panel10.BackColor = Color.FromArgb(255, 64, 64, 64);
			}
		}

		private void panel10_DragLeave(object sender, EventArgs e) {
			//Clear Displacement
			this.panel10.BackColor = Color.FromArgb(255, 18, 18, 18);
		}

		private void panel10_DragDrop(object sender, DragEventArgs e) {
			//Drop on Displacement
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0) {
				string file = files[0];
				this.panel10.BackgroundImage = Image.FromFile(file);
				this.texDisp = file;
				this.panel10.BackColor = Color.FromArgb(255, 18, 18, 18);

				this.textBox6.Text = this.panel10.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel10.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel15_Click(object sender, EventArgs e) {
			//Mode Dif
			if(modeDif == InpMode.NONE)
				modeDif = InpMode.WHITE;
			else if(modeDif == InpMode.WHITE)
				modeDif = InpMode.BLACK;
			else
				modeDif = InpMode.NONE;
			setModeImages();
		}

		private void pictureBox11_Click(object sender, EventArgs e) {
			//Metallic in
			switch(this.metIn) {
				case CHAN.AVERAGE:
					metIn = CHAN.RED;
					break;
				case CHAN.RED:
					metIn = CHAN.GREEN;
					break;
				case CHAN.GREEN:
					metIn = CHAN.BLUE;
					break;
				case CHAN.BLUE:
					metIn = CHAN.ALPHA;
					break;
				case CHAN.ALPHA:
					metIn = CHAN.AVERAGE;
					break;
			}

			this.pictureBox11.BackgroundImage = getImage(metIn);
		}

		private void pictureBox9_Click(object sender, EventArgs e) {
			this.useAlpha = !this.useAlpha;
			if(this.useAlpha)
				this.pictureBox9.BackgroundImage = global::TexPax.Properties.Resources.route_up;
			else
				this.pictureBox9.BackgroundImage = global::TexPax.Properties.Resources.route_right;
		}

		private void pictureBox10_Click(object sender, EventArgs e) {
			this.useDisp = !this.useDisp;
			if(this.useDisp)
				this.pictureBox10.BackgroundImage = global::TexPax.Properties.Resources.route_up;
			else
				this.pictureBox10.BackgroundImage = global::TexPax.Properties.Resources.route_right;
		}

		private void pictureBox12_Click(object sender, EventArgs e) {
			//Roughness in
			switch(this.roufIn) {
				case CHAN.AVERAGE:
					roufIn = CHAN.RED;
					break;
				case CHAN.RED:
					roufIn = CHAN.GREEN;
					break;
				case CHAN.GREEN:
					roufIn = CHAN.BLUE;
					break;
				case CHAN.BLUE:
					roufIn = CHAN.ALPHA;
					break;
				case CHAN.ALPHA:
					roufIn = CHAN.AVERAGE;
					break;
			}

			this.pictureBox12.BackgroundImage = getImage(roufIn);
		}

		private void panel3_DragDrop(object sender, DragEventArgs e) {
			//Drop on Diffuse
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0) {
				string file = files[0];
				this.panel3.BackgroundImage = Image.FromFile(file);
				this.texDif = file;

				this.textBox6.Text = this.panel3.BackgroundImage.Width.ToString();
				this.textBox7.Text = this.panel3.BackgroundImage.Height.ToString();
				this.textBox6.ReadOnly = true;
				this.textBox7.ReadOnly = true;
			}
		}

		private void panel3_DragEnter(object sender, DragEventArgs e) {
			//Drag enter
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
				this.panel3.BackColor = Color.FromArgb(255, 64, 64, 64);
			}
		}

		private void panel3_DragLeave(object sender, EventArgs e) {
			//Drag leave
			this.panel3.BackColor = Color.FromArgb(255, 18, 18, 18);
		}

		private void pictureBox13_Click(object sender, EventArgs e) {
			//Spec in
			switch(this.specIn) {
				case CHAN.AVERAGE:
					specIn = CHAN.RED;
					break;
				case CHAN.RED:
					specIn = CHAN.GREEN;
					break;
				case CHAN.GREEN:
					specIn = CHAN.BLUE;
					break;
				case CHAN.BLUE:
					specIn = CHAN.ALPHA;
					break;
				case CHAN.ALPHA:
					specIn = CHAN.AVERAGE;
					break;
			}

			this.pictureBox13.BackgroundImage = getImage(specIn);
		}

		private void pictureBox14_Click(object sender, EventArgs e) {
			//AO in
			switch(this.aoIn) {
				case CHAN.AVERAGE:
					aoIn = CHAN.RED;
					break;
				case CHAN.RED:
					aoIn = CHAN.GREEN;
					break;
				case CHAN.GREEN:
					aoIn = CHAN.BLUE;
					break;
				case CHAN.BLUE:
					aoIn = CHAN.ALPHA;
					break;
				case CHAN.ALPHA:
					aoIn = CHAN.AVERAGE;
					break;
			}

			this.pictureBox14.BackgroundImage = getImage(aoIn);
		}

		private void pictureBox15_Click(object sender, EventArgs e) {
			//Met Out
			switch(this.metOut) {
				case CHAN.RED:
					metOut = CHAN.GREEN;
					break;
				case CHAN.GREEN:
					metOut = CHAN.BLUE;
					break;
				case CHAN.BLUE:
					metOut = CHAN.ALPHA;
					break;
				case CHAN.ALPHA:
					metOut = CHAN.RED;
					break;
			}

			this.pictureBox15.BackgroundImage = getImage(metOut);
		}


		private void pictureBox16_Click(object sender, EventArgs e) {
			//Rouf Out
			switch(this.roufOut) {
				case CHAN.RED:
					roufOut = CHAN.GREEN;
					break;
				case CHAN.GREEN:
					roufOut = CHAN.BLUE;
					break;
				case CHAN.BLUE:
					roufOut = CHAN.ALPHA;
					break;
				case CHAN.ALPHA:
					roufOut = CHAN.RED;
					break;
			}

			this.pictureBox16.BackgroundImage = getImage(roufOut);
		}

		private void pictureBox17_Click(object sender, EventArgs e) {
			//Spec Out
			switch(this.specOut) {
				case CHAN.RED:
					specOut = CHAN.GREEN;
					break;
				case CHAN.GREEN:
					specOut = CHAN.BLUE;
					break;
				case CHAN.BLUE:
					specOut = CHAN.ALPHA;
					break;
				case CHAN.ALPHA:
					specOut = CHAN.RED;
					break;
			}

			this.pictureBox17.BackgroundImage = getImage(specOut);
		}

		private void pictureBox18_Click(object sender, EventArgs e) {
			//AO Out
			switch(this.aoOut) {
				case CHAN.RED:
					aoOut = CHAN.GREEN;
					break;
				case CHAN.GREEN:
					aoOut = CHAN.BLUE;
					break;
				case CHAN.BLUE:
					aoOut = CHAN.ALPHA;
					break;
				case CHAN.ALPHA:
					aoOut = CHAN.RED;
					break;
			}

			this.pictureBox18.BackgroundImage = getImage(aoOut);
		}

		private void button2_Click(object sender, EventArgs e) {
			//PROCESS RESULTS

			//Simple validate
			Boolean outR = false, outG = false, outB = false, outA = false, failed = false;

			if(metOut == CHAN.RED)
				outR = true;
			else if(metOut == CHAN.GREEN)
				outG = true;
			else if(metOut == CHAN.BLUE)
				outB = true;
			else if(metOut == CHAN.ALPHA)
				outA = true;

			if(roufOut == CHAN.RED) {
				if(outR)
					failed = true;
				outR = true;
			} else if(roufOut == CHAN.GREEN) {
				if(outG)
					failed = true;
				outG = true;
			} else if(roufOut == CHAN.BLUE) {
				if(outB)
					failed = true;
				outB = true;
			} else if(roufOut == CHAN.ALPHA) {
				if(outA)
					failed = true;
				outA = true;
			}

			if(specOut == CHAN.RED) {
				if(outR)
					failed = true;
				outR = true;
			} else if(specOut == CHAN.GREEN) {
				if(outG)
					failed = true;
				outG = true;
			} else if(specOut == CHAN.BLUE) {
				if(outB)
					failed = true;
				outB = true;
			} else if(specOut == CHAN.ALPHA) {
				if(outA)
					failed = true;
				outA = true;
			}

			if(aoOut == CHAN.RED) {
				if(outR)
					failed = true;
				outR = true;
			} else if(aoOut == CHAN.GREEN) {
				if(outG)
					failed = true;
				outG = true;
			} else if(aoOut == CHAN.BLUE) {
				if(outB)
					failed = true;
				outB = true;
			} else if(aoOut == CHAN.ALPHA) {
				if(outA)
					failed = true;
				outA = true;
			}

			if(failed) {
				MessageBox.Show("Cannot have two textures outputting to the same channel!", "You done messed up...", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.Description = "Folder to output new textures too...";

			if(dlg.ShowDialog() == DialogResult.OK) {
				this.outputPath = dlg.SelectedPath;
				GenerateTextures();

				MessageBox.Show("Saved your new textures to:\n" + outputPath, "Done!");
			}
		}

		private void GenerateTextures() {
			string outPrefix = this.textBox5.Text;
			string outExt = ".png";
			ImageFormat outFormat = ImageFormat.Png;
			switch(this.comboBox1.SelectedText) {
				case "PNG":
					outExt = ".png";
					outFormat = ImageFormat.Png;
					break;
				case "BMP":
					outExt = ".bmp";
					outFormat = ImageFormat.Bmp;
					break;
				case "JPG":
					outExt = ".jpg";
					outFormat = ImageFormat.Jpeg;
					break;
				case "TIFF":
					outExt = ".tiff";
					outFormat = ImageFormat.Tiff;
					break;
			}

			string difSuffix = this.textBox1.Text,
				compSuffix = this.textBox2.Text,
				normSuffix = this.textBox3.Text,
				dispSuffix = this.textBox4.Text;

			string strWidth = this.textBox6.Text,
				strHeight = this.textBox7.Text;
			int width = int.Parse(strWidth),
				height = int.Parse(strHeight);

			//Diffuse first
			Boolean makeDif = false;
			if((this.texDif != null && this.texDif.Length > 0) || this.modeDif != InpMode.NONE)
				makeDif = true;
			if((this.texAlpha != null && this.texAlpha.Length > 0) || this.modeAlpha != InpMode.NONE) {
				if(this.useAlpha)
					makeDif = true;
			}

			if(makeDif) {
				string difPath = outputPath + '\\' + outPrefix + difSuffix + outExt;
				generateDiffuse(difPath, width, height, outFormat);
			}

			//Composite second
			if(((this.texMet != null && this.texMet.Length > 0) || this.modeMet != InpMode.NONE) ||
				((this.texRouf != null && this.texRouf.Length > 0) || this.modeRouf != InpMode.NONE) ||
				((this.texSpec != null && this.texSpec.Length > 0) || this.modeSpec != InpMode.NONE) ||
				((this.texAO != null && this.texAO.Length > 0) || this.modeAO != InpMode.NONE)) {
				string compPath = outputPath + '\\' + outPrefix + compSuffix + outExt;
				generateComposite(compPath, width, height, outFormat);
			}

			//Normals third
			if((this.texNorm != null && this.texNorm.Length > 0) || this.modeNorm != InpMode.NONE || 
				((this.texDisp != null && this.texDisp.Length > 0) || this.modeDisp != InpMode.NONE) && useDisp) {
				string normPath = outputPath + '\\' + outPrefix + normSuffix + outExt;
				generateNormals(normPath, width, height, outFormat);
			}

			//If needed displacement output
			if(!useDisp && ((this.texDisp != null && this.texDisp.Length > 0) || this.modeDisp != InpMode.NONE)) {
				string dispPath = outputPath + '\\' + outPrefix + dispSuffix + outExt;
				generateDisplacement(dispPath, width, height, outFormat);
			}
		}

		private void generateDiffuse(string outFile, int width, int height, ImageFormat format) {
			//Load texture
			Bitmap diffIn = null, alphaIn = null;
			if(this.texDif != null && this.texDif.Length > 0)
				diffIn = (Bitmap)Image.FromFile(texDif);
			if(this.useAlpha && this.texAlpha != null && this.texAlpha.Length > 0)
				alphaIn = (Bitmap)Image.FromFile(texAlpha);

			//Make diffuse
			Bitmap diff = new Bitmap(width, height);
			Color samp;
			byte or, og, ob, oa;
			for(int x = 0; x < diff.Width; x++) {
				for(int y = 0; y < diff.Height; y++) {
					if(diffIn != null) {
						samp = diffIn.GetPixel(x, y);
						or = samp.R;
						og = samp.G;
						ob = samp.B;
					} else if(modeDif == InpMode.WHITE) {
						or = 255;
						og = 255;
						ob = 255;
					} else {
						or = 0;
						og = 0;
						ob = 0;
					}

					if(useAlpha && alphaIn != null) {
						samp = alphaIn.GetPixel(x, y);
						oa = (byte)((samp.R + samp.G + samp.B + samp.A) / 4);
					} else if(useAlpha && modeAlpha == InpMode.BLACK)
						oa = 0;
					else
						oa = 255;

					if(invDif) {
						or = (byte)(255 - or);
						og = (byte)(255 - og);
						ob = (byte)(255 - ob);
					}

					if(useAlpha && invAlpha) {
						oa = (byte)(255 - oa);
					}

					diff.SetPixel(x, y, Color.FromArgb(oa, or, og, ob));
				}
			}

			diff.Save(outFile, format);
		}

		private void generateComposite(string path, int width, int height, ImageFormat format) {
			Bitmap inpMet = null, inpRouf = null, inpSpec = null, inpAO = null;
			if(texMet != null && texMet.Length > 0)
				inpMet = (Bitmap)Image.FromFile(texMet);
			if(texRouf != null && texRouf.Length > 0)
				inpRouf = (Bitmap)Image.FromFile(texRouf);
			if(texSpec != null && texSpec.Length > 0)
				inpSpec = (Bitmap)Image.FromFile(texSpec);
			if(texAO != null && texAO.Length > 0)
				inpAO = (Bitmap)Image.FromFile(texAO);

			Bitmap outp = new Bitmap(width, height);
			byte met, rouf, spec, ao;
			byte or, og, ob, oa;
			for(int x = 0; x < outp.Width; x++) {
				for(int y = 0; y < outp.Height; y++) {
					or = og = ob = oa = 0;

					met = 0;
					if(inpMet != null) {
						met = modInputColor(inpMet.GetPixel(x, y), metIn);
					} else if(modeMet == InpMode.WHITE)
						met = 255;
					met = invMet ? (byte)(255 - met) : met;
					switch(metOut) {
						case CHAN.ALPHA:
							oa = met;
							break;
						case CHAN.RED:
							or = met;
							break;
						case CHAN.GREEN:
							og = met;
							break;
						case CHAN.BLUE:
							ob = met;
							break;
					}

					rouf = 0;
					if(inpRouf != null) {
						rouf = modInputColor(inpRouf.GetPixel(x, y), roufIn);
					} else if(modeRouf == InpMode.WHITE)
						rouf = 255;
					rouf = invRouf ? (byte)(255 - rouf) : rouf;
					switch(roufOut) {
						case CHAN.ALPHA:
							oa = rouf;
							break;
						case CHAN.RED:
							or = rouf;
							break;
						case CHAN.GREEN:
							og = rouf;
							break;
						case CHAN.BLUE:
							ob = rouf;
							break;
					}

					spec = 0;
					if(inpSpec != null) {
						spec = modInputColor(inpSpec.GetPixel(x, y), specIn);
					} else if(modeSpec == InpMode.WHITE)
						spec = 255;
					spec = invSpec ? (byte)(255 - spec) : spec;
					switch(specOut) {
						case CHAN.ALPHA:
							oa = spec;
							break;
						case CHAN.RED:
							or = spec;
							break;
						case CHAN.GREEN:
							og = spec;
							break;
						case CHAN.BLUE:
							ob = spec;
							break;
					}

					ao = 255;
					if(inpAO != null) {
						ao = modInputColor(inpAO.GetPixel(x, y), aoIn);
					} else if(modeSpec == InpMode.BLACK)
						ao = 0;
					ao = invSpec ? (byte)(255 - ao) : ao;
					switch(aoOut) {
						case CHAN.ALPHA:
							oa = ao;
							break;
						case CHAN.RED:
							or = ao;
							break;
						case CHAN.GREEN:
							og = ao;
							break;
						case CHAN.BLUE:
							ob = ao;
							break;
					}

					outp.SetPixel(x, y, Color.FromArgb(oa, or, og, ob));
				}
			}

			outp.Save(path, format);
		}

		private void generateNormals(string path, int width, int height, ImageFormat format) {
			Bitmap inpNorm = null, inpDisp = null;
			if(texNorm != null && texNorm.Length > 0)
				inpNorm = (Bitmap)Image.FromFile(texNorm);
			if(useDisp && texDisp != null && texDisp.Length > 0)
				inpDisp = (Bitmap)Image.FromFile(texDisp);

			Bitmap outp = new Bitmap(width, height);
			Color samp;
			byte or, og, ob, oa;
			for(int x = 0; x < outp.Width; x++) {
				for(int y = 0; y < outp.Height; y++) {
					or = og = ob = oa = 0;
					if(inpNorm!=null) {
						samp = inpNorm.GetPixel(x, y);
						or = samp.R;
						ob = samp.B;

						og = invNorm ? (byte)(255 - samp.G) : samp.G;
					} else
						ob = 255;

					if(useDisp) {
						if(inpDisp != null) {
							samp = inpDisp.GetPixel(x, y);
							oa = modInputColor(samp, CHAN.AVERAGE);
						} else if(modeDisp == InpMode.BLACK)
							oa = 0;
						else
							oa = 255;
					} else
						oa = 255;

					outp.SetPixel(x, y, Color.FromArgb(oa, or, og, ob));
				}
			}

			outp.Save(path, format);
		}

		private void generateDisplacement(String path, int width, int height, ImageFormat format) {
			Bitmap inpDisp = null;
			if(texDisp != null && texDisp.Length > 0)
				inpDisp = (Bitmap)Image.FromFile(texDisp);

			Bitmap outd = new Bitmap(width, height);
			Color samp;
			byte or;
			for(int x = 0; x < outd.Width; x++) {
				for(int y = 0; y < outd.Height; y++) {
					or = 0;
					if(inpDisp != null) {
						samp = inpDisp.GetPixel(x, y);
						or = modInputColor(samp, CHAN.AVERAGE);
						if(invDisp)
							or = (byte)(255 - or);
					} else if(modeDisp == InpMode.WHITE)
						or = 255;

					outd.SetPixel(x, y, Color.FromArgb(255, or, or, or));
				}
			}
			
			outd.Save(path, format);
		}

		private byte modInputColor(Color inp, CHAN mode) {
			switch(mode) {
				case CHAN.AVERAGE:
					byte avg = (byte)((inp.R + inp.G + inp.B + inp.A) / 4);
					return avg;
				case CHAN.RED:
					return inp.R;
				case CHAN.GREEN:
					return inp.G;
				case CHAN.BLUE:
					return inp.B;
				case CHAN.ALPHA:
					return inp.A;
			}
			return 0;
		}
	}
}
