   M S B A M L   `   `   `  ����?  ;Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Game.MVVM.ViewGame.MVVM.View  5      !    Game.MVVM.View.MainMenuView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Game.MVVM.View     5        � 	Resources  5	       
  arcade �� 
TargetType �� Property �� Value �� ImageSource �� 
TargetType �� Triggers P� Property P� Value	 �� 
TargetName
 T� Y��%  &	        �� 5
   
   " ��6   .�5      ��  �@�OverridesDefaultStyle$
 True��66   �� 5       � ��$6 0../../Resources/ARCADECLASSIC.ttf #ARCADECLASSIC)�6+   �� 5       � ��$ 26(�6)   �� 5       � ��
 �����6+   �� 5       � �� 5      �� 5      $# ../../Resources/UI_BUTTON.png��6!   5      �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $��border��6!   #��z��5      ._�5      � .+�5   "   �� $��content��6k   $��Center=�63   $��Center=�6P   6k   5      5       5      P� 5       �@J�IsMouseOver$
 True��6=   . �5   "   ��  � �� �	#FF4788c86?   5       P� 5!       � ��$
 True��6;   . �5"   "   �� $	 content��6)    � z� 5#   &   T� 5$   *   $	
 1.0Q�6=   5&   #   5'      P� 5(       � {�$ False��6;   . �5)   "   ��  � ��
 �����6?   �� 5*   "    � �� 5+   &   �� 5,   *   $' !../../Resources/UI_BUTTON_DIS.png��65   5.   #   5/      51      53      54      55      .��56      � -   5         �Name$ MainGrid��56       � RowDefinitions 57   
   �� 58      $��2* �6   �� 59      $��3* �6   .+�5;   
   �� -   5         ��Name$ Title��5;   �   $#��../../Resources/traplands.png��6   $	��500��67   $��50q�6C   $
��Left=�6O   $	��Top=�6j   6�   �� 5<   
   $��1��6   .+�5=      �� $	��250��6   #��[  6!   $��Singleplayer��6r   $��50��5>      $��200 10 200 10q�6   ��5=   B   �� *NavigateLevelManagerCommand+5?      �� 5@      $��50��6   $	��250��6!   #��[  6-   $��Multiplayer��6~   $��200 10 200 10q�5A      ��5@   M   �� *NavigateMultiGameMenuCommand+5B      �� 5C      $��50��6   $	��250��6!   #��[  6-   $��Level Editor��6|   $��200 10 200 10q�5D      ��5C   M   �� *NavigateLevelEditorCommand+5E      �� 5F      $��50��6   $	��250��6!   #��[  6-   $
��Exit��6f   $��200 100 200 10q�5G      ��5F   M   �� *Exit+5H      5I      5J      5K      