   M S B A M L   `   `   `  ����?  ;Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Game.MVVM.ViewGame.MVVM.View  5      /clr-namespace:Game.ConverterGame.Converter  5      -clr-namespace:Game.RendererGame.Renderer  5      (    "Game.MVVM.View.MultiplayerGameView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Game.MVVM.View     5      2	converterclr-namespace:Game.Converter     5      0rendererclr-namespace:Game.Renderer     5        � 	Resources  5      ,   &Game.Converter.GameVisibilityConverter   
GamePaused(   "Game.Converter.GameEffectConverter  GamePausedEffect.   (Game.Converter.ButtonActiveBoolConverter  ButtonActive&    Game.Converter.IdToNameConverter  IdToName+   %Game.Converter.StringToColorConverter  	IdToColor  
labelStyle �� 
TargetType �� Property �� Value 
 arcade �� ImageSource �� 
TargetType �� Triggers P� Property P� Value	 �� 
TargetName
 T� Y��%"  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 �       5   
   6,     5   
   6(     5   
   6.     5   
   6&     5   
   6+   �� 5   
   " ��6#   .�5      ��  � ��$) #../../Resources/novem.ttf #November)�6+   5      �� 5   
   " ��6   .�5      ��  �@�OverridesDefaultStyle$
 True��66   �� 5       � ��$6 0../../Resources/ARCADECLASSIC.ttf #ARCADECLASSIC)�6+   �� 5       � ��$ 26(�6)   �� 5       � ��
 �����6+   �� 5       � �� 5      �� 5      $# ../../Resources/UI_BUTTON.png��6!   5      �� 5       � �� 5       �� 5!      " ��6&   .R�5"      �� $��border��6!   #��z��5#      ._�5$      � .+�5%   "   �� $��content��6k   $��Center=�63   $��Center=�6P   6k   5&      5'       5(      P� 5)       �@J�IsMouseOver$
 True��6=   . �5*   "   ��  � �� �	#FF4788c86?   5+      P� 5,       � ��$
 True��6;   . �5-   "   �� $	 content��6)    � z� 5.   &   T� 5/   *   $	
 1.0Q�6=   51   #   52      P� 53       � {�$ False��6;   . �54   "   ��  � ��
 �����6?   �� 55   "    � �� 56   &   �� 57   *   $' !../../Resources/UI_BUTTON_DIS.png��65   59   #   5:      5<      5>      5?      5@      .��5A      � -   5         �Name$ MainGrid��5A      .+�5B   
   �� -   5         ��Name$ 
MainCamera��5B      $��2��6+   .��5C      '   !Game.Renderer.MultiplayerRenderer  -   5          Name$ Renderer��5C   +   $��2��6=   $
��3600��6N   $
��3600��6[   5D      � 5E   
   -   5        $ 	PauseMenu��5E   �   $	��400��6   $��Stretch=�6d   $��300,100,300,100q�6�   y�6   �� *
GamePaused+ �� 	Converter# [   � RowDefinitions 5F      �� 5G      $��3* �6    �� 5H      $��2* �6    �� 5I      $��8* �6     � ColumnDefinitions 5K      �� 5L      $��1* �6#   �� 5M      $��1* �6#   �� 5N      $��1* �6#   ��5P      �� 5Q       �� Color$ #ccb693��6"    J� Effect 5S      3  -System.Windows.Media.Effects.DropShadowEffect  5T      .+�5V      � $��2��6   $��3��6'   �� 5W      $��../../Resources/medal.png��6   $��1��67   $��50��6D   $��Right=�6O   6m   �� 5X      $��0��6   $��Center=�6!   $��3��6<   $��Center=�6n   
���   �$��	Helvetica)�6�   $��36(�6�   $��10q�6�   ��6P   �� *	GameState+6�   �� 5Y      $��1��6   $��2��6!   $��Center=�61   $
��Left=�6L   #��[ 5\      $��28(�5]      $��10q�6"   $
��Left=�5^      ��5Z      �� *Winner+# [ ��5[      �� *Winner+# [ 5^      �� 5_      #��[ 6   $��1��68   $��1��6E   $��Center=�6U   $��Center=�6p   $��Winner:��6�   
���   �$��28(�6�   $��10q�6�   $��Center=�6�   �� 5`      $��2��6   $��3��6&   $��50q�6:   $��Center=�6F   .+�5a      �� #��[ 6   $��40��6�   $��10,10,10,50q�6�   $��Resume��6�   $��0q�6�   y�69   �� *
GameOver+# [ ��6�   �� *
ResumeGame+��5b      �� 5c      $# ../../Resources/UI_BUTTON.png��6%   5e      �� 5f      #��[ 6   $��40��69   $��10q�6E   $��
Disconnect��6Q   $��0q�6f   ��6z   �� *DisconnectFromServer+��5g      �� 5h      $# ../../Resources/UI_BUTTON.png��6%   5j      5k      5l      5n      5o      