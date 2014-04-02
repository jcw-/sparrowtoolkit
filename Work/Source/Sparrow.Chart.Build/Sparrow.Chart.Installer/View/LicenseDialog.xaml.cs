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

namespace Sparrow.Chart.Installer
{   
    public partial class LicenseDialog : UserControl
    {
        public LicenseDialog()
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(Sparrow.Chart.Installer.Properties.Resources.license);   
            InitializeComponent();
            this.licenseText.Document.Blocks.Clear();
            this.licenseText.Document.Blocks.Add(paragraph);
        }
    }
}
