   M S B A M L   `   `   `  ���� a  ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes  D @ClientWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:ClientWPF	ClientWPF    ClientWPF.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     \themeJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero     )localclr-namespace:ClientWPF     -     � Title$  Client ScreenViewer��$	��350��$	��525��$	��180��$	��320�� � WindowStartupLocation$ CenterScreen=�.��� -   $��MainGrid�����#0e0e0e � 	Resources    TextBlockStyle �� 
TargetType �� Property �� Value��%E   &	        �� " ��.���  � ��
 �������  � ��$ 20(� � ColumnDefinitions �� -    �Name$ SideBarColumn��$��0 ��� $
��199* �.+��� -   $��ScreenCanvas��$��1��.+��� -   $��	MainImage������  �� ElementName$ ScreenCanvas��	 �� Path$	 ActualWidth����� $ ScreenCanvas��$	 ActualHeight�� -   
������ $	��150������ $ MainGrid��$	 ActualHeight�.+��� -   $��ChatButton_TextBlock��$��	-33,0,0,0q�#��[  $	��☰��$��33��$��33��
 �� TextAlignment$
 Center=�$
��Left=�$	��Top=����	#FF464646 � Triggers :�  :� RoutedEvent$ 
MouseEnter��.[��� .���� .����  �� TargetProperty$4 .(TextBlock.Background).(SolidColorBrush.Color)�	 �� To$ 	#FF686868X� g� Duration$ 	00:00:0.2D�:� $ 
MouseLeave��.[��� .���� .���� $4 .(TextBlock.Background).(SolidColorBrush.Color)�$ 	#FF464646X�$ 	00:00:0.2D�� -   $��ChatStackPanel�����	#FF3AB2F7 J� 	AllowDrop . .+�]� .+��� -	   $��MenuTextBlock��$	��Top=�#��[  $	��300��$
 Center=�Chat�� $
��Left=�$��0��$��0���� $��Right=�$��0��$��0���� $��Bottom=�$��Bottom=�$��0,0,0,10q�.+��� -
   $��To_Message_ComboBox��$��25��$��	15,0,15,0q�$��Center=��� -   $��Input_Chat_TextBox�� �� TextWrapping$
 Wrap=� �� AcceptsReturn . $��40��$��	15,0,15,0q� �� 	MaxLength$
 2000���� -   $��Send_Message_Button��$��24��$��	15,0,15,0q����	#FF8FC1FD.��Send�� $��
10,2,10,10q�$
��Auto=�$
��Auto=�.���� -   $��Chat_StackPanel��