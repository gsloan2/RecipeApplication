   M S B A M L   `   `   `  ����   
RecipesApp    RecipesApp.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Recipe Manager��$	��450�� � SizeToContent$ Width=� � WindowStartupLocation$ CenterScreen=� � 	Resources    ButtonStyle �� 
TargetType �� Property �� Value �� Triggers P� Property	 P� Value  CategoryListBoxStyle  RecipeListBoxStyle  IconButtonStyle��%�  &	        &	 0    &	 �    &	     �� " ��.���  � �� �	#FF6DA34D��  � ��
 �   ���  � ��
 �   ���  � ��$
 Bold"���  � ��$ 4q���  � ��$	 8,4q���  �@�Cursor$
 Hand����  � ��$ 2q� P�  �@J�IsMouseOver	 . . ���  � �� �	#FFF0F0F0��  � ��
 �   ��� " ��.���  � �� �	#FFF0F0F0��  � ��
 ���� ��  � ��$ 10q���  � ��$ 16(��� " ��.���  � �� �	#FFF0F0F0��  � ��
 ���� ��  � ��$ 10q���  � ��$ 14(��� " ��.���  � ��$ 50����  � ��$ 50����  � ��$ 5q���  � ��$ 5q�.��]� .+�c� $	��Top=�.@��� -   
 ��Name$
 AddCategoryButton��$��Add Category��#��[  �� -   $
 EditCategoryButton��$��Edit Category��#��[  �� -   $
 DeleteCategoryButton��$��Delete Category��#��[  �� �� -   $
 AddRecipeButton��$��
Add Recipe��#��[  �� -   $
 ImportRecipeButton��$��Import Recipe��#��[  �� -   $
 EditRecipeButton��$��Edit Recipe��#��[  �� -   $
 ExportRecipeButton��$��Export Recipe��#��[  �� -   $
 DeleteRecipeButton��$��Delete Recipe��#��[  �  � ColumnDefinitions �� $
��Auto ��� $	��200 ��� $��* �.+��� $��0�� �� Orientation$ Vertical=�.+��� #��[ .���� $��resources/home.png���� #��[ .���� $��resources/search.png���� -	    ��Name$ CategoryList��$��1�� �� SelectionMode$ Single=� �� DisplayMemberPath$
 Name��#��[  �� SelectedItem �� *SelectedCategory+ �� Mode$ TwoWay=��� -
   $ 
RecipeList��$��2��$ Title��#��[ 