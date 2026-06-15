using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doodlejump
{
    public partial class FormLeaderBoard : Form
    {
        public FormLeaderBoard(List<DodleJump.User> users)
        {
            InitializeComponent();
            this.Height = 450;
            this.Width = 400;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "LeaderBoard";
            listBoxLeaderBoard.Font = new Font("Courier New", 12, FontStyle.Bold);
            listBoxLeaderBoard.Items.Add(" Місце | Гравець   | Рекорд");
            listBoxLeaderBoard.Items.Add("------------------------------------");
            var topGamer = users.OrderByDescending(u => u.MaxScore).Take(10).ToList();
            int place = 1;
            foreach (var user in topGamer)
            {
                string namePadded = user.Name.PadRight(8);
                string placeStr = place.ToString().PadLeft(2);

                listBoxLeaderBoard.Items.Add($"  #{placeStr}  | {namePadded}  | {user.MaxScore}");
                place++;
            }

            if (topGamer.Count == 0)
            {
                listBoxLeaderBoard.Items.Add("");
                listBoxLeaderBoard.Items.Add("   Таблиця порожня. Будь першим!");
            }
        }

        private void FormLeaderBoard_Load(object sender, EventArgs e)
        {

        }

        private void buttonLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
