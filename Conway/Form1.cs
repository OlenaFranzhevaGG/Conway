﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;




namespace Conway
{

    public partial class Form1 : Form
    {
        public int K = 0; // field size
        int n1 = 53;
        int N1 = 0;
        // int[,] A = new int[50, 50];
        decimal[,] NewInitial;
        int count = 0;
        Function f = new Function();
        ControlSettings cs = new ControlSettings();
        private List<decimal[,]> Cell;
        private bool isFirstLaunch = true;
        public bool isControl = false;
        public Form1()
        {
            InitializeComponent();
            //funcSet.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //int k = Piecewise(1, 1);


        }
        public decimal[,] Life(decimal[,] ArrayStart)
        {
            //Algorithm of Cellular Automata Game 2D
            K = f.fieldSize;
            decimal[,] ret = new decimal[K, K];
            decimal[] inF = f.innerFunc();
            string funcType = f.MainFunction();
            decimal incode = 0;            
            
            decimal[] weightCoefficient = new decimal[inF.Length];

            for (int i = 0; i < inF.Length; i++)
                incode += inF[i];
            //double limitParameter = incode + 1;
            for (int i = 0; i < inF.Length; i++)
                weightCoefficient[i] = inF[i]/incode;


            for (int i = 0; i < K; i++)
            {
                for (int j = 0; j < K; j++)
                {
                    ret[i, j] = ArrayRecalculated(i, j, weightCoefficient, ArrayStart);
                }
            }
            return ret;
        }
        public double Piecewise(double x, int j, double limitParameter)
        {
            //Piecewise function for Tent map
            double piece = 0;
            if (j == 0)
            {
                if (x >= limitParameter-1 && x <= limitParameter)
                {
                    piece = x - limitParameter-1;
                }
                else if (x >= 0 && x <= 1)
                {
                    piece = 1 - x;
                }
            }
            else
            {
                if (x > (j - 1) && x < j)
                {
                    piece = x - j + 1;
                }
                else if (x >= j && x <= j + 1)
                {
                    piece = j - x + 1;
                }
            }
            return piece;
        }
        public decimal ArrayRecalculated(int i, int j, decimal [] innerCode, decimal[,] array)
        {
            K = f.fieldSize;
            return Func(
                        innerCode[0] * array[(i - 1) != -1 ? (i-1) : (K-1), (j - 1) != -1 ? (j-1) : (K-1)] +
                        innerCode[1] * array[(i - 1) != -1 ? (i - 1) : (K - 1), j] +
                        innerCode[2] * array[(i - 1) != -1 ? (i - 1) : (K - 1), (j + 1) !=K ? (j+1) : 0] +
                        innerCode[7] * array[i, (j - 1) != -1 ? (j - 1) : (K - 1)] +
                        innerCode[3] * array[i, (j + 1) != K ? (j + 1) : 0] +
                        innerCode[6] * array[(i + 1) != K ? (i+1) : 0, (j - 1) != -1 ? (j - 1) : (K - 1)] +
                        innerCode[5] * array[(i + 1) != K ? (i + 1) : 0, j] +
                        innerCode[4] * array[(i + 1) != K ? (i + 1) : 0, (j + 1) != K ? (j + 1) : 0] +
                        innerCode[8] * array[i, j]
                        );
        }
        public decimal Func(decimal x)
        {
            decimal a = 0; decimal incode = 0; string mainFunction = f.MainFunction();
            List<int> Param = f.GetFunc();
            decimal[] inf = f.innerFunc();
            
            //for (int i = 0; i < inf.Length; i++)
            //    incode += inf[i];
            decimal limitParameter = incode + 1;
            if (mainFunction == "Tent")
            {
                return PiecewiseNew(x);               
            }
            else
            {
                return Logistic(x);
            }


            //    Sum of Tent maps, that represents rules of Game
        }
        public decimal PiecewiseNew(decimal x)
        {
            decimal a = 4 * x * (1 - x);//(x >= 0 && x <= 0.5m) ? -16 * (x - 0.25m) * (x - 0.25m) + 1 : 0;// 4 * x * (1 - x);
            //1 - Math.Abs(2*x - 1);
            return a;
           
        }

