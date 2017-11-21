using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;

namespace asgn6 {
	
    /// <summary>
	/// Summary description for Transformer.
	/// </summary>
	public class Transformer : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		//private bool GetNewData();

		// basic data for Transformer

		int numpts = 0;
		int numlines = 0;
		bool gooddata = false;		
		double[,] vertices;
		double[,] scrnpts;
		double[,] ctrans = new double[4,4];  //your main transformation matrix
		private System.Windows.Forms.ImageList tbimages;
		int[,] lines;

		public Transformer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			Text = "COMP 4560:  Assignment 6 (20171121) (Shawn Kim)";
			ResizeRedraw = true;
			BackColor = Color.Black;
					
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Transformer));
            this.tbimages = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // tbimages
            // 
            this.tbimages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("tbimages.ImageStream")));
            this.tbimages.TransparentColor = System.Drawing.Color.Transparent;
            this.tbimages.Images.SetKeyName(0, "");
            this.tbimages.Images.SetKeyName(1, "");
            this.tbimages.Images.SetKeyName(2, "");
            this.tbimages.Images.SetKeyName(3, "");
            this.tbimages.Images.SetKeyName(4, "");
            this.tbimages.Images.SetKeyName(5, "");
            this.tbimages.Images.SetKeyName(6, "");
            this.tbimages.Images.SetKeyName(7, "");
            this.tbimages.Images.SetKeyName(8, "");
            this.tbimages.Images.SetKeyName(9, "");
            this.tbimages.Images.SetKeyName(10, "");
            this.tbimages.Images.SetKeyName(11, "");
            this.tbimages.Images.SetKeyName(12, "");
            this.tbimages.Images.SetKeyName(13, "");
            this.tbimages.Images.SetKeyName(14, "");
            this.tbimages.Images.SetKeyName(15, "");
            // 
            // Transformer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(10, 24);
            this.ClientSize = new System.Drawing.Size(508, 306);
            this.Name = "Transformer";
            this.Text = "7";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Transformer_Load);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Transformer());
		}

		protected override void OnPaint(PaintEventArgs pea) {
			Graphics grfx = pea.Graphics;

            Pen pen = new Pen(Color.Cyan, 3);
			double temp;
			int k;
            double height = grfx.ClipBounds.Height;
            double width = grfx.ClipBounds.Width;
            double screenCenterY = height / 2;
            double screenCenterX = width / 2;

            double minY = 0, maxY = 0;

            for (int row = 0; row < numpts; row++)
            {
                if (vertices[row, 1] < minY)
                {
                    minY = vertices[row, 1];
                }

                if (vertices[row, 1] > maxY)
                {
                    maxY = vertices[row, 1];
                }
            }

            double sFx = height / (maxY - minY) / 2;
            double sFy = height / (maxY - minY) / 2;
            double sFz = height / (maxY - minY) / 2;

            double[,] t1 = new double[,] {
                        { 1, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 0, 1, 0 },
                        { -vertices[0,0], -vertices[0,1], -vertices[0,2], 1}
            };

            double[,] s1 = new double[,] {
                        { sFx, 0, 0, 0 },
                        { 0, -sFy, 0, 0 },
                        { 0, 0, sFz, 0 },
                        { 0, 0, 0, 1}
            };

            double[,] t2 = new double[,] {
                        { 1, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 0, 1, 0 },
                        { screenCenterX, screenCenterY, 0, 1}
            };

            double[,] tnet = multMatrics(t1, s1);
            tnet = multMatrics(tnet, t2);

            ctrans = tnet;
                
            // scrnpts = vertices*ctrans
            for (int i = 0; i < numpts; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    temp = 0.0d;

                    for (k = 0; k < 4; k++)
                        temp += vertices[i, k] * ctrans[k, j];

                    scrnpts[i, j] = temp;

                }
            }

            for (int i = 0; i < numlines; i++)
            {
                var x1 = (int)scrnpts[lines[i, 0], 0];
                var y1 = (int)scrnpts[lines[i, 0], 1];
                var x2 = (int)scrnpts[lines[i, 1], 0];
                var y2 = (int)scrnpts[lines[i, 1], 1];


                if (i==0)
                {
                    Console.WriteLine(x1);
                    Console.WriteLine(y1);
                }

                grfx.DrawLine(pen, x1, y1, x2, y2);
            }
            
		} // end of OnPaint
    
        
        public double[,] multMatrics(double[,] a, double[,] b)
        {
            double[,] result = new double[a.GetLength(0),b.GetLength(0)];
            double temp;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    temp = 0.0d;

                    for (int k = 0; k < 4; k++)
                        temp += a[row, k] * b[k, col];

                    result[row, col] = temp;

                }
            }

            return result;
        }


		
	}

	
}
