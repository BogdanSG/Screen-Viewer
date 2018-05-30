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

namespace ClientWPF {
    /// <summary>
    /// Логика взаимодействия для ChatMessage.xaml
    /// </summary>
    public partial class ChatMessage : UserControl {
        public ChatMessage(bool IsSend, string Text, string From, string To = null) {

            InitializeComponent();

            if (IsSend) {

                this.MainBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFAFFF9E"));

                this.MainBorder.HorizontalAlignment = HorizontalAlignment.Left;

            }//if
            else {

                this.MainBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF9E"));

                this.MainBorder.HorizontalAlignment = HorizontalAlignment.Right;

            }//else

            this.MessageText.Text = $"{From} { (To != null ? $" => {To}" : "") } : {Text}";

        }//ChatMessage

    }//ChatMessage

}//ClientWPF
