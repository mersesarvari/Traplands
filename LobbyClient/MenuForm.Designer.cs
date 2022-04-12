namespace LobbyClient
{
    partial class MenuForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.codeBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.lobby_id_label = new System.Windows.Forms.Label();
            this.lobby_username_label = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lobby_start_button = new System.Windows.Forms.Button();
            this.lobby_id_box = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lobby_listbox = new System.Windows.Forms.ListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.game_down_button = new System.Windows.Forms.Button();
            this.game_right_button = new System.Windows.Forms.Button();
            this.game_up_button = new System.Windows.Forms.Button();
            this.game_left_button = new System.Windows.Forms.Button();
            this.game_item = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.game_item)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(87, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(218, 23);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(311, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Your Username:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 56);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(372, 38);
            this.button2.TabIndex = 4;
            this.button2.Text = "Join a Game";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // codeBox
            // 
            this.codeBox.Location = new System.Drawing.Point(126, 27);
            this.codeBox.Name = "codeBox";
            this.codeBox.Size = new System.Drawing.Size(252, 23);
            this.codeBox.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.codeBox);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(21, 172);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connect to a Game";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Enter the Lobby Code";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Location = new System.Drawing.Point(21, 278);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 170);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create a Game";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "label6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Lobby Code:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(216, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Lobby code will be randomly generated";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 56);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(372, 38);
            this.button3.TabIndex = 4;
            this.button3.Text = "Create a Game";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Red;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Segoe UI Historic", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button4.Location = new System.Drawing.Point(21, 97);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(384, 59);
            this.button4.TabIndex = 10;
            this.button4.Text = "Offline";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(136, 62);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(269, 23);
            this.textBox3.TabIndex = 11;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(423, 530);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 502);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 15);
            this.label9.TabIndex = 12;
            this.label9.Text = "label9";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.lobby_start_button);
            this.groupBox4.Controls.Add(this.lobby_id_box);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.lobby_listbox);
            this.groupBox4.Location = new System.Drawing.Point(441, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(409, 530);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button5);
            this.groupBox5.Controls.Add(this.lobby_id_label);
            this.groupBox5.Controls.Add(this.lobby_username_label);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox5.Location = new System.Drawing.Point(6, 22);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(391, 100);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "User Information";
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button5.Location = new System.Drawing.Point(580, 30);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(186, 55);
            this.button5.TabIndex = 4;
            this.button5.Text = "Start Game";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // lobby_id_label
            // 
            this.lobby_id_label.AutoSize = true;
            this.lobby_id_label.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lobby_id_label.Location = new System.Drawing.Point(63, 62);
            this.lobby_id_label.Name = "lobby_id_label";
            this.lobby_id_label.Size = new System.Drawing.Size(72, 30);
            this.lobby_id_label.TabIndex = 3;
            this.lobby_id_label.Text = "label9";
            // 
            // lobby_username_label
            // 
            this.lobby_username_label.AutoSize = true;
            this.lobby_username_label.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lobby_username_label.Location = new System.Drawing.Point(140, 32);
            this.lobby_username_label.Name = "lobby_username_label";
            this.lobby_username_label.Size = new System.Drawing.Size(84, 30);
            this.lobby_username_label.TabIndex = 2;
            this.lobby_username_label.Text = "label10";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(18, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 30);
            this.label7.TabIndex = 0;
            this.label7.Text = "Username:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(18, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 30);
            this.label8.TabIndex = 1;
            this.label8.Text = "Id:";
            // 
            // lobby_start_button
            // 
            this.lobby_start_button.Location = new System.Drawing.Point(9, 416);
            this.lobby_start_button.Name = "lobby_start_button";
            this.lobby_start_button.Size = new System.Drawing.Size(388, 23);
            this.lobby_start_button.TabIndex = 12;
            this.lobby_start_button.Text = "button2";
            this.lobby_start_button.UseVisualStyleBackColor = true;
            this.lobby_start_button.Click += new System.EventHandler(this.lobby_start_button_Click);
            // 
            // lobby_id_box
            // 
            this.lobby_id_box.Location = new System.Drawing.Point(118, 499);
            this.lobby_id_box.Name = "lobby_id_box";
            this.lobby_id_box.ReadOnly = true;
            this.lobby_id_box.Size = new System.Drawing.Size(279, 23);
            this.lobby_id_box.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(6, 495);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(106, 30);
            this.label11.TabIndex = 10;
            this.label11.Text = "Lobby Id:";
            // 
            // lobby_listbox
            // 
            this.lobby_listbox.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lobby_listbox.FormattingEnabled = true;
            this.lobby_listbox.ItemHeight = 30;
            this.lobby_listbox.Location = new System.Drawing.Point(8, 133);
            this.lobby_listbox.Name = "lobby_listbox";
            this.lobby_listbox.Size = new System.Drawing.Size(389, 274);
            this.lobby_listbox.TabIndex = 9;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.game_down_button);
            this.groupBox6.Controls.Add(this.game_right_button);
            this.groupBox6.Controls.Add(this.game_up_button);
            this.groupBox6.Controls.Add(this.game_left_button);
            this.groupBox6.Controls.Add(this.game_item);
            this.groupBox6.Location = new System.Drawing.Point(856, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(357, 530);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "groupBox6";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 15);
            this.label10.TabIndex = 6;
            this.label10.Text = "label10";
            // 
            // game_down_button
            // 
            this.game_down_button.Location = new System.Drawing.Point(142, 501);
            this.game_down_button.Name = "game_down_button";
            this.game_down_button.Size = new System.Drawing.Size(75, 23);
            this.game_down_button.TabIndex = 5;
            this.game_down_button.Text = "Down";
            this.game_down_button.UseVisualStyleBackColor = true;
            this.game_down_button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.game_down_button_MouseDown);
            this.game_down_button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.game_down_button_MouseUp);
            // 
            // game_right_button
            // 
            this.game_right_button.Location = new System.Drawing.Point(241, 454);
            this.game_right_button.Name = "game_right_button";
            this.game_right_button.Size = new System.Drawing.Size(75, 23);
            this.game_right_button.TabIndex = 4;
            this.game_right_button.Text = "Right";
            this.game_right_button.UseVisualStyleBackColor = true;
            this.game_right_button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.game_right_button_MouseDown);
            this.game_right_button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.game_right_button_MouseUp);
            // 
            // game_up_button
            // 
            this.game_up_button.Location = new System.Drawing.Point(142, 416);
            this.game_up_button.Name = "game_up_button";
            this.game_up_button.Size = new System.Drawing.Size(75, 23);
            this.game_up_button.TabIndex = 3;
            this.game_up_button.Text = "UP";
            this.game_up_button.UseVisualStyleBackColor = true;
            this.game_up_button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.game_up_button_MouseDown);
            this.game_up_button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.game_up_button_MouseUp);
            // 
            // game_left_button
            // 
            this.game_left_button.Location = new System.Drawing.Point(50, 454);
            this.game_left_button.Name = "game_left_button";
            this.game_left_button.Size = new System.Drawing.Size(75, 23);
            this.game_left_button.TabIndex = 2;
            this.game_left_button.Text = "Left";
            this.game_left_button.UseVisualStyleBackColor = true;
            this.game_left_button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.game_left_button_MouseDown);
            this.game_left_button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.game_left_button_MouseUp);
            // 
            // game_item
            // 
            this.game_item.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.game_item.Location = new System.Drawing.Point(200, 200);
            this.game_item.Name = "game_item";
            this.game_item.Size = new System.Drawing.Size(36, 29);
            this.game_item.TabIndex = 1;
            this.game_item.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1225, 554);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Name = "MenuForm";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.game_item)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private Label label2;
        private Button button2;
        private TextBox codeBox;
        private GroupBox groupBox1;
        private Label label3;
        private GroupBox groupBox2;
        private Label label4;
        private Button button3;
        private Label label6;
        private Label label5;
        private Button button4;
        private TextBox textBox3;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private Button button5;
        private Label lobby_id_label;
        private Label lobby_username_label;
        private Label label7;
        private Label label8;
        private Button lobby_start_button;
        private TextBox lobby_id_box;
        private Label label11;
        private ListBox lobby_listbox;
        private GroupBox groupBox6;
        public PictureBox game_item;
        private System.Windows.Forms.Timer timer1;
        private Button game_down_button;
        private Button game_right_button;
        private Button game_up_button;
        private Button game_left_button;
        private Label label9;
        private Label label10;
    }
}