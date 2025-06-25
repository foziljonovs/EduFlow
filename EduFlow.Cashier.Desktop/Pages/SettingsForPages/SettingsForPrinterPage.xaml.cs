using System.Management;
using System.Windows.Controls;

namespace EduFlow.Cashier.Desktop.Pages.SettingsForPages
{
    /// <summary>
    /// Interaction logic for SettingsForPrinterPage.xaml
    /// </summary>
    public partial class SettingsForPrinterPage : Page
    {
        public SettingsForPrinterPage()
        {
            InitializeComponent();
        }

        private void GetPrinters()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            bool printerFound = false;

            foreach (ManagementObject printer in searcher.Get())
            {
                string printerName = printer["Name"].ToString()!;

                if (printer["PortName"] != null && printer["PortName"].ToString()!.ToLower().Contains("usb"))
                {
                    printerComboBox.Items.Add(new ComboBoxItem
                    {
                        Content = printerName,
                        FontSize = 18
                    });

                    printerFound = true;
                }

                if (!printerFound)
                {
                    ComboBoxItem noPrinterItem = new ComboBoxItem
                    {
                        Content = "Printer topilmadi!",
                        IsEnabled = false,
                        FontSize = 18
                    };

                    printerComboBox.Items.Add(noPrinterItem);
                }
            }
        }

        private void printerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(printerComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                Properties.Settings.Default.PrinterName = selectedItem.Content.ToString()!;
                Properties.Settings.Default.Save();
            }
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            GetPrinters();
        }
    }
}
