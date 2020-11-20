using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//COMMIT 1
namespace Corsa_di_BGW
{
    public partial class FrmMain : Form
    {
        Random r = new Random();
        const int arrivo = 648;
        const int inizio = 0;
        public FrmMain()
        {
            InitializeComponent();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {       
            this.Text = "Corsa di BGW";
            btnAvvia.Text = "START";
            btnReset.Text = "RESET";
            Display.Text ="vincitore:";
            pbBlack.Location = new Point(inizio, pbBlack.Location.Y);
            pbRed.Location = new Point(inizio, pbRed.Location.Y);

        }
        private void btnAvvia_Click(object sender, EventArgs e)
        {
            btnAvvia.Enabled = false;
            bgw1.RunWorkerAsync(pbBlack.Location.X);
            bgw2.RunWorkerAsync(pbRed.Location.X);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Display.Text = "Vincitore:";
            bgw1.CancelAsync();
            bgw2.CancelAsync();
            btnAvvia.Enabled = true;
            pbBlack.Location = new Point(inizio, pbBlack.Location.Y);
            pbRed.Location = new Point(inizio, pbRed.Location.Y);
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
           BackgroundWorker bw = sender as BackgroundWorker;
            int posizione = (int)e.Argument;

            while (posizione < arrivo)
            {
                if (bw.CancellationPending)
                    return;

                posizione += r.Next(1, 5);
                bw.ReportProgress(posizione);

                System.Threading.Thread.Sleep(10);
            }

            if (bw == bgw1)
                e.Result = "Nero";
            else
                e.Result = "Rosso";
        }

        private void Bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            int pos = (int)e.ProgressPercentage;

            if (bw == bgw1)
            {
                if (pos > arrivo)
                    pbBlack.Location = new Point(arrivo, pbBlack.Location.Y);
                else
                    pbBlack.Location = new Point(e.ProgressPercentage, pbBlack.Location.Y);
            }
            else
            {
                if (pos > arrivo)
                    pbRed.Location = new Point(arrivo, pbRed.Location.Y);
                else
                    pbRed.Location = new Point(e.ProgressPercentage, pbRed.Location.Y);
            }
        }

        private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            Display.Text += (string)e.Result;
            if (bw == bgw1)
                bgw2.CancelAsync();
            else
                bgw1.CancelAsync();
        }
    }
}
