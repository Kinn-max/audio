﻿using System.Drawing;
using System.Windows.Forms;
using System;

namespace AudioCallApp
{
    partial class Form1
    { //Hoa
        private System.ComponentModel.IContainer components = null;
        private Button btnConnect;
        private Button btnEndCall;
        private Button btnTalk;
        private TextBox txtServerIP;
        private Label lblServerIP;
        private Label statusLabel;
        private TrackBar volumeSlider;
        private Label lblVolume;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            //Hoa
        }   
    }
}