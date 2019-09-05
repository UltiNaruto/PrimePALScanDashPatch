using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PrimePALScanDashPatch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            rand = new Random((int)(new DateTime().Ticks / TimeSpan.TicksPerMillisecond));
            InitializeComponent();
        }

        private void ISOtoPatch_btn_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "GC ISO File|*.iso";
                openFileDialog.FileName = "";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.ISOtoPatch_txtBox.Text = openFileDialog.SafeFileName;
                }
            }
        }

        private void Patch_btn_Click(object sender, EventArgs e)
        {
            using (var file = File.Open(this.ISOtoPatch_txtBox.Text, FileMode.Open))
            using (var BW = new BinaryWriter(file))
            using (var BR = new BinaryReader(file))
            {
                file.Seek(0x31FB2C8, SeekOrigin.Begin);
                ulong bytesToPatch = BitConverter.ToUInt32(BR.ReadBytes(4), 0);
                if (bytesToPatch == 0x10008241)
                {
                    MessageBox.Show("Unknown version used as source ISO");
                    return;
                }
                if (bytesToPatch == 0x18000048)
                {
                    MessageBox.Show("ISO is already patched");
                    return;
                }
                file.Seek(0x31FB2C8, SeekOrigin.Begin);
                BW.Write(0x18000048);
            }
            MessageBox.Show("Scan dash is now enabled for this ISO");
        }
    }
}
