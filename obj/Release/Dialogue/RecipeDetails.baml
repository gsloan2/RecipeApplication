   M S B A M L   `   `   `  ����   
RecipesApp'    !RecipesApp.Dialogue.RecipeDetails   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � SizeToContent$  Height=�$	��600�� � Title$ Recipe Details�� � 	Resources    TextBoxStyle �� 
TargetType �� Property �� Value  ComboBoxStyle  ButtonStyle �� Triggers P� Property P� Value��%W  &	        &	 �     &	      �� " ��.���  � ��$ 0q���  � ��$ 0q���  � �� �	#FFABADB3��  � ��$ 1q���  � ��$ 14(��� " ��.���  � ��$ 5q���  � ��$ 5q���  � �� �	#FFABADB3��  � ��$ 1q���  � ��$ 14(��� " ��.���  � ��$	 100����  � ��$ 5q���  � ��$ 5q���  � �� �	#FF6DA34D��  � ��
 �������  � �� �	#FF6DA34D��  � ��$ 1q���  �@�Cursor$
 Hand�� P�  �@J�IsMouseOver . . ���  � �� �	#FF8DBE75��  � �� �	#FF8DBE75.���� $��10q�.+��� 	 �� Orientation$	 
Horizontal=�$
��Left=�$��0,0,0,10q�.+��� -   
 ��Name$
 RecipeNameTextBox��$��Recipe Name��$
��Bold"�$��18(�$��0,0,10,0q�$��Center=�#��[  �� -    ��Name$ CategoryComboBox��$	��200��$��Center=� �� DisplayMemberPath$
 Name�� �� SelectedValuePath$
 Name��#��[ �� -    ��Name$ IngredientsTextBlock��$��Ingredients��$
��Bold"�$��0,0,0,5q��� -    ��Name$ IngredientsItemsControl������ *	ListItems+���� .R��� -   #��[  ���� *Text+ �� UpdateSourceTrigger$ PropertyChanged=��� -   $ InstructionsTextBlock��$��Instructions��$
��Bold"�$��	0,10 ,0,5q��� -   $
 InstructionsTextBox�� �� TextWrapping$
 Wrap=� �� AcceptsReturn . $	��200��" �� VerticalScrollBarVisibility$
 Auto=�#��[  �� -   $
��Save��#��[  �� 	IsDefault . 