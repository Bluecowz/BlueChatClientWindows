using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BlueChatClientWindows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Classes.ClientServices client;

        public Boolean Connected
        {
            get { return client == null ? false : client.Connected; }
        }

        public MainPage()
        {
            this.InitializeComponent();
            client = new Classes.ClientServices();
            client.StartClient();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageBody.Text;
            MessageBody.Text = String.Empty;

            if (!Connected)
            {
                MessageList.Items.Add("Not Connected.");
                return;
            }
                

            string response = await client.SendMessage(message);


            string messageOut = String.Format("{0}: {1}", client.Username, response);

            MessageList.Items.Add(messageOut);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            client.CloseClient();
            App.Current.Exit(); //This is traditionaly bad practice. Whatever. 
        }
    }
}
