namespace BIOSC_INI_CONFIGURATOR
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabCtrlGroup = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabCtrlGroup
            // 
            this.tabCtrlGroup.Location = new System.Drawing.Point(23, 12);
            this.tabCtrlGroup.Name = "tabCtrlGroup";
            this.tabCtrlGroup.SelectedIndex = 0;
            this.tabCtrlGroup.Size = new System.Drawing.Size(2171, 1232);
            this.tabCtrlGroup.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2205, 1254);
            this.Controls.Add(this.tabCtrlGroup);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabCtrlGroup;
    }
}