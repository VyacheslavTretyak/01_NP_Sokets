﻿using System;
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

namespace StreetClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			InitComboBox();
			BtnSend.Click += BtnSend_Click;
		}

		private void InitComboBox()
		{
			ComboBoxIndex.Items.Add("50001");
			ComboBoxIndex.Items.Add("50002");
			ComboBoxIndex.Items.Add("50003");
		}

		private void BtnSend_Click(object sender, RoutedEventArgs e)
		{
			string zip = (string)ComboBoxIndex.SelectedItem;
			MessageBlock.Items.Add($"Streets by Zip:{zip}");
			StreetDBClient client = new StreetDBClient("127.0.0.1", 1024, MessageBlock, zip);
		}
	}
}
