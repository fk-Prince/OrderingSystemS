namespace OrderingSystem.KioskApp.Beverage
{
    partial class BeverageFrm
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
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowPanel
            // 
            this.flowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowPanel.AutoScroll = true;
            this.flowPanel.Location = new System.Drawing.Point(1, 115);
            this.flowPanel.Margin = new System.Windows.Forms.Padding(10);
            this.flowPanel.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.flowPanel.MinimumSize = new System.Drawing.Size(796, 492);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.flowPanel.Size = new System.Drawing.Size(796, 612);
            this.flowPanel.TabIndex = 2;
            // 
            // BeverageFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(795, 727);
            this.Controls.Add(this.flowPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BeverageFrm";
            this.Text = "BeverageFrm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowPanel;
    }
}