        public decimal Logistic(decimal x)
        {
            decimal d = 17m;
            //dknw
            //if (x >= 5 / d && x <= 8 / d)
            //{
            //    return 1 - (Convert.ToDecimal(Math.Pow(17, 2)) / 4) * Convert.ToDecimal(Math.Pow(Convert.ToDouble(x - 6 / d), 2));//17 * x - 4;
            //}


            if (x >= 4 / d && x < 5 / d)
            {
                return 17 * x - 4;
            }
            else if (x >= 5 / d && x <= 7 / d)
            {
                return 1;
            }
            else if (x > 7 / d && x <= 8 / d)
            {
                return -17 * x + 8;
            }
            else
            {
                return 0;
            }




            //else
            //if (x >= 5/d && x <= 7/d)
            //{
            //    return 1;
            //}
            //else if (x > 7 / d && x <= 8 / d)
            //{
            //    return -17 * x + 8;
            //}
           
           
        }
       
        public decimal[,,] SetInitial()
        {
            K = f.fieldSize;
            decimal[,,] SetInit = new decimal[K, K, n1];

            decimal[,] b = new decimal[K, K];
            Random rand = new Random();
            int x, x1, x2, y, y1, y2;
            for (int i = 0; i < K * 10; i++)
            {
                x1 = rand.Next(K);
                x2 = rand.Next(K);
                x = (x1 + x2) / 2;
                y1 = rand.Next(K);
                y2 = rand.Next(K);
                y = (y1 + y2) / 2;
                SetInit[x, y, 0] = 1.0m;//Convert.ToDecimal(rand.Next(100)) / 100;

            }
            // планер в игре Жизнь
            SetInit[0, 1, 0] = 1;
            SetInit[1, 2, 0] = 1;
            SetInit[2, 0, 0] = 1;
            SetInit[2, 1, 0] = 1;
            SetInit[2, 2, 0] = 1;


            //decimal da = 3;
            //SetInit[0, 0, 0] = 2/da;
            //SetInit[1, 2, 0] = 1;
            //SetInit[2, 0, 0] = 0.9m;
            //SetInit[2, 1, 0] = 0.54m;
            //SetInit[2, 2, 0] = 0.63m;

            //SetInit[1, 0, 0] = 0.5m;//0.8m;
            //SetInit[3, 2, 0] = 0.77m;
            //SetInit[9, 0, 0] = 0.36m;
            //SetInit[2, 7, 0] = 1;
            //SetInit[4, 2, 0] = 0.2m;

            //decimal a = 0.8m;
            //SetInit[1, 1, 0] = a;
            //SetInit[1, 2, 0] = 1;
            //SetInit[2, 0, 0] = 1;
            //SetInit[2, 1, 0] = 1;
            //SetInit[2, 2, 0] = 1;
            //welcome



            for (int i = 0; i < K; i++)
                for (int j = 0; j < K; j++)
                {
                    b[i, j] = SetInit[i, j, 0];
                }

            Print(b);

            return SetInit;
        }
        //public void CAwork(double[,,] CA)
        //{
        //    double[,] h = new double[K, K];
        //    double[,] T = new double[K, K];
        //    for (int p = 0; p < n1 - 1; p++)
        //    {

        //        double[,] b = new double[K, K];
        //        for (int i = 0; i < K; i++)
        //            for (int j = 0; j < K; j++)
        //            {
        //                b[i, j] = CA[i, j, p];
        //            }
        //        h = Life(b);

        //        Print(h);
        //        for (int i = 0; i < K; i++)
        //            for (int j = 0; j < K; j++)
        //            {
        //                CA[i, j, p + 1] = h[i, j];

        //            }
        //        T = Temporary(h);
        //    }

        //    NewInitial = T;
        //}
        //double[,] Temporary(double[,] c)
        //{
        //    double[,] temp;

        //    temp = new double[K, K];
        //    for (int i = 0; i < K; i++)
        //        for (int j = 0; j < K; j++)
        //        {
        //            temp[i, j] = (c[i, j]);
        //        }
        //    return temp;

        //}

        private void Print(decimal[,] A)
        {
            CreateBitmapAtRuntime(A);
        }

        PictureBox pictureBox1 = new PictureBox();
        public void CreateBitmapAtRuntime(decimal[,] A)
        {
            K = f.fieldSize;
            var scale = f.scale;
            pictureBox1.Size = new Size(K * scale + 10, K * scale + 10);
            this.Controls.Add(pictureBox1);

            Bitmap myAutomataField = new Bitmap(K * scale, K * scale);
            Graphics flagGraphics = Graphics.FromImage(myAutomataField);            
            for (int i = 0; i < K; i++)
            {
                for (int j = 0; j < K; j++)
                {
                    int col = Convert.ToInt32(Math.Abs(1 - A[j, i]) * 255);
                    flagGraphics.FillRectangle(new SolidBrush(Color.FromArgb(col, col, col)), i * scale, j * scale, scale, scale);
                }
            }
            
            pictureBox1.Image = myAutomataField;
        }
        private void funcSet_Click(object sender, EventArgs e)
        {
            startTimerButton.Enabled = true;
            f.Show();
        }      

