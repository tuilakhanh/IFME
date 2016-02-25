﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FFmpegDotNet;
using System.IO;

namespace ifme
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); // shirnk exe size
            FormBorderStyle = FormBorderStyle.Sizable; // do accurate container size excluding window bezel
        }

        #region Form Action
        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        #endregion

        #region Queue Button
        private void btnQueueAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog getFiles = new OpenFileDialog();
            getFiles.Filter = "All Files|*.*";
            getFiles.FilterIndex = 1;
            getFiles.Multiselect = true;

            if (getFiles.ShowDialog() == DialogResult.OK)
                foreach (var item in getFiles.FileNames)
                    QueueAdd(item);
        }
        private void btnQueueRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstQueue.SelectedItems)
                item.Remove();
        }

        private void btnQueueMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstQueue.SelectedItems.Count > 0)
                {
                    ListViewItem selected = lstQueue.SelectedItems[0];
                    int indx = selected.Index;
                    int totl = lstQueue.Items.Count;

                    if (indx == 0)
                    {
                        lstQueue.Items.Remove(selected);
                        lstQueue.Items.Insert(totl - 1, selected);
                    }
                    else
                    {
                        lstQueue.Items.Remove(selected);
                        lstQueue.Items.Insert(indx - 1, selected);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQueueMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstQueue.SelectedItems.Count > 0)
                {
                    ListViewItem selected = lstQueue.SelectedItems[0];
                    int indx = selected.Index;
                    int totl = lstQueue.Items.Count;

                    if (indx == totl - 1)
                    {
                        lstQueue.Items.Remove(selected);
                        lstQueue.Items.Insert(0, selected);
                    }
                    else
                    {
                        lstQueue.Items.Remove(selected);
                        lstQueue.Items.Insert(indx + 1, selected);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQueueStop_Click(object sender, EventArgs e)
        {

        }

        private void btnQueuePause_Click(object sender, EventArgs e)
        {

        }

        private void btnQueueStart_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Queue List Action
        private void lstQueue_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstQueue_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void lstQueue_DragEnter(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
                QueueAdd(file);
        }
        #endregion

        #region Encoding Profile & Output Folder
        private void cboProfile_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnProfileSave_Click(object sender, EventArgs e)
        {

        }

        private void btnProfileDelete_Click(object sender, EventArgs e)
        {

        }

        private void txtOutputFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Tab Properties
        private void rdoMP4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdoMKV_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Tab Video
        private void cboVideoResolution_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboVideoResolution_Leave(object sender, EventArgs e)
        {

        }

        private void cboVideoFrameRate_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboVideoFrameRate_Leave(object sender, EventArgs e)
        {

        }

        private void cboVideoBitDepth_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboVideoChroma_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkVideoDeinterlace_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cboVideoDiMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboVideoDiField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboVideoEncoder_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboVideoPreset_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboVideoTune_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboVideoEncodingType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void nudVideoRateFactor_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nudVideoMultipass_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnVideoArgEdit_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Tab Audio
        private void btnAudioAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnAudioRemove_Click(object sender, EventArgs e)
        {

        }

        private void btnAudioMoveUp_Click(object sender, EventArgs e)
        {

        }

        private void btnAudioMoveDown_Click(object sender, EventArgs e)
        {

        }

        private void lstAudio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboAudioEncoder_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboAudioMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboAudioQuality_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboAudioFreq_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboAudioChannel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAudioEditArg_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Tab Subtitle
        private void btnSubAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnSubRemove_Click(object sender, EventArgs e)
        {

        }

        private void btnSubMoveUp_Click(object sender, EventArgs e)
        {

        }

        private void btnSubMoveDown_Click(object sender, EventArgs e)
        {

        }

        private void lstSub_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboSubLang_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Tab Attchment
        private void btnAttachAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnAttachDel_Click(object sender, EventArgs e)
        {

        }

        private void lstAttach_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Add Video to Queue
        private void QueueAdd(string filePath)
        {
            FFmpeg.Probe = Path.Combine(Environment.CurrentDirectory, "binary", "ffmpeg32", "ffprobe");

            var mi = new FFmpeg.Stream(filePath);
            var qi = new Queue();
            
            if (mi.Video.Count > 0)
            {
                qi.Enable = true;
                qi.MkvOut = true;

                foreach (var item in mi.Video)
                {
                    qi.Video.Add(new QueueVideo
                    {
                        File = filePath,
                        Id = item.Id,
                        Width = item.Width,
                        Height = item.Height,
                        FrameRate = item.FrameRate,
                        BitDepth = item.BitDepth,
                        Chroma = item.Chroma,

                        Deinterlace = false,
                        DeinterlaceField = 0,
                        DeinterlaceMode = 0,

                        Encoder = new Guid(),
                        EncoderPreset = "",
                        EncoderTune = "",
                        EncoderRateControl = "",
                        EncoderRateValue = "",
                        EncoderMultiPass = 0,
                        EncoderArgs = "",
                    });
                }

                foreach (var item in mi.Audio)
                {
                    qi.Audio.Add(new QueueAudio
                    {
                        File = filePath,
                        Id = item.Id,

                        BitDepth = item.BitDepth, // use for decoding, hidden from GUI

                        Encoder = new Guid(),
                        EncoderMode = 0,
                        EncoderValue = "0",
                        EncoderSampleRate = item.SampleRate,
                        EncoderChannel = item.Channel,
                        EncoderArgs = "",
                    });
                }

                foreach (var item in mi.Subtitle)
                {
                    qi.Subtitle.Add(new QueueSubtitle
                    {
                        File = filePath,
                        Id = item.Id,
                        Language = item.Language,
                    });
                }

                ListViewItem lst = new ListViewItem(new[] {
                    filePath,
                    TimeSpan.FromSeconds(mi.Duration).ToString("hh\\:mm\\:ss"),
                    "MKV",
                    "Ready",
                });
                lst.Tag = qi;
                lst.Checked = true;

                lstQueue.Items.Add(lst);

            }
        }
        #endregion
    }
}
