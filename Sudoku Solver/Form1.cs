using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sudoku_Solver;
using System.Threading;

namespace Sudoku_Solver
{
    public partial class Form1 : Form
    {
        public int countSkips = 0;
        public int skipMax = 1;
        public static int skipSkipMaxMax;
        public static int skipMaxReset;
        public static double factor = 12.5;

        public static Color fontColor = Color.Black;
        public static Color backColor = Color.White;
        public static Color newColor = Color.Tomato;
        public static Color changeColor = Color.Green;
        public static Color resetColor = Color.OrangeRed;

        public static int gridSize = 9;
        public static int gridSizeMinus = gridSize - 1;
        int[,] matriceInput = new int[gridSize, gridSize];
        int[,] matriceOutput = new int[gridSize, gridSize];


        public Form1()
        {
            InitializeComponent();

            checkBox1.Checked = false;

            //trackBar1.Visible = false;
            //label1.Visible = false;
            //label2.Visible = false;
            label3.Visible = true;
            label3.Text = "";

            trackBar1.Minimum = 1;
            trackBar1.Maximum = 5;
            trackBar1.Value = 3;
            label1.Text = trackBar1.Value.ToString();
            setSpeedAnimation();
        }

        private void setSpeedAnimation()
        {
            countSkips = 0;
            skipMax = 1;
            double doubleMax = trackBar1.Value * factor;
            skipSkipMaxMax = int.Parse(Math.Floor(doubleMax).ToString());
            skipMaxReset = Math.Max(1, skipSkipMaxMax - 15);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value >= 10)
            {
                label1.Text = trackBar1.Value + " ";
            }
            else
            {
                label1.Text = trackBar1.Value.ToString();
            }

            setSpeedAnimation();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //tableLayoutPanel1.GetControlFromPosition(5, 5).Text = "1";

            for (int a = 0; a <= 8; a++)
            {
                for (int b = 0; b <= 8; b++)
                {
                    matriceOutput[a, b] = b + 1;
                }
            }
        }

        private void solveSudoku(object sender, EventArgs e)
        {

            bool resetCounter = true;
            bool quitLoop = false;
            setSpeedAnimation();
            for (int x = 0; x <= 8; x++)
            {
                for (int y = 0; y <= 8; y++)
                {
                    try
                    {
                        string strValue = tableLayoutPanel1.GetControlFromPosition(x, y).Text;

                        //tableLayoutPanel1.GetControlFromPosition(x, y).Enabled = false;

                        if (strValue == "")
                        {
                            matriceInput[x, y] = 0;
                        }
                        else
                        {
                            colorCase(x, y, newColor);
                            matriceInput[x, y] = int.Parse(strValue);
                        }

                        if (matriceInput[x, y] < 0 || matriceInput[x, y] >= 10)
                        {
                            MessageBox.Show("You can only write whole numbers between 1 and 9 in Sudoku (tile :" + (x + 1) + ", " + (y + 1) + ")", "Error");
                            quitLoop = true;
                            break;
                        }
                    }

                    catch (FormatException)
                    {
                        MessageBox.Show("You can't write a number under 1 or over 9 nor letters in Sudoku (tile :" + (x + 1) + ", " + (y + 1) + ")", "Error");
                        quitLoop = true;
                        break;
                    }

                }

                if (quitLoop) { quitLoop = false; break; }
            }
            var sudoku = new SudokuClass();

            bool fullGraphicVersion = checkBox1.Checked;

            if (sudoku.Sudoku(matriceInput, showGrid, fullGraphicVersion, resetCounter))
            {
                matriceOutput = sudoku.getGrid();
                showGrid(matriceOutput, -1, -1);
                label3.Text =  "The method was called "+ sudoku.getCounter().ToString()+ " times";
            }
            else
            {
                MessageBox.Show("The grid is not correct", "Error");
            }

            //for (int x = 0; x <= 8; x++)
            //{
            //    for (int y = 0; y <= 8; y++)
            //    {
            //        MessageBox.Show("(tile :" + (x+1) + ", " + (y+1)+ ") : "+ matriceInput[x, y].ToString());
            //    }
            //}

        }

