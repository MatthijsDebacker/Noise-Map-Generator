   M S B A M L   `   `   `  ����L  HNoiseMapGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:NoiseMapGeneratorNoiseMapGenerator  5   	   K*clr-namespace:NoiseMapGenerator.ViewModelsNoiseMapGenerator.ViewModels  5   	   A%clr-namespace:NoiseMapGenerator.ViewsNoiseMapGenerator.Views  5   	   "    NoiseMapGenerator.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:NoiseMapGenerator     5   	   A
viewModels*clr-namespace:NoiseMapGenerator.ViewModels     5   	   7views%clr-namespace:NoiseMapGenerator.Views     5   	     � Title$  
MainWindow��5
   	   $
��1100��6   $
��1050��6*   $
��1100��67   $
��1050��6H   .��5      � 
������� � DataContext 5   
   0  *NoiseMapGenerator.ViewModels.MainViewModel  5       � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��* �6   .+�5   
   � $��0��6   
�����w�.+�5      ��  �� Orientation$ 
Horizontal=�6   $��Right=�62   .+�5      �� $
��JPEG��6   $��75��6(   $��5q�63    � ToolTip$. (Export current noise map as a jpeg file.��6b   ��6>   �� *SaveJpegCommand+6b   �� 5      $	��PNG��6   $��75��6'   $��5q�62   $- 'Export current noise map as a png file.��6`   ��6=   �� *SavePngCommand+6`   �� 5      $	��BMP��6   $��75��6'   $��5q�62   $- 'Export current noise map as a bmp file.��6`   ��6=   �� *SaveBmpCommand+6`   �� 5      $	��XML��6   $��75��6'   $��5q�62   $. (Export current noise map as an xml file.��6`   ��6=   �� *SaveXmlCommand+6`   5      5      +   %NoiseMapGenerator.Views.NoiseDataView  5   
   -   5          Name$ NoiseEditor��5      $��1��63      NoiseSource 6@   �� *	NoiseData+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5      5      