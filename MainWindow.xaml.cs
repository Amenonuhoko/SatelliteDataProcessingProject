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
		// Declare an instance of the Galileo library in the method and create the appropriate loop construct to populate the two LinkedList;
		// the data from Sensor A will populate the first LinkedList, while the data from Sensor B will populate the second LinkedList.
		// The LinkedList size will be hardcoded inside the method and must be equal to 400.
		// The input parameters are empty, and the return type is void.
		private void LoadData()
		{
			ReadData readData= new ReadData();
			double mu = Convert.ToDouble(nsMu.Value);
			double sigma = Convert.ToDouble(nsSigma.Value);

			int maxPopSize = 400;

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
			var myObservableCollection = new ObservableCollection<object>();

			for (int i = 0; i < 400; i++)
			{
				var itemA = sensorA.ElementAt(i);
				var itemB = sensorB.ElementAt(i);
				myObservableCollection.Add(new { Value1 = itemA, Value2 = itemB });
			}

			this.lvSensorData.ItemsSource = myObservableCollection;
		}

		// 4.4	Create a button and associated click method that will call the LoadData and ShowAllSensorData methods.
		// The input parameters are empty, and the return type is void.
		private void btnLoadSensorData_Click(object sender, RoutedEventArgs e)
		{
			LoadData();
			ShowAllSensorData();
		}
		#endregion

		#region UtilityMethods
		// 4.5	Create a method called “NumberOfNodes” that will return an integer which is the number of nodes(elements) in a LinkedList.
		// The method signature will have an input parameter of type LinkedList, and the calling code argument is the linkedlist name.
		private int NumberOfNodes(LinkedList<Double> selectedList)
		{
			return selectedList.Count();
		}
		// 4.6	Create a method called “DisplayListboxData” that will display the content of a LinkedList inside the appropriate ListBox.
		// The method signature will have two input parameters; a LinkedList, and the ListBox name.
		// The calling code argument is the linkedlist name and the listbox name.
		private void DisplayListboxData (LinkedList<Double> selectedList, ListBox selectedListBox)
		{
			selectedListBox.Items.Clear();
			foreach (var items in selectedList)
			{
				selectedListBox.Items.Add(items);
			}
		}
		#endregion

		#region SortAndSearchMethods
		// 4.7	Create a method called “SelectionSort” which has a single input parameter of type LinkedList, while the calling code argument is the linkedlist name.
		// The method code must follow the pseudo code supplied below in the Appendix. The return type is Boolean.
		private bool SelectionSort(LinkedList<Double> sensor)
		{
			try
			{
                int min = 0;
                int max = NumberOfNodes(sensor);
                for (int i = 0; i < max - 1; i++)
                {
                    min = i;
                    for (int j = i + 1; j < max; j++)
                    {
                        if (sensor.ElementAt(j) < sensor.ElementAt(min))
                        {
                            min = j;
                        }
                    }
                    LinkedListNode<double>? currentMin = sensor.Find(sensor.ElementAt(min));
                    LinkedListNode<double>? currentI = sensor.Find(sensor.ElementAt(i));

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
			try
			{
                int max = NumberOfNodes(sensor);
                for (int i = 0; i < max - 1; i++)
                {
                    for (int j = i + 1; j > 0; j--)
                    {
                        if (sensor.ElementAt(j - 1) < sensor.ElementAt(j))
                        {
                            LinkedListNode<Double>? current = sensor.Find(sensor.ElementAt(j));
                            LinkedListNode<Double>? previous = sensor.Find(sensor.ElementAt(j - 1));
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
                Debug.WriteLine(ex.ToString());
				return false;
            }
			
		}

        // 4.9	Create a method called “BinarySearchIterative” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        // This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        // The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        // The method code must follow the pseudo code supplied below in the Appendix.
		private int BinarySearchIterative(LinkedList<Double> selectedList, int sVal, int min, int max)
		{
            int count = 0;
            while (min <= max - 1)
			{
                int middle = (min + max) / 2;
				if (sVal == selectedList.ElementAt(middle) ) 
				{
                    return ++middle; 
				}
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
            return min;
		}

        // 4.10	Create a method called “BinarySearchRecursive” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        // This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        // The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        // The method code must follow the pseudo code supplied below in the Appendix.
		private int BinarySearchRecursive(LinkedList<Double> selectedList, int sVal, int min, int max)
		{
			if (min <= max - 1)
			{
				int middle = (min + max) / 2;
				if (sVal == selectedList.ElementAt(middle)) { return middle; }
				else if (sVal < selectedList.ElementAt(middle)) { return BinarySearchRecursive(selectedList, sVal, min, middle - 1); }
				else { return BinarySearchRecursive(selectedList, sVal, middle + 1, max); }
			}
			return min;
		}
        #endregion

        #region UIButtonMethods
        // 4.11	Create four button click methods that will search the LinkedList for an integer value entered into a textbox on the form. The four methods are:
        // 1.	Method for Sensor A and Binary Search Iterative
        private void btnBinarySearchIterative_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorA);
            Stopwatch sw = new Stopwatch();
			int result = -1;
			int sVal = Int32.Parse(tbSensorASearch.Text);

            sw.Start();
			result = BinarySearchIterative(sensorA, sVal , 0, NumberOfNodes(sensorA));
            if (result > -1)
			{
				sw.Stop();
                tbBinarySearchIterativeStopWatch.Text = sw.ElapsedMilliseconds.ToString() + "ms";
                DisplayListboxData(sensorA, lbSensorA);
                HighlightSearch(result);
            }
            sw.Reset();
        }
		// 2.	Method for Sensor A and Binary Search Recursive
		private void btnBinarySearchRecursive_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorA);
            Stopwatch sw = new Stopwatch();
            int result = -1;
            int sVal = Int32.Parse(tbSensorASearch.Text);

            sw.Start();
            result = BinarySearchRecursive(sensorA, sVal, 0, NumberOfNodes(sensorA));
            if (result > -1)
            {
                sw.Stop();
                tbBinarySearchRecursiveStopWatch.Text = sw.ElapsedMilliseconds.ToString() + "ms";
                DisplayListboxData(sensorA, lbSensorA);
                HighlightSearch(result);
            }
            sw.Reset();
        }
        // 3.	Method for Sensor B and Binary Search Iterative
        //
        // TODO
        //
        // 4.	Method for Sensor B and Binary Search Recursive
        //
        // TODO
        //
        // The search code must check to ensure the data is sorted, then start a stopwatch before calling the search method.
        // Once the search is complete the stopwatch will stop, and the number of ticks will be displayed in a read only textbox.
        // Finally, the code/method will call the “DisplayListboxData” method and highlight the search target number and two values on each side.
        private void HighlightSearch(int result)
		{
            lbSensorA.UnselectAll();
            lbSensorA.Focus();
            if (result == 0)
            {
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result + 1]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result + 2]);
            }
			else if (result == 1)
			{
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result - 1]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result + 1]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result + 2]);
            }
			else if (result == NumberOfNodes(sensorA) - 2)
			{
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result - 2]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result - 1]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result + 1]);
            }
            else if (result == NumberOfNodes(sensorA) - 1)
            {
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result - 2]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result - 1]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result]);
            }
			else if (result == NumberOfNodes(sensorA))
			{
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result - 2]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result - 1]);
            }
            else
            {
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result - 2]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result - 1]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result + 1]);
                lbSensorA.SelectedItems.Add(lbSensorA.Items[result + 2]);
            }

			
        }

        //4.12	Create four button click methods that will sort the LinkedList using the Selection and Insertion methods.The four methods are:
        //1.	Method for Sensor A and Selection Sort
        private void btnSelectionSort_Click(object sender, RoutedEventArgs e)
        {
			
			SelectionSort(sensorA);
            DisplayListboxData(sensorA, lbSensorA);
        }
        //2.	Method for Sensor A and Insertion Sort
        private void btnInsertionSort_Click(object sender, RoutedEventArgs e)
        {
			InsertionSort(sensorA);
			DisplayListboxData(sensorA, lbSensorA);
        }
        //3.	Method for Sensor B and Selection Sort
        //
        // TODO
        //
        //4.	Method for Sensor B and Insertion Sort
        //
        // TODO
        //
        //The button method must start a stopwatch before calling the sort method.
        //Once the sort is complete the stopwatch will stop, and the number of milliseconds will be displayed in a read only textbox.
        //Finally, the code/method will call the “ShowAllSensorData” method and “DisplayListboxData” for the appropriate sensor.
        //
        // TODO
        //


        //4.13	Add two numeric input controls for Sigma and Mu.The value for Sigma must be limited with a minimum of 10 and a maximum of 20.
        //Set the default value to 10. The value for Mu must be limited with a minimum of 35 and a maximum of 75. Set the default value to 50.
        //4.14	Add two textboxes for the search value; one for each sensor, ensure only numeric integer values can be entered.
        //
        // TODO
        //

        //4.15	All code is required to be adequately commented.Map the programming criteria and features to your code/methods by adding comments/regions above the method signatures.
        //Ensure your code is compliant with the CITEMS coding standards (refer http://www.citems.com.au/).
        //
        // TODO
        //


        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
		{

		}


    }
}
