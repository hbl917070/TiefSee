namespace Forms_合成GIF {
    partial class Form1 {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent() {
            this.button_執行 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_input_path = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_output_path = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_size = new System.Windows.Forms.Label();
            this.textBox_h = new System.Windows.Forms.TextBox();
            this.textBox_w = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox_size = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_open = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label_進度 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_執行
            // 
            this.button_執行.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_執行.Location = new System.Drawing.Point(7, 9);
            this.button_執行.Name = "button_執行";
            this.button_執行.Size = new System.Drawing.Size(90, 42);
            this.button_執行.TabIndex = 0;
            this.button_執行.Text = "執行";
            this.button_執行.UseVisualStyleBackColor = true;
            this.button_執行.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "輸出路徑";
            // 
            // textBox_input_path
            // 
            this.textBox_input_path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_input_path.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_input_path.Location = new System.Drawing.Point(150, 35);
            this.textBox_input_path.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_input_path.Name = "textBox_input_path";
            this.textBox_input_path.ReadOnly = true;
            this.textBox_input_path.Size = new System.Drawing.Size(259, 29);
            this.textBox_input_path.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 27);
            this.label2.TabIndex = 5;
            this.label2.Text = "輸入來源";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 27);
            this.label3.TabIndex = 7;
            this.label3.Text = "圖片原始大小";
            // 
            // textBox_output_path
            // 
            this.textBox_output_path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_output_path.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_output_path.Location = new System.Drawing.Point(0, 0);
            this.textBox_output_path.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_output_path.Name = "textBox_output_path";
            this.textBox_output_path.Size = new System.Drawing.Size(196, 29);
            this.textBox_output_path.TabIndex = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoScrollMinSize = new System.Drawing.Size(10, 0);
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label_size, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox_input_path, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox_h, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.textBox_w, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_size, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 56);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(409, 242);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // label_size
            // 
            this.label_size.AutoSize = true;
            this.label_size.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_size.ForeColor = System.Drawing.Color.White;
            this.label_size.Location = new System.Drawing.Point(153, 0);
            this.label_size.Name = "label_size";
            this.label_size.Size = new System.Drawing.Size(141, 27);
            this.label_size.TabIndex = 15;
            this.label_size.Text = "W:500  H:960";
            // 
            // textBox_h
            // 
            this.textBox_h.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_h.Location = new System.Drawing.Point(150, 195);
            this.textBox_h.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_h.Name = "textBox_h";
            this.textBox_h.Size = new System.Drawing.Size(164, 29);
            this.textBox_h.TabIndex = 14;
            // 
            // textBox_w
            // 
            this.textBox_w.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_w.Location = new System.Drawing.Point(150, 160);
            this.textBox_w.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_w.Name = "textBox_w";
            this.textBox_w.Size = new System.Drawing.Size(164, 29);
            this.textBox_w.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 195);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 27);
            this.label5.TabIndex = 13;
            this.label5.Text = "高度";
            // 
            // checkBox_size
            // 
            this.checkBox_size.AutoSize = true;
            this.checkBox_size.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.checkBox_size.ForeColor = System.Drawing.Color.White;
            this.checkBox_size.Location = new System.Drawing.Point(10, 128);
            this.checkBox_size.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.checkBox_size.Name = "checkBox_size";
            this.checkBox_size.Size = new System.Drawing.Size(115, 29);
            this.checkBox_size.TabIndex = 10;
            this.checkBox_size.Text = "修改大小";
            this.checkBox_size.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBox_output_path);
            this.panel2.Controls.Add(this.button_open);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(150, 70);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(259, 35);
            this.panel2.TabIndex = 18;
            // 
            // button_open
            // 
            this.button_open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_open.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_open.Location = new System.Drawing.Point(202, 0);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(57, 29);
            this.button_open.TabIndex = 11;
            this.button_open.Text = "瀏覽";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 27);
            this.label4.TabIndex = 12;
            this.label4.Text = "寬度";
            // 
            // label_進度
            // 
            this.label_進度.AutoSize = true;
            this.label_進度.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_進度.ForeColor = System.Drawing.Color.White;
            this.label_進度.Location = new System.Drawing.Point(105, 18);
            this.label_進度.Name = "label_進度";
            this.label_進度.Size = new System.Drawing.Size(22, 27);
            this.label_進度.TabIndex = 16;
            this.label_進度.Text = "  ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(415, 299);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label_進度);
            this.Controls.Add(this.button_執行);
            this.Name = "Form1";
            this.Text = "AeroPic - 輸出 GIF";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_執行;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_input_path;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_output_path;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_w;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox_size;
        private System.Windows.Forms.TextBox textBox_h;
        private System.Windows.Forms.Label label_size;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label_進度;
    }
}

