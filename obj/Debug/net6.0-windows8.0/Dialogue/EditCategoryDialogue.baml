   M S B A M L   `   `   `  ����E  ARecipesApp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:RecipesApp.DialogueRecipesApp.Dialogue  5   	   .    (RecipesApp.Dialogue.EditCategoryDialogue   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:RecipesApp.Dialogue     5   	     � Title$  Add Category��5   	   $	��150��6   $	��800��6+    � 	Resources 5	         TextBoxStyle �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value  ButtonStyle��%�  &	        &	 �    �� 5
   
   " ��6%   .�5      ��  � ��$ 30��6'   �� 5       � ��$ 10q�6'   �� 5       � ��$ 5q�6(   �� 5       � �� �	#FFABADB36,   �� 5       � ��$ 1q�60   �� 5       � ��$ Italic$�6*   �� 5       � ��
 �����6+    5      P� 5       �@J�	IsFocused$
 True��6/   . �5      ��  � ��
 �   �63   �� 5       � ��$ Normal$�62   �� 5       � �� �	#FF6DA34D64   5      5      �� 5   
   " ��6$   .�5      ��  � ��$	 100��6&   �� 5       � ��$ 5q�6'   �� 5       � ��$ 5q�6(   �� 5       � �� �	#FF6DA34D6+   �� 5       � ��
 �   �6+   �� 5        � ��
 �   �6,   �� 5!       � ��$ 1q�60   �� 5"       �@�Cursor$
 Hand��6'    5#      P� 5$       �@J�IsMouseOver$
 True��61   . �5%      ��  � �� �	#FF8DBE7563   �� 5&       � �� �	#FF8DBE7564   5'      5)      5*      .��5+      �� $��10q�6   .+�5,   
   �� -   5         ��Name$ CategoryNameTextBox��5,      #��[  6/   .��6U   	 � ToolTip	 5-      _� 5.      $��Enter category name here��6   .��5/      50      �� 52   
   
 �� Orientation$
 
Horizontal=�6   $��Center=�6.   .+�53      �� -   5        $
��Save��53      #��[ 6=    �� 	IsDefault$
 True��6b   �� 54      -   5        $��Cancel��54      #��[ 6A   55      56      57      