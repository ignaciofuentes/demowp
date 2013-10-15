using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using StackOJobs.Resources;
using System.Windows;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using System.Linq;

namespace StackOJobs.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        HttpClient client;
        public MainViewModel()
        {
            client = new HttpClient();
            this.Items = new ObservableCollection<Job>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<Job> Items { get; private set; }


        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadData()
        {
            // Sample data; replace with real data            
            try
            {
                var response = await client.GetAsync("http://careers.stackoverflow.com/jobs/feed");
                var content = await response.Content.ReadAsStringAsync();
                var xml = XElement.Parse(content);
                var channel = xml.Elements("channel");
                foreach (var item in channel.Elements("item"))
                {
                    var x = (string)item.Element("title");
                    var des = (string)item.Element("description");
                    var p = x.IndexOf("at");

                    var name = x.Substring(0, p);
                    var company = x.Substring(p += 2, x.Length - p);
                    var j = new Job { ID=(string)item.Element("guid"), Name = name.Trim(), CompanyName = company.Trim(), Description=des };
                    this.Items.Add(j);
                    IsDataLoaded = true;
                }
            }
            catch
            {
                MessageBox.Show("Check your internet connection and try again", "Something bad happened", MessageBoxButton.OK);
            }

            //this.Items.Add(new Job() { ID = "0", Name = "runtime one", CompanyName = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });

            // return x;

            //this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}