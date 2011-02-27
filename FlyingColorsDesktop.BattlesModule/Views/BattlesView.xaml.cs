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

namespace FlyingColorsDesktop.Views
{
	/// <summary>
	/// Interaction logic for BattlesView.xaml
	/// </summary>
	public partial class BattlesView : UserControl
	{
		public BattlesView()
		{
			InitializeComponent();
			this.DataContext = new BattlesViewModel();
		}
	}
}
