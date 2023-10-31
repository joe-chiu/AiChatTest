namespace WinFormsAiChat;

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
        pictureBox1 = new PictureBox();
        comboBox1 = new ComboBox();
        button1 = new Button();
        textBox1 = new TextBox();
        richTextBox1 = new RichTextBox();
        textBox2 = new TextBox();
        statusStrip1 = new StatusStrip();
        toolStripStatusLabel1 = new ToolStripStatusLabel();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // pictureBox1
        // 
        pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pictureBox1.Location = new Point(9, 211);
        pictureBox1.Margin = new Padding(2);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(450, 450);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        // 
        // comboBox1
        // 
        comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
        comboBox1.FormattingEnabled = true;
        comboBox1.Items.AddRange(new object[] { "Black Smith", "Computer Engineer", "Investment Advisor" });
        comboBox1.Location = new Point(9, 9);
        comboBox1.Margin = new Padding(2);
        comboBox1.Name = "comboBox1";
        comboBox1.Size = new Size(450, 56);
        comboBox1.TabIndex = 3;
        comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        // 
        // button1
        // 
        button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        button1.Location = new Point(1014, 593);
        button1.Margin = new Padding(2);
        button1.Name = "button1";
        button1.Size = new Size(115, 36);
        button1.TabIndex = 2;
        button1.Text = "Send";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // textBox1
        // 
        textBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        textBox1.Location = new Point(463, 559);
        textBox1.Margin = new Padding(2);
        textBox1.Multiline = true;
        textBox1.Name = "textBox1";
        textBox1.PlaceholderText = "Enter user prompt...";
        textBox1.Size = new Size(548, 102);
        textBox1.TabIndex = 1;
        textBox1.KeyDown += textBox1_KeyDown;
        // 
        // richTextBox1
        // 
        richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        richTextBox1.Location = new Point(463, 9);
        richTextBox1.Margin = new Padding(2);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.ReadOnly = true;
        richTextBox1.Size = new Size(668, 534);
        richTextBox1.TabIndex = 5;
        richTextBox1.Text = "";
        // 
        // textBox2
        // 
        textBox2.Location = new Point(9, 71);
        textBox2.Margin = new Padding(2);
        textBox2.Multiline = true;
        textBox2.Name = "textBox2";
        textBox2.ReadOnly = true;
        textBox2.Size = new Size(450, 136);
        textBox2.TabIndex = 4;
        // 
        // statusStrip1
        // 
        statusStrip1.ImageScalingSize = new Size(32, 32);
        statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
        statusStrip1.Location = new Point(0, 664);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Padding = new Padding(1, 0, 11, 0);
        statusStrip1.Size = new Size(1139, 32);
        statusStrip1.TabIndex = 7;
        statusStrip1.Text = "statusStrip1";
        // 
        // toolStripStatusLabel1
        // 
        toolStripStatusLabel1.Name = "toolStripStatusLabel1";
        toolStripStatusLabel1.Size = new Size(179, 25);
        toolStripStatusLabel1.Text = "toolStripStatusLabel1";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(144F, 144F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1139, 696);
        Controls.Add(statusStrip1);
        Controls.Add(textBox2);
        Controls.Add(richTextBox1);
        Controls.Add(textBox1);
        Controls.Add(button1);
        Controls.Add(comboBox1);
        Controls.Add(pictureBox1);
        Margin = new Padding(2);
        Name = "Form1";
        Text = "AI Chat";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private PictureBox pictureBox1;
    private ComboBox comboBox1;
    private Button button1;
    private TextBox textBox1;
    private RichTextBox richTextBox1;
    private TextBox textBox2;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel toolStripStatusLabel1;
}
