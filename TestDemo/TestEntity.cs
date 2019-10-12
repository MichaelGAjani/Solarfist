using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo
{
    public class Car
    {
        public enum TradeMake
        {
            Toyota,
            Ford,
            Audi,
            Nissan,
            Honda,
            BMW,
            Subaru,
            Volkswagen,
            Mitsubishi,
            MercedesBenz,
            Mazda,
            KIA,
            LandRover,
            Chrysler,
            Dodge,
            Lotus,
            Lexus,
            Suzuki,
            RAM,
            Chevrolet
        }
        public enum CarCategory
        {
            Car,
            SUV,
            MPV,
            Track
        }
        public enum Style
        {
            Convertible,
            Coupe,
            Hatchback,
            PassengerVan,
            Pickup,
            Sedan,
            Sport,
            Wagan
        }
        int id;
        DateTime salesDate;
        double discount;
        decimal modelPrice;
        MPV mpg=new MPV();
        Transmission trans=new Transmission();
        Power pw=new Power();
        int cylinders;
        TradeMake trademark;
        CarCategory category;
        Style bodyStyle;
        int doors;
        Color totalColor=Color.Red;
        string name;
        string modification;
        Image photo;
        int rate=4;
        [Category("Order Info")]
        public int Id { get => id; set => id = value; }
        [Category("Order Info")]
        public DateTime SalesDate { get => salesDate; set => salesDate = value; }
        public double Discount { get => discount; set => discount = value; }
        public decimal ModelPrice { get => modelPrice; set => modelPrice = value; }
        public MPV Mpg { get => mpg; set => mpg = value; }
        public Transmission Trans { get => trans; set => trans = value; }
        public Power Pw { get => pw; set => pw = value; }
        public int Cylinders { get => cylinders; set => cylinders = value; }
        public TradeMake Trademark { get => trademark; set => trademark = value; }
        public CarCategory Category { get => category; set => category = value; }
        public Style BodyStyle { get => bodyStyle; set => bodyStyle = value; }
        public int Doors { get => doors; set => doors = value; }
        public Color TotalColor { get => totalColor; set => totalColor = value; }
        public string Name { get => name; set => name = value; }
        public string Modification { get => modification; set => modification = value; }
        public Image Photo { get => photo; set => photo = value; }
        public int Rate { get => rate; set => rate = value; }
    }
    public class MPV
    {
        int city;
        int highway;

        public int City { get => city; set => city = value; }
        public int Highway { get => highway; set => highway = value; }
    }
    public class Transmission
    {
        public enum Type
        {
            Manuel,
            Automatic
        }

        Type transType;
        int speed;

        public Type TransType { get => transType; set => transType = value; }
        public int Speed { get => speed; set => speed = value; }
    }
    public class Power
    {
        string torque;
        string horsepower;

        public string Torque { get => torque; set => torque = value; }
        public string Horsepower { get => horsepower; set => horsepower = value; }
    }
}
