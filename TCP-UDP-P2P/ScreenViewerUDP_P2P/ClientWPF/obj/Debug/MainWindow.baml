   M S B A M L   `   `   `  ����a  ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes  5   	   D @ClientWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:ClientWPF	ClientWPF 5   	      ClientWPF.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   \themeJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero     5   	   )localclr-namespace:ClientWPF     5   	   -   5          � Title$  Client ScreenViewer��5	   	   $	��350��6%   $	��525��62   $	��180��6>   $	��320��6N    � WindowStartupLocation$ CenterScreen=�6�   .��5
      � -   5        $��MainGrid��5
      ���#0e0e0e � 	Resources 5   
      TextBlockStyle �� 
TargetType �� Property �� Value��%}   &	        �� 5      " ��6+   .�5      ��  � ��
 �����6@   �� 5       � ��$ 20(�6-   6;   5      5       � ColumnDefinitions 5   
   �� 5      -   5         �Name$ SideBarColumn��5      $��0 �64   �� 5      $
��199* �6   .+�5   
   �� -   5        $��ScreenCanvas��5      $��1��6%   .+�5      �� -   5        $��	MainImage��5   �   ��6   ��  �� ElementName$ ScreenCanvas��	 �� Path$	 ActualWidth���6Q   �� $ ScreenCanvas��$	 ActualHeight�6�   � 5       -   5        
������ $	��150��5    ,   ��68   �� $ MainGrid��$	 ActualHeight�.+�5"      �� -   5        $��ChatButton_TextBlock��5"      $��	-33,0,0,0q�68   #��[  6K   $	��☰��6s   $��33��6�   $��33��6�   
 �� TextAlignment$
 Center=�6�   $
��Left=�6�   $	��Top=�6�   ���	#FF464646 � Triggers 5#      :� 5$       :� RoutedEvent$ 
MouseEnter��6'   .[�5%      �� .��5&   "   �� .��5'   &   ��  �� TargetProperty$4 .(TextBlock.Background).(SolidColorBrush.Color)�5(   )   	 �� To$ 	#FF686868X�5)   )    g� Duration$ 	00:00:0.2D�5*   )   5,   #   5-      5.      :� 5/      $ 
MouseLeave��6'   .[�50      �� .��51   "   �� .��52   &   �� $4 .(TextBlock.Background).(SolidColorBrush.Color)�53   )   $ 	#FF464646X�54   )   $ 	00:00:0.2D�55   )   57   #   58      59      5;      5=      5?      � 5A   
   -   5        $��ChatStackPanel��5A   &   ���	#FF3AB2F7 J� 	AllowDrop . .+�5C      ]� .+�5E      �� -	   5        $��MenuTextBlock��5E   1   $	��Top=�6   #��[  6F   $	��300��6n   $
 Center=�6z   Chat6�   �� 5G      $
��Left=�6   $��0��6/   $��0��69   6F   �� 5I      $��Right=�6   $��0��60   $��0��6:   6G   �� 5K      $��Bottom=�6   $��Bottom=�65   $��0,0,0,10q�6P   .+�5M      �� -
   5        $��To_Message_ComboBox��5M      $��25��6:   $��	15,0,15,0q�6F   $��Center=�6Y   6}   �� 5O      -   5        $��Input_Chat_TextBox��5O   G    �� TextWrapping$
 Wrap=�6    �� AcceptsReturn . $��40��6a   $��	15,0,15,0q�6m    �� 	MaxLength$
 2000��6�   6�   �� 5Q      -   5        $��Send_Message_Button��5Q      $��24��68   $��	15,0,15,0q�6D   ���	#FF8FC1FD.��6�   Send6�   5S      �� 5U      $��
10,2,10,10q�6   $
��Auto=�63   $
��Auto=�6V   .��5W      �� -   5        $��Chat_StackPanel��5W   !   5Y      5[      5]      5_      5a      5b      