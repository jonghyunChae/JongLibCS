using System;
using System.Collections.Generic;
using System.Text;

namespace WpfMapAPI
{
    internal class MyLocale
    {
        internal MyLocale(string name, double lat, double lng)
        {
            Name = name;
            Lat = lat;
            Lng = lng;
        }

        internal string Name { get; private set; }
        internal double Lat { get; private set; }
        internal double Lng { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
