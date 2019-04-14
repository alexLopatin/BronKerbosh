using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace kursach
{
    public sealed class Vertex : INotifyPropertyChanged
    {
        public int Id;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private int x;
        private int y;
        private string name;
        private Vertex vertex;

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }
        public int CentreX
        {
            get
            {
                return x+25;
            }
        }
        public int CentreY
        {
            get
            {
                return y + 25;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vertex(Vertex vertex)
        {
            this.x = vertex.x;
            this.y = vertex.y;
            this.Id = vertex.Id;
            this.Name = vertex.Name;
        }
    }
}
