using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Xceed.Wpf.Toolkit;
using Galileo6;
using System.Text.RegularExpressions;

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
            DisableButtons();
		}


        #region GlobalVariables
        /// 4.1	Create two data structures using the LinkedList<T> class from the C# Systems.Collections.
        /// Generic namespace. 
        /// The data must be of type “double”; you are not permitted to use any additional classes, nodes, pointers or data structures (array, list, etc) in the implementation of this application. The two LinkedLists of type double are to be declared as global within the “public partial class”.
        private LinkedList<Double> sensorA = new LinkedList<Double>();
		private LinkedList<Double> sensorB = new LinkedList<Double>();

		#endregion

		#region Methods
		// 4.2	Copy the Galileo.DLL file into the root directory of your solution folder and add the appropriate reference in the solution explorer.
		// Create a method called “LoadData” which will populate both LinkedLists.
		// Declare an instance of the Galileo library in the method and create the appropriate loop construct to populate the two LinkedList;
		// the data from Sensor A will populate the first LinkedList, while the data from Sensor B will populate the second LinkedList.
		// The LinkedList size will be hardcoded inside the method and must be equal to 400.
		// The input parameters are empty, and the return type is void.
		private void LoadData()
		{
            // Create new instance
			ReadData readData= new ReadData();
            // Convert to double
			double mu = Convert.ToDouble(nsMu.Value);
			double sigma = Convert.ToDouble(nsSigma.Value);
            // Set size
			int maxPopSize = 400;
            // Loop through then populate
			for(int i = 0; i < maxPopSize; i++)
			{
				double dataA = readData.SensorA(mu, sigma);
				sensorA.AddFirst(dataA);
				double dataB = readData.SensorB(mu, sigma);
				sensorB.AddFirst(dataB);
			}
		}
		// 4.3	Create a custom method called “ShowAllSensorData” which will display both LinkedLists in a ListView.
		// Add column titles “Sensor A” and “Sensor B” to the ListView.
		// The input parameters are empty, and the return type is void.
		private void ShowAllSensorData()
		{
            // Create template
			var myObservableCollection = new ObservableCollection<object>();
            // Loop through and add to template
			for (int i = 0; i < 400; i++)
			{
				var itemA = sensorA.ElementAt(i);
				var itemB = sensorB.ElementAt(i);
				myObservableCollection.Add(new { Value1 = itemA, Value2 = itemB });
			}
            // Bind to LV
			this.lvSensorData.ItemsSource = myObservableCollection;
		}
		// 4.4	Create a button and associated click method that will call the LoadData and ShowAllSensorData methods.
		// The input parameters are empty, and the return type is void.
		private void btnLoadSensorData_Click(object sender, RoutedEventArgs e)
		{
            // Call when clicked
			LoadData();
			ShowAllSensorData();
		}
		#endregion

		#region UtilityMethods
		// 4.5	Create a method called “NumberOfNodes” that will return an integer which is the number of nodes(elements) in a LinkedList.
		// The method signature will have an input parameter of type LinkedList, and the calling code argument is the linkedlist name.
		private int NumberOfNodes(LinkedList<Double> selectedList)
		{
            // Count
			return selectedList.Count();
		}
		// 4.6	Create a method called “DisplayListboxData” that will display the content of a LinkedList inside the appropriate ListBox.
		// The method signature will have two input parameters; a LinkedList, and the ListBox name.
		// The calling code argument is the linkedlist name and the listbox name.
		private void DisplayListboxData (LinkedList<Double> selectedList, ListBox selectedListBox)
		{
            // Clear lb then add from LinkedList
			selectedListBox.Items.Clear();
			foreach (var items in selectedList)
			{
				selectedListBox.Items.Add(items);
			}
		}
        // Buttons
        #region Buttons
        private void DisableButtons()
        {
            btnBinarySearchIterativeA.IsEnabled = false;
            btnBinarySearchRecursiveA.IsEnabled = false;

            btnBinarySearchIterativeB.IsEnabled = false;
            btnBinarySearchRecursiveB.IsEnabled = false;
        }
        private void EnableButtonsA()
        {
            btnBinarySearchIterativeA.IsEnabled = true;
            btnBinarySearchRecursiveA.IsEnabled = true;
        }
        private void EnableButtonsB()
        {
            btnBinarySearchIterativeB.IsEnabled = true;
            btnBinarySearchRecursiveB.IsEnabled = true;
        }
        #endregion

        #endregion

        #region SortAndSearchMethods
        #region Sort
        // 4.7	Create a method called “SelectionSort” which has a single input parameter of type LinkedList, while the calling code argument is the linkedlist name.
        // The method code must follow the pseudo code supplied below in the Appendix. The return type is Boolean.
        private bool SelectionSort(LinkedList<Double> sensor)
		{
            // catch any errors
			try
			{
                // initiate min max
                int min = 0;
                int max = NumberOfNodes(sensor);
                // loop through until last in list
                for (int i = 0; i < max - 1; i++)
                {
                    // set min to current iteration
                    min = i;
                    // start loop from iteration until end
                    for (int j = i + 1; j < max; j++)
                    {
                        // if at iteration is less than what min is
                        if (sensor.ElementAt(j) < sensor.ElementAt(min))
                        {
                            // set min to iteration
                            min = j;
                        }
                    }
                    LinkedListNode<double>? currentMin = sensor.Find(sensor.ElementAt(min));
                    LinkedListNode<double>? currentI = sensor.Find(sensor.ElementAt(i));
                    // do swap
                    var temp = currentMin.Value;
                    currentMin.Value = currentI.Value;
                    currentI.Value = temp;
                }
				return true;
            }
            catch (Exception ex)
			{
				
				return false;
			}
        }

        // 4.8	Create a method called “InsertionSort” which has a single parameter of type LinkedList, while the calling code argument is the linkedlist name.
        // The method code must follow the pseudo code supplied below in the Appendix.The return type is Boolean.
		private bool InsertionSort(LinkedList<Double> sensor)
		{
            // catch any errors
			try
			{
                // find max
                int max = NumberOfNodes(sensor);
                // loop till end
                for (int i = 0; i < max - 1; i++)
                {
                    // loop from current to end backwards
                    for (int j = i + 1; j > 0; j--)
                    {
                        // if previous from current is less than current
                        if (sensor.ElementAt(j - 1) < sensor.ElementAt(j))
                        {
                            LinkedListNode<Double>? current = sensor.Find(sensor.ElementAt(j));
                            LinkedListNode<Double>? previous = sensor.Find(sensor.ElementAt(j - 1));
                            // swap
                            double temp = current.Value;
                            current.Value = sensor.ElementAt(j - 1);
                            previous.Value = temp;
                        }
                    }
                }
				return true;
            }
			catch (Exception ex)
			{
				return false;
            }
			
		}
        #endregion
        #region Search
        // 4.9	Create a method called “BinarySearchIterative” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        // This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        // The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        // The method code must follow the pseudo code supplied below in the Appendix.
        private int BinarySearchIterative(LinkedList<Double> selectedList, int sVal, int min, int max)
		{
            int count = 0;
            // loop until found or until end
            while (min <= max - 1)
			{
                // find the middle before cutting in half
                int middle = (min + max) / 2;
                // if found return straight away
				if (sVal == selectedList.ElementAt(middle) ) 
				{
                    return ++middle; 
				}
                // if not found then reduce by half from the bottom or from the top
                else if (sVal <  selectedList.ElementAt(middle)) 
				{
                    max = middle - 1; 
				}
				else 
				{
					min = middle + 1; 
				}
                count++;
            }
            // return where its found
            return min;
		}

        // 4.10	Create a method called “BinarySearchRecursive” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        // This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        // The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        // The method code must follow the pseudo code supplied below in the Appendix.
		private int BinarySearchRecursive(LinkedList<Double> selectedList, int sVal, int min, int max)
		{
            // loop until found
			if (min <= max - 1)
			{
                // find the middle
				int middle = (min + max) / 2;
                // if found return
				if (sVal == selectedList.ElementAt(middle)) { return middle; }
                // if not found then do the method again with middle cut from the top or bottom
				else if (sVal < selectedList.ElementAt(middle)) { return BinarySearchRecursive(selectedList, sVal, min, middle - 1); }
				else { return BinarySearchRecursive(selectedList, sVal, middle + 1, max); }
			}
			return min;
		}
        #endregion
        #endregion

        #region UIButtonMethods
        // 4.11	Create four button click methods that will search the LinkedList for an integer value entered into a textbox on the form. The four methods are:
        // 1.	Method for Sensor A and Binary Search Iterative
        private void btnBinarySearchIterativeA_Click(object sender, RoutedEventArgs e)
        {
            // set sensors
            var sensor = sensorA;
            var listbox = lbSensorA;
            int sVal = Int32.Parse(tbSensorASearch.Text);
            // make sure its sorted
            SelectionSort(sensor);
            // start stopwatch
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // set a condition for start
            int result = -1;
            // do the search and record time           
            result = BinarySearchIterative(sensor, sVal, 0, NumberOfNodes(sensor));
            sw.Stop();
            tbBinarySearchIterativeStopWatchA.Text = sw.ElapsedTicks.ToString() + " ticks";
            // if found
            if (result > -1)
            {
                // display it on the listbox then highlight
                DisplayListboxData(sensor, listbox);
                HighlightSearch(result, listbox);
            }
            // reset stopwatch
            sw.Reset();
        }
		// 2.	Method for Sensor A and Binary Search Recursive
		private void btnBinarySearchRecursiveA_Click(object sender, RoutedEventArgs e)
        {
            var sensor = sensorA;
            var listbox = lbSensorA;
            int sVal = Int32.Parse(tbSensorASearch.Text);

            SelectionSort(sensor);
            Stopwatch sw = new Stopwatch();
            int result = -1;

            sw.Start();
            result = BinarySearchRecursive(sensor, sVal, 0, NumberOfNodes(sensor));
            sw.Stop();
            tbBinarySearchRecursiveStopWatchA.Text = sw.ElapsedTicks.ToString() + " ticks";
            if (result > -1)
            {
                DisplayListboxData(sensor, listbox);
                HighlightSearch(result, listbox);

            }
            sw.Reset();
        }
        // 3.	Method for Sensor B and Binary Search Iterative
        private void btnBinarySearchIterativeB_Click(object sender, RoutedEventArgs e)
        {
            var sensor = sensorB;
            var listbox = lbSensorB;
            int sVal = Int32.Parse(tbSensorBSearch.Text);

            SelectionSort(sensor);
            Stopwatch sw = new Stopwatch();
            int result = -1;

            sw.Start();
            result = BinarySearchIterative(sensor, sVal, 0, NumberOfNodes(sensor));
            sw.Stop();
            tbBinarySearchIterativeStopWatchB.Text = sw.ElapsedTicks.ToString() + " ticks";
            if (result > -1)
            {
                DisplayListboxData(sensor, listbox);
                HighlightSearch(result, listbox);
            }
            sw.Reset();
        }
        // 4.	Method for Sensor B and Binary Search Recursive
        private void btnBinarySearchRecursiveB_Click(object sender, RoutedEventArgs e)
        {
            var sensor = sensorB;
            var listbox = lbSensorB;
            int sVal = Int32.Parse(tbSensorBSearch.Text);

            SelectionSort(sensor);
            Stopwatch sw = new Stopwatch();
            int result = -1;

            sw.Start();
            result = BinarySearchRecursive(sensor, sVal, 0, NumberOfNodes(sensor));
            sw.Stop();
            tbBinarySearchRecursiveStopWatchB.Text = sw.ElapsedTicks.ToString() + " ticks";
            if (result > -1)
            {
                DisplayListboxData(sensor, listbox);
                HighlightSearch(result, listbox);
            }
            sw.Reset();
        }
        // The search code must check to ensure the data is sorted, then start a stopwatch before calling the search method.
        // Once the search is complete the stopwatch will stop, and the number of ticks will be displayed in a read only textbox.
        // Finally, the code/method will call the “DisplayListboxData” method and highlight the search target number and two values on each side.
        private void HighlightSearch(int result, ListBox listbox)
        {
            // unselect all and focus on the listbox
            listbox.UnselectAll();
            listbox.Focus();
            // find max
            int totalNodes = NumberOfNodes(sensorA);
            // make sure that its either at the start or the end and cut off the amount highlighted based on start and end
            int start = Math.Max(result - 2, 0);
            int end = Math.Min(result + 2, totalNodes - 1);
            // loop and highlight appropriate amount  
            for (int i = start; i <= end; i++)
            {
                listbox.SelectedItems.Add(listbox.Items[i]);
            }
        }


        //4.12	Create four button click methods that will sort the LinkedList using the Selection and Insertion methods.The four methods are:
        //1.	Method for Sensor A and Selection Sort
        private void btnSelectionSortA_Click(object sender, RoutedEventArgs e)
        {
            // sort then display and enable buttons
            var sensor = sensorA;
            var listbox = lbSensorA;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SelectionSort(sensor);
            sw.Stop();
            selectionSortATimer.Text = sw.ElapsedMilliseconds.ToString() + "ms";
            DisplayListboxData(sensor, listbox);
            EnableButtonsA();
        }
        //2.	Method for Sensor A and Insertion Sort
        private void btnInsertionSortA_Click(object sender, RoutedEventArgs e)
        {
            var sensor = sensorA;
            var listbox = lbSensorA;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            InsertionSort(sensor);
            sw.Stop();
            insertionSortATimer.Text = sw.ElapsedMilliseconds.ToString() + "ms";
            DisplayListboxData(sensor, listbox);
            EnableButtonsA();
        }
        //3.	Method for Sensor B and Selection Sort
        private void btnSelectionSortB_Click(object sender, RoutedEventArgs e)
        {
            var sensor = sensorA;
            var listbox = lbSensorB;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SelectionSort(sensor);
            sw.Stop();
            selectionSortBTimer.Text = sw.ElapsedMilliseconds.ToString() + "ms";
            DisplayListboxData(sensor, listbox);
            EnableButtonsB();
        }
        //4.	Method for Sensor B and Insertion Sort
        private void btnInsertionSortB_Click(object sender, RoutedEventArgs e)
        {
            var sensor = sensorA;
            var listbox = lbSensorB;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            InsertionSort(sensor);
            sw.Stop();
            insertionSortBTimer.Text = sw.ElapsedMilliseconds.ToString() + "ms";
            DisplayListboxData(sensor, listbox);
            EnableButtonsB();
        }
        //The button method must start a stopwatch before calling the sort method.
        //Once the sort is complete the stopwatch will stop, and the number of milliseconds will be displayed in a read only textbox.
        //Finally, the code/method will call the “ShowAllSensorData” method and “DisplayListboxData” for the appropriate sensor.


        //4.13	Add two numeric input controls for Sigma and Mu.The value for Sigma must be limited with a minimum of 10 and a maximum of 20.
        //Set the default value to 10. The value for Mu must be limited with a minimum of 35 and a maximum of 75. Set the default value to 50.
        //
        // XAML
        //
        //4.14	Add two textboxes for the search value; one for each sensor, ensure only numeric integer values can be entered.
        private void tbSensorSearch_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //4.15	All code is required to be adequately commented.Map the programming criteria and features to your code/methods by adding comments/regions above the method signatures.
        //Ensure your code is compliant with the CITEMS coding standards (refer http://www.citems.com.au/).

        #endregion

    }
}
