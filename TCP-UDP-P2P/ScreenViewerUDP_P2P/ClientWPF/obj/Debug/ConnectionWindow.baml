   M S B A M L   `   `   `  ����D  @ClientWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:ClientWPF	ClientWPF  5   	        ClientWPF.ConnectionWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:ClientWPF     5   	   -   5          � Title$  
Connection��5   	   $��429.219��6   $	��300��6-   $��429.219��69   $	��300��6M    � WindowStartupLocation$ CenterScreen=�6\   ���	#FFF5F6FF.��5
      ��  � 	Resources 5   
    �� 
TargetType �� Property �� Value��%f  '��       '�� O     'G� �     �� 5      " ��6   .�5      ��  � ��$ 	10,0,10,0q�6+   5      �� 5      " ��6   .�5      ��  � ��$ 	10,0,10,0q�6+   �� 5       �@��	MaxLength$ 5��6.   5      �� 5      " G�6   .�5      ��  � ��$ 	10,0,10,0q�6+   �� 5       �@G�	MaxLength$ 20��6.   5      5      .+�5   
   �� .��6   IP :6   �� 5   
   -   5        $��
IP_TextBox��5       �� 	MaxLength$ 39��6$   6d   �� 5!   
   .��6   Port :6   �� 5"   
   -   5        $��Port_TextBox��5"      6Y   �� 5$   
   .��6   
NickName :6   �� 5%   
   -   5        $��NickName_TextBox��5%      $ 20��6*   6;   �� 5'   
   .��6   
Password :6   G� 5(   
   -   5        $��Password_PasswordBox��5(       G� 	MaxLength$ 20��62   6C   �� 5*   
   -   5        $��ShowBrokenImage_CheckBox��5*      $��10q�63   .��6�   Show Broken Image6�   �� 5,   
   -   5        $��RandomSend_CheckBox��5,      $��10q�6.   .��6�   #!Random Video and Audio send Ports6�   �� 5.   
   .��6   Video Send Port :6#   �� 5/   
   -   5        $��Video_Send_Port_TextBox��5/      6d   �� 51   
   .��6   Audio Send Port :6#   �� 52   
   -	   5        $��Audio_Send_Port_TextBox��52      6d   �� 54   
   -
   5        $��
10,10,10,0q�54      $��24��6%   
�������.��6]   	Connect6f   56      58      