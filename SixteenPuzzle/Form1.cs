using System;
using System.Windows.Forms;

namespace SixteenPuzzle
{
    public partial class Form1 : Form
    {
        /// <summary>1辺のサイズ</summary>
        private const int FIELD_SIZE = 4;

        /// <summary>番号の数</summary>
        private const int NUMBERS = FIELD_SIZE * FIELD_SIZE - 1;

        /// <summary>コマのサイズ</summary>
        private const int PANEL_SIZE = 48;

        /// <summary>シャッフルの回数</summary>
        private const int SHUFFLE_COUNT = 1000;

        /// <summary>コマリスト</summary>
        private Button[] buttons = null;

        /// <summary>盤面情報</summary>
        private int[,] field = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            Arrange();
            Shuffle();
        }
        
        /// <summary>
        /// コマの配置
        /// </summary>
        private void Arrange()
        {
            buttons = new Button[NUMBERS];
            field = new int[FIELD_SIZE, FIELD_SIZE];
            int x = 0;
            int y = 0;

            for (int i = 0; i < NUMBERS; i++)
            {
                Button btn = MakePanel(i + 1, x, y);
                this.Controls.Add(btn);
                buttons[i] = btn;
                field[x, y] = i + 1;
                x++;
                if (x == FIELD_SIZE)
                {
                    x = 0;
                    y++;
                }
            }
        }

        /// <summary>
        /// コマを作る
        /// </summary>
        /// <param name="num">番号</param>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns></returns>
        private Button MakePanel(int num, int x, int y)
        {
            Button btn = new Button();
            btn.SetBounds(x * PANEL_SIZE, y * PANEL_SIZE, PANEL_SIZE, PANEL_SIZE);
            btn.Text = num.ToString();
            btn.Tag = new int[] { x, y };
            btn.Click += new EventHandler(btn_Click);
            return btn;
        }

        /// <summary>
        /// シャッフル
        /// </summary>
        private void Shuffle()
        {
            Random rand = new Random();
            for(int i= 0; i < SHUFFLE_COUNT; i++)
            {
                int number = rand.Next(0, NUMBERS);
                Move1(buttons[number]);
            }
        }

        /// <summary>
        /// コマ押下時処理
        /// </summary>
        /// <param name="sender">イベント発生オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Move1(btn);
            Judge();
        }

        /// <summary>
        /// 押されたコマを動かす
        /// </summary>
        /// <param name="btn">押されたコマ</param>
        private void Move1(Button btn)
        {
            int[] xy = (int[])btn.Tag;
            int x = xy[0];
            int y = xy[1];
            if (x > 0 && field[x - 1, y] == 0)
            {
                // 左に動かせるなら左に動かす
                Move2(x, y, x - 1, y, btn);
            }
            else if (x < FIELD_SIZE - 1 && field[x + 1, y] == 0)
            {
                // 右に動かせるなら右に動かす
                Move2(x, y, x + 1, y, btn);
            }
            else if (y > 0 && field[x, y - 1] == 0)
            {
                // 上に動かせるなら上に動かす
                Move2(x, y, x, y - 1, btn);
            }
            else if (y < FIELD_SIZE - 1 && field[x, y + 1] == 0)
            {
                // 下に動かせるなら下に動かす
                Move2(x, y, x, y + 1, btn);
            }
        }

        /// <summary>
        /// コマの移動
        /// </summary>
        /// <param name="fx">移動元X</param>
        /// <param name="fy">移動元Y</param>
        /// <param name="tx">移動先X</param>
        /// <param name="ty">移動先Y</param>
        /// <param name="btn">コマ</param>
        private void Move2(int fx, int fy, int tx, int ty, Button btn)
        {
            field[fx, fy] = 0;
            field[tx, ty] = int.Parse(btn.Text);
            btn.Tag = new int[] { tx, ty };
            btn.SetBounds(tx * PANEL_SIZE, ty * PANEL_SIZE, 0, 0, BoundsSpecified.Location);
        }

        /// <summary>
        /// 判定
        /// </summary>
        private void Judge()
        {
            bool clear = true;
            int x = 0;
            int y = 0;
            for (int i = 0; i < NUMBERS; i++)
            {
                if (field[x, y] != i + 1)
                {
                    clear = false;
                    break;
                }

                x++;
                if (x == FIELD_SIZE)
                {
                    x = 0;
                    y++;
                }
            }

            if (clear)
            {
                MessageBox.Show("Congratulations!");
            }
        }
    }
}
