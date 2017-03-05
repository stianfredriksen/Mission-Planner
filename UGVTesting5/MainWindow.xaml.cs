using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UGVTesting5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UGV ugv;
        public MainWindow()
        {
            ugv = new UGV();
            InitializeComponent();
            txtFuel.Text = ugv.Fuel + "%";
        }
        private bool returnToBaseIsClicked = false;
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            monitorBoxes();
            initiateStart();
        }

        private void btnCheckSystems_Click(object sender, RoutedEventArgs e)
        {
            checkSystem();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            returnToBASE();
        }

        private void btnStatistics_Click(object sender, RoutedEventArgs e)
        {
            showTotalStats();
        }


        public void initiateStart()
        {
            txtInfo.Text = "";
            txtCoord.Visibility = System.Windows.Visibility.Visible;
            txtCoord.Text = "";
            btnCoord.Visibility = System.Windows.Visibility.Visible;
            txtInfo.Visibility = System.Windows.Visibility.Collapsed;
            txtCoord.IsEnabled = true;

            // buttons hidden
            //checkSystems.Visibility = System.Windows.Visibility.Hidden;
            //totalStats.Visibility = System.Windows.Visibility.Hidden;
            //returnToBase.Visibility = System.Windows.Visibility.Hidden;

        }

        public async void checkSystem()
        {
            imgFuel.Visibility = System.Windows.Visibility.Hidden;
            //Uncomment comments when buttons added
            //initiateSystems.Visibility = System.Windows.Visibility.Hidden;
            //totalStats.Visibility = System.Windows.Visibility.Hidden;
            //returnToBase.Visibility = System.Windows.Visibility.Hidden;
            txtInfo.Text = "Initiating system check... \n";
            await Task.Delay(3000);
            txtInfo.Text += "Checking Engine... \n";
            await Task.Delay(3000);
            txtInfo.Text += "Checking Sensors... \n";
            await Task.Delay(3000);
            if (ugv.Fuel <= 75)
            {
                string messageBoxText = " Fuel low! ";
                MessageBoxButton button = MessageBoxButton.YesNo;

                // MessageBox.Show("UGV is low on Fuel. Would you like to refuel? ", messageBoxText, button);
                MessageBoxResult result = MessageBox.Show("UGV is low on Fuel, only " + ugv.Fuel + " % left. Would you like to refuel?", messageBoxText, button);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        refuel();
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Not refueling, returning to system");
                        break;

                }
                txtInfo.Text += "Fuel level: " + ugv.Fuel + "% \n";
            }
            //  infoBox.Text += "UGV fuel level low, "  + ugv.Fuel + " do you want to refuel? \n"; }

            else { txtInfo.Text += "Checking Fuel... " + " Fuel level: " + ugv.Fuel + "% \n"; }
            await Task.Delay(1000);
            txtInfo.Text += "Checking system status... \n";
            await Task.Delay(2000);
            txtInfo.Text += "All systems OK! \n";

            //Uncommet these codes when buttons added. 
            //initiateSystems.Visibility = System.Windows.Visibility.Visible;
            //totalStats.Visibility = System.Windows.Visibility.Visible;
            //returnToBase.Visibility = System.Windows.Visibility.Visible;
        }

        public void refuel()
        {
            ugv.Fuel = 100;
            txtFuel.Text = ugv.Fuel + "%";
        }

        public void lastTripReport()
        {
            txtInfo.Text = "Report for last trip \n";
            //txtInfo.Text += "Fuel consumption: " + (100 - ugv.Fuel) + "% \n";
            txtInfo.Text += "Mines Found: " + ugv.MinesFound + "\n";
            txtInfo.Text += "Distance traveled " + ugv.Distance + " km \n";
        }

        public async void returnToBASE()
        {
            // Hide buttons
            //checkSystems.Visibility = System.Windows.Visibility.Hidden;
            //totalStats.Visibility = System.Windows.Visibility.Hidden;
            //initiateSystems.Visibility = System.Windows.Visibility.Hidden;


            returnToBaseIsClicked = true;
            txtInfo.Text = "UGV is Returning to base";
            await Task.Delay(3000);
            lastTripReport();
            //await Task.Delay(5000);
            
            refuel();
            await Task.Delay(2000);
            txtInfo.Text += "UGV is now at Base" + " refueling.. \n";
            await Task.Delay(3000);
            txtInfo.Text += "All Systems Ready\n" + "Fuel level: " + ugv.Fuel + "%";
            returnToBaseIsClicked = false;
            //   lastTripReport();
            btnStart.IsEnabled = true;
            btnStatistics.Visibility = System.Windows.Visibility.Visible;
            btnCheckSystems.Visibility = System.Windows.Visibility.Visible;

            imgMap.Visibility = System.Windows.Visibility.Hidden;
            resetStats();

        }


        public async void contiuneStart()
        {
            txtInfo.Visibility = System.Windows.Visibility.Visible;
            txtCoord.Visibility = System.Windows.Visibility.Hidden;
            btnCoord.Visibility = System.Windows.Visibility.Hidden;
            //label.Visibility = System.Windows.Visibility.Hidden;
            //returnToBase.Visibility = System.Windows.Visibility.Visible;

            

            txtInfo.Text = "Initiating startup.";
            await Task.Delay(2000);
            txtInfo.Text = "Recieved coordinates is: " + ugv.Coords + " \n";
            await Task.Delay(3000);
            txtInfo.Text += "Navigating to target...";
            for (int k = 0; k < 3; k++)
            {
                // Break mechanism for loop
                if (returnToBaseIsClicked)
                {
                    break;
                }

                await Task.Delay(2000);
                txtInfo.Text = "Navigating to target... \n";
                txtInfo.Text += "Fuel level: " + ugv.Fuel + " % \n" + "Distance: " + ugv.Distance + "km";
                txtFuel.Text = ugv.Fuel + "%";
                txtDist.Text = ugv.Distance + "km";
                ugv.Fuel -= 1;
                ugv.Distance += 0.33;
            }
            await Task.Delay(6200);
            txtInfo.Text = "Target reached, starting to demine";
            await Task.Delay(5000);
            demine();

        }


        public async void demine()
        {
            imgMap.Visibility = System.Windows.Visibility.Visible;
            btnCheckSystems.Visibility = System.Windows.Visibility.Hidden;
            btnStatistics.Visibility = System.Windows.Visibility.Hidden;
            // returnToBase.Visibility = System.Windows.Visibility.Hidden;


            txtInfo.Text = "Initiating Systems, please wait..";
            await Task.Delay(1000);
            Random rnd = new Random();
            btnStart.IsEnabled = false;
            // infoBox.Text = "All Systems initiated";
            txtInfo.Text = "UGV is running mission";
            for (int i = ugv.Fuel; i > 15; i--)
            {
                
                if (ugv.Fuel == 20) { txtInfo.Text = " Fuel critically low, returning to base"; await Task.Delay(2000); returnToBASE(); }
                int tmp = rnd.Next(1, 3);
                if (returnToBaseIsClicked)
                {
                    break;
                }

                if (i <= 25)
                {
                    if (tmp == 2) { ugv.MinesFound++; }
                    txtInfo.Text = "UGV is running low on fuel, please return to base. \n";
                    txtInfo.Text += "Fuel level: " + i + " % \n";
                    txtFuel.Text = ugv.Fuel + "%";
                    txtInfo.Text += "Distance: " + ugv.Distance + " km";
                    txtDist.Text = ugv.Distance + " km";
                    await Task.Delay(3000);
                    ugv.Fuel--;
                    ugv.Distance = ugv.Distance + 1.66;
                    txtMines.Text = ugv.MinesFound.ToString();
                    //minesFound.Text = "Mines found by UGV: " + ugv.MinesFound.ToString();
                }
                else {
                    if (tmp == 2) { ugv.MinesFound++; }
                    txtInfo.Text = "UGV is running mission. \n";
                    //txtInfo.Text += "Fuel level: " + i + " % \n";
                    txtFuel.Text = ugv.Fuel + "%";
                    //txtInfo.Text += "Distance: " + ugv.Distance + " km";
                    txtDist.Text = ugv.Distance + "km";
                    await Task.Delay(3000);
                    ugv.Fuel--;
                    ugv.Distance = ugv.Distance + 1.66;
                    txtMines.Text = ugv.MinesFound.ToString();
                    //minesFound.Text = "Mines found by UGV: " + ugv.MinesFound.ToString();
                }
            }
        }

        private void btnCoord_Click(object sender, RoutedEventArgs e)
        {
            sendCoordinates();
        }

        public void sendCoordinates()
        {
           
            ugv.Coords = txtCoord.Text;
            contiuneStart();
        }

        public void resetStats()
        {
            ugv.MinesFoundTot += ugv.MinesFound;
            ugv.MinesFound = 0;
            txtMines.Text = ugv.MinesFound.ToString() ;

            ugv.DistanceTot += ugv.Distance;
            ugv.Distance = 0;
            txtDist.Text = ugv.Distance + " km";

            //ugv.TotalFuel += ugv.Fuel;

        }

        public void showTotalStats()
        {
            imgFuel.Visibility = System.Windows.Visibility.Visible;
            txtFuel.Visibility = System.Windows.Visibility.Hidden;
            txtMines.Visibility = System.Windows.Visibility.Hidden;
            txtDist.Visibility = System.Windows.Visibility.Hidden;

            labFuel.Visibility = System.Windows.Visibility.Hidden;
            labMines.Visibility = System.Windows.Visibility.Hidden;
            labDist.Visibility = System.Windows.Visibility.Hidden;

            txtInfo.Text = "Total mines found: " + ugv.MinesFoundTot + " \n";
            txtInfo.Text += "Total distance for ugv: " + ugv.DistanceTot + " km  \n";
            //txtInfo.Text += "Total fuel consumption: " + ugv.TotalFuel + " litres \n";

        }

        public void monitorBoxes()
        {
            imgFuel.Visibility = System.Windows.Visibility.Hidden;
            txtFuel.Visibility = System.Windows.Visibility.Visible;
            txtMines.Visibility = System.Windows.Visibility.Visible;
            txtDist.Visibility = System.Windows.Visibility.Visible;

            labFuel.Visibility = System.Windows.Visibility.Visible;
            labMines.Visibility = System.Windows.Visibility.Visible;
            labDist.Visibility = System.Windows.Visibility.Visible;
        }

     
    }

    class UGV
    {
        private int fuel;
        private double distance;
        private int minesFound;
        private int minesFoundTot;
        private double distanceTot;
        private double totalFuel;
        private string coords;

        public UGV()
        {
            this.fuel = 70;
            this.distance = 0;
            this.minesFound = 0;
            this.minesFoundTot = 0;
            this.distanceTot = 0;
            this.totalFuel = 0;
            this.coords = "";

        }

        public int Fuel
        {
            get { return fuel; }
            set { fuel = value; }
        }

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public int MinesFound
        {
            get { return minesFound; }
            set { minesFound = value; }
        }

        public int MinesFoundTot
        {
            get { return minesFoundTot; }
            set { minesFoundTot = value; }
        }
        public double DistanceTot
        {
            get { return distanceTot; }
            set { distanceTot = value; }
        }
        public double TotalFuel
        {
            get { return totalFuel; }
            set { totalFuel = value; }
        }

        public string Coords
        {
            get { return coords; }
            set { coords = value; }
        }

    }
}


