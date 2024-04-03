﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ForumApp
{
    public partial class Home : Form
    {
        Posts post = new Posts();
        public Home()
        {
            InitializeComponent();

            logoutBtn.FlatStyle = FlatStyle.Flat;
            logoutBtn.FlatAppearance.BorderSize = 0;
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Users.username = "";

            this.Hide();

            LoginForm login = new LoginForm();
            login.Closed += (s, args) => this.Close();
            login.Show();
        }

        private void MakePanels(Dictionary<string, (string title, DateTime date)> filteredPosts = null)
        {
            flowLayoutPosts.Controls.Clear();

            int panelSpacing = 20;
            int containerWidth = flowLayoutPosts.ClientSize.Width;

            Dictionary<string, (string title, DateTime date)> panelData;

            if (filteredPosts != null)
            {
                panelData = filteredPosts;
            } else
            {
                panelData = post.GetPanelData();
            }

            for (int i = 0; i < panelData.Count; i++)
            {
                var kvp = panelData.ElementAt(i);
                var panelId = kvp.Key;
                var panelInfo = kvp.Value;

                var panel = new Panel
                {
                    Name = panelId,
                    Tag = panelId,
                    Size = new Size(flowLayoutPosts.Width - 39, 40),
                    Margin = i == 0 ? new Padding(10, 20, 5, 20) : new Padding(10, 5, 10, 20),
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Hand,
                };

                var titleLabel = new Label
                {
                    Text = panelInfo.title,
                    Location = new Point(10, 10),
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    ForeColor = Color.Black,
                    AutoSize = true
                };
                panel.Controls.Add(titleLabel);
                titleLabel.Click += post_Click;

                var dateLabel = new Label
                {
                    Text = panelInfo.date.ToString("yyyy-MM-dd"),
                    Location = new Point(panel.Width - 110, 10),
                    Font = new Font("Segoe UI", 8),
                    TextAlign = ContentAlignment.MiddleRight
                };
                panel.Controls.Add(dateLabel);
                dateLabel.Click += post_Click;

                panel.Click += post_Click;

                flowLayoutPosts.Controls.Add(panel);
            }
        }

        private void post_Click(object sender, EventArgs e)
        {
            var clickedPanel = (Panel)sender;
            string panelId = clickedPanel.Tag.ToString();

            Dictionary<string, (string title, DateTime date)> panelData = post.GetPanelData();

            if (panelData.ContainsKey(panelId))
            {
                //string title = panelData[panelId].title;
                //DateTime date = panelData[panelId].date;

                this.Hide();

                PostForm post = new PostForm(panelId);
                post.Closed += (s, args) => this.Close();
                post.Show();
            }
            else
            {
                MessageBox.Show("Related post doesn't exist.");
            }
        }

        private void Home_Load(object sender, EventArgs e)
        {
            usernameTxt.Text = Users.username;
            MakePanels();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string keyword = searchTxt.Text;
            MakePanels(post.SearchPost(keyword));
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            using (Pen pen = new Pen(Color.LightGray, 1))
            {
                e.Graphics.DrawLine(pen, 0, panel.Height - 1, panel.Width, panel.Height - 1);
            }
        }
    }
}
