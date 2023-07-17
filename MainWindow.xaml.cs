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
using Galileo6;

/// SatelliteDataProcessingProject
/// Mauriza Arianne
/// P252069
/// 17/07/2023

namespace SatelliteDataProcessingProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SatelliteDataProcessingWindow : Window
    {
        public SatelliteDataProcessingWindow()
        {
            InitializeComponent();
            LoadData();
        }

        /// 4.1	Create two data structures using the LinkedList<T> class from the C# Systems.Collections.
        /// Generic namespace. 
        /// The data must be of type “double”; you are not permitted to use any additional classes, nodes, pointers or data structures (array, list, etc) in the implementation of this application. The two LinkedLists of type double are to be declared as global within the “public partial class”.
        #region GlobalVariables

        private LinkedList<Double> sensorA = new LinkedList<Double>();
        private LinkedList<Double> sensorB = new LinkedList<Double>();

        #endregion

        #region Methods
        // 4.2	Copy the Galileo.DLL file into the root directory of your solution folder and add the appropriate reference in the solution explorer.
        // Create a method called “LoadData” which will populate both LinkedLists.
        // Declare an instance of the Galileo library in the method and create the appropriate loop construct to populate the two LinkedList; the data from Sensor A will populate the first LinkedList, while the data from Sensor B will populate the second LinkedList.
        // The LinkedList size will be hardcoded inside the method and must be equal to 400.
        // The input parameters are empty, and the return type is void.
        private void LoadData()
        {
            ReadData readData= new ReadData();

            /// add double mu
            /// add double sigma
            /// from input

            int maxPopSize = 400;

            for(int i = 0; i < maxPopSize; i++)
            {
                double dataA = readData.SensorA(10, 2.5);
                sensorA.AddLast(dataA);
                double dataB = readData.SensorB(10, 2.5);
                sensorB.AddLast(dataB);
            }
            //foreach(double data in  sensorA)
            //{
            //    exampleListBox.Items.Add(data);
            //}



        }

        #endregion
    }
}
