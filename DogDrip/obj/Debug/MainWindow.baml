   M S B A M L   `   `   `  ����B  >DogDrip, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:DogDripDogDrip  5   	   Q MHotKeyManagement.WPF.4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullsNclr-namespace:BondTech.HotKeyManagement.WPF._4;assembly=HotKeyManagement.WPF.4 BondTech.HotKeyManagement.WPF._4 5   	       DogDrip.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:DogDrip     5   	   ]hkNclr-namespace:BondTech.HotKeyManagement.WPF._4;assembly=HotKeyManagement.WPF.4     5   	   -   5           
CustomFont#��[  5	   	     � Title$  평↔m² 변환기 v1.0��6-   $	��232��6C   $	��230��6P    � 
ResizeMode$ NoResize=�6\   .��5
      � -   5        .+�5   
   �� -   5         ��Name$ 	checkBox1��5      #��[  6b   $��15(�6�   $��단위 출력��6�   $
��Left=�6�   $��	10,10,0,0q�6�   $	��Top=�6�   �� 5   
   -   5        $ 	checkBox2��5      #��[  6b   $��15(�6�   $��m²→평 변환��6�   $
��Left=�6�   $��10,32.333,0,0q�6�   $	��Top=�6�   �� 5   
   -   5        $ 	checkBox3��5      #��[  6b   $��15(�6�   $��트레이 알림 끄기��6�   $
��Left=�6�   $��10,54.666,0,0q�6�   $	��Top=�6�   �� 5   
   -   5        $ 	checkBox4��5      #��[  6&    d� 	IsChecked$
 TrueY�6J   $��15(�6[   $#��단축키에 보조키 사용��6i   $
��Left=�6   $��	10,77,0,0q�6�   $	��Top=�6�   �� 5   
   -   5         ��Name$ 
textBlock2��5      #��[  6(   $
��Left=�6L   $��10,111.331,0,0q�6g   $
��Wrap=�6   $��단축키 입력 : ��6�   $	��Top=�6�   $��15(�6�   �� 5   
   -   5        $ 
textBlock3��5      #��[  6(   $
��Left=�6L   $��10,137.331,0,0q�6g   $
��Wrap=�6   $#��소수점 자릿수 입력 : ��6�   $	��Top=�6�   $��15(�6�   �� 5   
   -	   5        $ 	watermark��5      #��[  6'   $��Italic$�6K   $
��Left=�6^   $��17.333��6y   $��113.334,186.667,0,0q�6�   $
��Wrap=�6�   $��created by soyoon��6�   $	��Top=�6�   $��111.333��6�   $��14(�6�   4  .BondTech.HotKeyManagement.WPF._4.HotKeyControl  5   
   -
   5        $��textBox1��5      #��[  6+   $��12(�6O   $
��Left=�6]   $��Center=�6x   $��23��6�   $��101,108,0,0q�6�    �� TextWrapping$
 Wrap=�6�      Text$ Control + D��6�    �� TextAlignment$ Center=�6�   $	��Top=�6�   $	��114��6     ForceModifiers 6  �� 	 �� ElementName$	 	checkBox4��
 �� Path$
 	IsChecked��� 5   
   -   5         ��Name$ textBox2��5      #��[  6$   $
��Left=�6h   $��Center=�6�   $��23��6�   $��151,136.438,0,0q�6�   $
 Wrap=�6�   $��0��6�   $ Center=�6�   $	��Top=�6�   $��48.666��6  5      5      