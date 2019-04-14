using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace kursach
{
   
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private AdjancencyMatrix adjMatrix;
        public bool isDisabled = false;
        private int GiveId()
        {
            if (Vertices.Count <= 1)
                return Vertices.Count;

            if (Vertices[0].Id > 0)
                return 0;

            for (int i = 1; i < Vertices.Count; i++)
                if (Vertices[i - 1].Id < i && Vertices[i].Id > i)
                    return Vertices[i - 1].Id + 1;
            return Vertices.Count;
        }

        public ObservableCollection<Vertex> Vertices { get; set; }
        public ObservableCollection<Edge> Edges { get; set; }
        public CompositeCollection Composite { get; set; }
        public ObservableCollection<string> Results { get; set; }

        private void ClearResult()
        {
            Results.Clear();
        }
        private void ClearCanvas()
        {
            Vertices.Clear();
            int count = Edges.Count;
            if(count>1)
            for (int i=1; i< count; i++)
                Edges.RemoveAt(1);
        }
        private void TransformToResult(List<List<Vertex>> result)
        {
            ClearResult();
            for(int i=0; i<result.Count; i++)
            {
                string res = (i + 1).ToString() + ")  { ";
                for(int j=0; j<result[i].Count; j++)
                    if(j!= result[i].Count-1)
                        res += "V" + result[i][j].Id.ToString() + ",";
                    else
                        res += "V" + result[i][j].Id.ToString();
                res += " } ";
                Results.Add(res);
            }
        }

        public ItemsControl canvasPos;

        // команда добавления нового объекта
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      
                      var mouse = Mouse.GetPosition(canvasPos);
                      Vertex v =new Vertex((int)mouse.X - 25, (int)mouse.Y - 25);
                      v.Id = GiveId();
                      v.Name = v.Id.ToString();
                      
                      Vertices.Insert(v.Id, v);
                      
                  }));
            }
        }

        private RelayCommand computeCommand;
        public RelayCommand ComputeCommand
        {
            get
            {
                return computeCommand ??
                  (computeCommand = new RelayCommand(obj =>
                  {
                      var res = BronKerboshMethod.Alghorithm(adjMatrix, Vertices);
                      TransformToResult(res);
                  }, 
                  (obj) =>
                  {
                      return Vertices.Count>0;
                  }));
            }
        }

        private RelayCommand clearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                  (clearCommand = new RelayCommand(obj =>
                  {

                      ClearCanvas();
                      ClearResult();
                      adjMatrix.RemoveAll();
                  }));
            }
        }
        private System.Windows.Point lastMousePosition;
        private bool isEdit = false;
        private Vertex currVertex;
        private Edge currLoop;
        void AddLoopHandler(object sender, MouseEventArgs e)
        {
            var mouse = Mouse.GetPosition(canvasPos);
            System.Windows.Point start = new System.Windows.Point(-1, 0);
            mouse.X -= currLoop.FirstVertex.CentreX;
            mouse.Y -= currLoop.FirstVertex.CentreY;
            double dist = Math.Sqrt(mouse.X * mouse.X + mouse.Y * mouse.Y);
            mouse.X /= dist;
            mouse.Y /= dist;
            double det  = -mouse.X * start.Y + mouse.Y * start.X;
            
            
            double rotation = mouse.X * start.X + mouse.Y * start.Y;
            
            rotation = Math.Atan2(det, rotation) * 180 / Math.PI;

            currLoop.LoopRotation = rotation;
            if (Keyboard.IsKeyDown(Key.Escape) || e.RightButton == MouseButtonState.Pressed)
            {
                canvasPos.MouseMove -= new MouseEventHandler(AddLoopHandler);
                isEdit = false;
            }
            lastMousePosition = mouse;

        }
        void AddEdgeHandler(object sender, MouseEventArgs e)
        {
            var mouse = Mouse.GetPosition(canvasPos);
            Vertex v = new Vertex((int)mouse.X - 25, (int)mouse.Y - 25);
            Edges[0].SecondVertex = v;
            if(Keyboard.IsKeyDown(Key.Escape)||e.RightButton==MouseButtonState.Pressed)
            {
                Edges[0].SecondVertex = null;
                Edges[0].FirstVertex = null;
                canvasPos.MouseMove -= new MouseEventHandler(AddEdgeHandler);
                isEdit = false;
            }
                
        }
        void AddVertexHandler(object sender, MouseEventArgs e)
        {
            var mouse = Mouse.GetPosition(canvasPos);
            currVertex.X = (int)mouse.X - 25;
            currVertex.Y = (int)mouse.Y - 25;
            if ( Mouse.LeftButton == MouseButtonState.Pressed)
            {
                canvasPos.MouseMove -= new MouseEventHandler(AddVertexHandler);
                canvasPos.MouseLeftButtonDown -= new MouseButtonEventHandler(MouseLBVertexHandler);
                isDisabled = false;
            }
        }
        void MouseLBVertexHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                canvasPos.MouseMove -= new MouseEventHandler(AddVertexHandler);
                canvasPos.MouseLeftButtonDown -= new MouseButtonEventHandler(MouseLBVertexHandler);
                isDisabled = false;
            }
        }
        void MouseLBLoopHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                canvasPos.MouseMove -= new MouseEventHandler(AddLoopHandler);
                canvasPos.MouseLeftButtonDown -= new MouseButtonEventHandler(MouseLBLoopHandler);
                isDisabled = false;
            }
        }

        private bool edgeOrArc = true;

        private RelayCommand stopCommand;
        public RelayCommand StopCommand
        {
            get
            {
                return stopCommand ??
                  (stopCommand = new RelayCommand(obj =>
                  {
                      
                      Vertex v = obj as Vertex;
                      isEdit = false;
                      canvasPos.MouseMove -= new MouseEventHandler(AddEdgeHandler);

                      if (!edgeOrArc)
                      {
                          if (Edges.Any(edge => edge.FirstVertex == v && edge.SecondVertex == currVertex))
                          {
                              var ed = Edges.Where(edge => edge.FirstVertex == v && edge.SecondVertex == currVertex).ToList()[0];
                              adjMatrix.AddOneWay(currVertex, v);
                              ed.WayType = WayType.Edge;

                          }
                          else
                          if (!(Edges.Any(edge => edge.FirstVertex == currVertex && edge.SecondVertex == v)))
                          {
                              if (currVertex == v)
                              {
                                  Edges.Add(new Edge(currVertex, v, WayType.Loop));
                                  adjMatrix.AddTwoWay(currVertex, v);
                              }
                              else
                              {
                                  Edges.Add(new Edge(currVertex, v, WayType.Arc));
                                  adjMatrix.AddOneWay(currVertex, v);
                              }

                          }

                      }
                      else
                      {
                          if (currVertex == v)
                          {
                              Edges.Add(new Edge(v, v, WayType.Loop));
                              adjMatrix.AddTwoWay(v, v);
                          }
                          else
                          {
                              Edges.Add(new Edge(currVertex, v, WayType.Edge));
                              adjMatrix.AddTwoWay(currVertex, v);
                          }
                          
                      }

  
                        
                      Edges[0].SecondVertex = null;
                      Edges[0].FirstVertex = null;
                     
                  },
                  (obj) =>
                  {
                      return isEdit;
                  }
                  ));
            }
        }

        private RelayCommand moveCommand;
        public RelayCommand MoveCommand
        {
            get
            {
                return moveCommand ??
                  (moveCommand = new RelayCommand(obj =>
                  {
                      Vertex v = obj as Vertex;
                      currVertex = v;
                      canvasPos.MouseMove += new MouseEventHandler(AddVertexHandler);
                      canvasPos.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(MouseLBVertexHandler);
                      isDisabled = true;
                  },
                  
                  (obj) =>
                  {
                      return !isEdit&&!isDisabled;
                  }));
            }

        }

        private RelayCommand rotateCommand;
        public RelayCommand RotateCommand
        {
            get
            {
                return rotateCommand ??
                  (rotateCommand = new RelayCommand(obj =>
                  {
                      Edge v = obj as Edge;
                      currLoop = v;
                      canvasPos.MouseMove += new MouseEventHandler(AddLoopHandler);
                      canvasPos.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(MouseLBLoopHandler);
                      isDisabled = true;
                  },

                  (obj) =>
                  {
                      return !isEdit && !isDisabled;
                  }));
            }

        }

        private RelayCommand newCommand;
        public RelayCommand NewCommand
        {
            get
            {
                return newCommand ??
                  (newCommand = new RelayCommand(obj =>
                  {
                      edgeOrArc = true;
                      Vertex v = obj as Vertex;
                      Edges[0].FirstVertex = v;
                      Edges[0].SecondVertex = v;
                      currVertex = v;
                      isEdit = true;
                      canvasPos.MouseMove += new MouseEventHandler(AddEdgeHandler);
                  },

                  (obj) =>
                  {
                      return !isDisabled;
                  }));
            }
        }

        private RelayCommand newOneWayCommand;
        public RelayCommand NewOneWayCommand
        {
            get
            {
                return newOneWayCommand ??
                  (newOneWayCommand = new RelayCommand(obj =>
                  {
                      edgeOrArc = false;
                      Vertex v = obj as Vertex;
                      Edges[0].FirstVertex = v;
                      Edges[0].SecondVertex = v;
                      currVertex = v;
                      isEdit = true;
                      canvasPos.MouseMove += new MouseEventHandler(AddEdgeHandler);
                  },

                  (obj) =>
                  {
                      return !isDisabled;
                  }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                (removeCommand = new RelayCommand(obj =>
                {
                    if(typeof(Vertex)==obj.GetType())
                    {
                        Vertex v = obj as Vertex;
                        if (v != null)
                        {
                            Vertices.Remove(v);
                            Edges.RemoveAll(p => p.FirstVertex == v || p.SecondVertex == v);
                            adjMatrix.RemoveVertex(v);
                        }
                    }
                    else if(typeof(Edge) == obj.GetType())
                    {
                        Edge v = obj as Edge;
                        if (v != null)
                        {
                            if(v.WayType == WayType.Edge|| v.WayType == WayType.Loop)
                            adjMatrix.RemoveTwoWay(v.FirstVertex, v.SecondVertex);
                            else if(v.WayType == WayType.Arc)
                                adjMatrix.RemoveOneWay(v.FirstVertex, v.SecondVertex);

                            Edges.Remove(v);
                        }
                    }

                },
                (obj) =>
                {
                    if (obj == null||isEdit|| isDisabled)
                        return false;
                    if(typeof(Vertex) == obj.GetType())
                    return Vertices.Count > 0;
                    else
                        return Edges.Count > 1;
                }
                ));
            }
        }

        public ApplicationViewModel(ItemsControl canvas)
        {
            adjMatrix = new AdjancencyMatrix();
            Vertices = new ObservableCollection<Vertex>();
            Edges = new ObservableCollection<Edge>();
            Results = new ObservableCollection<string>();
            Edges.Add(new Edge(null,null, WayType.Edge));
            Composite = new CompositeCollection();
            Composite.Add(new CollectionContainer() { Collection = Edges });
            Composite.Add(new CollectionContainer() { Collection = Vertices });
            
            canvasPos = canvas;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
