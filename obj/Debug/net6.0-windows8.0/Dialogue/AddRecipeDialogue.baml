   M S B A M L   `   `   `  ����  
RecipesApp+    %RecipesApp.Dialogue.AddRecipeDialogue   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � SizeToContent$  Height=�5   	   $	��600��5   	    � Title$ Recipe Details��5   	    � 	Resources 5	         TextBoxStyle �� 
TargetType �� Property �� Value  ComboBoxStyle  ButtonStyle �� Triggers P� Property P� Value��%�  &	        &	 �     &	 �    �� 5
   
   " ��6%   .�5      ��  � ��$ 0q�6'   �� 5       � ��$ 0q�6(   �� 5       � �� �	#FFABADB36,   �� 5       � ��$ 1q�60   �� 5       � ��$ 14(�6)   5      �� 5   
   " ��6&   .�5      ��  � ��$ 5q�6'   �� 5       � ��$ 5q�6(   �� 5       � �� �	#FFABADB36,   �� 5       � ��$ 1q�60   �� 5       � ��$ 14(�6)   5      �� 5   
   " ��6$   .�5      ��  � ��$	 100��6&   �� 5       � ��$ 5q�6'   �� 5       � ��$ 5q�6(   �� 5       � �� �	#FF6DA34D6+   �� 5       � ��
 �����6+   �� 5       � �� �	#FF6DA34D6,   �� 5       � ��$ 1q�60   �� 5        �@�Cursor$
 Hand��6'    5!      P� 5"       �@J�IsMouseOver$
 True��61   . �5#      ��  � �� �	#FF8DBE7563   �� 5$       � �� �	#FF8DBE7564   5%      5'      5(      .��5)      �� $��10q�6   .+�5*   
   �� 	 �� Orientation$	 
Horizontal=�6   $
��Left=�6.   $��0,0,0,10q�6I   .+�5+      �� -   5        
 ��Name$
 RecipeNameTextBox��5+      $	��150��61   $��Recipe Name��6=   $
��Bold"�6P   $��18(�6b   $��0,0,10,0q�6p   $��Center=�6�   #��[  6�   6�   �� 5,      -   5         ��Name$ CategoryComboBox��5,      $	��200��61   $��Center=�6=    �� DisplayMemberPath$
 Name��6X    �� SelectedValuePath$
 Name��6q   #��[ 6�   5.      5/      �� 51   
   -   5         ��Name$ IngredientsTextBlock��51      $��Ingredients��62   $
��Bold"�6E   $��0,0,0,5q�6W   �� 52   
   -   5         ��Name$ IngredientsItemsControl��52      ��68   �� *	ListItems+��53      �� 54      .R�55      �� -   5        #��[  55   �   ��6   �� *Text+ �� UpdateSourceTrigger$ PropertyChanged=�6�   56      58      �� 59   
   -   5        $ InstructionsTextBlock��59      $��Instructions��63   $
��Bold"�6G   $��	0,10 ,0,5q�6Y   �� 5:   
   -   5        $
 InstructionsTextBox��5:       �� TextWrapping$
 Wrap=�6/    �� AcceptsReturn$
 True��6C   $	��200��6X   " �� VerticalScrollBarVisibility$
 Auto=�6e   #��[  6�   �� 5<   
   $	 
Horizontal=�6   $��Center=�6.   .+�5=      �� -   5        $
��Save��5=      #��[ 6=    �� 	IsDefault$
 True��6b   �� 5>      -	   5        $��Cancel��5>      #��[ 6A   5?      5@      5A      