        private void timer1_Tick(object sender, EventArgs e)
        {
            K = f.fieldSize;
            decimal[,] h = new decimal[K, K];
            decimal[,] b = new decimal[K, K];
            // decimal[,] bc = new decimal[K, K];
            //decimal div = 4.0m;
            //decimal gamma = 0.66m;
            //decimal a1 = isControl ? 0.556m : 1; decimal a2 = isControl ? 0.333m : 0; decimal a3 = isControl ? 0.111m: 0;
            //decimal b1 = isControl ? 2 / div : 1; decimal b2 = isControl ? 2 / div : 0;
            for (int i = 0; i < K; i++)
                for (int j = 0; j < K; j++)
                {
                    b[i, j] = Cell[N1][i, j];
                }
            if (isControl && N1>3)
            {
                b = PredicativeControl(b);
            }
            
            //for (int i = 0; i < K; i++)
            //for (int j = 0; j < K; j++)
            //{
            //   if (N1 > 3)
            //   {                  
            //      // bc[i, j] = b1 * Cell[N1-1][i, j] + b2 * Cell[N1 - 3][i, j];
            //   }
            //   else
            //   {
            //       b[i, j] = Cell[N1][i, j];
            //   }
            //}
            //if (isControl)
            //{
            //    var b_temp = Life(b);
            //    var bc_temp = Life(bc);
            //    for (int i = 0; i < K; i++)
            //        for (int j = 0; j < K; j++)
            //        {
            //            h[i,j] = (1 - gamma) * b_temp[i, j] + gamma * bc_temp[i, j];
            //        }
            //}
            //else
            //{
           
            h = Life(b);
                        
            Print(h);

            Cell.Add(h);               
            N1 += 1;
        }
        public decimal[,] PredicativeControl(decimal[,] Xn)
        {
            //let's cycle equals 2 T=2
            int CAsize = f.fieldSize;
            int Tcycle = 1; decimal div = 3.0m; decimal Epsilon = 0.000000000001m;
            decimal Tetta = 8.0m;//1 / div * (Convert.ToDecimal(Math.Pow(2, Convert.ToDouble(Tcycle + 1))) - 1) + Epsilon; //4.0m;
            decimal a1 = Tetta / (1 + Tetta); decimal a2 = 1 / (1 + Tetta);
            decimal[,] Xn_predicative = new decimal[CAsize,CAsize];
            decimal[,] Xn_predicativeTemporary = Xn;
            decimal[,] Xn_controled = new decimal[CAsize, CAsize];

            //Calculating for predicative part of control
            for (int p = 0; p < Tcycle; p++)
            {
                Xn_predicative = Life(Xn_predicativeTemporary);
                Xn_predicativeTemporary = Xn_predicative;                              
            }
            //calculating control body for main function
            for (int i = 0; i < CAsize; i++)
                for (int j = 0; j < CAsize; j++)
                {                   
                   Xn_controled[i, j] = a1 * Xn[i, j] + a2 * Xn_predicative[i, j];                  
                }
            return Xn_controled;
        }

        //Practical implementation todo:
        public decimal PictureCoder(decimal X0)
        {
            decimal alfa = 2 - 1 / X0;
            if (X0 >= 0 && X0 < alfa)
                return X0 / alfa;
            else if (X0 >= alfa && X0 <= 1)
                return (1 - X0) / (1 - alfa);
            else
                return 0;
        }
        private void startTimerButton_Click(object sender, EventArgs e)
        {
            K = f.fieldSize;
            if (isFirstLaunch)
            {
                Cell = new List<decimal[,]>();
                var temp = SetInitial();
                decimal[,] temp2 = new decimal[K, K];
                for (int i = 0; i < K; i++)
                {
                    for (int j = 0; j < K; j++)
                    {
                        temp2[i, j] = temp[i, j, 0];
                    }
                }
                Cell.Add(temp2);
                N1 = 0;
                timer1.Enabled = true;
                isFirstLaunch = false;
            }

            timer1.Enabled = true;

        }

        private void stopTimer_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void Control_btn_Click(object sender, EventArgs e)
        {
            isControl = isControl ? false : true;
            //cs.Show();
        }
    }
}
