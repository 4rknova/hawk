using System;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;

namespace Hawk
{
    public partial class Main : Form
    {
        private TextBox nmlist;
        private TextBox nkm;
        private TextBox msg;
        private Label lbl_nkm;
        private Label lbl_msg;
        private TextBox log;
    
        public Main()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.log.ScrollBars  = ScrollBars.Vertical;
            this.nmlist.ScrollBars = ScrollBars.Vertical;
            this.log.ReadOnly    = true;
            this.nmlist.ReadOnly = true;
            this.msg.KeyDown    += new KeyEventHandler(tb_KeyDown);
            this.nkm.KeyDown    += new KeyEventHandler(nm_KeyDown);
            Logger.evhlog       += new EventHandler(HandleLogEvent);
            Bumper.evhnkm       += new EventHandler(HandleNickEvent);

            Text = "Hawk";
        }

        private void HandleLogEvent(object sender, EventArgs args)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true.
            try
            {
                if (log.InvokeRequired)
                {
                    log.Invoke((MethodInvoker)delegate
                    {
                        HandleLogEvent((LogEventArgs)args);
                    });
                }
                else
                {
                    HandleLogEvent((LogEventArgs)args);
                }
            }
            catch (Exception)
            {
                // Suppress the expected disposed object exception when
                // the application is terminated and the admin form is 
                // open.
            }
        }

        private void HandleLogEvent(LogEventArgs args)
        {
            log.Text += args.Message + "\r\n";
            log.SelectionStart = log.Text.Length;
            log.ScrollToCaret();
        }

        private void HandleNickEvent(object sender, EventArgs args)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true.
            try
            {
                if (log.InvokeRequired)
                {
                    log.Invoke((MethodInvoker)delegate
                    {
                        HandleNickEvent(null);
                    });
                }
                else
                {
                    HandleNickEvent(null);
                }
            }
            catch (Exception)
            {
                // Suppress the expected disposed object exception when
                // the application is terminated and the admin form is 
                // open.
            }
        }

        private void HandleNickEvent(LogEventArgs args)
        {
            nmlist.Clear();

            foreach (string name in Bumper.NickNames)
            {
                nmlist.Text += name + "\r\n";
            }
            
            log.ScrollToCaret();
        }
                
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Bumper.SendText(msg.Text);
                msg.Clear();
            }
        }

        private void nm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Bumper.NickName = nkm.Text;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.log = new System.Windows.Forms.TextBox();
            this.nmlist = new System.Windows.Forms.TextBox();
            this.nkm = new System.Windows.Forms.TextBox();
            this.msg = new System.Windows.Forms.TextBox();
            this.lbl_nkm = new System.Windows.Forms.Label();
            this.lbl_msg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // log
            // 
            this.log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.log.Location = new System.Drawing.Point(12, 12);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(550, 152);
            this.log.TabIndex = 0;
            // 
            // nmlist
            // 
            this.nmlist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmlist.Location = new System.Drawing.Point(568, 12);
            this.nmlist.Multiline = true;
            this.nmlist.Name = "nmlist";
            this.nmlist.Size = new System.Drawing.Size(103, 195);
            this.nmlist.TabIndex = 1;
            // 
            // nkm
            // 
            this.nkm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nkm.Location = new System.Drawing.Point(13, 187);
            this.nkm.Name = "nkm";
            this.nkm.Size = new System.Drawing.Size(98, 20);
            this.nkm.TabIndex = 2;
            // 
            // msg
            // 
            this.msg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msg.Location = new System.Drawing.Point(117, 187);
            this.msg.Name = "msg";
            this.msg.Size = new System.Drawing.Size(445, 20);
            this.msg.TabIndex = 3;
            // 
            // lbl_nkm
            // 
            this.lbl_nkm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_nkm.AutoSize = true;
            this.lbl_nkm.Location = new System.Drawing.Point(10, 168);
            this.lbl_nkm.Name = "lbl_nkm";
            this.lbl_nkm.Size = new System.Drawing.Size(55, 13);
            this.lbl_nkm.TabIndex = 4;
            this.lbl_nkm.Text = "Nickname";
            // 
            // lbl_msg
            // 
            this.lbl_msg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_msg.AutoSize = true;
            this.lbl_msg.Location = new System.Drawing.Point(114, 168);
            this.lbl_msg.Name = "lbl_msg";
            this.lbl_msg.Size = new System.Drawing.Size(50, 13);
            this.lbl_msg.TabIndex = 5;
            this.lbl_msg.Text = "Message";
            // 
            // Main
            // 
            this.ClientSize = new System.Drawing.Size(684, 219);
            this.Controls.Add(this.lbl_msg);
            this.Controls.Add(this.lbl_nkm);
            this.Controls.Add(this.msg);
            this.Controls.Add(this.nkm);
            this.Controls.Add(this.nmlist);
            this.Controls.Add(this.log);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 210);
            this.Name = "Main";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
