   M S B A M L   `   `   `  ���� B  >kursach, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:kursachkursach      kursach.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:kursach       � Title$%  Курсовое задание��$	��850��$	��950�� � Icon$ icon.png�� � 	Resources  �� 
TargetType �� Property �� Value��%H   '��       �� " ��.���  �@��BitmapScalingMode$ HighQuality=�.��� ����  �� ImageSource$- 'Emblem_of_Moscow_Aviation_Institute.png�� j� Stretch$ UniformToFill=� � ColumnDefinitions �� $&�� 5                                ��� �� $��5 �	 � RowDefinitions	 �� $��40 ��� �� $��35 ��� $	��238 �.+�o� $��3��$��0,0,0,10q�.@�m� $��Вычислить��$��30��
 m� Command
 �� *ComputeCommand+m� $��Очистить��
 �� *ClearCommand+m� -   $��Справка���� -   $��br��$��2q�
�������$��1��$��0,2,0,0q�$��1��._��� -   $��canvas������ *	Composite+    kursach.BindingProxy 	  proxy   Data �� DataType   kursach.Vertex J� InputBindings d� Gesture �� Command �� Source �� CommandParameter � ContextMenu m� CommandParameter   kursach.Edge J� RenderTransformOrigin �� CenterX �� CenterY �� Angle	 �� X1	 �� Y1	 �� X2	 �� Y2��%�  &	        (��      *, +)7   7   7   7   7   7   (�� �    *, +)7   7   7      �� �� "  .R��  d� $ 	LeftClicka� �� *Data.StopCommand+8    �� d� $ LeftDoubleClicka� �� *Data.MoveCommand+8   ��  �� .@�m� $!��Добавить ребро��
 �� *Data.NewCommand+8   �� m� $��Добавить дугу��
 �� *Data.NewOneWayCommand+8   �� m� $+��%Переместить вершину��
 �� *Data.MoveCommand+8   �� m� $#��Удалить вершину��
 �� *Data.RemoveCommand+8   �� .+�A� 
���   �$��50��$��50��A� 
�������$��45��$��45���� $��Center=�$��Center=�$��22(�$��Verdana)����� *Name+�� "  .R�� .+�A� 
������ 
���   �$��4��$ 1,0.5&�}�.  $��Right=�$��Bottom=�$��0,0,0,-12.5q����� *	LoopWidth+���� *
LoopHeight+ �� .@�m� $#��Повернуть петлю��
 �� *Data.RotateCommand+8    �� m� $��Удалить петлю��
 �� *Data.RemoveCommand+8   �� z��� $ 0Q�$ 0Q� �� *LoopRotation+�� 
���   �$��4�� �� *FirstVertex.CentreX+ �� *FirstVertex.CentreY+ �� *SecondVertex.CentreX+ �� *SecondVertex.CentreY+ �� .@�m� $��Удалить Ребро��
 �� *Data.RemoveCommand+8   �� �� 
���   �$��4�� �� *FirstArrowPoint.X+ �� *FirstArrowPoint.Y+ �� *TopArrowPoint.X+ �� *TopArrowPoint.Y+�� 
���   �$��4�� �� *SecondArrowPoint.X+ �� *SecondArrowPoint.Y+ �� *TopArrowPoint.X+ �� *TopArrowPoint.Y+���� .R��� 
�������}�.  �� .@�m� $%��Добавить вершину��
 �� *
AddCommand+���� " ��.���  �@��Left �� *X+��  �@��Top �� *Y+�� $��0,10,0,0q�$��1��$��2��$��Center=�$��14(�$��25��$
��Bold"���Результаты вычисления максимально внутренне устойчивых подмножеств графа:�� $��1��$��5,5,0,5q�$��3��.����  �� Orientation$ Vertical=�.+��� ���� *	Results+���� .R��� $��0,0,0,0q� �� Padding$ 5,2,5,2q�$
��Left=�$	��Top=�$��20(����� ���� .R�$�  $� Orientation$ Vertical=