        public void showGrid(int[,] matriceOutput, int x, int y)
        {
            //int x = matriceOutput[gridSizeMinus, 0];
            //int y = matriceOutput[0, gridSizeMinus];

            //MessageBox.Show(matriceOutput[5,5].ToString(), "Test");

            countSkips++;
            if (countSkips == skipMax && x!= -1)
            {
                //label3.Text = skipMax.ToString();
                //label3.Refresh();
                skipMax++;
                if(skipMax == skipSkipMaxMax) { skipMax = skipMaxReset; }

                countSkips = 0;

                if (tableLayoutPanel1.GetControlFromPosition(x,y).Text != "")
                {
                    colorCase(x, y, changeColor);
                    deleteFrowardTiles(x, y);
                }

                tableLayoutPanel1.GetControlFromPosition(x, y).Text = matriceOutput[x, y].ToString();
                tableLayoutPanel1.Refresh();
                x = -1;
            }

            if (x == -1)
            {
                for (int a = 0; a <= 8; a++)
                {
                    for (int b = 0; b <= 8; b++)
                    {
                        if (matriceOutput[a, b] != 0)
                        {
                            if (tableLayoutPanel1.GetControlFromPosition(a,b).BackColor != newColor)
                            {
                                tableLayoutPanel1.GetControlFromPosition(a, b).BackColor = backColor;
                            }
                            tableLayoutPanel1.GetControlFromPosition(a, b).Text = matriceOutput[a, b].ToString();
                        }
                    }
                }
                tableLayoutPanel1.Refresh();
            }

        }

        private void deleteFrowardTiles(int x, int y)
        {
            for (int a = x; a <= gridSizeMinus; a++)
            {
                for (int b = y; b <= gridSizeMinus; b++)
                {
                    if(tableLayoutPanel1.GetControlFromPosition(a, b).BackColor != newColor)
                    {
                        tableLayoutPanel1.GetControlFromPosition(a, b).Text = "";
                    }
                }
            }
        }

        private void gridReset(object sender, EventArgs e)
        {
            int minValue = 0;
            int maxValue = 8;
            bool all = true;

            for (int a = 0; a <= 5; a++)
            {
                resetRectangle(minValue + a, maxValue - a, all, resetColor);
            }
            for (int a = 0; a <= 5; a++)
            {
                resetRectangle(minValue + a, maxValue - a, all, backColor);
            }


            /*for (int a = 0; a <= 8; a++)
            {
                for (int b = 0; b <= 8; b++)
                {
                    tableLayoutPanel1.GetControlFromPosition(a, b).Text = "";
                }
            }*/
        }

        private void resetRectangle(int minValue, int maxValue, bool all, Color specialColor)
        {
            for (int b = minValue; b <= maxValue; b++)
            {
                if (all)
                {
                    tableLayoutPanel1.GetControlFromPosition(minValue, b).Text = "";
                    colorCase(minValue, b, specialColor);
                }
                else if (tableLayoutPanel1.GetControlFromPosition(minValue, b).BackColor != newColor)
                {
                    tableLayoutPanel1.GetControlFromPosition(minValue, b).Text = "";
                    colorCase(minValue, b, specialColor);
                }
            }

            for (int c = minValue; c <= maxValue; c++)
            {
                if (all)
                {
                    tableLayoutPanel1.GetControlFromPosition(c, minValue).Text = "";
                    colorCase(c, minValue, specialColor);
                }
                else if (tableLayoutPanel1.GetControlFromPosition(c, minValue).BackColor != newColor)
                {
                    tableLayoutPanel1.GetControlFromPosition(c, minValue).Text = "";
                    colorCase(c, minValue, specialColor);
                }
            }

            for (int w = minValue; w <= maxValue; w++)
            {
                if (all)
                {
                    tableLayoutPanel1.GetControlFromPosition(maxValue, w).Text = "";
                    colorCase(maxValue, w, specialColor);
                }
                else if (tableLayoutPanel1.GetControlFromPosition(maxValue, w).BackColor != newColor)
                {
                    tableLayoutPanel1.GetControlFromPosition(maxValue, w).Text = "";
                    colorCase(maxValue, w, specialColor);
                }
            }

            for (int x = minValue; x <= maxValue; x++)
            {
                if (all)
                {
                    tableLayoutPanel1.GetControlFromPosition(x, maxValue).Text = "";
                    colorCase(x, maxValue, specialColor);
                }
                else if (tableLayoutPanel1.GetControlFromPosition(x, maxValue).BackColor != newColor)
                {
                    tableLayoutPanel1.GetControlFromPosition(x, maxValue).Text = "";
                    colorCase(x, maxValue, specialColor);
                }
            }
            tableLayoutPanel1.Refresh();
        }

        private void colorCase(int x, int y, Color specialColor)
        {
            tableLayoutPanel1.GetControlFromPosition(x, y).BackColor = specialColor;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int minValue = 0;
            int maxValue = 8;
            bool all = false;

            for (int a = 0; a <= 5; a++)
            {
                resetRectangle(minValue + a, maxValue - a, all, resetColor);
            }
            for (int a = 0; a <= 5; a++)
            {
                resetRectangle(minValue + a, maxValue - a, all, backColor);
            }

        }
    }
}
