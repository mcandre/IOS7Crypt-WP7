using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace IOS7Crypt
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        public void EncryptClick(object sender, EventArgs e)
        {
            HashBox.Text = Cipher.encrypt(PasswordBox.Text);
        }

        public void DecryptClick(object sender, EventArgs e)
        {
            PasswordBox.Text = Cipher.decrypt(HashBox.Text);
        }
    }
}