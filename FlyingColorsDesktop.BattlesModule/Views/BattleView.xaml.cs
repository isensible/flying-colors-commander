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
using Microsoft.Practices.Unity;
using FlyingColorsDesktop.ViewModels;
using Microsoft.Windows.Controls;
using FlyingColors;

namespace FlyingColorsDesktop.Views
{
	/// <summary>
	/// Interaction logic for BattleView.xaml
	/// </summary>
	public partial class BattleView : UserControl
	{
		public BattleView()
		{
			InitializeComponent();
		}

		[Dependency]
		public BattleViewModel ViewModel
		{
			set { DataContext = value; }
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

		private void HullHitsSpinner_Spin(object sender, SpinEventArgs e)
		{
			ButtonSpinner spinner = (ButtonSpinner)sender;
			var battleShip = (BattleShip)spinner.DataContext;
			if (e.Direction == SpinDirection.Increase)
			{
				battleShip.HullHits++;
				if (battleShip.Vunerable)
				{
					spinner.Foreground = Brushes.Red;
				}
				else if (battleShip.Damaged)
				{
					spinner.Foreground = Brushes.Yellow;
				}
				else
				{
					spinner.Foreground = Brushes.Green;
				}
			}
			else
			{
				battleShip.HullHits--;
				if (battleShip.Vunerable)
				{
					spinner.Foreground = Brushes.Red;
				}
				else if (battleShip.Damaged)
				{
					spinner.Foreground = Brushes.Yellow;					
				}
				else
				{
					spinner.Foreground = Brushes.Green;
				}
			}
		}
	}
}
