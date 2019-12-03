using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AdventOfCode;
using AoC.y2018;

namespace AoC.Forms
{
    public partial class D10Form : Form
    {
        protected internal Day10 Data;

        private int sec = 0;

        public D10Form()
        {
            InitializeComponent();

            Data = new Day10();

            while (GetLogs().Max(log => log.Position.Item1) - GetLogs().Min(log => log.Position.Item1) > 90)
            {
                Tick(1);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            foreach (var log in GetLogs())
            {
                var dot = new Rectangle(log.Position.Item1, log.Position.Item2, 1, 1);
                g.DrawEllipse(Pens.Black, dot);
                g.FillEllipse(Brushes.Black, dot);

            }
        }

        List<Day10.Log> GetLogs()
        {
            return Data.Logs;
        }

        private void Tick(int delta)
        {
            sec += delta;
            GetLogs().ForEach(log => log.Tick(delta));

            Invalidate();

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Right)
            {
                Tick(1);
                Utils.Answer(10, 1, "Read it, press arrows till you can read!");
                Utils.Answer(10, 2, sec);
            }
            if (e.KeyCode == Keys.Left)
            {
                Tick(-1);
                Utils.Answer(10, 1, "Read it, press arrows till you can read!");
                Utils.Answer(10, 2, sec);
            }
        }
    }
}
