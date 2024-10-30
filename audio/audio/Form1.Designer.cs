namespace AudioCall2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnStartCall;
        private System.Windows.Forms.Button btnStopCall;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Label lblServerIP;

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
            this.btnStartCall = new System.Windows.Forms.Button();
            this.btnStopCall = new System.Windows.Forms.Button();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStartCall
            // 
            this.btnStartCall.Location = new System.Drawing.Point(80, 100);
            this.btnStartCall.Name = "btnStartCall";
            this.btnStartCall.Size = new System.Drawing.Size(120, 40);
            this.btnStartCall.TabIndex = 0;
            this.btnStartCall.Text = "Start Call";
            this.btnStartCall.UseVisualStyleBackColor = true;
            this.btnStartCall.Click += new System.EventHandler(this.btnStartCall_Click);
            // 
            // btnStopCall
            // 
            this.btnStopCall.Location = new System.Drawing.Point(240, 100);
            this.btnStopCall.Name = "btnStopCall";
            this.btnStopCall.Size = new System.Drawing.Size(120, 40);
            this.btnStopCall.TabIndex = 1;
            this.btnStopCall.Text = "Stop Call";
            this.btnStopCall.UseVisualStyleBackColor = true;
            this.btnStopCall.Click += new System.EventHandler(this.btnStopCall_Click);
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(140, 50);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(220, 22);
            this.txtServerIP.TabIndex = 2;
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Location = new System.Drawing.Point(40, 50);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(70, 16);
            this.lblServerIP.TabIndex = 3;
            this.lblServerIP.Text = "Server IP:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 180);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.txtServerIP);
            this.Controls.Add(this.btnStartCall);
            this.Controls.Add(this.btnStopCall);
            this.Name = "Form1";
            this.Text = "Audio Call";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
