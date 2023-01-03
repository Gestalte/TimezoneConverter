using System;
using System.Linq;
using System.Windows;

namespace TimezoneConverter.UI
{
    // TODO: Add tab to UI for converting between GMT time offsets eg What is GMT+2 in GMT-4?

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var timezones = Converter.Timezones.MakeTimezones();

            timezones
                .ToList()
                .ForEach(t =>
                {
                    comboFrom.Items.Add(t.Name);
                    comboTo.Items.Add(t.Name);
                });

            txbTime.Text = DateTime.Now.ToShortTimeString();
        }

        private void ItemChanged(object sender, EventArgs e)
        {
            if (txbOutput==null)
            {
                return;
            }

            txbOutput.Text = "";

            var fromTimezone = Converter.Conversions.GetTimezoneFromName(comboFrom.SelectedValue?.ToString());
            var toTimezone = Converter.Conversions.GetTimezoneFromName(comboTo.SelectedValue?.ToString());

            string time = txbTime.Text;

            var timeSplit = time.Split(':');

            if (!int.TryParse(timeSplit[0], out int hours))
            {
                return;
            }

            if (!int.TryParse(timeSplit[1], out int minutes))
            {
                return;
            }

            if (fromTimezone == null || toTimezone == null)
            {
                return;
            }

            txbOutput.Text = Converter.Conversions.CalculateTime
                (new Converter.Time(hours, minutes)
                , fromTimezone.Value
                , toTimezone.Value
                ).Value;
        }
    }
}
