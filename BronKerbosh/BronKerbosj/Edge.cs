using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;


namespace kursach
{
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public enum WayType
    {
        Edge, Arc, Loop
    }
    public class Edge : INotifyPropertyChanged
    {
        private double loopRotation;
        public double LoopRotation
        {
            get
            {
                return loopRotation;
            }
            set
            {
                loopRotation = value;
                OnPropertyChanged("LoopRotation");
            }
        }
        public bool IsLoop
        {
            get
            {
                return wayType == WayType.Loop;
            }
        }

        private WayType wayType;
        public WayType WayType
        {
            get
            {
                return wayType;
            }
            set
            {
                wayType = value;
                OnPropertyChanged("WayType");
                OnPropertyChanged("firstArrowPoint");
                OnPropertyChanged("secondArrowPoint");
                OnPropertyChanged("topArrowPoint");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            //PropertyChanged += FirstVertex.PropertyChanged;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            
        }

        private Point p = new Point(0, 0);
        public Point TopArrowPoint
        {
            get
            {
                if (FirstVertex == null || SecondVertex == null)
                    return p;
                
                if (wayType == WayType.Arc)
                {
                    //Point result = new Point(SecondVertex);
                    p.X = SecondVertex.CentreX;
                    p.Y = SecondVertex.CentreY;
                    p.X -= FirstVertex.CentreX;
                    p.Y -= FirstVertex.CentreY;
                    double dist = Math.Sqrt(p.X * p.X + p.Y * p.Y);
                    p.X = (p.X / dist);
                    p.Y = (p.Y / dist);

                    double wayX = p.X;
                    double wayY = p.Y;
                    p.X += SecondVertex.CentreX - wayX * 25;
                    p.Y += SecondVertex.CentreY - wayY * 25;

                }
                else
                {
                    p.X = SecondVertex.CentreX;
                    p.Y = SecondVertex.CentreY;
                }
                
                return p;
            }
        }
        public Point FirstArrowPoint
        {
            get
            {
                if (FirstVertex == null || SecondVertex == null)
                    return p;
                
                if (wayType == WayType.Arc)
                {
                    //Point result = new Point(SecondVertex);
                    p.X = SecondVertex.CentreX;
                    p.Y = SecondVertex.CentreY;
                    p.X -= FirstVertex.CentreX;
                    p.Y -= FirstVertex.CentreY;
                    double dist = Math.Sqrt(p.X * p.X + p.Y * p.Y);
                    p.X = (p.X / dist);
                    p.Y = (p.Y / dist);

                    double wayX = p.X;
                    double wayY = p.Y;
                    double normalX = -p.Y;
                    double normalY = p.X;
                    p.X *= -50;
                    p.Y *= -50;

                    p.X += normalX * 10;
                    p.Y += normalY * 10;
                    p.X += SecondVertex.CentreX;
                    p.Y += SecondVertex.CentreY;

                }
                else
                {
                    p.X = SecondVertex.CentreX;
                    p.Y = SecondVertex.CentreY;
                }
                
                return p;
            }
        }
        public Point SecondArrowPoint
        {
            get
            {
                if (FirstVertex == null || SecondVertex == null)
                    return p;

                if (wayType == WayType.Arc)
                {
                    //Point result = new Point(SecondVertex);
                    
                    p.X = SecondVertex.CentreX;
                    p.Y = SecondVertex.CentreY;
                    p.X -= FirstVertex.CentreX;
                    p.Y -= FirstVertex.CentreY;
                    double dist = Math.Sqrt(p.X * p.X + p.Y * p.Y);
                    p.X = (p.X / dist);
                    p.Y = (p.Y / dist);

                    double wayX = p.X;
                    double wayY = p.Y;
                    double normalX = -p.Y;
                    double normalY = p.X;
                    p.X *= -50;
                    p.Y *= -50;

                    p.X -= normalX * 10;
                    p.Y -= normalY * 10;
                    p.X += SecondVertex.CentreX;
                    p.Y += SecondVertex.CentreY;

                }
                else
                {
                    p.X = SecondVertex.CentreX;
                    p.Y = SecondVertex.CentreY;
                }
                
                return p;
            }
            set
            {
                
            }
        }

        public int LoopWidth
        {
            get
            {
                if (wayType == WayType.Loop)
                    return 75;
                else
                    return 0;
            }
        }
        public int LoopHeight
        {
            get
            {
                if (wayType == WayType.Loop)
                    return 25;
                else
                    return 0;
            }
        }

        private Vertex firstVertex;
        private Vertex secondVertex;

        public Vertex FirstVertex
        {
            get
            {
                return firstVertex;
            }
            set
            {
                firstVertex = value;
                OnPropertyChanged("FirstVertex");
                OnPropertyChanged("firstArrowPoint");
                OnPropertyChanged("secondArrowPoint");
                OnPropertyChanged("topArrowPoint");
                
            }
        }
        public Vertex SecondVertex
        {
            get
            {
                return secondVertex;
            }
            set
            {

                secondVertex = value;
                OnPropertyChanged("SecondVertex");
                OnPropertyChanged("firstArrowPoint");
                OnPropertyChanged("secondArrowPoint");
                OnPropertyChanged("topArrowPoint");
                if (IsLoop)
                    OnPropertyChanged("FirstVertex");
            }
        }
        public Edge(Vertex First, Vertex Second, WayType wayType)
        {
            this.wayType = wayType;
            LoopRotation = 0;
            FirstVertex = First;
            SecondVertex = Second;
            if (wayType == WayType.Loop)
                FirstVertex = SecondVertex;
            PropertyChangedEventHandler eh =
                new PropertyChangedEventHandler(ChildChanged);
            if(FirstVertex!=null)
            FirstVertex.PropertyChanged += eh;
            if (SecondVertex != null)
                secondVertex.PropertyChanged += eh;

        }
        public void ChildChanged(object sender, PropertyChangedEventArgs e)
        {
            Vertex child = sender as Vertex;
            if(child==firstVertex)
            {
                if(IsLoop)
                    OnPropertyChanged("SecondVertex");
                OnPropertyChanged("FirstVertex");
                OnPropertyChanged("firstArrowPoint");
                OnPropertyChanged("secondArrowPoint");
                OnPropertyChanged("topArrowPoint");
            }
            else
            {
                OnPropertyChanged("SecondVertex");
                OnPropertyChanged("firstArrowPoint");
                OnPropertyChanged("secondArrowPoint");
                OnPropertyChanged("topArrowPoint");
            }
                
        }

    }
}
