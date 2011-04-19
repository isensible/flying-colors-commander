using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlyingColorsDesktop.ViewModels;
using FlyingColors;
using Microsoft.Practices.Unity;

namespace FlyingColorsDesktop.Views
{
	/// <summary>
	/// Interaction logic for BattleGroupView.xaml
	/// </summary>
	public partial class BattleGroupView : UserControl
	{
		public BattleGroupView()
		{
			InitializeComponent();
		}

		[Dependency]
		public BattleGroupViewModel ViewModel
		{
			set { DataContext = value; }
		}

		private void shipGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				var viewModel = (BattleGroupViewModel)DataContext;
				viewModel.SelectedShip = (BattleShip)e.AddedItems[0];
			}
		}

		private void shipGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			//Get the newly selected cells
			IList<DataGridCellInfo> selectedcells = e.AddedCells;

			var viewModel = (BattleGroupViewModel)DataContext;

			//Get the value of each newly selected cell
			//foreach (DataGridCellInfo di in selectedcells)
			//{
				//Cast the DataGridCellInfo.Item to the source object type
				//In this case the ItemsSource is a DataTable and individual items are DataRows
			if (selectedcells.Count > 0)
			{
				var ship = (BattleShip)selectedcells[0].Item;
				viewModel.SelectedShip = ship;
			}
			//}
		}


		private void Details_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// the original source is what was clicked.  For example 
				// a button.
				DependencyObject dep = (DependencyObject)e.OriginalSource;

				// iteratively traverse the visual tree upwards looking for
				// the clicked row.
				while ((dep != null) && !(dep is DataGridRow))
				{
					dep = VisualTreeHelper.GetParent(dep);
				}

				// if we found the clicked row
				if (dep != null && dep is DataGridRow)
				{
					// get the row
					DataGridRow row = (DataGridRow)dep;

					// change the details visibility
					if (row.DetailsVisibility == Visibility.Collapsed)
					{
						row.DetailsVisibility = Visibility.Visible;
					}
					else
					{
						row.DetailsVisibility = Visibility.Collapsed;
					}
				}
			}
			catch (System.Exception)
			{
			}
		}

		private void Fire_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Fire!");
		}
	}
}
